// Fetches every episode of the show from the Spotify Web API and writes data/episodes.json.
// Run with `yarn fetch-episodes`; credentials come from SPOTIFY_CLIENT_ID / SPOTIFY_CLIENT_SECRET
// (env vars in CI, or .env.local locally). The output is stable (sorted, no timestamps), so the
// file only changes when an episode is added or edited on Spotify.
import { writeFile } from "node:fs/promises";

const SHOW_ID = "280aceAx85AKZslVytXsrB";
const MARKET = "CZ";
const OUTPUT = new URL("../data/episodes.json", import.meta.url);

interface SpotifyImage {
  url: string;
  width: number | null;
}

interface SpotifyEpisode {
  id: string;
  name: string;
  description: string;
  html_description: string;
  release_date: string;
  duration_ms: number;
  images: SpotifyImage[];
}

interface Page {
  items: (SpotifyEpisode | null)[];
  next: string | null;
}

interface Episode {
  id: string;
  name: string;
  description: string;
  releaseDate: string;
  durationMs: number;
  image: string | null;
}

function requireEnv(name: string): string {
  const value = process.env[name];
  if (!value) throw new Error(`Missing ${name} (set it in .env.local or as an environment variable)`);
  return value;
}

async function getToken(clientId: string, clientSecret: string): Promise<string> {
  const res = await fetch("https://accounts.spotify.com/api/token", {
    method: "POST",
    headers: {
      Authorization: `Basic ${Buffer.from(`${clientId}:${clientSecret}`).toString("base64")}`,
      "Content-Type": "application/x-www-form-urlencoded",
    },
    body: "grant_type=client_credentials",
  });
  if (!res.ok) throw new Error(`Spotify token request failed: ${res.status} ${await res.text()}`);
  return ((await res.json()) as { access_token: string }).access_token;
}

async function getAllEpisodes(token: string): Promise<SpotifyEpisode[]> {
  const episodes: SpotifyEpisode[] = [];
  let url: string | null = `https://api.spotify.com/v1/shows/${SHOW_ID}/episodes?market=${MARKET}&limit=50`;
  while (url) {
    const res = await fetch(url, { headers: { Authorization: `Bearer ${token}` } });
    if (!res.ok) throw new Error(`Spotify episodes request failed: ${res.status} ${await res.text()}`);
    const page = (await res.json()) as Page;
    episodes.push(...page.items.filter((e): e is SpotifyEpisode => e !== null));
    url = page.next;
  }
  return episodes;
}

// Spotify's plain `description` glues paragraphs together ("…si!AI agenti…"), so the text is
// rebuilt from `html_description`, keeping paragraph and line breaks as "\n".
const ENTITIES: Record<string, string> = { amp: "&", lt: "<", gt: ">", quot: '"', apos: "'", nbsp: " " };

function htmlToText(html: string): string {
  return html
    .replace(/<br\s*\/?>/gi, "\n")
    .replace(/<\/(p|div|li|h[1-6])>/gi, "\n")
    .replace(/<li[^>]*>/gi, "• ")
    .replace(/<[^>]+>/g, "")
    .replace(/&(#x[0-9a-f]+|#\d+|\w+);/gi, (m, e: string) =>
      e[0] === "#"
        ? String.fromCodePoint(e[1].toLowerCase() === "x" ? parseInt(e.slice(2), 16) : Number(e.slice(1)))
        : (ENTITIES[e.toLowerCase()] ?? m),
    )
    .split("\n")
    .map((line) => line.replace(/\s+/g, " ").trim())
    .filter(Boolean)
    .join("\n");
}

// Spotify lists images largest first (640, 300, 64); the ~300px one is plenty for a card.
function pickImage(images: SpotifyImage[]): string | null {
  const medium = images.find((i) => i.width !== null && i.width <= 320);
  return (medium ?? images[0])?.url ?? null;
}

const token = await getToken(requireEnv("SPOTIFY_CLIENT_ID"), requireEnv("SPOTIFY_CLIENT_SECRET"));
const raw = await getAllEpisodes(token);
if (raw.length === 0) throw new Error("Spotify returned no episodes, refusing to overwrite data/episodes.json");

const episodes: Episode[] = raw
  .map((e) => ({
    id: e.id,
    name: e.name.trim(),
    description: e.html_description ? htmlToText(e.html_description) : e.description.trim(),
    releaseDate: e.release_date,
    durationMs: e.duration_ms,
    image: pickImage(e.images),
  }))
  .sort((a, b) => b.releaseDate.localeCompare(a.releaseDate) || a.id.localeCompare(b.id));

await writeFile(OUTPUT, `${JSON.stringify({ episodes }, null, 2)}\n`, "utf8");
console.log(`Wrote ${episodes.length} episodes to data/episodes.json`);
