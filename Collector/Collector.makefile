# Compiler flags...
CPP_COMPILER = g++
C_COMPILER = gcc

# Include paths...
Debug_Include_Path=
Debug_Include_Path=
Release_Include_Path=
Release_Include_Path=

# Library paths...
Debug_Library_Path=
Debug_Library_Path=
Release_Library_Path=
Release_Library_Path=

# Additional libraries...
Debug_Libraries=
Debug_Libraries=
Release_Libraries=
Release_Libraries=

# Preprocessor definitions...
Debug_Preprocessor_Definitions=-D _MBCS -D _CRT_SECURE_NO_WARNINGS -D GCC_BUILD 
Debug_Preprocessor_Definitions=-D _MBCS -D GCC_BUILD 
Release_Preprocessor_Definitions=-D _MBCS -D _CRT_SECURE_NO_WARNINGS -D GCC_BUILD 
Release_Preprocessor_Definitions=-D GCC_BUILD 

# Implictly linked object files...
Debug_Implicitly_Linked_Objects=
Debug_Implicitly_Linked_Objects=
Release_Implicitly_Linked_Objects=
Release_Implicitly_Linked_Objects=

# Compiler flags...
Debug_Compiler_Flags=-O0 -g 
Debug_Compiler_Flags=-O0 -g 
Release_Compiler_Flags=-O2 -g 
Release_Compiler_Flags=-O2 -g 

# Builds all configurations for this project...
.PHONY: build_all_configurations
build_all_configurations: Debug Debug Release Release 

# Builds the Debug configuration...
.PHONY: Debug
Debug: create_folders gccDebug/Allocation.o gccDebug/Buffer.o gccDebug/CallStack.o gccDebug/CallStack_Win32.o gccDebug/Free.o gccDebug/MemoryOverrides.o gccDebug/Packet.o gccDebug/Registration.o gccDebug/Screenshot.o gccDebug/SetArchitecture.o gccDebug/SetSymbols.o gccDebug/Thread_Win32.o gccDebug/Transport.o gccDebug/Win32Transport.o 
	ar rcs gccDebug/libCollector.a gccDebug/Allocation.o gccDebug/Buffer.o gccDebug/CallStack.o gccDebug/CallStack_Win32.o gccDebug/Free.o gccDebug/MemoryOverrides.o gccDebug/Packet.o gccDebug/Registration.o gccDebug/Screenshot.o gccDebug/SetArchitecture.o gccDebug/SetSymbols.o gccDebug/Thread_Win32.o gccDebug/Transport.o gccDebug/Win32Transport.o  $(Debug_Implicitly_Linked_Objects)

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

# Compiles file CallStack.cpp for the Debug configuration...
-include gccDebug/CallStack.d
gccDebug/CallStack.o: CallStack.cpp
	$(CPP_COMPILER) $(Debug_Preprocessor_Definitions) $(Debug_Compiler_Flags) -c CallStack.cpp $(Debug_Include_Path) -o gccDebug/CallStack.o
	$(CPP_COMPILER) $(Debug_Preprocessor_Definitions) $(Debug_Compiler_Flags) -MM CallStack.cpp $(Debug_Include_Path) > gccDebug/CallStack.d

# Compiles file CallStack_Win32.cpp for the Debug configuration...
-include gccDebug/CallStack_Win32.d
gccDebug/CallStack_Win32.o: CallStack_Win32.cpp
	$(CPP_COMPILER) $(Debug_Preprocessor_Definitions) $(Debug_Compiler_Flags) -c CallStack_Win32.cpp $(Debug_Include_Path) -o gccDebug/CallStack_Win32.o
	$(CPP_COMPILER) $(Debug_Preprocessor_Definitions) $(Debug_Compiler_Flags) -MM CallStack_Win32.cpp $(Debug_Include_Path) > gccDebug/CallStack_Win32.d

