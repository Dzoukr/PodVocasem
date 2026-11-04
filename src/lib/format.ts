/** Czech plural: plural(1, "díl", "díly", "dílů") → "1 díl", 3 → "3 díly", 5 → "5 dílů" */
export function plural(n: number, one: string, few: string, many: string): string {
  const word = n === 1 ? one : n >= 2 && n <= 4 ? few : many;
  return `${n} ${word}`;
}

/** "https://www.csas.cz/" → "www.csas.cz" */
export function hostname(url: string): string {
  return new URL(url).hostname;
}

/** First paragraph-ish text for cards: line breaks become spaces (the card clamps it anyway). */
export function excerpt(description: string): string {
  return description.replace(/\s*\n\s*/g, " ");
}

const MONTHS_GENITIVE = [
  "ledna", "února", "března", "dubna", "května", "června",
  "července", "srpna", "září", "října", "listopadu", "prosince",
];

/** "2021-10-13" → "od října 2021" */
export function sinceLabel(isoDate: string): string {
  const [year, month] = isoDate.split("-").map(Number);
  return `od ${MONTHS_GENITIVE[month - 1]} ${year}`;
}

export const seasonCode = (n: number) => `S${String(n).padStart(2, "0")}`;
export const seasonAnchor = (n: number) => `s${String(n).padStart(2, "0")}`;
