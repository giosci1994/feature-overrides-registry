# 💽 Feature Overrides — Native NVMe toggle (Windows)

**Enable or disable the hidden Windows *Feature Management* overrides that unlock
experimental "Native NVMe" support — via one-click script, `.reg` files or a small
.NET tool, with a clean rollback and SHA256-verified releases.**

**🇬🇧 English** · [🇮🇹 Italiano](#-italiano)

<p align="center">
  <a href="https://github.com/giosci1994/feature-overrides-registry/actions/workflows/build.yml"><img src="https://github.com/giosci1994/feature-overrides-registry/actions/workflows/build.yml/badge.svg" alt="Build" /></a>
  <a href="https://github.com/giosci1994/feature-overrides-registry/releases/latest"><img src="https://img.shields.io/github/v/release/giosci1994/feature-overrides-registry?display_name=tag" alt="Release" /></a>
  <img src="https://img.shields.io/github/downloads/giosci1994/feature-overrides-registry/total" alt="Downloads" />
  <img src="https://img.shields.io/badge/platform-Windows-0078D6?logo=windows&logoColor=white" alt="Windows" />
  <a href="LICENSE"><img src="https://img.shields.io/github/license/giosci1994/feature-overrides-registry" alt="License" /></a>
</p>

> ⚠️ **Unofficial / experimental on Windows 11.** These overrides modify the
> Windows Registry under `HKLM`. Microsoft officially supports "Native NVMe" **only
> on Windows Server 2025** (October 2025 update or later). On Windows 11 this is an
> unofficial, experimental use. **Create a restore point / backup first.**

---

## 🔧 What it does

The tool creates (or removes) three **DWORD (32-bit)** values in the registry:

```
HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Policies\Microsoft\FeatureManagement\Overrides
```

| Value name | Data |
|------------|------|
| `735209102` | `1` |
| `1853569164` | `1` |
| `156965516` | `1` |

These keys are commonly used as *feature overrides* to enable a **"Native NVMe"**
mode in the driver/storage stack. On many Windows client installs, NVMe SSDs are
surfaced through a SCSI/Storport path (e.g. via `disk.sys`) rather than a more
direct NVMe path. Turning on this hidden feature can make NVMe disks appear under
**"Storage disks"** and use a dedicated NVMe driver path, with potential gains in
some metrics (IOPS, reads/writes at certain block sizes) — but with **compatibility
caveats**.

### ⚠️ Important warnings

- **Back up / create a restore point** before touching the registry.
- On some systems anomalies have been reported (duplicate drives, unrecognized
  drives, management tools that "lose" the NVMe, etc.).
- "Native NVMe" is officially supported **only on Windows Server 2025** (October
  update or later). On Windows 11 it is unofficial / experimental.

---

## ✅ Requirements

- **Windows** (x64) with **administrator privileges** (all changes are under `HKLM`).
- For the `.NET` tool: nothing to install — release binaries are self-contained. To
  build from source you need the **.NET 8 SDK**.

---

## 🚀 Usage

> Everything requires **administrator privileges** (HKLM).

### 1) One-click (recommended)

`scripts/installer.ps1` — interactive menu (Enable / Disable / Status / optional Fix
Safe Mode). Run from an **elevated** PowerShell:

```powershell
powershell -ExecutionPolicy Bypass -File .\scripts\installer.ps1
```

### 2) PowerShell scripts (non-interactive)

```powershell
# Enable
powershell -ExecutionPolicy Bypass -File .\scripts\set-overrides.ps1
# Disable
powershell -ExecutionPolicy Bypass -File .\scripts\remove-overrides.ps1
# Status
powershell -ExecutionPolicy Bypass -File .\scripts\status-overrides.ps1
```

### 3) `.reg` files

- `reg/feature-overrides-enable.reg`
- `reg/feature-overrides-disable.reg`

### 4) .NET tool (exe)

Download the release artifact (or build it), then:

```powershell
FeatureOverridesSetter.exe status
FeatureOverridesSetter.exe enable
FeatureOverridesSetter.exe disable
```

---

## 🏗️ Build & Release (GitHub Actions)

- **CI build** on push to `main` / PR → [`.github/workflows/build.yml`](.github/workflows/build.yml)
- **Automated release** on tag `v*` (e.g. `v1.0.0`) → [`.github/workflows/release.yml`](.github/workflows/release.yml)

Each release publishes:

- `FeatureOverridesSetter.exe` — self-contained Windows x64 executable
- `FeatureOverridesSetter-win-x64-<tag>.zip` — zipped binaries
- `feature-overrides-registry-<tag>.zip` — packaged sources
- `SHA256SUMS.txt` — checksums of every asset
- `VERIFY.ps1` — automatic verification script

```powershell
git tag v1.0.0 && git push origin v1.0.0   # the Action builds & publishes
```

---

## 🔐 Integrity verification (SHA256)

Every official release includes `SHA256SUMS.txt` and `VERIFY.ps1`, so you can
confirm the downloaded files have not been altered, corrupted or tampered with.

1. From the release, download `FeatureOverridesSetter.exe`, `SHA256SUMS.txt` and
   `VERIFY.ps1`.
2. Put all files **in the same folder** (e.g. `Downloads\FeatureOverrides`).
3. Open PowerShell in that folder and run:

   ```powershell
   powershell -ExecutionPolicy Bypass -File .\VERIFY.ps1
   ```

---

## 📄 License

[MIT](LICENSE) © Giovanni La Cascia

<br>

---
---

<a id="-italiano"></a>

# 🇮🇹 Italiano

**Abilita o disabilita gli *override* nascosti di *Feature Management* di Windows
che sbloccano il supporto sperimentale "Native NVMe" — tramite script one-click,
file `.reg` o un piccolo tool .NET, con rollback pulito e release verificate SHA256.**

> ⚠️ **Non ufficiale / sperimentale su Windows 11.** Questi override modificano il
> registro di Windows sotto `HKLM`. Microsoft supporta ufficialmente "Native NVMe"
> **solo su Windows Server 2025** (update di ottobre 2025 o successivi). Su Windows
> 11 è un uso non ufficiale e sperimentale. **Crea prima un punto di ripristino.**

## 🔧 Cosa fa

Crea (o rimuove) tre valori **DWORD (32-bit)** nel registro:

```
HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Policies\Microsoft\FeatureManagement\Overrides
```

| Nome valore | Dato |
|-------------|------|
| `735209102` | `1` |
| `1853569164` | `1` |
| `156965516` | `1` |

Queste chiavi sono comunemente usate come *feature overrides* per abilitare una
modalità **"Native NVMe"** nello stack driver/storage. In molte installazioni
Windows client gli SSD NVMe vengono gestiti attraverso un percorso SCSI/Storport
(es. tramite `disk.sys`) invece di una gestione NVMe più diretta. Attivare questa
funzionalità nascosta può far comparire i dischi NVMe sotto **"Storage disks"** e
usare un percorso/driver NVMe dedicato, con potenziali miglioramenti in alcune
metriche (IOPS, letture/scritture a certe dimensioni di blocco) — ma con **caveat
di compatibilità**.

### ⚠️ Avvertenze importanti

- **Backup / punto di ripristino** prima di modificare il registro.
- Su alcuni sistemi sono state riportate anomalie (drive duplicati, drive non
  riconosciuti, strumenti di gestione che "perdono" l'NVMe, ecc.).
- "Native NVMe" è supportato ufficialmente **solo su Windows Server 2025** (update
  di ottobre o successivi). Su Windows 11 è uso non ufficiale/sperimentale.

## ✅ Requisiti

- **Windows** (x64) con **privilegi di amministratore** (tutte le modifiche sono in `HKLM`).
- Per il tool `.NET`: nulla da installare, i binari della release sono
  self-contained. Per compilare dai sorgenti serve il **.NET 8 SDK**.

## 🚀 Modalità d'uso

> Tutto richiede **privilegi di amministratore** (HKLM).

### 1) One-click (consigliato)

`scripts/installer.ps1` — menu interattivo (Enable / Disable / Status / opzionale
Fix Safe Mode). Esegui da PowerShell **come Admin**:

```powershell
powershell -ExecutionPolicy Bypass -File .\scripts\installer.ps1
```

### 2) Script PowerShell (non interattivi)

```powershell
# Abilita
powershell -ExecutionPolicy Bypass -File .\scripts\set-overrides.ps1
# Disabilita
powershell -ExecutionPolicy Bypass -File .\scripts\remove-overrides.ps1
# Stato
powershell -ExecutionPolicy Bypass -File .\scripts\status-overrides.ps1
```

### 3) File `.reg`

- `reg/feature-overrides-enable.reg`
- `reg/feature-overrides-disable.reg`

### 4) Tool .NET (exe)

Scarica l'artefatto della release (oppure compilalo), poi:

```powershell
FeatureOverridesSetter.exe status
FeatureOverridesSetter.exe enable
FeatureOverridesSetter.exe disable
```

## 🏗️ Build & Release (GitHub Actions)

- **Build CI** su push su `main` / PR → [`.github/workflows/build.yml`](.github/workflows/build.yml)
- **Release automatica** su tag `v*` (es. `v1.0.0`) → [`.github/workflows/release.yml`](.github/workflows/release.yml)

Ogni release pubblica: l'eseguibile `FeatureOverridesSetter.exe` (self-contained
Windows x64), lo ZIP dei binari, lo ZIP dei sorgenti, `SHA256SUMS.txt` e `VERIFY.ps1`.

## 🔐 Verifica integrità (SHA256)

Ogni release ufficiale include `SHA256SUMS.txt` e `VERIFY.ps1`: puoi verificare che
i file scaricati non siano stati modificati, corrotti o manomessi. Metti
`FeatureOverridesSetter.exe`, `SHA256SUMS.txt` e `VERIFY.ps1` nella stessa cartella,
apri PowerShell lì ed esegui:

```powershell
powershell -ExecutionPolicy Bypass -File .\VERIFY.ps1
```

## 📄 Licenza

[MIT](LICENSE) © Giovanni La Cascia
