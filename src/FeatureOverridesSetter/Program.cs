using Microsoft.Win32;
using System.Security.Principal;

static bool IsAdministrator()
{
    using var identity = WindowsIdentity.GetCurrent();
    var principal = new WindowsPrincipal(identity);
    return principal.IsInRole(WindowsBuiltInRole.Administrator);
}

static int EnsureAdmin()
{
    if (!IsAdministrator())
    {
        Console.Error.WriteLine("ERROR: avvia come Amministratore (HKLM richiede privilegi elevati).");
        return 1;
    }
    return 0;
}

const string RegPath = @"SYSTEM\CurrentControlSet\Policies\Microsoft\FeatureManagement\Overrides";
static readonly string[] Names = { "735209102", "1853569164", "156965516" };

static void Enable()
{
    using var baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
    using var key = baseKey.CreateSubKey(RegPath, writable: true) ?? throw new Exception("Impossibile creare/aprire la chiave registro.");
    foreach (var name in Names)
        key.SetValue(name, 1, RegistryValueKind.DWord);

    Console.WriteLine("OK: impostate 3 DWORD a 1.");
    Console.WriteLine("Riavvia Windows per applicare il cambio.");
}

static void Disable()
{
    using var baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
    using var key = baseKey.OpenSubKey(RegPath, writable: true);
    if (key is null)
    {
        Console.WriteLine("Chiave non presente: niente da fare.");
        return;
    }

    foreach (var name in Names)
        key.DeleteValue(name, throwOnMissingValue: false);

    Console.WriteLine("OK: rimosse 3 DWORD (se esistevano).");
    Console.WriteLine("Riavvia Windows per applicare il cambio.");
}

static int Status()
{
    using var baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
    using var key = baseKey.OpenSubKey(RegPath, writable: false);
    if (key is null)
    {
        Console.WriteLine("STATUS: chiave Overrides non presente.");
        return 2;
    }

    var ok = true;
    foreach (var name in Names)
    {
        var v = key.GetValue(name);
        if (v is int i)
        {
            Console.WriteLine($"{name} = {i}");
            if (i != 1) ok = false;
        }
        else
        {
            Console.WriteLine($"{name} = (missing)");
            ok = false;
        }
    }

    Console.WriteLine(ok ? "STATUS: ENABLED" : "STATUS: NOT fully enabled");
    return ok ? 0 : 3;
}

static void FixSafeBoot()
{
    // Community reported: Safe Mode may lack "Storage Disks" class ID entries.
    const string clsid = "{75416E63-5912-4DFA-AE8F-3EFACCAFFB14}";
    string[] paths = {
        $@"SYSTEM\CurrentControlSet\Control\SafeBoot\Network\{clsid}",
        $@"SYSTEM\CurrentControlSet\Control\SafeBoot\Minimal\{clsid}",
    };

    using var baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
    foreach (var p in paths)
    {
        using var key = baseKey.CreateSubKey(p, writable: true) ?? throw new Exception("Impossibile creare chiave SafeBoot.");
        key.SetValue("", "Storage Disks", RegistryValueKind.String);
    }

    Console.WriteLine("OK: aggiunte chiavi SafeBoot per Storage Disks (se mancanti).");
    Console.WriteLine("Riavvia e TESTA Safe Mode manualmente.");
}

static void PrintHelp()
{
    Console.WriteLine("FeatureOverridesSetter");
    Console.WriteLine("Usage:");
    Console.WriteLine("  FeatureOverridesSetter.exe enable");
    Console.WriteLine("  FeatureOverridesSetter.exe disable");
    Console.WriteLine("  FeatureOverridesSetter.exe status");
    Console.WriteLine("  FeatureOverridesSetter.exe fix-safeboot   (advanced)");
}

if (args.Length == 0)
{
    PrintHelp();
    return 0;
}

var cmd = args[0].Trim().ToLowerInvariant();
switch (cmd)
{
    case "enable":
        if (EnsureAdmin() != 0) return 1;
        Enable();
        return 0;
    case "disable":
        if (EnsureAdmin() != 0) return 1;
        Disable();
        return 0;
    case "status":
        return Status();
    case "fix-safeboot":
        if (EnsureAdmin() != 0) return 1;
        FixSafeBoot();
        return 0;
    case "-h":
    case "--help":
    case "help":
        PrintHelp();
        return 0;
    default:
        Console.Error.WriteLine($"Comando sconosciuto: {cmd}");
        PrintHelp();
        return 2;
}
