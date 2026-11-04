import { Clock, Lock, Play } from "lucide-react";
import type { Episode } from "@/lib/data";
import { excerpt } from "@/lib/format";
import { platformIcons } from "./platform";

export type CardVariant = "regular" | "newest" | "bonus";

const codeStyles: Record<CardVariant, string> = {
  regular: "bg-card-2",
  newest: "bg-brand-red",
  bonus: "bg-bonus/20",
};

function AvailabilityChip({ href, icon, label }: { href: string; icon: string; label: string }) {
  return (
    <a
      href={href}
      target="_blank"
      rel="noopener"
      className="inline-flex items-center gap-1.5 rounded-full bg-white/8 py-1 pr-2 pl-1 text-xs text-on-card transition hover:bg-white/15"
    >
      <img src={icon} alt="" className="size-4" />
      {label}
    </a>
  );
}

export function EpisodeCard({ episode, variant = "regular" }: { episode: Episode; variant?: CardVariant }) {
  const code = (episode.code ?? "Speciál").toUpperCase();
  return (
    <article className="flex flex-col overflow-hidden rounded-lg bg-card shadow-[0_10px_15px_#0000002a]">
      <div className="flex flex-1 gap-4 p-4">
        <div className="flex w-28 shrink-0 flex-col gap-2 sm:w-[140px]">
          <img
            src={episode.image ?? "/img/default.png"}
            alt=""
            loading="lazy"
            className="aspect-square w-full rounded-lg bg-gray-50 object-cover"
          />
          <a
            href={episode.spotifyUrl}
            target="_blank"
            rel="noopener"
            className="flex items-center justify-center gap-1.5 rounded-lg border border-on-card px-2 py-2.5 text-center text-[13px] text-on-card transition hover:bg-on-card hover:text-card"
          >
            {variant === "bonus" ? (
              <>
                <Lock className="size-3.5 shrink-0" aria-hidden /> Přehrát bonus
              </>
            ) : (
              <>
                <Play className="size-3.5 shrink-0 fill-current" aria-hidden /> Přehrát epizodu
              </>
            )}
          </a>
        </div>
        <div className="flex min-w-0 flex-1 flex-col items-start gap-2">
          <span
            className={`rounded px-1.5 py-0.5 text-[11px] font-semibold tracking-[0.5px] text-on-card ${codeStyles[variant]}`}
          >
            {variant === "newest" ? `${code} · NOVINKA` : code}
          </span>
          <h3 className="text-[15px] leading-[1.35] font-bold text-on-card">{episode.title}</h3>
          <p className="line-clamp-5 text-[13px] leading-normal [overflow-wrap:anywhere] text-on-card-muted">{excerpt(episode.description)}</p>
        </div>
      </div>
      <div className="flex flex-wrap items-center gap-2 border-t border-card-2 bg-card-bar px-4 py-2.5">
        <span className="text-xs text-gray-400">{variant === "bonus" ? "Pro podporovatele" : "Dostupné na"}</span>
        <AvailabilityChip href={episode.spotifyUrl} icon={platformIcons.spotify.color} label="Spotify" />
        {episode.heroheroUrl && (
          <AvailabilityChip
            href={episode.heroheroUrl}
            icon={platformIcons.herohero.mono}
            label={variant === "bonus" ? "Herohero" : "Celý díl na Herohero"}
          />
        )}
      </div>
    </article>
  );
}

export function UpcomingCard({ code, description }: { code: string; description: string }) {
  return (
    <article className="flex gap-4 rounded-lg border border-gray-300 bg-gray-50 p-4">
      <div className="flex aspect-square w-28 shrink-0 items-center self-start justify-center rounded-lg bg-gray-200 sm:w-[140px]">
        <Clock className="size-8 text-gray-400" aria-hidden />
      </div>
      <div className="flex flex-col items-start gap-2">
        <span className="rounded bg-gray-200 px-1.5 py-0.5 text-[11px] font-semibold tracking-[0.5px] text-gray-500">
          {code}
        </span>
        <h3 className="text-[15px] font-bold text-heading">Připravujeme</h3>
        <p className="text-[13px] leading-normal text-gray-500">{description}</p>
      </div>
    </article>
  );
}
