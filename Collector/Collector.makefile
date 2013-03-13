# Compiler flags...
CPP_COMPILER = g++
C_COMPILER = gcc

# Include paths...
Installer_Include_Path=
Analysis_Include_Path=
Debug_Collector_Include_Path=
Debug_Include_Path=
Release_Include_Path=
Release_Collector_Include_Path=

# Library paths...
Installer_Library_Path=
Analysis_Library_Path=
Debug_Collector_Library_Path=
Debug_Library_Path=
Release_Library_Path=
Release_Collector_Library_Path=

# Additional libraries...
Installer_Libraries=
Analysis_Libraries=
Debug_Collector_Libraries=
Debug_Libraries=
Release_Libraries=
Release_Collector_Libraries=

# Preprocessor definitions...
Installer_Preprocessor_Definitions=-D GCC_BUILD 
Analysis_Preprocessor_Definitions=-D GCC_BUILD 
Debug_Collector_Preprocessor_Definitions=-D GCC_BUILD 
Debug_Preprocessor_Definitions=-D GCC_BUILD 
Release_Preprocessor_Definitions=-D GCC_BUILD 
Release_Collector_Preprocessor_Definitions=-D GCC_BUILD 

# Implictly linked object files...
Installer_Implicitly_Linked_Objects=
Analysis_Implicitly_Linked_Objects=
Debug_Collector_Implicitly_Linked_Objects=
Debug_Implicitly_Linked_Objects=
Release_Implicitly_Linked_Objects=
Release_Collector_Implicitly_Linked_Objects=

# Compiler flags...
Installer_Compiler_Flags=-O2 -g 
Analysis_Compiler_Flags=-O0 -g 
Debug_Collector_Compiler_Flags=-O0 -g 
Debug_Compiler_Flags=-O0 -g 
Release_Compiler_Flags=-O2 -g 
Release_Collector_Compiler_Flags=-O2 -g 

# Builds all configurations for this project...
.PHONY: build_all_configurations
build_all_configurations: Installer Analysis Debug_Collector Debug Release Release_Collector 

# Builds the Installer configuration...
.PHONY: Installer
Installer: create_folders gccInstaller/Allocation.o gccInstaller/Buffer.o gccInstaller/MemoryOverrides.o gccInstaller/Packet.o gccInstaller/Queue.o gccInstaller/Registration.o gccInstaller/Screenshot.o gccInstaller/Transport.o gccInstaller/Win32Transport.o 
	ar rcs gccInstaller/libCollector.a gccInstaller/Allocation.o gccInstaller/Buffer.o gccInstaller/MemoryOverrides.o gccInstaller/Packet.o gccInstaller/Queue.o gccInstaller/Registration.o gccInstaller/Screenshot.o gccInstaller/Transport.o gccInstaller/Win32Transport.o  $(Installer_Implicitly_Linked_Objects)

# Compiles file Allocation.cpp for the Installer configuration...
-include gccInstaller/Allocation.d
gccInstaller/Allocation.o: Allocation.cpp
	$(CPP_COMPILER) $(Installer_Preprocessor_Definitions) $(Installer_Compiler_Flags) -c Allocation.cpp $(Installer_Include_Path) -o gccInstaller/Allocation.o
	$(CPP_COMPILER) $(Installer_Preprocessor_Definitions) $(Installer_Compiler_Flags) -MM Allocation.cpp $(Installer_Include_Path) > gccInstaller/Allocation.d

# Compiles file Buffer.cpp for the Installer configuration...
-include gccInstaller/Buffer.d
gccInstaller/Buffer.o: Buffer.cpp
	$(CPP_COMPILER) $(Installer_Preprocessor_Definitions) $(Installer_Compiler_Flags) -c Buffer.cpp $(Installer_Include_Path) -o gccInstaller/Buffer.o
	$(CPP_COMPILER) $(Installer_Preprocessor_Definitions) $(Installer_Compiler_Flags) -MM Buffer.cpp $(Installer_Include_Path) > gccInstaller/Buffer.d

# Compiles file MemoryOverrides.cpp for the Installer configuration...
-include gccInstaller/MemoryOverrides.d
gccInstaller/MemoryOverrides.o: MemoryOverrides.cpp
	$(CPP_COMPILER) $(Installer_Preprocessor_Definitions) $(Installer_Compiler_Flags) -c MemoryOverrides.cpp $(Installer_Include_Path) -o gccInstaller/MemoryOverrides.o
	$(CPP_COMPILER) $(Installer_Preprocessor_Definitions) $(Installer_Compiler_Flags) -MM MemoryOverrides.cpp $(Installer_Include_Path) > gccInstaller/MemoryOverrides.d

# Compiles file Packet.cpp for the Installer configuration...
-include gccInstaller/Packet.d
gccInstaller/Packet.o: Packet.cpp
	$(CPP_COMPILER) $(Installer_Preprocessor_Definitions) $(Installer_Compiler_Flags) -c Packet.cpp $(Installer_Include_Path) -o gccInstaller/Packet.o
	$(CPP_COMPILER) $(Installer_Preprocessor_Definitions) $(Installer_Compiler_Flags) -MM Packet.cpp $(Installer_Include_Path) > gccInstaller/Packet.d

