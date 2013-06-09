@echo off

del /q /f build.log
msbuild ../Collector/Collector.sln /p:Configuration=Debug;Platform=Win32 /t:Clean;Build > build.log
msbuild ../Collector/Collector.sln /p:Configuration=Release;Platform=Win32 /t:Clean;Build > build.log
msbuild ../Collector/Collector.sln /p:Configuration=Debug;Platform=x64 /t:Clean;Build > build.log
msbuild ../Collector/Collector.sln /p:Configuration=Release;Platform=x64 /t:Clean;Build > build.log
msbuild ../Alloclave.sln /p:Configuration=Installer;Platform=x64 /t:Clean;Build > build.log

rd /s /q working
mkdir working
xcopy ..\Visualizer\bin\x64\Installer\Alloclave.exe working /q /h /r /y /i
xcopy ..\Visualizer\bin\x64\Installer\*.dll working /q /h /r /y /i

mkdir working\plugins
mkdir working\plugins\transport
mkdir working\plugins\symbollookup
mkdir working\plugins\transport\win32transport
mkdir working\plugins\transport\win32transport\Properties
xcopy ..\Visualizer\plugins\transport\Win32Transport.dll working\plugins\transport /q /h /r /y /i
xcopy ..\Visualizer\plugins\transport\Win32Transport\*.cs working\plugins\transport\win32transport /q /h /r /y /i
xcopy ..\Visualizer\plugins\transport\Win32Transport\_Win32Transport.csproj working\plugins\transport\win32transport /q /h /r /y /i
xcopy ..\Visualizer\plugins\transport\Win32Transport\*.sln working\plugins\transport\win32transport /q /h /r /y /i
xcopy ..\Visualizer\plugins\transport\Win32Transport\Properties\* working\plugins\transport\win32transport\Properties /q /h /r /y /i
xcopy ..\Visualizer\plugins\transport\Win32Transport\bin\x64\Installer\ConstantsBridge.dll working\plugins\transport\win32transport /q /h /r /y /i
xcopy ..\Visualizer\plugins\symbollookup\SymbolLookup_PDB.dll working\plugins\symbollookup /q /h /r /y /i
xcopy AlloclaveLicense.txt working /q /h /r /y /i

mkdir working\licenses
xcopy ..\Licenses\* working\licenses\ /q /h /r /y /i /e

mkdir working\collector
mkdir working\collector\bin\x86\debug
mkdir working\collector\bin\x86\release
mkdir working\collector\bin\x64\debug
mkdir working\collector\bin\x64\release
xcopy ..\Collector\*.h working\collector /q /h /r /y /i
xcopy ..\Collector\*.cpp working\collector /q /h /r /y /i
xcopy ..\Collector\*.makefile working\collector /q /h /r /y /i
xcopy ..\Collector\*.vcproj working\collector /q /h /r /y /i
xcopy ..\Collector\*.vcxproj working\collector /q /h /r /y /i
xcopy ..\Collector\*.filters working\collector /q /h /r /y /i
xcopy ..\Collector\*.sln working\collector /q /h /r /y /i
xcopy ..\Collector\license.txt working\collector /q /h /r /y /i
xcopy ..\Collector\Makefile working\collector /q /h /r /y /i
xcopy ..\Collector\Debug\AlloclaveCollector.lib working\collector\bin\x86\debug /q /h /r /y /i
xcopy ..\Collector\Release\AlloclaveCollector.lib working\collector\bin\x86\release /q /h /r /y /i
xcopy ..\Collector\Release\TestAlloclaveCollector.exe working\collector\bin\x86\release /q /h /r /y /i
xcopy ..\Collector\Release\TestAlloclaveCollector.pdb working\collector\bin\x86\release /q /h /r /y /i
xcopy ..\Collector\x64\Debug\AlloclaveCollector.lib working\collector\bin\x64\debug /q /h /r /y /i
xcopy ..\Collector\x64\Release\AlloclaveCollector.lib working\collector\bin\x64\release /q /h /r /y /i

mkdir working\collector\testcollector
xcopy ..\Collector\TestCollector\*.h working\collector\testcollector /q /h /r /y /i
xcopy ..\Collector\TestCollector\*.cpp working\collector\testcollector /q /h /r /y /i
xcopy ..\Collector\TestCollector\*.makefile working\collector\testcollector /q /h /r /y /i
xcopy ..\Collector\TestCollector\*.vcproj working\collector\testcollector /q /h /r /y /i
xcopy ..\Collector\TestCollector\*.vcxproj working\collector\testcollector /q /h /r /y /i
xcopy ..\Collector\TestCollector\*.filters working\collector\testcollector /q /h /r /y /i

: Doesn't currently work
:ilmerge /wildcards /zeropekind /log:merge.log /targetplatform:v4,"%ProgramFiles(x86)%\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5" /target:winexe /out:working\Alloclave_merged.exe working\Alloclave.exe working\*.dll

:rummage -o -p -s -i -j ".\ObfuscationMap.xml" --no-evaluation working\Alloclave.exe working\Alloclave.exe

makensis /Oinstaller.log InstallerScript.nsi

kSignCMD /d "Alloclave Installer" /du http://www.alloclave.com /f %~dp0\certificate.pfx /p aaabbb %~dp0\InstallAlloclave.exe