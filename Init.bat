set CurrentDir=%cd%

cd /d %CurrentDir%/Share/Tool/
dotnet build Share.Tool.csproj

cd /d %CurrentDir%/Bin
dotnet Tool.dll --AppType=ExcelExporter
dotnet Tool.dll --AppType=Proto2CS
echo success
pause