# Compiles file Queue.cpp for the Installer configuration...
-include gccInstaller/Queue.d
gccInstaller/Queue.o: Queue.cpp
	$(CPP_COMPILER) $(Installer_Preprocessor_Definitions) $(Installer_Compiler_Flags) -c Queue.cpp $(Installer_Include_Path) -o gccInstaller/Queue.o
	$(CPP_COMPILER) $(Installer_Preprocessor_Definitions) $(Installer_Compiler_Flags) -MM Queue.cpp $(Installer_Include_Path) > gccInstaller/Queue.d

# Compiles file Registration.cpp for the Installer configuration...
-include gccInstaller/Registration.d
gccInstaller/Registration.o: Registration.cpp
	$(CPP_COMPILER) $(Installer_Preprocessor_Definitions) $(Installer_Compiler_Flags) -c Registration.cpp $(Installer_Include_Path) -o gccInstaller/Registration.o
	$(CPP_COMPILER) $(Installer_Preprocessor_Definitions) $(Installer_Compiler_Flags) -MM Registration.cpp $(Installer_Include_Path) > gccInstaller/Registration.d

# Compiles file Screenshot.cpp for the Installer configuration...
-include gccInstaller/Screenshot.d
gccInstaller/Screenshot.o: Screenshot.cpp
	$(CPP_COMPILER) $(Installer_Preprocessor_Definitions) $(Installer_Compiler_Flags) -c Screenshot.cpp $(Installer_Include_Path) -o gccInstaller/Screenshot.o
	$(CPP_COMPILER) $(Installer_Preprocessor_Definitions) $(Installer_Compiler_Flags) -MM Screenshot.cpp $(Installer_Include_Path) > gccInstaller/Screenshot.d

# Compiles file Transport.cpp for the Installer configuration...
-include gccInstaller/Transport.d
gccInstaller/Transport.o: Transport.cpp
	$(CPP_COMPILER) $(Installer_Preprocessor_Definitions) $(Installer_Compiler_Flags) -c Transport.cpp $(Installer_Include_Path) -o gccInstaller/Transport.o
	$(CPP_COMPILER) $(Installer_Preprocessor_Definitions) $(Installer_Compiler_Flags) -MM Transport.cpp $(Installer_Include_Path) > gccInstaller/Transport.d

# Compiles file Win32Transport.cpp for the Installer configuration...
-include gccInstaller/Win32Transport.d
gccInstaller/Win32Transport.o: Win32Transport.cpp
	$(CPP_COMPILER) $(Installer_Preprocessor_Definitions) $(Installer_Compiler_Flags) -c Win32Transport.cpp $(Installer_Include_Path) -o gccInstaller/Win32Transport.o
	$(CPP_COMPILER) $(Installer_Preprocessor_Definitions) $(Installer_Compiler_Flags) -MM Win32Transport.cpp $(Installer_Include_Path) > gccInstaller/Win32Transport.d

# Builds the Analysis configuration...
.PHONY: Analysis
Analysis: create_folders gccAnalysis/Allocation.o gccAnalysis/Buffer.o gccAnalysis/MemoryOverrides.o gccAnalysis/Packet.o gccAnalysis/Queue.o gccAnalysis/Registration.o gccAnalysis/Screenshot.o gccAnalysis/Transport.o gccAnalysis/Win32Transport.o 
	ar rcs gccAnalysis/libCollector.a gccAnalysis/Allocation.o gccAnalysis/Buffer.o gccAnalysis/MemoryOverrides.o gccAnalysis/Packet.o gccAnalysis/Queue.o gccAnalysis/Registration.o gccAnalysis/Screenshot.o gccAnalysis/Transport.o gccAnalysis/Win32Transport.o  $(Analysis_Implicitly_Linked_Objects)

# Compiles file Allocation.cpp for the Analysis configuration...
-include gccAnalysis/Allocation.d
gccAnalysis/Allocation.o: Allocation.cpp
	$(CPP_COMPILER) $(Analysis_Preprocessor_Definitions) $(Analysis_Compiler_Flags) -c Allocation.cpp $(Analysis_Include_Path) -o gccAnalysis/Allocation.o
	$(CPP_COMPILER) $(Analysis_Preprocessor_Definitions) $(Analysis_Compiler_Flags) -MM Allocation.cpp $(Analysis_Include_Path) > gccAnalysis/Allocation.d

# Compiles file Buffer.cpp for the Analysis configuration...
-include gccAnalysis/Buffer.d
gccAnalysis/Buffer.o: Buffer.cpp
	$(CPP_COMPILER) $(Analysis_Preprocessor_Definitions) $(Analysis_Compiler_Flags) -c Buffer.cpp $(Analysis_Include_Path) -o gccAnalysis/Buffer.o
	$(CPP_COMPILER) $(Analysis_Preprocessor_Definitions) $(Analysis_Compiler_Flags) -MM Buffer.cpp $(Analysis_Include_Path) > gccAnalysis/Buffer.d

# Compiles file MemoryOverrides.cpp for the Analysis configuration...
-include gccAnalysis/MemoryOverrides.d
gccAnalysis/MemoryOverrides.o: MemoryOverrides.cpp
	$(CPP_COMPILER) $(Analysis_Preprocessor_Definitions) $(Analysis_Compiler_Flags) -c MemoryOverrides.cpp $(Analysis_Include_Path) -o gccAnalysis/MemoryOverrides.o
	$(CPP_COMPILER) $(Analysis_Preprocessor_Definitions) $(Analysis_Compiler_Flags) -MM MemoryOverrides.cpp $(Analysis_Include_Path) > gccAnalysis/MemoryOverrides.d

