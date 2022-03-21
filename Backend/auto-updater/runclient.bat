@ECHO OFF
set installerShare=\\10.200.0.2\ProductionManagerInstaller
set installDir="%AppData%\Solarponics\ProductionManager"
set ProductionManagerExe=Solarponics.ProductionManager.exe
set canOverrideUpdate=false

if [%1] == [-updateSelf] (
	copy %installerShare%\runClient.bat %temp%\runClient.bat /V /Y
	call %temp%\runClient.bat
	exit 0
) else (
	echo Running Solarponics Production Manager
)

:: Check local / remote versions
dir %installerShare% >NUL
if %ERRORLEVEL% NEQ 0 (
	echo Unable to reach %installerShare%. Cannot update
	start /D%installDir% %ProductionManagerExe% runFromBatFile
	pause
	exit /b 0
) else (
	echo Found %installerShare%. Checking for updates.
)

:: Is this the first install?
if not EXIST %installDir%\version.txt (
	echo Solarponics Production Manager not found. Installing.
	set firstInstall=true
	goto :updateClient
) else (
	echo Solarponics Production Manager found. Comparing versions.
	set firstInstall=false
)

:: If local version is out-of-date, run an update
fc /B %installDir%\version.txt %installerShare%\version.txt >NUL
set outOfDate=%ERRORLEVEL%

if [%outOfDate%] NEQ [0] (
	goto :updateClient
) else (
	echo Solarponics Production Manager is up to date, starting local version
	start /D%installDir% %ProductionManagerExe%
	exit /b 0
)

echo An unknown error has occurred. 
exit /b 1

:updateClient
	:: Prompt user to confirm they want a new version
	echo Local copy of Solarponics Production Manager is out-of-date.
	if [%canOverrideUpdate%] == [true] (
		set /p getLatest=Do you want to get the latest version? [Y/N] 
	) else (
		set getLatest=Y
	)
	if /i [%getLatest%] == [Y]  (
		echo Updating ...
		xcopy %installerShare%\* %installDir% /E /V /I /H /R /K /Y /Z
	) else (
		echo Not updating.
	)
	echo Latest version updated, starting
	start /D%installDir% %ProductionManagerExe%
	exit /b 0
