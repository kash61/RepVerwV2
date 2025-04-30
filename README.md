# Reparaturverwaltung – Byte & Bean GmbH

Schulprojekt der Otto-Bennemann-Schule - ITK 231

Ein einfaches Konsolenprogramm zur Verwaltung von Reparaturaufträgen für Kaffeemaschinen und ähnliche Geräte. Es ermöglicht die Erfassung, Bearbeitung, Speicherung und Auswertung von Reparaturvorgängen.

## 📦 Funktionen

- Geräte erfassen (z. B. Kaffeemaschinen, Mühlen)
- Reparaturaufträge anlegen und verwalten
- Statusänderung (z. B. Offen → In Bearbeitung → Abgeschlossen)
- Übersicht über alle Aufträge mit farblicher Statusanzeige
- Bericht über abgeschlossene Reparaturen (inkl. Datum + Uhrzeit)
- Speichern aller Daten in Textdateien (`geraete.txt`, `auftraege.txt`)
- Löschen von Geräten und Aufträgen
- Alle Eingaben erfolgen über ein simples Konsolenmenü

---

## ▶️ Nutzung

### Voraussetzungen
- .NET 6 oder höher (z. B. Visual Studio oder `dotnet` CLI)
- Projekt enthält zwei Textdateien im Projektverzeichnis:
  - `geraete.txt` – Liste aller Geräte
  - `auftraege.txt` – Liste aller Aufträge

### Starten

1. Projekt öffnen und kompilieren (z. B. mit Visual Studio)
2. Anwendung ausführen:
   ```bash
   dotnet run