# Compiles file Packet.cpp for the Analysis configuration...
-include gccAnalysis/Packet.d
gccAnalysis/Packet.o: Packet.cpp
	$(CPP_COMPILER) $(Analysis_Preprocessor_Definitions) $(Analysis_Compiler_Flags) -c Packet.cpp $(Analysis_Include_Path) -o gccAnalysis/Packet.o
	$(CPP_COMPILER) $(Analysis_Preprocessor_Definitions) $(Analysis_Compiler_Flags) -MM Packet.cpp $(Analysis_Include_Path) > gccAnalysis/Packet.d

# Compiles file Queue.cpp for the Analysis configuration...
-include gccAnalysis/Queue.d
gccAnalysis/Queue.o: Queue.cpp
	$(CPP_COMPILER) $(Analysis_Preprocessor_Definitions) $(Analysis_Compiler_Flags) -c Queue.cpp $(Analysis_Include_Path) -o gccAnalysis/Queue.o
	$(CPP_COMPILER) $(Analysis_Preprocessor_Definitions) $(Analysis_Compiler_Flags) -MM Queue.cpp $(Analysis_Include_Path) > gccAnalysis/Queue.d

# Compiles file Registration.cpp for the Analysis configuration...
-include gccAnalysis/Registration.d
gccAnalysis/Registration.o: Registration.cpp
	$(CPP_COMPILER) $(Analysis_Preprocessor_Definitions) $(Analysis_Compiler_Flags) -c Registration.cpp $(Analysis_Include_Path) -o gccAnalysis/Registration.o
	$(CPP_COMPILER) $(Analysis_Preprocessor_Definitions) $(Analysis_Compiler_Flags) -MM Registration.cpp $(Analysis_Include_Path) > gccAnalysis/Registration.d

# Compiles file Screenshot.cpp for the Analysis configuration...
-include gccAnalysis/Screenshot.d
gccAnalysis/Screenshot.o: Screenshot.cpp
	$(CPP_COMPILER) $(Analysis_Preprocessor_Definitions) $(Analysis_Compiler_Flags) -c Screenshot.cpp $(Analysis_Include_Path) -o gccAnalysis/Screenshot.o
	$(CPP_COMPILER) $(Analysis_Preprocessor_Definitions) $(Analysis_Compiler_Flags) -MM Screenshot.cpp $(Analysis_Include_Path) > gccAnalysis/Screenshot.d

# Compiles file Transport.cpp for the Analysis configuration...
-include gccAnalysis/Transport.d
gccAnalysis/Transport.o: Transport.cpp
	$(CPP_COMPILER) $(Analysis_Preprocessor_Definitions) $(Analysis_Compiler_Flags) -c Transport.cpp $(Analysis_Include_Path) -o gccAnalysis/Transport.o
	$(CPP_COMPILER) $(Analysis_Preprocessor_Definitions) $(Analysis_Compiler_Flags) -MM Transport.cpp $(Analysis_Include_Path) > gccAnalysis/Transport.d

# Compiles file Win32Transport.cpp for the Analysis configuration...
-include gccAnalysis/Win32Transport.d
gccAnalysis/Win32Transport.o: Win32Transport.cpp
	$(CPP_COMPILER) $(Analysis_Preprocessor_Definitions) $(Analysis_Compiler_Flags) -c Win32Transport.cpp $(Analysis_Include_Path) -o gccAnalysis/Win32Transport.o
	$(CPP_COMPILER) $(Analysis_Preprocessor_Definitions) $(Analysis_Compiler_Flags) -MM Win32Transport.cpp $(Analysis_Include_Path) > gccAnalysis/Win32Transport.d

# Builds the Debug_Collector configuration...
.PHONY: Debug_Collector
Debug_Collector: create_folders gccDebug_Collector/Allocation.o gccDebug_Collector/Buffer.o gccDebug_Collector/MemoryOverrides.o gccDebug_Collector/Packet.o gccDebug_Collector/Queue.o gccDebug_Collector/Registration.o gccDebug_Collector/Screenshot.o gccDebug_Collector/Transport.o gccDebug_Collector/Win32Transport.o 
	ar rcs gccDebug_Collector/libCollector.a gccDebug_Collector/Allocation.o gccDebug_Collector/Buffer.o gccDebug_Collector/MemoryOverrides.o gccDebug_Collector/Packet.o gccDebug_Collector/Queue.o gccDebug_Collector/Registration.o gccDebug_Collector/Screenshot.o gccDebug_Collector/Transport.o gccDebug_Collector/Win32Transport.o  $(Debug_Collector_Implicitly_Linked_Objects)

# Compiles file Allocation.cpp for the Debug_Collector configuration...
-include gccDebug_Collector/Allocation.d
gccDebug_Collector/Allocation.o: Allocation.cpp
	$(CPP_COMPILER) $(Debug_Collector_Preprocessor_Definitions) $(Debug_Collector_Compiler_Flags) -c Allocation.cpp $(Debug_Collector_Include_Path) -o gccDebug_Collector/Allocation.o
	$(CPP_COMPILER) $(Debug_Collector_Preprocessor_Definitions) $(Debug_Collector_Compiler_Flags) -MM Allocation.cpp $(Debug_Collector_Include_Path) > gccDebug_Collector/Allocation.d

