#requires -RunAsAdministrator
$ErrorActionPreference = "Stop"

$path = "HKLM:\SYSTEM\CurrentControlSet\Policies\Microsoft\FeatureManagement\Overrides"
$names = @("735209102","1853569164","156965516")

foreach ($n in $names) {
  Remove-ItemProperty -Path $path -Name $n -ErrorAction SilentlyContinue
}

Write-Host "OK: rimosse 3 DWORD da $path (se esistevano)"
Write-Host "Riavvia Windows per applicare il cambio."
