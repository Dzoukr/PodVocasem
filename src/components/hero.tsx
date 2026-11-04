import { Mail } from "lucide-react";
import { profile } from "@/lib/data";
import { PlatformButton } from "./platform";

export function Hero() {
  return (
    <header
      className="bg-cover bg-center"
      style={{
        backgroundImage:
          "linear-gradient(135deg, #adddebf2 0%, #92d3bfd9 100%), url(/img/vocas.jpg)",
      }}
    >
      <div className="container-page flex flex-col gap-7 pt-10 pb-12 sm:pt-16 sm:pb-20">
        <img src="/img/logo.png" alt="PodVocasem" className="h-32 w-auto self-start sm:h-[200px]" />

        <div className="flex flex-col gap-3">
          <h1 className="text-[44px] leading-[1.05] font-semibold tracking-[-1px] sm:text-6xl lg:text-[80px]">
            IT podcast, který rozhodně
            <br className="hidden sm:inline" /> neplave po povrchu.
          </h1>
          <p className="text-xl sm:text-2xl">
            {profile.subtitle}{" "}
            <a href={profile.platforms.herohero} className="text-link hover:underline">
              {profile.hashtag}
            </a>
          </p>
        </div>

        <div className="flex flex-col gap-2.5">
          <p>Poslouchej a sleduj nás na své oblíbené platformě:</p>
          <div className="flex flex-wrap gap-2.5">
            <PlatformButton platform="spotify" label="Spotify" />
            <PlatformButton platform="apple" label="Apple Podcasts" />
            <PlatformButton platform="youtube" label="YouTube" />
          </div>
        </div>

        <div className="flex flex-col items-start gap-2.5">
          <p>Podpoř naši tvorbu:</p>
          <a href={profile.platforms.herohero} target="_blank" rel="noopener">
            <img src="/img/herohero.png" alt="Herohero" className="h-[42px] w-32 rounded-md object-cover" />
          </a>
          <a href={profile.platforms.herohero} className="text-[13px] text-link hover:underline">
            herohero.co/podvocasem
          </a>
        </div>

        <div className="flex flex-col gap-2.5">
          <p>Napiš nám do studia:</p>
          {profile.hosts.map((h) => (
            <a key={h.email} href={`mailto:${h.email}`} className="group flex flex-wrap items-center gap-1.5 text-sm">
              <Mail className="size-3.5 text-heading" aria-hidden />
              {h.name} -<span className="text-link group-hover:underline">{h.email}</span>
            </a>
          ))}
        </div>
      </div>
    </header>
  );
}
