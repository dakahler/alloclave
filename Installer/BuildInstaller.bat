@echo off

del /q /f build.log
msbuild ../Collector/Collector.sln /p:Configuration=Debug;Platform=x64 /t:Clean;Build > build.log
msbuild ../Collector/Collector.sln /p:Configuration=Release;Platform=x64 /t:Clean;Build > build.log
msbuild ../Alloclave.sln /p:Configuration=Installer;Platform=x64 /t:Clean;Build > build.log

rd /s /q working
mkdir working
xcopy ..\Visualizer\bin\x64\Installer\Alloclave.exe working /q /h /r /y /i
xcopy ..\Visualizer\bin\x64\Installer\*.dll working /q /h /r /y /i

mkdir working\plugins
mkdir working\plugins\transport
mkdir working\plugins\callstack
mkdir working\plugins\transport\win32
mkdir working\plugins\transport\win32\Properties
xcopy ..\Visualizer\plugins\transport\Win32Transport.dll working\plugins\transport /q /h /r /y /i
xcopy ..\Visualizer\plugins\transport\Win32Transport\*.cs working\plugins\transport\win32 /q /h /r /y /i
xcopy ..\Visualizer\plugins\transport\Win32Transport\*.csproj working\plugins\transport\win32 /q /h /r /y /i
xcopy ..\Visualizer\plugins\transport\Win32Transport\*.csproj working\plugins\transport\win32 /q /h /r /y /i
xcopy ..\Visualizer\plugins\transport\Win32Transport\*.sln working\plugins\transport\win32 /q /h /r /y /i
xcopy ..\Visualizer\plugins\transport\Win32Transport\Properties\* working\plugins\transport\win32\Properties /q /h /r /y /i

mkdir working\collector
mkdir working\collector\bin\debug
mkdir working\collector\bin\release
xcopy ..\Collector\*.h working\collector /q /h /r /y /i
xcopy ..\Collector\*.cpp working\collector /q /h /r /y /i
xcopy ..\Collector\*.makefile working\collector /q /h /r /y /i
xcopy ..\Collector\*.vcxproj working\collector /q /h /r /y /i
xcopy ..\Collector\*.filters working\collector /q /h /r /y /i
xcopy ..\Collector\*.sln working\collector /q /h /r /y /i
xcopy ..\Collector\Makefile working\collector /q /h /r /y /i
xcopy ..\Collector\Debug\Collector.lib working\collector\bin\debug /q /h /r /y /i
xcopy ..\Collector\Release\Collector.lib working\collector\bin\release /q /h /r /y /i

mkdir working\collector\test
xcopy ..\Collector\TestCollector\*.h working\collector\test /q /h /r /y /i
xcopy ..\Collector\TestCollector\*.cpp working\collector\test /q /h /r /y /i
xcopy ..\Collector\TestCollector\*.makefile working\collector\test /q /h /r /y /i
xcopy ..\Collector\TestCollector\*.vcxproj working\collector\test /q /h /r /y /i
xcopy ..\Collector\TestCollector\*.filters working\collector\test /q /h /r /y /i

: Doesn't currently work
:ilmerge /wildcards /zeropekind /log:merge.log /targetplatform:v4,"%ProgramFiles(x86)%\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5" /target:winexe /out:working\Alloclave_merged.exe working\Alloclave.exe working\*.dll

rummage -o -p -s -i -j ".\ObfuscationMap.xml" --no-evaluation working\Alloclave.exe working\Alloclave.exe

makensis /Oinstaller.log InstallerScript.nsi