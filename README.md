# podvocasem.cz

Website of the PodVocasem IT podcast. Next.js with strict static export (`output: "export"`), Tailwind CSS 4, deployed to Azure Static Web Apps.

```sh
corepack enable
yarn install
yarn dev              # http://localhost:3000
yarn build            # static site in ./out
yarn fetch-episodes   # refresh data/episodes.json from Spotify
```

## Content

All content lives in `data/*.json` and is compiled into the HTML at build time. It is validated during the build (`src/lib/data.ts`), so invalid data fails `yarn build`.

| File | What |
|---|---|
| `data/episodes.json` | **Generated** by `yarn fetch-episodes` from the Spotify API. Don't edit by hand. |
| `data/seasons.json` | `defaultPlannedEpisodes` (drives the "x ze 6" progress and "Připravujeme" cards), `heroheroFromSeason` (episodes from this season on get the "Celý díl na Herohero" link) and optional per-season `youtubePlaylist`. |
| `data/groups.json` | Episodes outside seasons, matched by title `prefix` (e.g. `Changelog #3`). One group can be the `fallback` for titles that match nothing. `herohero: true` marks supporter content. |
| `data/sponsorship.json` | Partners of the current season. Shown only while `season` equals the latest season, so a stale file simply disappears from the site. |
| `data/profile.json` | Hosts, platform links, texts and company info. |

### How episode titles are read

| Title | Result |
|---|---|
| `S16E05 - Host - Topic` | Season 16, episode 5 |
| `S07 - Bloopers…` | Special attached to season 7 |
| `<group prefix> #N - …` | Episode of that group |
| anything else | The fallback group ("Speciály") |

## Spotify fetch

`yarn fetch-episodes` needs `SPOTIFY_CLIENT_ID` and `SPOTIFY_CLIENT_SECRET`, locally in `.env.local` (not committed):

```
SPOTIFY_CLIENT_ID=…
SPOTIFY_CLIENT_SECRET=…
```

The output is sorted and has no timestamps, so the file only changes when Spotify does.

## CI

`.github/workflows/Web.yml` runs daily (06:00 UTC), on push to `master` and manually. One run fetches episodes, commits `data/episodes.json` if it changed, then type-checks, builds and deploys `out/`. On a code push a failed Spotify fetch only warns and the committed JSON is deployed; scheduled and manual runs fail instead. `public/staticwebapp.config.json` makes Azure serve the exported `404.html`. Secrets: `SPOTIFY_CLIENT_ID`, `SPOTIFY_CLIENT_SECRET`, `AZURE_STATIC_WEB_APPS_TOKEN`.

Design source: `podvocasem.cz.pen` (pen.dev).
