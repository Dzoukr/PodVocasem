import { ChevronDown, CirclePlay } from "lucide-react";
import { profile, type Season } from "@/lib/data";
import { plural, seasonAnchor } from "@/lib/format";
import { PlatformIconButton, platformIcons } from "./platform";

// Collapsible rows use native <details>, so the archive needs no client-side JavaScript.
function SeriesRow({ season }: { season: Season }) {
  const episodes = [...season.episodes, ...season.specials];
  const badge = [
    plural(season.episodes.length, "díl", "díly", "dílů"),
    season.specials.length > 0 && plural(season.specials.length, "speciál", "speciály", "speciálů"),
    "kompletní",
  ]
    .filter(Boolean)
    .join(" · ");

  return (
    <details id={seasonAnchor(season.number)} className="group scroll-mt-6">
      <summary className="flex cursor-pointer list-none flex-wrap items-center gap-x-6 gap-y-4 rounded-lg border border-gray-200 bg-white px-4 py-4 sm:px-6 sm:py-5 group-open:rounded-b-none">
        <div className="flex size-16 shrink-0 flex-col items-center justify-center rounded-lg bg-card">
          <span className="text-[9px] font-semibold tracking-[1px] text-gray-400">SÉRIE</span>
          <span className="text-2xl font-bold text-on-card">{String(season.number).padStart(2, "0")}</span>
        </div>
        <div className="flex min-w-0 flex-1 basis-64 flex-col gap-1">
          <div className="flex flex-wrap items-center gap-2.5">
            <h3 className="text-xl font-semibold text-heading">PodVocasem - {season.number}. série</h3>
            <span className="rounded-full bg-gray-100 px-2 py-0.5 text-xs text-gray-500">{badge}</span>
          </div>
          <p className="text-sm text-gray-500">{season.guests.join(", ")}</p>
        </div>
        <div className="flex items-center gap-2">
          <span className="text-[13px] text-gray-500">Celá série:</span>
          <PlatformIconButton platform="spotify" href={profile.platforms.spotify} label="Spotify" />
          <PlatformIconButton platform="apple" href={profile.platforms.apple} label="Apple Podcasts" />
          {season.youtubePlaylist && (
            <PlatformIconButton platform="youtube" href={season.youtubePlaylist} label="YouTube playlist" />
          )}
        </div>
        <span className="inline-flex items-center gap-1.5 rounded-lg border border-heading px-3.5 py-2.5 text-sm font-medium text-heading group-open:bg-card group-open:text-on-card">
          <span className="group-open:hidden">Zobrazit epizody</span>
          <span className="hidden group-open:inline">Skrýt epizody</span>
          <ChevronDown className="size-4 transition group-open:rotate-180" aria-hidden />
        </span>
      </summary>
      <ul className="rounded-b-lg border border-t-0 border-gray-200 bg-gray-50 px-4 py-1 sm:px-6">
        {episodes.map((e) => (
          <li key={e.spotifyId} className="border-b border-gray-200 last:border-b-0">
            <a
              href={e.spotifyUrl}
              target="_blank"
              rel="noopener"
              className="group/row flex items-center gap-4 py-3 text-[15px] text-heading"
            >
              <CirclePlay className="size-5 shrink-0" aria-hidden />
              <span className="w-16 shrink-0 text-[13px] font-semibold text-gray-500">{e.code}</span>
              <span className="flex-1 group-hover/row:underline">{e.title}</span>
              <img src={platformIcons.spotify.color} alt="Spotify" className="size-[18px] shrink-0" />
            </a>
          </li>
        ))}
      </ul>
    </details>
  );
}

export function Archive({ seasons }: { seasons: Season[] }) {
  if (seasons.length === 0) return null;
  const episodes = seasons.reduce((n, s) => n + s.episodes.length, 0);
  const specials = seasons.reduce((n, s) => n + s.specials.length, 0);
  const range = `Série ${seasons.at(-1)!.number} – ${seasons[0].number}`;
  const count = [plural(episodes, "díl", "díly", "dílů"), specials > 0 && plural(specials, "speciál", "speciály", "speciálů")]
    .filter(Boolean)
    .join(" + ");

  return (
    <section className="flex flex-col gap-3">
      <div className="flex flex-col gap-1.5 border-b border-gray-200 pb-4">
        <h2 className="text-3xl font-semibold text-heading sm:text-4xl">Starší série</h2>
        <p className="text-sm text-gray-500">
          {range} · {count} · všechny díly dostupné na Spotify a Apple Podcasts
        </p>
      </div>
      {seasons.map((s) => (
        <SeriesRow key={s.number} season={s} />
      ))}
    </section>
  );
}
