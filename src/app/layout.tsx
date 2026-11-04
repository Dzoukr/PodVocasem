import type { Metadata } from "next";
import { Inter } from "next/font/google";
import "./globals.css";

const inter = Inter({
  subsets: ["latin", "latin-ext"],
  variable: "--font-inter",
});

export const metadata: Metadata = {
  metadataBase: new URL("https://www.podvocasem.cz"),
  title: "PodVocasem",
  description: "IT podcast, který rozhodně neplave po povrchu. Zajímaví hosté v hodinovém pořadu vysílaném #PodVocasem.",
  openGraph: {
    title: "PodVocasem",
    description: "IT podcast, který rozhodně neplave po povrchu.",
    url: "https://www.podvocasem.cz",
    siteName: "PodVocasem",
    locale: "cs_CZ",
    type: "website",
    images: ["/img/logo.png"],
  },
  icons: {
    icon: "/img/favicon/favicon.ico",
    apple: "/img/favicon/apple-touch-icon.png",
  },
};

export default function RootLayout({ children }: Readonly<{ children: React.ReactNode }>) {
  return (
    <html lang="cs" className={inter.variable}>
      <body>{children}</body>
    </html>
  );
}