# Compiles file Free.cpp for the Debug configuration...
-include gccDebug/Free.d
gccDebug/Free.o: Free.cpp
	$(CPP_COMPILER) $(Debug_Preprocessor_Definitions) $(Debug_Compiler_Flags) -c Free.cpp $(Debug_Include_Path) -o gccDebug/Free.o
	$(CPP_COMPILER) $(Debug_Preprocessor_Definitions) $(Debug_Compiler_Flags) -MM Free.cpp $(Debug_Include_Path) > gccDebug/Free.d

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

# Compiles file SetArchitecture.cpp for the Debug configuration...
-include gccDebug/SetArchitecture.d
gccDebug/SetArchitecture.o: SetArchitecture.cpp
	$(CPP_COMPILER) $(Debug_Preprocessor_Definitions) $(Debug_Compiler_Flags) -c SetArchitecture.cpp $(Debug_Include_Path) -o gccDebug/SetArchitecture.o
	$(CPP_COMPILER) $(Debug_Preprocessor_Definitions) $(Debug_Compiler_Flags) -MM SetArchitecture.cpp $(Debug_Include_Path) > gccDebug/SetArchitecture.d

# Compiles file SetSymbols.cpp for the Debug configuration...
-include gccDebug/SetSymbols.d
gccDebug/SetSymbols.o: SetSymbols.cpp
	$(CPP_COMPILER) $(Debug_Preprocessor_Definitions) $(Debug_Compiler_Flags) -c SetSymbols.cpp $(Debug_Include_Path) -o gccDebug/SetSymbols.o
	$(CPP_COMPILER) $(Debug_Preprocessor_Definitions) $(Debug_Compiler_Flags) -MM SetSymbols.cpp $(Debug_Include_Path) > gccDebug/SetSymbols.d

# Compiles file Thread_Win32.cpp for the Debug configuration...
-include gccDebug/Thread_Win32.d
gccDebug/Thread_Win32.o: Thread_Win32.cpp
	$(CPP_COMPILER) $(Debug_Preprocessor_Definitions) $(Debug_Compiler_Flags) -c Thread_Win32.cpp $(Debug_Include_Path) -o gccDebug/Thread_Win32.o
	$(CPP_COMPILER) $(Debug_Preprocessor_Definitions) $(Debug_Compiler_Flags) -MM Thread_Win32.cpp $(Debug_Include_Path) > gccDebug/Thread_Win32.d

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

# Builds the Debug configuration...
.PHONY: Debug
Debug: create_folders x64/gccDebug/Allocation.o x64/gccDebug/Buffer.o x64/gccDebug/CallStack.o x64/gccDebug/CallStack_Win32.o x64/gccDebug/Free.o x64/gccDebug/MemoryOverrides.o x64/gccDebug/Packet.o x64/gccDebug/Registration.o x64/gccDebug/Screenshot.o x64/gccDebug/SetArchitecture.o x64/gccDebug/SetSymbols.o x64/gccDebug/Thread_Win32.o x64/gccDebug/Transport.o x64/gccDebug/Win32Transport.o 
	ar rcs x64/gccDebug/libCollector.a x64/gccDebug/Allocation.o x64/gccDebug/Buffer.o x64/gccDebug/CallStack.o x64/gccDebug/CallStack_Win32.o x64/gccDebug/Free.o x64/gccDebug/MemoryOverrides.o x64/gccDebug/Packet.o x64/gccDebug/Registration.o x64/gccDebug/Screenshot.o x64/gccDebug/SetArchitecture.o x64/gccDebug/SetSymbols.o x64/gccDebug/Thread_Win32.o x64/gccDebug/Transport.o x64/gccDebug/Win32Transport.o  $(Debug_Implicitly_Linked_Objects)

# Compiles file Allocation.cpp for the Debug configuration...
-include x64/gccDebug/Allocation.d
x64/gccDebug/Allocation.o: Allocation.cpp
	$(CPP_COMPILER) $(Debug_Preprocessor_Definitions) $(Debug_Compiler_Flags) -c Allocation.cpp $(Debug_Include_Path) -o x64/gccDebug/Allocation.o
	$(CPP_COMPILER) $(Debug_Preprocessor_Definitions) $(Debug_Compiler_Flags) -MM Allocation.cpp $(Debug_Include_Path) > x64/gccDebug/Allocation.d

