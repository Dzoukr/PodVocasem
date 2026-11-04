import { Archive } from "@/components/archive";
import { Hero } from "@/components/hero";
import { Partners } from "@/components/partners";
import { GroupSection, SeasonSection } from "@/components/sections";
import { SeriesNavigator } from "@/components/series-navigator";
import { SiteFooter } from "@/components/site-footer";
import { groups, seasons } from "@/lib/data";

// The current season is featured right under the partners; the previous one gets cards
// further down, and everything older collapses into the archive.
const [current, previous, ...older] = seasons;

export default function Home() {
  return (
    <>
      <Hero />
      <Partners />
      <main className="container-page flex flex-col gap-16 pt-4 pb-24 sm:gap-[72px]">
        <SeasonSection season={current} featured />
        <SeriesNavigator />
        {groups.map((g) => (
          <GroupSection key={g.id} group={g} />
        ))}
        {previous && <SeasonSection season={previous} />}
        <Archive seasons={older} />
      </main>
      <SiteFooter />
    </>
  );
}
