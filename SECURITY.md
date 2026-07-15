# Security Policy

## Supported versions

Security fixes target the **latest release** and the current `main` branch.
Older tags are not guaranteed to receive updates.

| Version           | Supported |
|-------------------|-----------|
| `latest` / `main` | ✅ |
| older tags        | ❌ |

## Reporting a vulnerability

Please report security issues **privately** — do **not** open a public issue,
pull request or discussion that reveals the details.

Preferred channels:

1. **GitHub Security Advisories** — use *Security → Report a vulnerability* on
   this repository to open a private advisory.
2. Otherwise, contact the maintainer privately on GitHub
   ([@giosci1994](https://github.com/giosci1994)).

Please include a description and impact, steps to reproduce (a proof of concept
if possible), and the affected version/commit plus your environment
(Windows build, x64). We practice **responsible disclosure**: please give us a
chance to release a fix before making details public.

---

## ⚠️ Safety notes (this is a registry-modifying tool)

This project changes values under
`HKLM\SYSTEM\CurrentControlSet\Policies\Microsoft\FeatureManagement\Overrides`.
"Native NVMe" is officially supported **only on Windows Server 2025**; on
Windows 11 it is **unofficial / experimental**.

Before enabling the overrides:

- create a **System Restore point**;
- note the current state of your disk drivers in **Device Manager**.

If you notice problems (duplicate/unrecognized drives, tools that "lose" the
NVMe, etc.): run the **Disable** action and reboot.

### Safe Mode (advanced)

Some users report that, after the storage-stack change, booting into Safe Mode
may require the `"Storage Disks"` class-id under `SafeBoot\Network` and
`SafeBoot\Minimal`. This repo offers an optional `fix-safeboot` command, but it
is **not an official modification** — use it only if you understand why you need
it.

> Only run scripts/binaries from an official [release](../../releases) and verify
> them against the published `SHA256SUMS.txt` (see the README).
