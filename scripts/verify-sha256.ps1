<#
VERIFY SHA256
- Verifica i file nella cartella corrente usando SHA256SUMS.txt
- Pensato per controllare gli asset scaricati dalla Release

Uso:
  # nella cartella dove hai scaricato assets + SHA256SUMS.txt
  powershell -ExecutionPolicy Bypass -File .\VERIFY.ps1

Oppure:
  powershell -ExecutionPolicy Bypass -File .\verify-sha256.ps1 -SumsPath .\SHA256SUMS.txt
#>

param(
  [string]$SumsPath = ".\SHA256SUMS.txt"
)

$ErrorActionPreference = "Stop"

if (-not (Test-Path $SumsPath)) {
  throw "File non trovato: $SumsPath. Metti SHA256SUMS.txt nella stessa cartella e riprova."
}

$lines = Get-Content $SumsPath | Where-Object { $_.Trim() -ne "" }

if ($lines.Count -eq 0) {
  throw "SHA256SUMS.txt è vuoto."
}

Write-Host "Leggo somme da: $SumsPath"
$ok = 0
$bad = 0
$missing = 0

foreach ($line in $lines) {
  # formato: <hash>  <filename>
  if ($line -notmatch '^(?<hash>[a-f0-9]{64})\s{2}(?<file>.+)$') {
    Write-Warning "Riga non valida (saltata): $line"
    continue
  }

  $expected = $Matches["hash"]
  $file = $Matches["file"].Trim()

  if (-not (Test-Path $file)) {
    Write-Host "MISSING: $file"
    $missing++
    continue
  }

  $actual = (Get-FileHash -Algorithm SHA256 $file).Hash.ToLower()

  if ($actual -eq $expected) {
    Write-Host "OK:      $file"
    $ok++
  } else {
    Write-Host "FAILED:  $file"
    Write-Host "  expected: $expected"
    Write-Host "  actual:   $actual"
    $bad++
  }
}

Write-Host ""
Write-Host "Risultato:"
Write-Host "  OK:      $ok"
Write-Host "  MISSING: $missing"
Write-Host "  FAILED:  $bad"

if ($bad -gt 0 -or $missing -gt 0) {
  exit 2
}

exit 0
