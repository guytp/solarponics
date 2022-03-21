@ECHO OFF
set installerShare=\\10.200.0.2\ProductionManagerInstaller

copy %installerShare%\runClient.bat %temp%\runClient.bat /V /Y
call %temp%\runClient.bat -updateSelf
exit 0