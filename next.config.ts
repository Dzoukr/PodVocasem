import type { NextConfig } from "next";

const nextConfig: NextConfig = {
  // Strict static site generation: `next build` emits plain HTML/CSS/JS into `out/`.
  // Any server-only feature (route handlers, ISR, dynamic rendering) fails the build.
  output: "export",
  images: { unoptimized: true },
};

export default nextConfig;
