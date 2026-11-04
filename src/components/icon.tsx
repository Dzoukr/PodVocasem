import { Mic, Newspaper, Sparkles, Star } from "lucide-react";

type IconComponent = React.ComponentType<{ className?: string; "aria-hidden"?: boolean }>;

// Names used in /data/*.json → Lucide components. Platform logos (Spotify, Apple, …) are
// brand SVGs in /public/svg, since Lucide no longer ships brand icons.
const registry: Record<string, IconComponent> = {
  mic: Mic,
  newspaper: Newspaper,
  sparkles: Sparkles,
  star: Star,
};

export function Icon({ name, className }: { name: string; className?: string }) {
  const Component = registry[name];
  if (!Component) throw new Error(`Unknown icon "${name}" (known: ${Object.keys(registry).join(", ")})`);
  return <Component className={className} aria-hidden />;
}