# Compiles file Buffer.cpp for the Debug_Collector configuration...
-include gccDebug_Collector/Buffer.d
gccDebug_Collector/Buffer.o: Buffer.cpp
	$(CPP_COMPILER) $(Debug_Collector_Preprocessor_Definitions) $(Debug_Collector_Compiler_Flags) -c Buffer.cpp $(Debug_Collector_Include_Path) -o gccDebug_Collector/Buffer.o
	$(CPP_COMPILER) $(Debug_Collector_Preprocessor_Definitions) $(Debug_Collector_Compiler_Flags) -MM Buffer.cpp $(Debug_Collector_Include_Path) > gccDebug_Collector/Buffer.d

# Compiles file MemoryOverrides.cpp for the Debug_Collector configuration...
-include gccDebug_Collector/MemoryOverrides.d
gccDebug_Collector/MemoryOverrides.o: MemoryOverrides.cpp
	$(CPP_COMPILER) $(Debug_Collector_Preprocessor_Definitions) $(Debug_Collector_Compiler_Flags) -c MemoryOverrides.cpp $(Debug_Collector_Include_Path) -o gccDebug_Collector/MemoryOverrides.o
	$(CPP_COMPILER) $(Debug_Collector_Preprocessor_Definitions) $(Debug_Collector_Compiler_Flags) -MM MemoryOverrides.cpp $(Debug_Collector_Include_Path) > gccDebug_Collector/MemoryOverrides.d

# Compiles file Packet.cpp for the Debug_Collector configuration...
-include gccDebug_Collector/Packet.d
gccDebug_Collector/Packet.o: Packet.cpp
	$(CPP_COMPILER) $(Debug_Collector_Preprocessor_Definitions) $(Debug_Collector_Compiler_Flags) -c Packet.cpp $(Debug_Collector_Include_Path) -o gccDebug_Collector/Packet.o
	$(CPP_COMPILER) $(Debug_Collector_Preprocessor_Definitions) $(Debug_Collector_Compiler_Flags) -MM Packet.cpp $(Debug_Collector_Include_Path) > gccDebug_Collector/Packet.d

# Compiles file Queue.cpp for the Debug_Collector configuration...
-include gccDebug_Collector/Queue.d
gccDebug_Collector/Queue.o: Queue.cpp
	$(CPP_COMPILER) $(Debug_Collector_Preprocessor_Definitions) $(Debug_Collector_Compiler_Flags) -c Queue.cpp $(Debug_Collector_Include_Path) -o gccDebug_Collector/Queue.o
	$(CPP_COMPILER) $(Debug_Collector_Preprocessor_Definitions) $(Debug_Collector_Compiler_Flags) -MM Queue.cpp $(Debug_Collector_Include_Path) > gccDebug_Collector/Queue.d

# Compiles file Registration.cpp for the Debug_Collector configuration...
-include gccDebug_Collector/Registration.d
gccDebug_Collector/Registration.o: Registration.cpp
	$(CPP_COMPILER) $(Debug_Collector_Preprocessor_Definitions) $(Debug_Collector_Compiler_Flags) -c Registration.cpp $(Debug_Collector_Include_Path) -o gccDebug_Collector/Registration.o
	$(CPP_COMPILER) $(Debug_Collector_Preprocessor_Definitions) $(Debug_Collector_Compiler_Flags) -MM Registration.cpp $(Debug_Collector_Include_Path) > gccDebug_Collector/Registration.d

# Compiles file Screenshot.cpp for the Debug_Collector configuration...
-include gccDebug_Collector/Screenshot.d
gccDebug_Collector/Screenshot.o: Screenshot.cpp
	$(CPP_COMPILER) $(Debug_Collector_Preprocessor_Definitions) $(Debug_Collector_Compiler_Flags) -c Screenshot.cpp $(Debug_Collector_Include_Path) -o gccDebug_Collector/Screenshot.o
	$(CPP_COMPILER) $(Debug_Collector_Preprocessor_Definitions) $(Debug_Collector_Compiler_Flags) -MM Screenshot.cpp $(Debug_Collector_Include_Path) > gccDebug_Collector/Screenshot.d

# Compiles file Transport.cpp for the Debug_Collector configuration...
-include gccDebug_Collector/Transport.d
gccDebug_Collector/Transport.o: Transport.cpp
	$(CPP_COMPILER) $(Debug_Collector_Preprocessor_Definitions) $(Debug_Collector_Compiler_Flags) -c Transport.cpp $(Debug_Collector_Include_Path) -o gccDebug_Collector/Transport.o
	$(CPP_COMPILER) $(Debug_Collector_Preprocessor_Definitions) $(Debug_Collector_Compiler_Flags) -MM Transport.cpp $(Debug_Collector_Include_Path) > gccDebug_Collector/Transport.d

