# Feature Overrides — Native NVMe toggle (Windows)

Questo repository crea/rimuove tre valori **DWORD (32-bit)** nel registro:

`HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Policies\Microsoft\FeatureManagement\Overrides`

- `735209102` = `1`
- `1853569164` = `1`
- `156965516` = `1`

## Cosa fanno (in pratica)

Sono *feature overrides* che vengono comunemente usati per abilitare una modalità “Native NVMe” (driver/storage stack) introdotta per Windows Server 2025, e sperimentata anche su alcune build di Windows 11 (non ufficiale).

> Nota: su Windows 11 è un uso non supportato ufficialmente; procedi solo se sai cosa stai facendo e tieni pronto il rollback.

## Avvertenze
- Crea un **restore point** / backup prima.
- Possibili anomalie di compatibilità (alcuni tool potrebbero non vedere i drive, drive duplicati, ecc.).
- Riavvio richiesto dopo Enable/Disable.

## Modalità d’uso (Admin)

### One‑click (consigliato)
```powershell
powershell -ExecutionPolicy Bypass -File .\scripts\installer.ps1
```

### Script PowerShell
Enable:
```powershell
powershell -ExecutionPolicy Bypass -File .\scripts\set-overrides.ps1
```

Disable:
```powershell
powershell -ExecutionPolicy Bypass -File .\scripts\remove-overrides.ps1
```

### .NET tool
```powershell
FeatureOverridesSetter.exe status
FeatureOverridesSetter.exe enable
FeatureOverridesSetter.exe disable
```

## Build & Release
- CI: `.github/workflows/build.yml`
- Release su tag `v*`: `.github/workflows/release.yml`

## Licenza
MIT
