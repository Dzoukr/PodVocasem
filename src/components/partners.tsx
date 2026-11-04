import { ArrowUpRight, Handshake } from "lucide-react";
import { currentPartners, currentSeason } from "@/lib/data";
import { hostname } from "@/lib/format";

/** Sponsors of the current season; hidden when sponsorship.json doesn't match it. */
export function Partners() {
  if (currentPartners.length === 0) return null;
  const single = currentPartners.length === 1;
  return (
    // The soft teal wash continues the hero gradient, so the partners read as part of the intro.
    <section className="bg-linear-to-b from-[#92d3bf33] to-white">
      <div className="container-page flex flex-col items-center gap-8 py-14 sm:py-20">
        <div className="flex flex-col items-center gap-3 text-center">
          <span className="inline-flex items-center gap-2 rounded-full bg-white px-3 py-1 text-xs font-semibold tracking-[1px] text-brand-red shadow-[0_1px_3px_#0000001f]">
            <Handshake className="size-4" aria-hidden />
            {single ? "PARTNER" : "PARTNEŘI"} {currentSeason.number}. SÉRIE
          </span>
          <h2 className="text-3xl font-semibold text-heading sm:text-4xl">
            {single ? "Tuhle sérii podporuje" : "Tuhle sérii podporují"}
          </h2>
          <p className="max-w-xl text-gray-500">
            Děkujeme {single ? "našemu partnerovi" : "našim partnerům"} za neúnavnou podporu.
          </p>
        </div>
        <div className="flex w-full flex-col items-stretch justify-center gap-6 sm:flex-row">
          {currentPartners.map((p) => (
            <a
              key={p.url}
              href={p.url}
              target="_blank"
              rel="noopener"
              className="group flex flex-col items-center gap-5 rounded-2xl border border-gray-200 bg-white px-10 pt-10 pb-6 shadow-[0_10px_30px_#0000000f] transition hover:-translate-y-1 hover:shadow-[0_16px_40px_#0000001a] sm:w-[360px]"
            >
              <img src={p.logo} alt={p.name} className="h-[120px] w-full max-w-[260px] object-contain" />
              <span className="inline-flex items-center gap-1 text-sm text-heading group-hover:underline">
                {hostname(p.url)}
                <ArrowUpRight className="size-4 text-gray-400 transition group-hover:text-heading" aria-hidden />
              </span>
            </a>
          ))}
        </div>
      </div>
    </section>
  );
}