# Compiles file Buffer.cpp for the Debug configuration...
-include x64/gccDebug/Buffer.d
x64/gccDebug/Buffer.o: Buffer.cpp
	$(CPP_COMPILER) $(Debug_Preprocessor_Definitions) $(Debug_Compiler_Flags) -c Buffer.cpp $(Debug_Include_Path) -o x64/gccDebug/Buffer.o
	$(CPP_COMPILER) $(Debug_Preprocessor_Definitions) $(Debug_Compiler_Flags) -MM Buffer.cpp $(Debug_Include_Path) > x64/gccDebug/Buffer.d

# Compiles file CallStack.cpp for the Debug configuration...
-include x64/gccDebug/CallStack.d
x64/gccDebug/CallStack.o: CallStack.cpp
	$(CPP_COMPILER) $(Debug_Preprocessor_Definitions) $(Debug_Compiler_Flags) -c CallStack.cpp $(Debug_Include_Path) -o x64/gccDebug/CallStack.o
	$(CPP_COMPILER) $(Debug_Preprocessor_Definitions) $(Debug_Compiler_Flags) -MM CallStack.cpp $(Debug_Include_Path) > x64/gccDebug/CallStack.d

# Compiles file CallStack_Win32.cpp for the Debug configuration...
-include x64/gccDebug/CallStack_Win32.d
x64/gccDebug/CallStack_Win32.o: CallStack_Win32.cpp
	$(CPP_COMPILER) $(Debug_Preprocessor_Definitions) $(Debug_Compiler_Flags) -c CallStack_Win32.cpp $(Debug_Include_Path) -o x64/gccDebug/CallStack_Win32.o
	$(CPP_COMPILER) $(Debug_Preprocessor_Definitions) $(Debug_Compiler_Flags) -MM CallStack_Win32.cpp $(Debug_Include_Path) > x64/gccDebug/CallStack_Win32.d

# Compiles file Free.cpp for the Debug configuration...
-include x64/gccDebug/Free.d
x64/gccDebug/Free.o: Free.cpp
	$(CPP_COMPILER) $(Debug_Preprocessor_Definitions) $(Debug_Compiler_Flags) -c Free.cpp $(Debug_Include_Path) -o x64/gccDebug/Free.o
	$(CPP_COMPILER) $(Debug_Preprocessor_Definitions) $(Debug_Compiler_Flags) -MM Free.cpp $(Debug_Include_Path) > x64/gccDebug/Free.d

# Compiles file MemoryOverrides.cpp for the Debug configuration...
-include x64/gccDebug/MemoryOverrides.d
x64/gccDebug/MemoryOverrides.o: MemoryOverrides.cpp
	$(CPP_COMPILER) $(Debug_Preprocessor_Definitions) $(Debug_Compiler_Flags) -c MemoryOverrides.cpp $(Debug_Include_Path) -o x64/gccDebug/MemoryOverrides.o
	$(CPP_COMPILER) $(Debug_Preprocessor_Definitions) $(Debug_Compiler_Flags) -MM MemoryOverrides.cpp $(Debug_Include_Path) > x64/gccDebug/MemoryOverrides.d

# Compiles file Packet.cpp for the Debug configuration...
-include x64/gccDebug/Packet.d
x64/gccDebug/Packet.o: Packet.cpp
	$(CPP_COMPILER) $(Debug_Preprocessor_Definitions) $(Debug_Compiler_Flags) -c Packet.cpp $(Debug_Include_Path) -o x64/gccDebug/Packet.o
	$(CPP_COMPILER) $(Debug_Preprocessor_Definitions) $(Debug_Compiler_Flags) -MM Packet.cpp $(Debug_Include_Path) > x64/gccDebug/Packet.d

