import { profile } from "@/lib/data";
import { platformIcons } from "./platform";

const socials = [
  { href: profile.platforms.spotify, icon: platformIcons.spotify.mono, label: "Spotify", size: "size-8" },
  { href: profile.platforms.apple, icon: platformIcons.apple.mono, label: "Apple Podcasts", size: "size-8" },
  { href: profile.platforms.youtube, icon: platformIcons.youtube.mono, label: "YouTube", size: "size-8" },
  { href: profile.platforms.herohero, icon: platformIcons.herohero.mono, label: "Herohero", size: "size-8" },
  { href: profile.platforms.bluesky, icon: "/svg/bluesky.svg", label: "Bluesky", size: "size-6 invert" },
];

export function SiteFooter() {
  // "#PodVocasem" in the about text links to Herohero, like in the hero.
  const [before, after] = profile.about.split(profile.hashtag);
  return (
    <footer>
      <section className="bg-gray-100">
        <div className="container-page flex flex-col items-center gap-8 py-12 md:flex-row">
          <img
            src="/img/mcs.jpg"
            alt="Džoukr a Poli ve studiu"
            loading="lazy"
            className="aspect-[8/5] w-full object-cover md:w-[480px] md:shrink-0"
          />
          <p className="leading-[1.6]">
            {before}
            <a href={profile.platforms.herohero} className="text-link hover:underline">
              {profile.hashtag}
            </a>
            {after}
          </p>
        </div>
      </section>
      <section className="bg-gray-900">
        <div className="container-page flex flex-col gap-8 py-12 md:flex-row md:items-center md:justify-between">
          <div className="flex flex-col gap-1">
            <p className="text-sm font-semibold text-gray-200">{profile.company.name}</p>
            {[profile.company.address, `IČO: ${profile.company.ico}`, profile.company.registry].map((line) => (
              <p key={line} className="text-[13px] leading-[1.6] text-gray-400">
                {line}
              </p>
            ))}
          </div>
          <div className="flex items-center gap-4">
            {socials.map((s) => (
              <a
                key={s.label}
                href={s.href}
                target="_blank"
                rel="noopener"
                aria-label={s.label}
                title={s.label}
                className="opacity-90 transition hover:opacity-100"
              >
                <img src={s.icon} alt="" className={s.size} />
              </a>
            ))}
          </div>
        </div>
      </section>
    </footer>
  );
}