# Compiles file Win32Transport.cpp for the Debug_Collector configuration...
-include gccDebug_Collector/Win32Transport.d
gccDebug_Collector/Win32Transport.o: Win32Transport.cpp
	$(CPP_COMPILER) $(Debug_Collector_Preprocessor_Definitions) $(Debug_Collector_Compiler_Flags) -c Win32Transport.cpp $(Debug_Collector_Include_Path) -o gccDebug_Collector/Win32Transport.o
	$(CPP_COMPILER) $(Debug_Collector_Preprocessor_Definitions) $(Debug_Collector_Compiler_Flags) -MM Win32Transport.cpp $(Debug_Collector_Include_Path) > gccDebug_Collector/Win32Transport.d

# Builds the Debug configuration...
.PHONY: Debug
Debug: create_folders gccDebug/Allocation.o gccDebug/Buffer.o gccDebug/MemoryOverrides.o gccDebug/Packet.o gccDebug/Queue.o gccDebug/Registration.o gccDebug/Screenshot.o gccDebug/Transport.o gccDebug/Win32Transport.o 
	ar rcs gccDebug/libCollector.a gccDebug/Allocation.o gccDebug/Buffer.o gccDebug/MemoryOverrides.o gccDebug/Packet.o gccDebug/Queue.o gccDebug/Registration.o gccDebug/Screenshot.o gccDebug/Transport.o gccDebug/Win32Transport.o  $(Debug_Implicitly_Linked_Objects)

# Compiles file Allocation.cpp for the Debug configuration...
-include gccDebug/Allocation.d
gccDebug/Allocation.o: Allocation.cpp
	$(CPP_COMPILER) $(Debug_Preprocessor_Definitions) $(Debug_Compiler_Flags) -c Allocation.cpp $(Debug_Include_Path) -o gccDebug/Allocation.o
	$(CPP_COMPILER) $(Debug_Preprocessor_Definitions) $(Debug_Compiler_Flags) -MM Allocation.cpp $(Debug_Include_Path) > gccDebug/Allocation.d

# Compiles file Buffer.cpp for the Debug configuration...
-include gccDebug/Buffer.d
gccDebug/Buffer.o: Buffer.cpp
	$(CPP_COMPILER) $(Debug_Preprocessor_Definitions) $(Debug_Compiler_Flags) -c Buffer.cpp $(Debug_Include_Path) -o gccDebug/Buffer.o
	$(CPP_COMPILER) $(Debug_Preprocessor_Definitions) $(Debug_Compiler_Flags) -MM Buffer.cpp $(Debug_Include_Path) > gccDebug/Buffer.d

# Compiles file MemoryOverrides.cpp for the Debug configuration...
-include gccDebug/MemoryOverrides.d
gccDebug/MemoryOverrides.o: MemoryOverrides.cpp
	$(CPP_COMPILER) $(Debug_Preprocessor_Definitions) $(Debug_Compiler_Flags) -c MemoryOverrides.cpp $(Debug_Include_Path) -o gccDebug/MemoryOverrides.o
	$(CPP_COMPILER) $(Debug_Preprocessor_Definitions) $(Debug_Compiler_Flags) -MM MemoryOverrides.cpp $(Debug_Include_Path) > gccDebug/MemoryOverrides.d

# Compiles file Packet.cpp for the Debug configuration...
-include gccDebug/Packet.d
gccDebug/Packet.o: Packet.cpp
	$(CPP_COMPILER) $(Debug_Preprocessor_Definitions) $(Debug_Compiler_Flags) -c Packet.cpp $(Debug_Include_Path) -o gccDebug/Packet.o
	$(CPP_COMPILER) $(Debug_Preprocessor_Definitions) $(Debug_Compiler_Flags) -MM Packet.cpp $(Debug_Include_Path) > gccDebug/Packet.d

# Compiles file Queue.cpp for the Debug configuration...
-include gccDebug/Queue.d
gccDebug/Queue.o: Queue.cpp
	$(CPP_COMPILER) $(Debug_Preprocessor_Definitions) $(Debug_Compiler_Flags) -c Queue.cpp $(Debug_Include_Path) -o gccDebug/Queue.o
	$(CPP_COMPILER) $(Debug_Preprocessor_Definitions) $(Debug_Compiler_Flags) -MM Queue.cpp $(Debug_Include_Path) > gccDebug/Queue.d

# Compiles file Registration.cpp for the Debug configuration...
-include gccDebug/Registration.d
gccDebug/Registration.o: Registration.cpp
	$(CPP_COMPILER) $(Debug_Preprocessor_Definitions) $(Debug_Compiler_Flags) -c Registration.cpp $(Debug_Include_Path) -o gccDebug/Registration.o
	$(CPP_COMPILER) $(Debug_Preprocessor_Definitions) $(Debug_Compiler_Flags) -MM Registration.cpp $(Debug_Include_Path) > gccDebug/Registration.d

# Compiles file Screenshot.cpp for the Debug configuration...
-include gccDebug/Screenshot.d
gccDebug/Screenshot.o: Screenshot.cpp
	$(CPP_COMPILER) $(Debug_Preprocessor_Definitions) $(Debug_Compiler_Flags) -c Screenshot.cpp $(Debug_Include_Path) -o gccDebug/Screenshot.o
	$(CPP_COMPILER) $(Debug_Preprocessor_Definitions) $(Debug_Compiler_Flags) -MM Screenshot.cpp $(Debug_Include_Path) > gccDebug/Screenshot.d