# Compiles file Registration.cpp for the Debug configuration...
-include x64/gccDebug/Registration.d
x64/gccDebug/Registration.o: Registration.cpp
	$(CPP_COMPILER) $(Debug_Preprocessor_Definitions) $(Debug_Compiler_Flags) -c Registration.cpp $(Debug_Include_Path) -o x64/gccDebug/Registration.o
	$(CPP_COMPILER) $(Debug_Preprocessor_Definitions) $(Debug_Compiler_Flags) -MM Registration.cpp $(Debug_Include_Path) > x64/gccDebug/Registration.d

# Compiles file Screenshot.cpp for the Debug configuration...
-include x64/gccDebug/Screenshot.d
x64/gccDebug/Screenshot.o: Screenshot.cpp
	$(CPP_COMPILER) $(Debug_Preprocessor_Definitions) $(Debug_Compiler_Flags) -c Screenshot.cpp $(Debug_Include_Path) -o x64/gccDebug/Screenshot.o
	$(CPP_COMPILER) $(Debug_Preprocessor_Definitions) $(Debug_Compiler_Flags) -MM Screenshot.cpp $(Debug_Include_Path) > x64/gccDebug/Screenshot.d

# Compiles file SetArchitecture.cpp for the Debug configuration...
-include x64/gccDebug/SetArchitecture.d
x64/gccDebug/SetArchitecture.o: SetArchitecture.cpp
	$(CPP_COMPILER) $(Debug_Preprocessor_Definitions) $(Debug_Compiler_Flags) -c SetArchitecture.cpp $(Debug_Include_Path) -o x64/gccDebug/SetArchitecture.o
	$(CPP_COMPILER) $(Debug_Preprocessor_Definitions) $(Debug_Compiler_Flags) -MM SetArchitecture.cpp $(Debug_Include_Path) > x64/gccDebug/SetArchitecture.d

# Compiles file SetSymbols.cpp for the Debug configuration...
-include x64/gccDebug/SetSymbols.d
x64/gccDebug/SetSymbols.o: SetSymbols.cpp
	$(CPP_COMPILER) $(Debug_Preprocessor_Definitions) $(Debug_Compiler_Flags) -c SetSymbols.cpp $(Debug_Include_Path) -o x64/gccDebug/SetSymbols.o
	$(CPP_COMPILER) $(Debug_Preprocessor_Definitions) $(Debug_Compiler_Flags) -MM SetSymbols.cpp $(Debug_Include_Path) > x64/gccDebug/SetSymbols.d

# Compiles file Thread_Win32.cpp for the Debug configuration...
-include x64/gccDebug/Thread_Win32.d
x64/gccDebug/Thread_Win32.o: Thread_Win32.cpp
	$(CPP_COMPILER) $(Debug_Preprocessor_Definitions) $(Debug_Compiler_Flags) -c Thread_Win32.cpp $(Debug_Include_Path) -o x64/gccDebug/Thread_Win32.o
	$(CPP_COMPILER) $(Debug_Preprocessor_Definitions) $(Debug_Compiler_Flags) -MM Thread_Win32.cpp $(Debug_Include_Path) > x64/gccDebug/Thread_Win32.d

# Compiles file Transport.cpp for the Debug configuration...
-include x64/gccDebug/Transport.d
x64/gccDebug/Transport.o: Transport.cpp
	$(CPP_COMPILER) $(Debug_Preprocessor_Definitions) $(Debug_Compiler_Flags) -c Transport.cpp $(Debug_Include_Path) -o x64/gccDebug/Transport.o
	$(CPP_COMPILER) $(Debug_Preprocessor_Definitions) $(Debug_Compiler_Flags) -MM Transport.cpp $(Debug_Include_Path) > x64/gccDebug/Transport.d

# Compiles file Win32Transport.cpp for the Debug configuration...
-include x64/gccDebug/Win32Transport.d
x64/gccDebug/Win32Transport.o: Win32Transport.cpp
	$(CPP_COMPILER) $(Debug_Preprocessor_Definitions) $(Debug_Compiler_Flags) -c Win32Transport.cpp $(Debug_Include_Path) -o x64/gccDebug/Win32Transport.o
	$(CPP_COMPILER) $(Debug_Preprocessor_Definitions) $(Debug_Compiler_Flags) -MM Win32Transport.cpp $(Debug_Include_Path) > x64/gccDebug/Win32Transport.d

