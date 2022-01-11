dotnet publish -r win-x64 -c Release -p:PublishReadyToRun=true -p:PublishSingleFile=true -p:PublishTrimmed=true --self-contained true -p:IncludeNativeLibrariesForSelfExtract=true
dotnet publish -r osx.10.14-x64 -c Release /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true
dotnet publish -r ubuntu.18.04-x64 -c Release /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true
dotnet publish -r linux-x64 -c Release /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true

cd test

copy /Y "C:\Stuff\HdHomerunM3U\HdHomerunM3U\bin\Release\net5.0\win-x64\publish\HdHomerunM3U.exe" .
"C:\Program Files\7-Zip\7z" a -tzip HdHomerunM3U-WIN.zip HdHomerunM3U.exe

copy /Y "C:\Stuff\HdHomerunM3U\HdHomerunM3U\bin\Release\net5.0\osx.10.14-x64\publish\HdHomerunM3U" .
"C:\Program Files\7-Zip\7z" a -t7z HdHomerunM3U-OSX.7z HdHomerunM3U

copy /Y "C:\Stuff\HdHomerunM3U\HdHomerunM3U\bin\Release\net5.0\ubuntu.18.04-x64\publish\HdHomerunM3U" .
"C:\Program Files\7-Zip\7z" a -t7z HdHomerunM3U-UBU.7z HdHomerunM3U

copy /Y "C:\Stuff\HdHomerunM3U\HdHomerunM3U\bin\Release\net5.0\linux-x64\publish\HdHomerunM3U" .
"C:\Program Files\7-Zip\7z" a -t7z HdHomerunM3U-LIN64.7z HdHomerunM3U
