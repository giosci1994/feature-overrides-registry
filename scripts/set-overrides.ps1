#requires -RunAsAdministrator
$ErrorActionPreference = "Stop"

$path = "HKLM:\SYSTEM\CurrentControlSet\Policies\Microsoft\FeatureManagement\Overrides"
$names = @("735209102","1853569164","156965516")

if (-not (Test-Path $path)) {
  New-Item -Path $path -Force | Out-Null
}

foreach ($n in $names) {
  New-ItemProperty -Path $path -Name $n -PropertyType DWord -Value 1 -Force | Out-Null
}

Write-Host "OK: impostate 3 DWORD a 1 in $path"
Write-Host "Riavvia Windows per applicare il cambio."