# Builds the Release configuration...
.PHONY: Release
Release: create_folders gccRelease/Allocation.o gccRelease/Buffer.o gccRelease/CallStack.o gccRelease/CallStack_Win32.o gccRelease/Free.o gccRelease/MemoryOverrides.o gccRelease/Packet.o gccRelease/Registration.o gccRelease/Screenshot.o gccRelease/SetArchitecture.o gccRelease/SetSymbols.o gccRelease/Thread_Win32.o gccRelease/Transport.o gccRelease/Win32Transport.o 
	ar rcs gccRelease/libCollector.a gccRelease/Allocation.o gccRelease/Buffer.o gccRelease/CallStack.o gccRelease/CallStack_Win32.o gccRelease/Free.o gccRelease/MemoryOverrides.o gccRelease/Packet.o gccRelease/Registration.o gccRelease/Screenshot.o gccRelease/SetArchitecture.o gccRelease/SetSymbols.o gccRelease/Thread_Win32.o gccRelease/Transport.o gccRelease/Win32Transport.o  $(Release_Implicitly_Linked_Objects)

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

# Compiles file CallStack.cpp for the Release configuration...
-include gccRelease/CallStack.d
gccRelease/CallStack.o: CallStack.cpp
	$(CPP_COMPILER) $(Release_Preprocessor_Definitions) $(Release_Compiler_Flags) -c CallStack.cpp $(Release_Include_Path) -o gccRelease/CallStack.o
	$(CPP_COMPILER) $(Release_Preprocessor_Definitions) $(Release_Compiler_Flags) -MM CallStack.cpp $(Release_Include_Path) > gccRelease/CallStack.d

# Compiles file CallStack_Win32.cpp for the Release configuration...
-include gccRelease/CallStack_Win32.d
gccRelease/CallStack_Win32.o: CallStack_Win32.cpp
	$(CPP_COMPILER) $(Release_Preprocessor_Definitions) $(Release_Compiler_Flags) -c CallStack_Win32.cpp $(Release_Include_Path) -o gccRelease/CallStack_Win32.o
	$(CPP_COMPILER) $(Release_Preprocessor_Definitions) $(Release_Compiler_Flags) -MM CallStack_Win32.cpp $(Release_Include_Path) > gccRelease/CallStack_Win32.d

# Compiles file Free.cpp for the Release configuration...
-include gccRelease/Free.d
gccRelease/Free.o: Free.cpp
	$(CPP_COMPILER) $(Release_Preprocessor_Definitions) $(Release_Compiler_Flags) -c Free.cpp $(Release_Include_Path) -o gccRelease/Free.o
	$(CPP_COMPILER) $(Release_Preprocessor_Definitions) $(Release_Compiler_Flags) -MM Free.cpp $(Release_Include_Path) > gccRelease/Free.d

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

# Compiles file SetArchitecture.cpp for the Release configuration...
-include gccRelease/SetArchitecture.d
gccRelease/SetArchitecture.o: SetArchitecture.cpp
	$(CPP_COMPILER) $(Release_Preprocessor_Definitions) $(Release_Compiler_Flags) -c SetArchitecture.cpp $(Release_Include_Path) -o gccRelease/SetArchitecture.o
	$(CPP_COMPILER) $(Release_Preprocessor_Definitions) $(Release_Compiler_Flags) -MM SetArchitecture.cpp $(Release_Include_Path) > gccRelease/SetArchitecture.d

# Compiles file SetSymbols.cpp for the Release configuration...
-include gccRelease/SetSymbols.d
gccRelease/SetSymbols.o: SetSymbols.cpp
	$(CPP_COMPILER) $(Release_Preprocessor_Definitions) $(Release_Compiler_Flags) -c SetSymbols.cpp $(Release_Include_Path) -o gccRelease/SetSymbols.o
	$(CPP_COMPILER) $(Release_Preprocessor_Definitions) $(Release_Compiler_Flags) -MM SetSymbols.cpp $(Release_Include_Path) > gccRelease/SetSymbols.d