# Compiles file Transport.cpp for the Debug configuration...
-include gccDebug/Transport.d
gccDebug/Transport.o: Transport.cpp
	$(CPP_COMPILER) $(Debug_Preprocessor_Definitions) $(Debug_Compiler_Flags) -c Transport.cpp $(Debug_Include_Path) -o gccDebug/Transport.o
	$(CPP_COMPILER) $(Debug_Preprocessor_Definitions) $(Debug_Compiler_Flags) -MM Transport.cpp $(Debug_Include_Path) > gccDebug/Transport.d

# Compiles file Win32Transport.cpp for the Debug configuration...
-include gccDebug/Win32Transport.d
gccDebug/Win32Transport.o: Win32Transport.cpp
	$(CPP_COMPILER) $(Debug_Preprocessor_Definitions) $(Debug_Compiler_Flags) -c Win32Transport.cpp $(Debug_Include_Path) -o gccDebug/Win32Transport.o
	$(CPP_COMPILER) $(Debug_Preprocessor_Definitions) $(Debug_Compiler_Flags) -MM Win32Transport.cpp $(Debug_Include_Path) > gccDebug/Win32Transport.d

# Builds the Release configuration...
.PHONY: Release
Release: create_folders gccRelease/Allocation.o gccRelease/Buffer.o gccRelease/MemoryOverrides.o gccRelease/Packet.o gccRelease/Queue.o gccRelease/Registration.o gccRelease/Screenshot.o gccRelease/Transport.o gccRelease/Win32Transport.o 
	ar rcs gccRelease/libCollector.a gccRelease/Allocation.o gccRelease/Buffer.o gccRelease/MemoryOverrides.o gccRelease/Packet.o gccRelease/Queue.o gccRelease/Registration.o gccRelease/Screenshot.o gccRelease/Transport.o gccRelease/Win32Transport.o  $(Release_Implicitly_Linked_Objects)

# Compiles file Allocation.cpp for the Release configuration...
-include gccRelease/Allocation.d
gccRelease/Allocation.o: Allocation.cpp
	$(CPP_COMPILER) $(Release_Preprocessor_Definitions) $(Release_Compiler_Flags) -c Allocation.cpp $(Release_Include_Path) -o gccRelease/Allocation.o
	$(CPP_COMPILER) $(Release_Preprocessor_Definitions) $(Release_Compiler_Flags) -MM Allocation.cpp $(Release_Include_Path) > gccRelease/Allocation.d

# Compiles file Buffer.cpp for the Release configuration...
-include gccRelease/Buffer.d
gccRelease/Buffer.o: Buffer.cpp
	$(CPP_COMPILER) $(Release_Preprocessor_Definitions) $(Release_Compiler_Flags) -c Buffer.cpp $(Release_Include_Path) -o gccRelease/Buffer.o
	$(CPP_COMPILER) $(Release_Preprocessor_Definitions) $(Release_Compiler_Flags) -MM Buffer.cpp $(Release_Include_Path) > gccRelease/Buffer.d

# Compiles file MemoryOverrides.cpp for the Release configuration...
-include gccRelease/MemoryOverrides.d
gccRelease/MemoryOverrides.o: MemoryOverrides.cpp
	$(CPP_COMPILER) $(Release_Preprocessor_Definitions) $(Release_Compiler_Flags) -c MemoryOverrides.cpp $(Release_Include_Path) -o gccRelease/MemoryOverrides.o
	$(CPP_COMPILER) $(Release_Preprocessor_Definitions) $(Release_Compiler_Flags) -MM MemoryOverrides.cpp $(Release_Include_Path) > gccRelease/MemoryOverrides.d

# Compiles file Packet.cpp for the Release configuration...
-include gccRelease/Packet.d
gccRelease/Packet.o: Packet.cpp
	$(CPP_COMPILER) $(Release_Preprocessor_Definitions) $(Release_Compiler_Flags) -c Packet.cpp $(Release_Include_Path) -o gccRelease/Packet.o
	$(CPP_COMPILER) $(Release_Preprocessor_Definitions) $(Release_Compiler_Flags) -MM Packet.cpp $(Release_Include_Path) > gccRelease/Packet.d

# Compiles file Queue.cpp for the Release configuration...
-include gccRelease/Queue.d
gccRelease/Queue.o: Queue.cpp
	$(CPP_COMPILER) $(Release_Preprocessor_Definitions) $(Release_Compiler_Flags) -c Queue.cpp $(Release_Include_Path) -o gccRelease/Queue.o
	$(CPP_COMPILER) $(Release_Preprocessor_Definitions) $(Release_Compiler_Flags) -MM Queue.cpp $(Release_Include_Path) > gccRelease/Queue.d

# Compiles file Registration.cpp for the Release configuration...
-include gccRelease/Registration.d
gccRelease/Registration.o: Registration.cpp
	$(CPP_COMPILER) $(Release_Preprocessor_Definitions) $(Release_Compiler_Flags) -c Registration.cpp $(Release_Include_Path) -o gccRelease/Registration.o
	$(CPP_COMPILER) $(Release_Preprocessor_Definitions) $(Release_Compiler_Flags) -MM Registration.cpp $(Release_Include_Path) > gccRelease/Registration.d

