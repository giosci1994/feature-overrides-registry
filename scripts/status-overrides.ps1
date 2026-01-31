$path = "HKLM:\SYSTEM\CurrentControlSet\Policies\Microsoft\FeatureManagement\Overrides"
$names = @("735209102","1853569164","156965516")

if (-not (Test-Path $path)) {
  Write-Host "Chiave non presente: $path"
  exit 1
}

$allOk = $true
foreach ($n in $names) {
  try {
    $v = (Get-ItemProperty -Path $path -Name $n -ErrorAction Stop).$n
    Write-Host "$n = $v"
    if ($v -ne 1) { $allOk = $false }
  } catch {
    Write-Host "$n = (missing)"
    $allOk = $false
  }
}
if ($allOk) {
  Write-Host "STATUS: ENABLED"
  exit 0
} else {
  Write-Host "STATUS: NOT fully enabled"
  exit 2
}