# Compiles file Thread_Win32.cpp for the Release configuration...
-include gccRelease/Thread_Win32.d
gccRelease/Thread_Win32.o: Thread_Win32.cpp
	$(CPP_COMPILER) $(Release_Preprocessor_Definitions) $(Release_Compiler_Flags) -c Thread_Win32.cpp $(Release_Include_Path) -o gccRelease/Thread_Win32.o
	$(CPP_COMPILER) $(Release_Preprocessor_Definitions) $(Release_Compiler_Flags) -MM Thread_Win32.cpp $(Release_Include_Path) > gccRelease/Thread_Win32.d

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

# Builds the Release configuration...
.PHONY: Release
Release: create_folders x64/gccRelease/Allocation.o x64/gccRelease/Buffer.o x64/gccRelease/CallStack.o x64/gccRelease/CallStack_Win32.o x64/gccRelease/Free.o x64/gccRelease/MemoryOverrides.o x64/gccRelease/Packet.o x64/gccRelease/Registration.o x64/gccRelease/Screenshot.o x64/gccRelease/SetArchitecture.o x64/gccRelease/SetSymbols.o x64/gccRelease/Thread_Win32.o x64/gccRelease/Transport.o x64/gccRelease/Win32Transport.o 
	ar rcs x64/gccRelease/libCollector.a x64/gccRelease/Allocation.o x64/gccRelease/Buffer.o x64/gccRelease/CallStack.o x64/gccRelease/CallStack_Win32.o x64/gccRelease/Free.o x64/gccRelease/MemoryOverrides.o x64/gccRelease/Packet.o x64/gccRelease/Registration.o x64/gccRelease/Screenshot.o x64/gccRelease/SetArchitecture.o x64/gccRelease/SetSymbols.o x64/gccRelease/Thread_Win32.o x64/gccRelease/Transport.o x64/gccRelease/Win32Transport.o  $(Release_Implicitly_Linked_Objects)

# Compiles file Allocation.cpp for the Release configuration...
-include x64/gccRelease/Allocation.d
x64/gccRelease/Allocation.o: Allocation.cpp
	$(CPP_COMPILER) $(Release_Preprocessor_Definitions) $(Release_Compiler_Flags) -c Allocation.cpp $(Release_Include_Path) -o x64/gccRelease/Allocation.o
	$(CPP_COMPILER) $(Release_Preprocessor_Definitions) $(Release_Compiler_Flags) -MM Allocation.cpp $(Release_Include_Path) > x64/gccRelease/Allocation.d

# Compiles file Buffer.cpp for the Release configuration...
-include x64/gccRelease/Buffer.d
x64/gccRelease/Buffer.o: Buffer.cpp
	$(CPP_COMPILER) $(Release_Preprocessor_Definitions) $(Release_Compiler_Flags) -c Buffer.cpp $(Release_Include_Path) -o x64/gccRelease/Buffer.o
	$(CPP_COMPILER) $(Release_Preprocessor_Definitions) $(Release_Compiler_Flags) -MM Buffer.cpp $(Release_Include_Path) > x64/gccRelease/Buffer.d

# Compiles file CallStack.cpp for the Release configuration...
-include x64/gccRelease/CallStack.d
x64/gccRelease/CallStack.o: CallStack.cpp
	$(CPP_COMPILER) $(Release_Preprocessor_Definitions) $(Release_Compiler_Flags) -c CallStack.cpp $(Release_Include_Path) -o x64/gccRelease/CallStack.o
	$(CPP_COMPILER) $(Release_Preprocessor_Definitions) $(Release_Compiler_Flags) -MM CallStack.cpp $(Release_Include_Path) > x64/gccRelease/CallStack.d

