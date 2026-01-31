@echo off
set KEY=HKLM\SYSTEM\CurrentControlSet\Policies\Microsoft\FeatureManagement\Overrides

reg add "%KEY%" /v 735209102 /t REG_DWORD /d 1 /f
reg add "%KEY%" /v 1853569164 /t REG_DWORD /d 1 /f
reg add "%KEY%" /v 156965516 /t REG_DWORD /d 1 /f

echo.
echo Done. Please REBOOT.
