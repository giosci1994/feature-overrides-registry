<#
One-click installer / toggler.
- Self-elevates if not running as admin.
- Enable / Disable / Status.
- Optional: "Fix Safe Mode" adds a Storage Disks class id under SafeBoot.
  WARNING: This is community-reported behavior; use only if you know why you need it.
#>

param(
  [ValidateSet("menu","enable","disable","status","fix-safeboot")]
  [string]$Action = "menu"
)

function Test-Admin {
  $id = [Security.Principal.WindowsIdentity]::GetCurrent()
  $p  = New-Object Security.Principal.WindowsPrincipal($id)
  return $p.IsInRole([Security.Principal.WindowsBuiltinRole]::Administrator)
}

function Relaunch-AsAdmin {
  param([string]$Args)
  $psi = New-Object System.Diagnostics.ProcessStartInfo
  $psi.FileName = "powershell.exe"
  $psi.Arguments = $Args
  $psi.Verb = "runas"
  try {
    [System.Diagnostics.Process]::Start($psi) | Out-Null
  } catch {
    Write-Host "UAC rifiutato. Uscita."
  }
  exit
}

$script = $MyInvocation.MyCommand.Path
if (-not (Test-Admin)) {
  $args = "-ExecutionPolicy Bypass -File `"$script`" -Action $Action"
  Relaunch-AsAdmin -Args $args
}

$here = Split-Path -Parent $script

function Enable-Overrides {
  & (Join-Path $here "set-overrides.ps1")
}
function Disable-Overrides {
  & (Join-Path $here "remove-overrides.ps1")
}
function Status-Overrides {
  powershell -ExecutionPolicy Bypass -File (Join-Path $here "status-overrides.ps1")
}
function Fix-SafeBoot {
  # Community-reported: ensure Storage Disks class id exists under SafeBoot to avoid issues when the NVMe stack changes.
  $clsid = "{75416E63-5912-4DFA-AE8F-3EFACCAFFB14}"
  $paths = @(
    "HKLM:\SYSTEM\CurrentControlSet\Control\SafeBoot\Network\$clsid",
    "HKLM:\SYSTEM\CurrentControlSet\Control\SafeBoot\Minimal\$clsid"
  )
  foreach ($p in $paths) {
    if (-not (Test-Path $p)) { New-Item -Path $p -Force | Out-Null }
    New-ItemProperty -Path $p -Name "(Default)" -PropertyType String -Value "Storage Disks" -Force | Out-Null
  }
  Write-Host "OK: aggiunte chiavi SafeBoot per Storage Disks (se mancanti)."
  Write-Host "Riavvia e TESTA Safe Mode manualmente."
}

switch ($Action) {
  "enable" { Enable-Overrides; exit }
  "disable" { Disable-Overrides; exit }
  "status" { Status-Overrides; exit }
  "fix-safeboot" { Fix-SafeBoot; exit }
  default { }
}

Write-Host ""
Write-Host "=== Feature Overrides - Native NVMe toggle ==="
Write-Host "1) Enable (set DWORD=1)"
Write-Host "2) Disable (remove DWORD)"
Write-Host "3) Status"
Write-Host "4) Fix Safe Mode (optional / advanced)"
Write-Host "0) Exit"
Write-Host ""
$choice = Read-Host "Seleziona"

switch ($choice) {
  "1" { Enable-Overrides }
  "2" { Disable-Overrides }
  "3" { Status-Overrides }
  "4" { Fix-SafeBoot }
  default { Write-Host "Bye." }
}