# Compiles file CallStack_Win32.cpp for the Release configuration...
-include x64/gccRelease/CallStack_Win32.d
x64/gccRelease/CallStack_Win32.o: CallStack_Win32.cpp
	$(CPP_COMPILER) $(Release_Preprocessor_Definitions) $(Release_Compiler_Flags) -c CallStack_Win32.cpp $(Release_Include_Path) -o x64/gccRelease/CallStack_Win32.o
	$(CPP_COMPILER) $(Release_Preprocessor_Definitions) $(Release_Compiler_Flags) -MM CallStack_Win32.cpp $(Release_Include_Path) > x64/gccRelease/CallStack_Win32.d

# Compiles file Free.cpp for the Release configuration...
-include x64/gccRelease/Free.d
x64/gccRelease/Free.o: Free.cpp
	$(CPP_COMPILER) $(Release_Preprocessor_Definitions) $(Release_Compiler_Flags) -c Free.cpp $(Release_Include_Path) -o x64/gccRelease/Free.o
	$(CPP_COMPILER) $(Release_Preprocessor_Definitions) $(Release_Compiler_Flags) -MM Free.cpp $(Release_Include_Path) > x64/gccRelease/Free.d

# Compiles file MemoryOverrides.cpp for the Release configuration...
-include x64/gccRelease/MemoryOverrides.d
x64/gccRelease/MemoryOverrides.o: MemoryOverrides.cpp
	$(CPP_COMPILER) $(Release_Preprocessor_Definitions) $(Release_Compiler_Flags) -c MemoryOverrides.cpp $(Release_Include_Path) -o x64/gccRelease/MemoryOverrides.o
	$(CPP_COMPILER) $(Release_Preprocessor_Definitions) $(Release_Compiler_Flags) -MM MemoryOverrides.cpp $(Release_Include_Path) > x64/gccRelease/MemoryOverrides.d

# Compiles file Packet.cpp for the Release configuration...
-include x64/gccRelease/Packet.d
x64/gccRelease/Packet.o: Packet.cpp
	$(CPP_COMPILER) $(Release_Preprocessor_Definitions) $(Release_Compiler_Flags) -c Packet.cpp $(Release_Include_Path) -o x64/gccRelease/Packet.o
	$(CPP_COMPILER) $(Release_Preprocessor_Definitions) $(Release_Compiler_Flags) -MM Packet.cpp $(Release_Include_Path) > x64/gccRelease/Packet.d

# Compiles file Registration.cpp for the Release configuration...
-include x64/gccRelease/Registration.d
x64/gccRelease/Registration.o: Registration.cpp
	$(CPP_COMPILER) $(Release_Preprocessor_Definitions) $(Release_Compiler_Flags) -c Registration.cpp $(Release_Include_Path) -o x64/gccRelease/Registration.o
	$(CPP_COMPILER) $(Release_Preprocessor_Definitions) $(Release_Compiler_Flags) -MM Registration.cpp $(Release_Include_Path) > x64/gccRelease/Registration.d

# Compiles file Screenshot.cpp for the Release configuration...
-include x64/gccRelease/Screenshot.d
x64/gccRelease/Screenshot.o: Screenshot.cpp
	$(CPP_COMPILER) $(Release_Preprocessor_Definitions) $(Release_Compiler_Flags) -c Screenshot.cpp $(Release_Include_Path) -o x64/gccRelease/Screenshot.o
	$(CPP_COMPILER) $(Release_Preprocessor_Definitions) $(Release_Compiler_Flags) -MM Screenshot.cpp $(Release_Include_Path) > x64/gccRelease/Screenshot.d

# Compiles file SetArchitecture.cpp for the Release configuration...
-include x64/gccRelease/SetArchitecture.d
x64/gccRelease/SetArchitecture.o: SetArchitecture.cpp
	$(CPP_COMPILER) $(Release_Preprocessor_Definitions) $(Release_Compiler_Flags) -c SetArchitecture.cpp $(Release_Include_Path) -o x64/gccRelease/SetArchitecture.o
	$(CPP_COMPILER) $(Release_Preprocessor_Definitions) $(Release_Compiler_Flags) -MM SetArchitecture.cpp $(Release_Include_Path) > x64/gccRelease/SetArchitecture.d

