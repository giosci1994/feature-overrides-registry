# Feature Overrides — Native NVMe toggle (Windows)

Questo repository crea/rimuove tre valori **DWORD (32-bit)** nel registro:

`HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Policies\Microsoft\FeatureManagement\Overrides`

- `735209102` = `1`
- `1853569164` = `1`
- `156965516` = `1`

Queste chiavi vengono comunemente usate come *feature overrides* per abilitare una modalità “Native NVMe” (driver/storage stack) introdotta e **supportata ufficialmente su Windows Server 2025** (con update di ottobre o successivi), e sperimentata anche su alcune build di Windows 11 (non supportata ufficialmente). citeturn1view2

## Cosa fanno (in pratica)

In molte installazioni Windows client, gli SSD NVMe possono apparire/operare attraverso uno stack “SCSI/Storport” (ad esempio tramite `disk.sys`) invece di una gestione NVMe “più diretta”. Questi override forzano l’attivazione di una funzionalità nascosta che può far comparire i dischi NVMe sotto **“Storage disks”** e usare un percorso/driver NVMe dedicato, con potenziali miglioramenti in alcune metriche (IOPS, letture/scritture in certe dimensioni di blocco) — ma con **caveat di compatibilità**. citeturn1view1turn0search7turn1view2

### Avvertenze importanti
- **Backup/restore point consigliato** prima di modificare il registro.
- È stato riportato che su alcuni sistemi possono verificarsi anomalie (drive duplicati, drive non riconosciuti, strumenti di gestione che “perdono” l’NVMe, ecc.). citeturn1view1
- Microsoft ha indicato chiaramente che **“Native NVMe” è supportato solo su Windows Server 2025** (con l’update di ottobre o successivi). Su Windows 11 si tratta di un uso non ufficiale/sperimentale. citeturn1view2

## Modalità d’uso

> Tutto richiede **privilegi di amministratore** (HKLM).

### 1) One‑click (consigliato)
- `scripts/installer.ps1` — menu interattivo (Enable / Disable / Status / (opzionale) Fix Safe Mode)

Esegui da PowerShell **come Admin**:

```powershell
powershell -ExecutionPolicy Bypass -File .\scripts\installer.ps1
```

### 2) Script PowerShell (non interattivo)
Abilita:
```powershell
powershell -ExecutionPolicy Bypass -File .\scripts\set-overrides.ps1
```

Disabilita:
```powershell
powershell -ExecutionPolicy Bypass -File .\scripts\remove-overrides.ps1
```

### 3) File .reg
- `reg/feature-overrides-enable.reg`
- `reg/feature-overrides-disable.reg`

### 4) Tool .NET (exe)
Compila o scarica l’artefatto della release (GitHub Actions).
Esempi:

```powershell
FeatureOverridesSetter.exe status
FeatureOverridesSetter.exe enable
FeatureOverridesSetter.exe disable
```

## Build & Release (GitHub Actions)

- **CI build** su push/PR: `.github/workflows/build.yml`
- **Release automatica** su tag `v*` (es. `v1.0.0`): `.github/workflows/release.yml`  
  Produce:
  - ZIP dei sorgenti “packaged”
  - `.exe` self-contained per Windows x64

## Licenza
MIT — vedi `LICENSE`.
