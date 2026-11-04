import { firstReleaseDate, groups, seasons, stats, type Season } from "@/lib/data";
import { plural, sinceLabel, seasonAnchor, seasonCode } from "@/lib/format";
import { Icon } from "./icon";

// Seasons grouped into one column per year (oldest year left, newest season on top).
const years: { year: number; seasons: Season[] }[] = (() => {
  const map = new Map<number, Season[]>();
  for (const s of seasons) map.set(s.year, [...(map.get(s.year) ?? []), s]);
  return [...map.entries()].sort(([a], [b]) => a - b).map(([year, list]) => ({ year, seasons: list }));
})();

function ColumnLabel({ children }: { children: React.ReactNode }) {
  return <span className="text-[11px] font-semibold tracking-[1px] text-gray-500">{children}</span>;
}

function SeasonPill({ season }: { season: Season }) {
  const running = season.isRunning;
  return (
    <a
      href={`#${seasonAnchor(season.number)}`}
      className={`flex items-center justify-between gap-2 rounded-lg border px-3 py-2.5 transition hover:-translate-y-0.5 hover:shadow-sm ${
        running ? "border-card bg-card" : "border-gray-200 bg-white hover:border-gray-400"
      }`}
    >
      <span className={`text-[15px] font-bold ${running ? "text-on-card" : "text-heading"}`}>
        {seasonCode(season.number)}
      </span>
      <span className={`text-[11px] ${running ? "text-red-300" : "text-gray-500"}`}>
        {running
          ? `● ${season.episodes.length} / ${season.plannedEpisodes}`
          : plural(season.episodes.length, "díl", "díly", "dílů")}
      </span>
    </a>
  );
}

export function SeriesNavigator() {
  return (
    <nav aria-label="Série" className="flex flex-col gap-6 rounded-xl bg-gray-100 p-4 sm:p-6">
      <div className="flex flex-col gap-1">
        <h2 className="text-2xl font-semibold text-heading sm:text-[28px]">Všechny epizody podle sérií</h2>
        <p className="text-sm text-gray-500">
          {plural(stats.seasons, "série", "série", "sérií")} · {plural(stats.episodes, "díl", "díly", "dílů")} ·{" "}
          {plural(stats.groupEpisodes, "díl", "díly", "dílů")} mimo sérii · {sinceLabel(firstReleaseDate)}
        </p>
      </div>

      <div className="flex flex-col gap-6 lg:flex-row">
        <div className="flex flex-col gap-2 lg:w-[200px] lg:shrink-0">
          <ColumnLabel>MIMO SÉRII</ColumnLabel>
          <div className="grid grid-cols-[repeat(auto-fill,minmax(150px,1fr))] gap-2 lg:grid-cols-1">
            {groups.map((g) => (
              <a
                key={g.id}
                href={`#${g.id}`}
                className="flex items-center gap-2.5 rounded-lg border border-amber-300 bg-amber-50 px-3 py-2.5 transition hover:-translate-y-0.5 hover:shadow-sm"
              >
                <Icon name={g.chip.icon} className="size-[18px] shrink-0 text-bonus" />
                <span className="flex flex-col">
                  <span className="text-sm font-semibold text-heading">{g.chip.label}</span>
                  <span className="text-[11px] text-gray-500">{plural(g.episodes.length, "díl", "díly", "dílů")}</span>
                </span>
              </a>
            ))}
          </div>
        </div>

        <div className="hidden w-px shrink-0 bg-gray-200 lg:block" />

        <div className="grid flex-1 grid-cols-2 gap-x-3 gap-y-5 sm:grid-cols-3 lg:auto-cols-fr lg:grid-flow-col lg:grid-cols-none">
          {years.map(({ year, seasons: list }) => (
            <div key={year} className="flex flex-col gap-2">
              <ColumnLabel>{year}</ColumnLabel>
              {list.map((s) => (
                <SeasonPill key={s.number} season={s} />
              ))}
            </div>
          ))}
        </div>
      </div>
    </nav>
  );
}
