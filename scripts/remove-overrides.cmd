@echo off
set KEY=HKLM\SYSTEM\CurrentControlSet\Policies\Microsoft\FeatureManagement\Overrides

reg delete "%KEY%" /v 735209102 /f
reg delete "%KEY%" /v 1853569164 /f
reg delete "%KEY%" /v 156965516 /f

echo.
echo Done. Please REBOOT.