# Compiles file Screenshot.cpp for the Release configuration...
-include gccRelease/Screenshot.d
gccRelease/Screenshot.o: Screenshot.cpp
	$(CPP_COMPILER) $(Release_Preprocessor_Definitions) $(Release_Compiler_Flags) -c Screenshot.cpp $(Release_Include_Path) -o gccRelease/Screenshot.o
	$(CPP_COMPILER) $(Release_Preprocessor_Definitions) $(Release_Compiler_Flags) -MM Screenshot.cpp $(Release_Include_Path) > gccRelease/Screenshot.d

# Compiles file Transport.cpp for the Release configuration...
-include gccRelease/Transport.d
gccRelease/Transport.o: Transport.cpp
	$(CPP_COMPILER) $(Release_Preprocessor_Definitions) $(Release_Compiler_Flags) -c Transport.cpp $(Release_Include_Path) -o gccRelease/Transport.o
	$(CPP_COMPILER) $(Release_Preprocessor_Definitions) $(Release_Compiler_Flags) -MM Transport.cpp $(Release_Include_Path) > gccRelease/Transport.d

# Compiles file Win32Transport.cpp for the Release configuration...
-include gccRelease/Win32Transport.d
gccRelease/Win32Transport.o: Win32Transport.cpp
	$(CPP_COMPILER) $(Release_Preprocessor_Definitions) $(Release_Compiler_Flags) -c Win32Transport.cpp $(Release_Include_Path) -o gccRelease/Win32Transport.o
	$(CPP_COMPILER) $(Release_Preprocessor_Definitions) $(Release_Compiler_Flags) -MM Win32Transport.cpp $(Release_Include_Path) > gccRelease/Win32Transport.d

# Builds the Release_Collector configuration...
.PHONY: Release_Collector
Release_Collector: create_folders gccRelease_Collector/Allocation.o gccRelease_Collector/Buffer.o gccRelease_Collector/MemoryOverrides.o gccRelease_Collector/Packet.o gccRelease_Collector/Queue.o gccRelease_Collector/Registration.o gccRelease_Collector/Screenshot.o gccRelease_Collector/Transport.o gccRelease_Collector/Win32Transport.o 
	ar rcs gccRelease_Collector/libCollector.a gccRelease_Collector/Allocation.o gccRelease_Collector/Buffer.o gccRelease_Collector/MemoryOverrides.o gccRelease_Collector/Packet.o gccRelease_Collector/Queue.o gccRelease_Collector/Registration.o gccRelease_Collector/Screenshot.o gccRelease_Collector/Transport.o gccRelease_Collector/Win32Transport.o  $(Release_Collector_Implicitly_Linked_Objects)

# Compiles file Allocation.cpp for the Release_Collector configuration...
-include gccRelease_Collector/Allocation.d
gccRelease_Collector/Allocation.o: Allocation.cpp
	$(CPP_COMPILER) $(Release_Collector_Preprocessor_Definitions) $(Release_Collector_Compiler_Flags) -c Allocation.cpp $(Release_Collector_Include_Path) -o gccRelease_Collector/Allocation.o
	$(CPP_COMPILER) $(Release_Collector_Preprocessor_Definitions) $(Release_Collector_Compiler_Flags) -MM Allocation.cpp $(Release_Collector_Include_Path) > gccRelease_Collector/Allocation.d

# Compiles file Buffer.cpp for the Release_Collector configuration...
-include gccRelease_Collector/Buffer.d
gccRelease_Collector/Buffer.o: Buffer.cpp
	$(CPP_COMPILER) $(Release_Collector_Preprocessor_Definitions) $(Release_Collector_Compiler_Flags) -c Buffer.cpp $(Release_Collector_Include_Path) -o gccRelease_Collector/Buffer.o
	$(CPP_COMPILER) $(Release_Collector_Preprocessor_Definitions) $(Release_Collector_Compiler_Flags) -MM Buffer.cpp $(Release_Collector_Include_Path) > gccRelease_Collector/Buffer.d

# Compiles file MemoryOverrides.cpp for the Release_Collector configuration...
-include gccRelease_Collector/MemoryOverrides.d
gccRelease_Collector/MemoryOverrides.o: MemoryOverrides.cpp
	$(CPP_COMPILER) $(Release_Collector_Preprocessor_Definitions) $(Release_Collector_Compiler_Flags) -c MemoryOverrides.cpp $(Release_Collector_Include_Path) -o gccRelease_Collector/MemoryOverrides.o
	$(CPP_COMPILER) $(Release_Collector_Preprocessor_Definitions) $(Release_Collector_Compiler_Flags) -MM MemoryOverrides.cpp $(Release_Collector_Include_Path) > gccRelease_Collector/MemoryOverrides.d

# Compiles file Packet.cpp for the Release_Collector configuration...
-include gccRelease_Collector/Packet.d
gccRelease_Collector/Packet.o: Packet.cpp
	$(CPP_COMPILER) $(Release_Collector_Preprocessor_Definitions) $(Release_Collector_Compiler_Flags) -c Packet.cpp $(Release_Collector_Include_Path) -o gccRelease_Collector/Packet.o
	$(CPP_COMPILER) $(Release_Collector_Preprocessor_Definitions) $(Release_Collector_Compiler_Flags) -MM Packet.cpp $(Release_Collector_Include_Path) > gccRelease_Collector/Packet.d

