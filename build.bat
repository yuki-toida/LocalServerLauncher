@setlocal
@echo off

set PATH=%PATH%;%PROGRAMFILES(x86)%\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin\
set OutputPath=%USERPROFILE%\Desktop\LocalServerLauncher
rd /s /q %OutputPath%

for /f %%s in ('dir /b *.sln') do set Solution=%%s

set MSBuild=msbuild %Solution% /m /t:Rebuild /p:OutputPath=%OutputPath% /p:TransformOnBuild=true /p:TransformOutOfDateOnly=false /p:PrecompileBeforePublish=true
echo %MSBuild%
%MSBuild%

@endlocal
pause