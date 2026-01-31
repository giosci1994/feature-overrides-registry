using System;
using Microsoft.Win32;

namespace FeatureOverridesSetter
{
    internal static class Program
    {
        private const string RegPath = @"SYSTEM\CurrentControlSet\Policies\Microsoft\FeatureManagement\Overrides";
        private static readonly string[] Keys =
        {
            "735209102",
            "1853569164",
            "156965516"
        };

        private static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                PrintHelp();
                return 1;
            }

            var cmd = args[0].Trim().ToLowerInvariant();

            try
            {
                return cmd switch
                {
                    "enable" => Enable(),
                    "disable" => Disable(),
                    "status" => Status(),
                    "help" or "-h" or "--help" => HelpOk(),
                    _ => Unknown(cmd)
                };
            }
            catch (UnauthorizedAccessException)
            {
                Console.Error.WriteLine("ERROR: permessi insufficienti. Esegui come Amministratore.");
                return 5;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"ERROR: {ex.GetType().Name}: {ex.Message}");
                return 10;
            }
        }

        private static int HelpOk()
        {
            PrintHelp();
            return 0;
        }

        private static int Unknown(string cmd)
        {
            Console.Error.WriteLine($"Comando non valido: {cmd}");
            PrintHelp();
            return 2;
        }

        private static void PrintHelp()
        {
            Console.WriteLine("FeatureOverridesSetter");
            Console.WriteLine("Uso:");
            Console.WriteLine("  FeatureOverridesSetter.exe enable   # imposta le 3 DWORD a 1");
            Console.WriteLine("  FeatureOverridesSetter.exe disable  # rimuove le 3 DWORD");
            Console.WriteLine("  FeatureOverridesSetter.exe status   # mostra lo stato attuale");
        }

        private static RegistryKey OpenBaseKeyWritable()
        {
            // HKLM richiede admin
            var baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            return baseKey.CreateSubKey(RegPath, writable: true)
                   ?? throw new Exception("Impossibile creare/aprire la chiave di registro target.");
        }

        private static RegistryKey OpenBaseKeyReadable()
        {
            var baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            return baseKey.OpenSubKey(RegPath, writable: false)
                   ?? throw new Exception("Chiave non trovata (non esiste ancora).");
        }

        private static int Enable()
        {
            using var key = OpenBaseKeyWritable();
            foreach (var name in Keys)
            {
                key.SetValue(name, 1, RegistryValueKind.DWord);
            }

            Console.WriteLine("OK: override abilitati (DWORD=1).");
            return 0;
        }

        private static int Disable()
        {
            using var key = OpenBaseKeyWritable();
            foreach (var name in Keys)
            {
                // DeleteValue con throwOnMissing = false
                try { key.DeleteValue(name, throwOnMissingValue: false); } catch { /* ignore */ }
            }

            Console.WriteLine("OK: override rimossi.");
            return 0;
        }

        private static int Status()
        {
            using var key = OpenBaseKeyReadable();

            Console.WriteLine($@"HKLM\{RegPath}");
            foreach (var name in Keys)
            {
                var val = key.GetValue(name);
                if (val is int i)
                    Console.WriteLine($"  {name} = {i}");
                else if (val is null)
                    Console.WriteLine($"  {name} = (missing)");
                else
                    Console.WriteLine($"  {name} = {val} ({val.GetType().Name})");
            }

            return 0;
        }
    }
}
