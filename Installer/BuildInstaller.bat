@echo off

del /q /f build.log
msbuild ../Collector/Collector.sln /p:Configuration=Debug;Platform=Win32 /t:Clean;Build > build.log
msbuild ../Collector/Collector.sln /p:Configuration=Release;Platform=Win32 /t:Clean;Build > build.log
msbuild ../Collector/Collector.sln /p:Configuration=Debug;Platform=x64 /t:Clean;Build > build.log
msbuild ../Collector/Collector.sln /p:Configuration=Release;Platform=x64 /t:Clean;Build > build.log
msbuild ../Alloclave.sln /p:Configuration=Installer;Platform=x64 /t:Clean;Build > build.log
