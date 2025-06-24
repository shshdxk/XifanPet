@echo off
setlocal

REM Get the directory of the current batch file
set "BATDIR=%~dp0"
for %%I in ("%BATDIR:~0,-1%") do set "CURDIR=%%~nxI"

REM Source directory: Clock2\bin\Debug\net8.0-windows
set "SRC=%BATDIR%bin\Debug\net8.0-windows"
REM Destination directory: ..\XifanPet\bin\Debug\net8.0-windows\plugins\<current directory name>
set "DEST=%BATDIR%..\XifanPet\bin\Debug\net8.0-windows\plugins\%CURDIR%"

if not exist "%DEST%" (
    mkdir "%DEST%"
)

xcopy "%SRC%\*" "%DEST%\" /E /Y /I

echo Copy finished!
endlocal