# Compiles file SetSymbols.cpp for the Release configuration...
-include x64/gccRelease/SetSymbols.d
x64/gccRelease/SetSymbols.o: SetSymbols.cpp
	$(CPP_COMPILER) $(Release_Preprocessor_Definitions) $(Release_Compiler_Flags) -c SetSymbols.cpp $(Release_Include_Path) -o x64/gccRelease/SetSymbols.o
	$(CPP_COMPILER) $(Release_Preprocessor_Definitions) $(Release_Compiler_Flags) -MM SetSymbols.cpp $(Release_Include_Path) > x64/gccRelease/SetSymbols.d

# Compiles file Thread_Win32.cpp for the Release configuration...
-include x64/gccRelease/Thread_Win32.d
x64/gccRelease/Thread_Win32.o: Thread_Win32.cpp
	$(CPP_COMPILER) $(Release_Preprocessor_Definitions) $(Release_Compiler_Flags) -c Thread_Win32.cpp $(Release_Include_Path) -o x64/gccRelease/Thread_Win32.o
	$(CPP_COMPILER) $(Release_Preprocessor_Definitions) $(Release_Compiler_Flags) -MM Thread_Win32.cpp $(Release_Include_Path) > x64/gccRelease/Thread_Win32.d

# Compiles file Transport.cpp for the Release configuration...
-include x64/gccRelease/Transport.d
x64/gccRelease/Transport.o: Transport.cpp
	$(CPP_COMPILER) $(Release_Preprocessor_Definitions) $(Release_Compiler_Flags) -c Transport.cpp $(Release_Include_Path) -o x64/gccRelease/Transport.o
	$(CPP_COMPILER) $(Release_Preprocessor_Definitions) $(Release_Compiler_Flags) -MM Transport.cpp $(Release_Include_Path) > x64/gccRelease/Transport.d

# Compiles file Win32Transport.cpp for the Release configuration...
-include x64/gccRelease/Win32Transport.d
x64/gccRelease/Win32Transport.o: Win32Transport.cpp
	$(CPP_COMPILER) $(Release_Preprocessor_Definitions) $(Release_Compiler_Flags) -c Win32Transport.cpp $(Release_Include_Path) -o x64/gccRelease/Win32Transport.o
	$(CPP_COMPILER) $(Release_Preprocessor_Definitions) $(Release_Compiler_Flags) -MM Win32Transport.cpp $(Release_Include_Path) > x64/gccRelease/Win32Transport.d

# Creates the intermediate and output folders for each configuration...
.PHONY: create_folders
create_folders:
	mkdir -p gccDebug
	mkdir -p x64/gccDebug
	mkdir -p gccRelease
	mkdir -p x64/gccRelease

# Cleans intermediate and output files (objects, libraries, executables)...
.PHONY: clean
clean:
	rm -f gccDebug/*.o
	rm -f gccDebug/*.d
	rm -f gccDebug/*.a
	rm -f gccDebug/*.so
	rm -f gccDebug/*.dll
	rm -f gccDebug/*.exe
	rm -f x64/gccDebug/*.o
	rm -f x64/gccDebug/*.d
	rm -f x64/gccDebug/*.a
	rm -f x64/gccDebug/*.so
	rm -f x64/gccDebug/*.dll
	rm -f x64/gccDebug/*.exe
	rm -f gccRelease/*.o
	rm -f gccRelease/*.d
	rm -f gccRelease/*.a
	rm -f gccRelease/*.so
	rm -f gccRelease/*.dll
	rm -f gccRelease/*.exe
	rm -f x64/gccRelease/*.o
	rm -f x64/gccRelease/*.d
	rm -f x64/gccRelease/*.a
	rm -f x64/gccRelease/*.so
	rm -f x64/gccRelease/*.dll
	rm -f x64/gccRelease/*.exe