# Compiles file Queue.cpp for the Release_Collector configuration...
-include gccRelease_Collector/Queue.d
gccRelease_Collector/Queue.o: Queue.cpp
	$(CPP_COMPILER) $(Release_Collector_Preprocessor_Definitions) $(Release_Collector_Compiler_Flags) -c Queue.cpp $(Release_Collector_Include_Path) -o gccRelease_Collector/Queue.o
	$(CPP_COMPILER) $(Release_Collector_Preprocessor_Definitions) $(Release_Collector_Compiler_Flags) -MM Queue.cpp $(Release_Collector_Include_Path) > gccRelease_Collector/Queue.d

# Compiles file Registration.cpp for the Release_Collector configuration...
-include gccRelease_Collector/Registration.d
gccRelease_Collector/Registration.o: Registration.cpp
	$(CPP_COMPILER) $(Release_Collector_Preprocessor_Definitions) $(Release_Collector_Compiler_Flags) -c Registration.cpp $(Release_Collector_Include_Path) -o gccRelease_Collector/Registration.o
	$(CPP_COMPILER) $(Release_Collector_Preprocessor_Definitions) $(Release_Collector_Compiler_Flags) -MM Registration.cpp $(Release_Collector_Include_Path) > gccRelease_Collector/Registration.d

# Compiles file Screenshot.cpp for the Release_Collector configuration...
-include gccRelease_Collector/Screenshot.d
gccRelease_Collector/Screenshot.o: Screenshot.cpp
	$(CPP_COMPILER) $(Release_Collector_Preprocessor_Definitions) $(Release_Collector_Compiler_Flags) -c Screenshot.cpp $(Release_Collector_Include_Path) -o gccRelease_Collector/Screenshot.o
	$(CPP_COMPILER) $(Release_Collector_Preprocessor_Definitions) $(Release_Collector_Compiler_Flags) -MM Screenshot.cpp $(Release_Collector_Include_Path) > gccRelease_Collector/Screenshot.d

# Compiles file Transport.cpp for the Release_Collector configuration...
-include gccRelease_Collector/Transport.d
gccRelease_Collector/Transport.o: Transport.cpp
	$(CPP_COMPILER) $(Release_Collector_Preprocessor_Definitions) $(Release_Collector_Compiler_Flags) -c Transport.cpp $(Release_Collector_Include_Path) -o gccRelease_Collector/Transport.o
	$(CPP_COMPILER) $(Release_Collector_Preprocessor_Definitions) $(Release_Collector_Compiler_Flags) -MM Transport.cpp $(Release_Collector_Include_Path) > gccRelease_Collector/Transport.d

# Compiles file Win32Transport.cpp for the Release_Collector configuration...
-include gccRelease_Collector/Win32Transport.d
gccRelease_Collector/Win32Transport.o: Win32Transport.cpp
	$(CPP_COMPILER) $(Release_Collector_Preprocessor_Definitions) $(Release_Collector_Compiler_Flags) -c Win32Transport.cpp $(Release_Collector_Include_Path) -o gccRelease_Collector/Win32Transport.o
	$(CPP_COMPILER) $(Release_Collector_Preprocessor_Definitions) $(Release_Collector_Compiler_Flags) -MM Win32Transport.cpp $(Release_Collector_Include_Path) > gccRelease_Collector/Win32Transport.d

# Creates the intermediate and output folders for each configuration...
.PHONY: create_folders
create_folders:
	mkdir -p gccInstaller
	mkdir -p gccAnalysis
	mkdir -p gccDebug_Collector
	mkdir -p gccDebug
	mkdir -p gccRelease
	mkdir -p gccRelease_Collector

# Cleans intermediate and output files (objects, libraries, executables)...
.PHONY: clean
clean:
	rm -f gccInstaller/*.o
	rm -f gccInstaller/*.d
	rm -f gccInstaller/*.a
	rm -f gccInstaller/*.so
	rm -f gccInstaller/*.dll
	rm -f gccInstaller/*.exe
	rm -f gccAnalysis/*.o
	rm -f gccAnalysis/*.d
	rm -f gccAnalysis/*.a
	rm -f gccAnalysis/*.so
	rm -f gccAnalysis/*.dll
	rm -f gccAnalysis/*.exe
	rm -f gccDebug_Collector/*.o
	rm -f gccDebug_Collector/*.d
	rm -f gccDebug_Collector/*.a
	rm -f gccDebug_Collector/*.so
	rm -f gccDebug_Collector/*.dll
	rm -f gccDebug_Collector/*.exe
	rm -f gccDebug/*.o
	rm -f gccDebug/*.d
	rm -f gccDebug/*.a
	rm -f gccDebug/*.so
	rm -f gccDebug/*.dll
	rm -f gccDebug/*.exe
	rm -f gccRelease/*.o
	rm -f gccRelease/*.d
	rm -f gccRelease/*.a
	rm -f gccRelease/*.so
	rm -f gccRelease/*.dll
	rm -f gccRelease/*.exe
	rm -f gccRelease_Collector/*.o
	rm -f gccRelease_Collector/*.d
	rm -f gccRelease_Collector/*.a
	rm -f gccRelease_Collector/*.so
	rm -f gccRelease_Collector/*.dll
	rm -f gccRelease_Collector/*.exe

