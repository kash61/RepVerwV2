using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ReparaturVerwaltung
{
    // Klasse zur Beschreibung eines Geräts
    public class Geraet
    {
        public int Id { get; set; } = 0; // Eindeutige ID
        public string Name { get; set; } = string.Empty; // Gerätebezeichnung
        public string Seriennummer { get; set; } = string.Empty; // Seriennummer
    }

    // Klasse zur Beschreibung eines Reparaturauftrags
    public class Reparaturauftrag
    {
        public int AuftragId { get; set; } // Eindeutige Auftragsnummer
        public Geraet Geraet { get; set; } = new Geraet(); // Zugeordnetes Gerät
        public string Fehlerbeschreibung { get; set; } = string.Empty; // Fehlerbeschreibung
        public string Status { get; set; } = "Offen"; // Aktueller Status
        public DateTime Erstellungsdatum { get; set; } = DateTime.Now; // Zeitstempel des Auftrags
    }

    class Program
    {
        // Listen zur Verwaltung aller Geräte und Aufträge
        static List<Geraet> geraeteListe = new List<Geraet>();
        static List<Reparaturauftrag> auftragsListe = new List<Reparaturauftrag>();

        // Zähler für IDs
        static int geraetCounter = 1;
        static int auftragCounter = 1;

        // Dateipfade
        static string geraetePfad = "geraete.txt";
        static string auftraegePfad = "auftraege.txt";

        // Mögliche Status
        static readonly Dictionary<int, string> statusMap = new()
        {
            { 1, "Offen" },
            { 2, "In Bearbeitung" },
            { 3, "Warten auf Ersatzteil" },
            { 4, "Abgeschlossen" }
        };

        static void Main(string[] args)
        {
            Console.Title = "Reparaturverwaltung Byte & Bean";
            DatenLaden();        // Bestehende Daten aus Dateien laden
            Hauptmenue();        // Hauptmenü anzeigen
            DatenSpeichern();    // Änderungen beim Beenden speichern
        }

        // Zeigt das Hauptmenü
        static void Hauptmenue()
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("===== Byte & Bean Reparaturverwaltung =====\n");
                Console.ResetColor();

                // Menüauswahl
                Console.WriteLine("1. Gerät erfassen");
                Console.WriteLine("2. Reparaturauftrag erstellen");
                Console.WriteLine("3. Auftragsstatus anzeigen");
                Console.WriteLine("4. Bericht abgeschlossener Reparaturen anzeigen");
                Console.WriteLine("5. Gerät löschen");
                Console.WriteLine("6. Auftrag löschen");
                Console.WriteLine("7. Beenden\n");
                Console.Write("Auswahl: ");

                switch (Console.ReadLine())
                {
                    case "1": GeraetErfassen(); break;
                    case "2": ReparaturauftragErstellen(); break;
                    case "3": StatusAnzeigen(); break;
                    case "4": BerichtErstellen(); break;
                    case "5": GeraetLoeschen(); break;
                    case "6": AuftragLoeschen(); break;
                    case "7": return;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Ungültige Eingabe. Drücken Sie eine Taste.");
                        Console.ResetColor();
                        Console.ReadKey();
                        break;
                }
            }
        }

        // Gerät erfassen
        static void GeraetErfassen()
        {
            do
            {
                Console.Clear();
                Console.WriteLine("=== Neues Gerät erfassen ===\n");
                Console.Write("Gerätename: ");
                string? name = Console.ReadLine();
                Console.Write("Seriennummer: ");
                string? sn = Console.ReadLine();

                geraeteListe.Add(new Geraet
                {
                    Id = geraetCounter++,
                    Name = name ?? string.Empty,
                    Seriennummer = sn ?? string.Empty
                });

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nGerät erfolgreich erfasst!");
                Console.ResetColor();
            } while (WiederholenOderZurueck());
        }

        // Auftrag erstellen
        static void ReparaturauftragErstellen()
        {
            do
            {
                Console.Clear();
                Console.WriteLine("=== Reparaturauftrag erstellen ===\n");

                if (!geraeteListe.Any())
                {
                    Console.WriteLine("Keine Geräte vorhanden.");
                    Console.ReadKey();
                    return;
                }

                foreach (var g in geraeteListe)
                    Console.WriteLine($"{g.Id}: {g.Name} (SN: {g.Seriennummer})");

                Console.Write("\nGeräte-ID: ");
                if (!int.TryParse(Console.ReadLine(), out int id) || !geraeteListe.Any(g => g.Id == id))
                {
                    Console.WriteLine("Ungültige Eingabe.");
                    Console.ReadKey();
                    return;
                }

                var geraet = geraeteListe.First(g => g.Id == id);

                Console.Write("Fehlerbeschreibung: ");
                string? fehler = Console.ReadLine();

                auftragsListe.Add(new Reparaturauftrag
                {
                    AuftragId = auftragCounter++,
                    Geraet = geraet,
                    Fehlerbeschreibung = fehler ?? string.Empty
                });

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nReparaturauftrag erstellt!");
                Console.ResetColor();
            } while (WiederholenOderZurueck());
        }

        // Auftragstatus anzeigen & ändern
        static void StatusAnzeigen()
        {
            do
            {
                Console.Clear();
                Console.WriteLine("=== Auftragsstatus ===\n");

                foreach (var a in auftragsListe)
                {
                    Console.ForegroundColor = a.Status switch
                    {
                        "Offen" => ConsoleColor.Red,
                        "In Bearbeitung" => ConsoleColor.Yellow,
                        "Warten auf Ersatzteil" => ConsoleColor.DarkYellow,
                        "Abgeschlossen" => ConsoleColor.Green,
                        _ => ConsoleColor.White
                    };

                    Console.WriteLine($"#{a.AuftragId} | Gerät: {a.Geraet.Name} | Fehler: {a.Fehlerbeschreibung} | Status: {a.Status}");
                    Console.ResetColor();
                }

                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("\nAuftrag bearbeiten? (j/n): ");
                Console.ResetColor();

                if (Console.ReadLine()?.ToLower() == "j")
                {
                    Console.Write("Auftrags-ID: ");
                    if (int.TryParse(Console.ReadLine(), out int id) && auftragsListe.Any(a => a.AuftragId == id))
                    {
                        var auftrag = auftragsListe.First(a => a.AuftragId == id);
                        Console.WriteLine("\nNeuen Status wählen:");

                        foreach (var s in statusMap)
                        {
                            Console.ForegroundColor = s.Key switch
                            {
                                1 => ConsoleColor.Red,
                                2 => ConsoleColor.Yellow,
                                3 => ConsoleColor.DarkYellow,
                                4 => ConsoleColor.Green,
                                _ => ConsoleColor.White
                            };
                            Console.WriteLine($"{s.Key}. {s.Value}");
                        }
                        Console.ResetColor();

                        Console.Write("Statusnummer: ");
                        if (int.TryParse(Console.ReadLine(), out int newStatus) && statusMap.ContainsKey(newStatus))
                        {
                            auftrag.Status = statusMap[newStatus];
                            Console.WriteLine("Status geändert!");
                        }
                    }
                }
            } while (WiederholenOderZurueck());
        }

        // Bericht abgeschlossener Aufträge
        static void BerichtErstellen()
        {
            Console.Clear();
            Console.WriteLine("=== Abgeschlossene Reparaturen ===\n");

            var abgeschlossene = auftragsListe.Where(a => a.Status == "Abgeschlossen").ToList();

            if (!abgeschlossene.Any())
            {
                Console.WriteLine("Keine abgeschlossenen Reparaturen.");
            }
            else
            {
                foreach (var a in abgeschlossene)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"#{a.AuftragId} | Gerät: {a.Geraet.Name} | Fehler: {a.Fehlerbeschreibung} | Fertig am: {a.Erstellungsdatum:dd.MM.yyyy HH:mm}");
                    Console.ResetColor();
                }
            }

            Console.WriteLine("\nDrücke eine Taste zum Zurückkehren...");
            Console.ReadKey();
        }

        // Gerät löschen
        static void GeraetLoeschen()
        {
            Console.Clear();
            Console.WriteLine("=== Gerät löschen ===\n");

            foreach (var g in geraeteListe)
                Console.WriteLine($"{g.Id}: {g.Name} (SN: {g.Seriennummer})");

            Console.Write("\nGeräte-ID zum Löschen: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var g = geraeteListe.FirstOrDefault(x => x.Id == id);
                if (g != null && !auftragsListe.Any(a => a.Geraet.Id == id))
                {
                    geraeteListe.Remove(g);
                    Console.WriteLine("Gerät gelöscht.");
                }
                else Console.WriteLine("Nicht löschbar (existiert nicht oder mit Auftrag verknüpft).");
            }
            Console.ReadKey();
        }

        // Auftrag löschen
        static void AuftragLoeschen()
        {
            Console.Clear();
            Console.WriteLine("=== Auftrag löschen ===\n");

            foreach (var a in auftragsListe)
                Console.WriteLine($"{a.AuftragId}: Gerät: {a.Geraet.Name}, Fehler: {a.Fehlerbeschreibung}");

            Console.Write("\nAuftrags-ID zum Löschen: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var a = auftragsListe.FirstOrDefault(x => x.AuftragId == id);
                if (a != null)
                {
                    auftragsListe.Remove(a);
                    Console.WriteLine("Auftrag gelöscht.");
                }
            }
            Console.ReadKey();
        }

        // Fragt, ob weitergemacht werden soll
        static bool WiederholenOderZurueck()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("\nWeitere Einträge hinzufügen? (j/n): ");
            Console.ResetColor();
            return Console.ReadLine()?.ToLower() == "j";
        }

        // Daten speichern
        static void DatenSpeichern()
        {
            using (var w = new StreamWriter(geraetePfad))
                foreach (var g in geraeteListe)
                    w.WriteLine($"{g.Id};{g.Name};{g.Seriennummer}");

            using (var w = new StreamWriter(auftraegePfad))
                foreach (var a in auftragsListe)
                    w.WriteLine($"{a.AuftragId};{a.Geraet.Id};{a.Fehlerbeschreibung};{a.Status};{a.Erstellungsdatum}");
        }

        // Daten laden
        static void DatenLaden()
        {
            if (File.Exists(geraetePfad))
                foreach (var line in File.ReadAllLines(geraetePfad))
                {
                    var p = line.Split(';');
                    geraeteListe.Add(new Geraet { Id = int.Parse(p[0]), Name = p[1], Seriennummer = p[2] });
                }

            if (File.Exists(auftraegePfad))
                foreach (var line in File.ReadAllLines(auftraegePfad))
                {
                    var p = line.Split(';');
                    var geraet = geraeteListe.FirstOrDefault(g => g.Id == int.Parse(p[1]));
                    if (geraet != null)
                    {
                        auftragsListe.Add(new Reparaturauftrag
                        {
                            AuftragId = int.Parse(p[0]),
                            Geraet = geraet,
                            Fehlerbeschreibung = p[2],
                            Status = p[3],
                            Erstellungsdatum = DateTime.Parse(p[4])
                        });
                    }
                }

            if (geraeteListe.Any()) geraetCounter = geraeteListe.Max(g => g.Id) + 1;
            if (auftragsListe.Any()) auftragCounter = auftragsListe.Max(a => a.AuftragId) + 1;
        }
    }
}
