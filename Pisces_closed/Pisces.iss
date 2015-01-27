#define AppName "Pisces"
#define SrcApp ".\bin\x86\debug\Pisces.exe"
#define FileVerStr GetStringFileInfo(SrcApp, "ProductVersion")
;#define public StripBuild(str aVerStr) Copy(aVerStr, 1, RPos(".", aVerStr)-1)
;#define AppVerStr StripBuild(FileVerStr)
[Setup]
AppId={{9DC6B8F6-D59D-491E-8FCC-D601FE200836}
AppName={#AppName}
AppVersion={#FileVerStr}
AppVerName={#AppName} {#FileVerStr}
UninstallDisplayName={#AppName} {#FileVerStr}
VersionInfoVersion={#FileVerStr}
VersionInfoTextVersion={#FileVerStr}
OutputBaseFilename=Pisces2-setup


LicenseFile=.\..\license.md

;AppPublisher=Reclamation
DefaultDirName={sd}\Pisces2
DefaultGroupName=Pisces
;AppPublisherURL=http://www.usbr.gov/pn/hydromet/pisces
;AppSupportURL=http://forum.heidisql.com/
;AppUpdatesURL=http://download.heidisql.com/

Compression=lzma
SolidCompression=yes
;VersionInfoProductName="Pisces"
;VersionInfoCompany="Reclamation"
PrivilegesRequired=lowest

SetupIconFile=".\images\fish_icon_blue.ico"



[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked


[Files]
Source:  ".\bin\x86\debug\Pisces.exe";   DestDir: "{app}";   Flags: ignoreversion
Source:  ".\bin\x86\debug\plugins.txt";   DestDir: "{app}";   Flags: ignoreversion
Source:  ".\bin\x86\debug\Pisces.exe.config";   DestDir: "{app}";   Flags: ignoreversion
Source:  "..\private.config";   DestDir: "{app}";   Flags: ignoreversion
Source:  ".\bin\x86\debug\images\*";   DestDir: "{app}\images";   Flags: ignoreversion
Source:  ".\bin\x86\debug\Aga.Controls.dll";   DestDir: "{app}";   Flags: ignoreversion
Source:  ".\bin\x86\debug\ICSharpCode.SharpZipLib.dll";   DestDir: "{app}";   Flags: ignoreversion
Source:  ".\bin\x86\debug\Reclamation.Core.dll";   DestDir: "{app}";   Flags: ignoreversion
Source:  ".\bin\x86\debug\Reclamation.TimeSeries.dll";   DestDir: "{app}";   Flags: ignoreversion
Source:  ".\bin\x86\debug\Reclamation.TimeSeries.Excel.dll";   DestDir: "{app}";   Flags: ignoreversion
Source:  ".\bin\x86\debug\Reclamation.TimeSeries.Modsim.dll";   DestDir: "{app}";   Flags: ignoreversion
Source:  ".\bin\x86\debug\Reclamation.TimeSeries.Forms.dll";   DestDir: "{app}";   Flags: ignoreversion
Source:  ".\bin\x86\debug\Reclamation.TimeSeries.OracleHdb.dll";   DestDir: "{app}";   Flags: ignoreversion
Source:  ".\bin\x86\debug\Reclamation.TimeSeries.ArmyCorps.dll";   DestDir: "{app}";   Flags: ignoreversion
;Source:  ".\bin\x86\debug\Reclamation.TimeSeries.Urgsim.dll";   DestDir: "{app}";   Flags: ignoreversion
Source:  ".\bin\x86\debug\SpreadsheetGear.dll";   DestDir: "{app}";   Flags: ignoreversion
;Source:  ".\bin\x86\debug\System.Data.SqlServerCe.dll";   DestDir: "{app}";   Flags: ignoreversion
Source:  ".\bin\x86\debug\SQLite.Interop.dll";   DestDir: "{app}";   Flags: ignoreversion
Source:  ".\bin\x86\debug\System.Data.SQLite.dll";   DestDir: "{app}";   Flags: ignoreversion
Source:  ".\bin\x86\debug\TeeChart.dll";   DestDir: "{app}";   Flags: ignoreversion
Source:  ".\bin\x86\debug\hdb-poet.exe";   DestDir: "{app}";   Flags: ignoreversion
;Source:  ".\bin\x86\debug\Tamir.SharpSSH.dll";   DestDir: "{app}";   Flags: ignoreversion
Source:  ".\bin\x86\debug\DSSUTL.EXE";   DestDir: "{app}";   Flags: ignoreversion
Source:  ".\bin\x86\debug\Reclamation.TimeSeries.Graphing.dll";   DestDir: "{app}";   Flags: ignoreversion
Source:  ".\bin\x86\debug\Npgsql.dll";   DestDir: "{app}";   Flags: ignoreversion
Source:  ".\bin\x86\debug\Mono.Security.dll";   DestDir: "{app}";   Flags: ignoreversion
Source:  ".\bin\x86\debug\DgvFilterPopup.dll";   DestDir: "{app}";   Flags: ignoreversion

; ORACLE
Source:  "..\PaidThirdParty\Devart.Data.Oracle.dll";   DestDir: "{app}";   Flags: ignoreversion
Source:  "..\PaidThirdParty\Devart.Data.dll";   DestDir: "{app}";   Flags: ignoreversion

Source:  "V:\PN6200\Hydromet\ConfigurationData\site.csv";   DestDir: "{app}";   Flags: ignoreversion
Source:  "V:\PN6200\Hydromet\ConfigurationData\site_gp.csv";   DestDir: "{app}";   Flags: ignoreversion
Source:  "V:\PN6200\Hydromet\ConfigurationData\pcode.csv";   DestDir: "{app}";   Flags: ignoreversion
Source:  "V:\PN6200\Hydromet\ConfigurationData\pcode_gp.csv";   DestDir: "{app}";   Flags: ignoreversion
Source:  "V:\PN6200\Hydromet\ConfigurationData\site1.csv";   DestDir: "{app}";   Flags: ignoreversion

Source:  "V:\PN6200\Hydromet\ConfigurationData\nwcc_inventory.csv";   DestDir: "{app}";   Flags: ignoreversion
Source:  "V:\PN6200\Hydromet\ConfigurationData\snotel_site_list2.csv";   DestDir: "{app}";   Flags: ignoreversion
Source:  "V:\PN6200\Hydromet\ConfigurationData\boise_arc.dat";   DestDir: "{app}";   Flags: ignoreversion

Source:  ".\bin\x86\debug\alglibnet2.dll";   DestDir: "{app}";   Flags: ignoreversion
Source:  ".\bin\x86\debug\libsim.dll";   DestDir: "{app}";   Flags: ignoreversion
Source:  ".\bin\x86\debug\XYFile.dll";   DestDir: "{app}";   Flags: ignoreversion
Source:  ".\bin\x86\debug\A2CM.dll";   DestDir: "{app}";   Flags: ignoreversion
;
; Sample Data
;
;Source:  ".\pisces\sample data\SnakeRiverFlow.xls";   DestDir: "{app}\sample data";   Flags: ignoreversion
;Source:  ".\pisces\sample data\SnakeRiverTemperature.txt";   DestDir: "{app}\sample data";   Flags: ignoreversion
;Source:  ".\pisces\sample data\SpecificationTestData.xls";   DestDir: "{app}\sample data";   Flags: ignoreversion
;Source:  ".\pisces\sample data\HydrometDailyValues.sdf";   DestDir: "{app}\sample data";   Flags: ignoreversion

;Source:  ".\Database Samples\EMI RuleCurve.xls";   DestDir: "{app}\sample data";   Flags: ignoreversion
;Source:  ".\Database Samples\Talent2.sdf";   DestDir: "{app}\sample data";   Flags: ignoreversion


; NOTE: Don't use "Flags: ignoreversion" on any shared system files
[Icons]
Name: "{group}\Pisces2"; Filename: "{app}\Pisces.exe"
Name: "{commondesktop}\Pisces2"; Filename: "{app}\Pisces.exe"; Tasks: desktopicon

[Code]


function HaveDotNet20: boolean;
var ResultDWord:Cardinal;
begin
result:=RegQueryDWordValue(HKEY_LOCAL_MACHINE, 'SOFTWARE\Microsoft\NET Framework Setup\NDP\v2.0.50727','Install', ResultDWord);
if ResultDWord = 1 then
begin
result:= True;
end;
end;

function HaveDotNet35: boolean;
var ResultDWord:Cardinal;
begin
result:=RegQueryDWordValue(HKEY_LOCAL_MACHINE, 'Software\Microsoft\NET Framework Setup\NDP\v3.5','Install', ResultDWord);
if ResultDWord = 1 then
begin
result:= True;
end;
end;


function InitializeSetup(): Boolean;

begin
if not HaveDotNet35 then
  begin
 MsgBox('WARNING: You do not have the Microsoft .NET Framework 3.5 installed.  Pisces requires the .NET Framework 3.5 ', mbInformation, MB_OK);
 end ;
 result:= true;
 end;




function HaveOdp: boolean  ;
begin
result:= DirExists( ExpandConstant('{app}\oracle'));

end;

