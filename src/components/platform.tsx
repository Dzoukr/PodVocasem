import { profile } from "@/lib/data";

export type PlatformId = "spotify" | "apple" | "youtube" | "herohero";

export const platformIcons: Record<PlatformId, { color: string; mono: string }> = {
  spotify: { color: "/svg/spotify.svg", mono: "/svg/spotify-bw.svg" },
  apple: { color: "/svg/apple-podcast.svg", mono: "/svg/apple-podcast-bw.svg" },
  youtube: { color: "/svg/youtube.svg", mono: "/svg/youtube-bw.svg" },
  herohero: { color: "/svg/herohero-bw.svg", mono: "/svg/herohero-bw.svg" },
};

/** White pill with a colour logo ("Platform Button" in the design). */
export function PlatformButton({
  platform,
  label,
  href = profile.platforms[platform],
  bordered = false,
}: {
  platform: PlatformId;
  label: string;
  href?: string;
  bordered?: boolean;
}) {
  return (
    <a
      href={href}
      target="_blank"
      rel="noopener"
      className={`inline-flex items-center gap-2 rounded-md bg-white py-1.5 pr-3 pl-2 text-[13px] text-gray-900 shadow-[0_1px_3px_#0000001f] transition hover:shadow-md ${
        bordered ? "border border-gray-200" : ""
      }`}
    >
      <img src={platformIcons[platform].color} alt="" className="size-[22px]" />
      {label}
    </a>
  );
}

/** Square 36px icon-only button (series rows). */
export function PlatformIconButton({ platform, href, label }: { platform: PlatformId; href: string; label: string }) {
  return (
    <a
      href={href}
      target="_blank"
      rel="noopener"
      aria-label={label}
      title={label}
      className="flex size-9 items-center justify-center rounded-md border border-gray-200 bg-white transition hover:border-gray-400"
    >
      <img src={platformIcons[platform].color} alt="" className="size-5" />
    </a>
  );
}
