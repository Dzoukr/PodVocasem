import { currentPartners, profile, type Group, type Season } from "@/lib/data";
import { plural, seasonAnchor, seasonCode } from "@/lib/format";
import { EpisodeCard, UpcomingCard } from "./episode-card";
import { Icon } from "./icon";
import { PlatformButton, platformIcons } from "./platform";

function SectionHeader({ children, right }: { children: React.ReactNode; right?: React.ReactNode }) {
  return (
    <div className="flex flex-col gap-4 border-b border-gray-200 pb-4 md:flex-row md:items-end md:justify-between">
      <div className="flex flex-col gap-2.5">{children}</div>
      {right}
    </div>
  );
}

function CardGrid({ children }: { children: React.ReactNode }) {
  return <div className="grid gap-6 md:grid-cols-2 lg:gap-8 xl:grid-cols-3">{children}</div>;
}

function ListenOn({ label, children }: { label: string; children: React.ReactNode }) {
  return (
    <div className="flex flex-col gap-2 md:items-end">
      <span className="text-[13px] text-gray-500">{label}</span>
      <div className="flex flex-wrap gap-2">{children}</div>
    </div>
  );
}

function StatusBadge({ running }: { running: boolean }) {
  return running ? (
    <span className="inline-flex items-center gap-1.5 rounded-full bg-red-100 px-2.5 py-1 text-[13px] font-semibold text-red-700">
      <span className="size-2 rounded-full bg-brand-red" />
      Právě vychází
    </span>
  ) : (
    <span className="inline-flex items-center gap-1.5 rounded-full bg-gray-100 px-2.5 py-1 text-[13px] font-semibold text-heading">
      <span className="size-2 rounded-full bg-card" />
      Kompletní
    </span>
  );
}

function ProgressBar({ season }: { season: Season }) {
  const done = season.isRunning ? "bg-brand-red" : "bg-card";
  return (
    <div className="flex gap-1" aria-hidden>
      {Array.from({ length: Math.max(season.plannedEpisodes, season.episodes.length) }, (_, i) => (
        <span key={i} className={`h-1.5 w-7 rounded-[3px] ${i < season.episodes.length ? done : "bg-gray-200"}`} />
      ))}
    </div>
  );
}

/** `featured` = the current season, shown right under the partners in a highlighted panel. */
export function SeasonSection({ season, featured = false }: { season: Season; featured?: boolean }) {
  const released = season.episodes.length;
  const meta = season.isRunning
    ? [
        `${released} ze ${season.plannedEpisodes} dílů venku`,
        currentPartners.length > 0 && `partneři: ${currentPartners.map((p) => p.name).join(", ")}`,
      ]
        .filter(Boolean)
        .join(" · ")
    : `${released} ze ${released} dílů · série je kompletní`;
  const newest = season.isRunning ? season.episodes.at(-1) : undefined;
  const upcoming = Array.from({ length: season.upcoming }, (_, i) => released + i + 1);

  return (
    <section
      id={seasonAnchor(season.number)}
      className={`flex scroll-mt-6 flex-col gap-6 ${
        featured ? "rounded-2xl bg-gray-50 p-4 shadow-[0_1px_2px_#0000000d] ring-1 ring-gray-200 sm:p-8" : ""
      }`}
    >
      <SectionHeader
        right={
          <ListenOn label="Celou sérii poslouchej na:">
            <PlatformButton platform="spotify" label="Spotify" bordered />
            <PlatformButton platform="apple" label="Apple Podcasts" bordered />
            {season.youtubePlaylist && (
              <PlatformButton platform="youtube" label="YouTube playlist" href={season.youtubePlaylist} bordered />
            )}
          </ListenOn>
        }
      >
        <div className="flex flex-wrap items-center gap-x-3.5 gap-y-2">
          <h2 className="text-3xl font-semibold text-heading sm:text-4xl">PodVocasem - {season.number}. série</h2>
          <StatusBadge running={season.isRunning} />
        </div>
        <div className="flex flex-wrap items-center gap-3">
          <ProgressBar season={season} />
          <span className="text-sm text-gray-500">{meta}</span>
        </div>
      </SectionHeader>
      <CardGrid>
        {season.episodes.map((e) => (
          <EpisodeCard key={e.spotifyId} episode={e} variant={e === newest ? "newest" : "regular"} />
        ))}
        {season.specials.map((e) => (
          <EpisodeCard key={e.spotifyId} episode={e} />
        ))}
        {upcoming.map((n) => (
          <UpcomingCard
            key={n}
            code={`${seasonCode(season.number)}E${String(n).padStart(2, "0")}`}
            description={
              n === season.plannedEpisodes
                ? `Závěrečný díl ${season.number}. série. Sleduj nás na Spotify nebo YouTube a nic ti neuteče.`
                : `Další díl ${season.number}. série je na cestě. Sleduj nás na Spotify nebo YouTube a nic ti neuteče.`
            }
          />
        ))}
      </CardGrid>
    </section>
  );
}

export function GroupSection({ group }: { group: Group }) {
  return (
    <section id={group.id} className="flex scroll-mt-6 flex-col gap-6">
      <SectionHeader
        right={
          group.herohero ? (
            <a
              href={profile.platforms.herohero}
              target="_blank"
              rel="noopener"
              className="inline-flex items-center gap-2 self-start rounded-lg bg-herohero px-4 py-2.5 text-sm font-semibold text-white transition hover:bg-violet-700 md:self-auto"
            >
              <img src={platformIcons.herohero.mono} alt="" className="size-[18px]" />
              Odemknout na Herohero
            </a>
          ) : (
            <ListenOn label="Poslouchej na:">
              <PlatformButton platform="spotify" label="Spotify" bordered />
              {group.youtubePlaylist && (
                <PlatformButton platform="youtube" label="YouTube playlist" href={group.youtubePlaylist} bordered />
              )}
            </ListenOn>
          )
        }
      >
        <h2 className="flex items-center gap-3 text-3xl font-semibold text-heading sm:text-4xl">
          <Icon name={group.chip.icon} className="size-7 shrink-0 text-bonus sm:size-8" />
          {group.title}
        </h2>
        <p className="text-sm text-gray-500">
          {plural(group.episodes.length, "díl", "díly", "dílů")} · {group.description}
        </p>
      </SectionHeader>
      <CardGrid>
        {group.episodes.map((e) => (
          <EpisodeCard key={e.spotifyId} episode={e} variant={group.herohero ? "bonus" : "regular"} />
        ))}
      </CardGrid>
    </section>
  );
}
