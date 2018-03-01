#define AppName "Hydromet Tools"
#define SrcApp ".\bin\debug\HydrometTools.exe"
#define FileVerStr GetFileVersion(SrcApp)
#define public StripBuild(str aVerStr) Copy(aVerStr, 1, RPos(".", aVerStr)-1)
#define AppVerStr StripBuild(FileVerStr)
[Setup]
AppId={{16D58642-80F4-4DB0-ABC4-3C40193CF82A}
AppName={#AppName}
AppVersion={#AppVerStr}
AppVerName={#AppName} {#AppVerStr}
AppPublisher=Reclamation
DefaultDirName={sd}\HydrometTools
DefaultGroupName=HydrometTools
OutputBaseFilename=HydrometTools-setup
Compression=lzma
SolidCompression=yes
VersionInfoVersion={#FileVerStr}
VersionInfoTextVersion={#AppVerStr}
VersionInfoProductName="HydrometTools
VersionInfoCompany="Reclamation"
PrivilegesRequired=none
;show dialogs even if previous install found
DisableWelcomePage=no
DisableDirPage=no
DisableProgramGroupPage=no
AlwaysShowDirOnReadyPage=yes
AlwaysShowGroupOnReadyPage=yes

[InstallDelete]
Type: files;  Name: "{app}\reclamationcgi.csv"

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}";  GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked


[Files]
Source:  "bin\debug\HydrometTools.exe";   DestDir: "{app}";   Flags: ignoreversion
Source:  "bin\debug\HydrometNotifications.exe";   DestDir: "{app}";   Flags: ignoreversion
Source:  "bin\debug\HydrometTools.exe.config";   DestDir: "{app}";   Flags: ignoreversion
Source:  "bin\debug\Aga.Controls.dll";   DestDir: "{app}";   Flags: ignoreversion
Source:  "bin\debug\ICSharpCode.SharpZipLib.dll";   DestDir: "{app}";   Flags: ignoreversion
Source:  "bin\debug\Reclamation.Core.dll";   DestDir: "{app}";   Flags: ignoreversion
Source:  "bin\debug\Reclamation.TimeSeries.dll";   DestDir: "{app}";   Flags: ignoreversion
Source:  "bin\debug\Reclamation.TimeSeries.Excel.dll";   DestDir: "{app}";   Flags: ignoreversion
Source:  "bin\debug\Reclamation.TimeSeries.Forms.dll";   DestDir: "{app}";   Flags: ignoreversion
Source:  "bin\debug\Reclamation.TimeSeries.Graphing.dll";   DestDir: "{app}";   Flags: ignoreversion
Source:  "bin\debug\Pisces.exe";   DestDir: "{app}";   Flags: ignoreversion
Source:  "bin\debug\pscp.exe";   DestDir: "{app}";   Flags: ignoreversion
;Source:  "bin\debug\Excel.dll";   DestDir: "{app}";   Flags: ignoreversion
Source:  "bin\debug\SpreadsheetGear.dll";   DestDir: "{app}";   Flags: ignoreversion
Source:  "bin\debug\TeeChart.dll";   DestDir: "{app}";   Flags: ignoreversion
Source:  "bin\debug\HydrometNotifications.exe";   DestDir: "{app}";   Flags: ignoreversion
Source:  "bin\debug\Npgsql.dll";   DestDir: "{app}";   Flags: ignoreversion
Source:  "bin\debug\ZedGraph.dll";   DestDir: "{app}";   Flags: ignoreversion
Source:  "bin\debug\DgvFilterPopup.dll";   DestDir: "{app}";   Flags: ignoreversion
Source:  "bin\debug\System.Threading.Tasks.Extensions.dll";   DestDir: "{app}";   Flags: ignoreversion
Source:  "bin\debug\Renci.SshNet.dll";   DestDir: "{app}";   Flags: ignoreversion
Source:  "bin\debug\Newtonsoft.Json.dll";   DestDir: "{app}";   Flags: ignoreversion
Source:  "bin\debug\RestSharp.dll";   DestDir: "{app}";   Flags: ignoreversion

Source:  ".\bin\debug\x86\SQLite.Interop.dll";   DestDir: "{app}\x86";   Flags: ignoreversion
Source:  ".\bin\debug\x64\SQLite.Interop.dll";   DestDir: "{app}\x64";   Flags: ignoreversion
Source:  ".\bin\debug\System.Data.SQLite.dll";   DestDir: "{app}";   Flags: ignoreversion


Source:  "bin\debug\YakimaStatusTemplate.txt";   DestDir: "{app}";   Flags: ignoreversion
Source:  "bin\debug\YakimaOthersAboveParker.csv";   DestDir: "{app}";   Flags: ignoreversion
Source:  "..\cfg\gp\*.*";   DestDir: "{app}\cfg\gp";   Flags: ignoreversion
Source:  "..\cfg\yak\*.*";   DestDir: "{app}\cfg\yak";   Flags: ignoreversion
Source:  "..\cfg\rating_tables\*.*";   DestDir: "{app}\cfg\rating_tables";   Flags: ignoreversion
Source:  "..\cfg\RuleCurves\*.*";   DestDir: "{app}\cfg\RuleCurves";   Flags: ignoreversion
Source:  "..\cfg\site.csv";   DestDir: "{app}\cfg";   Flags: ignoreversion
Source:  "..\cfg\pcode.csv";   DestDir: "{app}\cfg";   Flags: ignoreversion
Source:  "..\cfg\daily_pcode.csv";   DestDir: "{app}\cfg";   Flags: ignoreversion
Source:  "..\cfg\instant_pcode.csv";   DestDir: "{app}\cfg";   Flags: ignoreversion
Source:  "..\cfg\monthly_pcode.csv";   DestDir: "{app}\cfg";   Flags: ignoreversion
Source:  "..\cfg\data_import_sites.csv";   DestDir: "{app}\cfg";   Flags: ignoreversion
Source:  "..\cfg\goes.csv";   DestDir: "{app}\cfg";   Flags: ignoreversion


Source:  "..\cfg\group.csv";   DestDir: "{app}\cfg";   Flags: ignoreversion
Source:  "..\cfg\cc.dat";   DestDir: "{app}\cfg";   Flags: ignoreversion
Source:  "..\cfg\nwcc_inventory.csv";   DestDir: "{app}\cfg";   Flags: ignoreversion
Source:  "..\cfg\snotel_site_list2.csv";   DestDir: "{app}\cfg";   Flags: ignoreversion
Source:  "..\cfg\boise_arc.dat";   DestDir: "{app}\cfg";   Flags: ignoreversion
Source:  "..\cfg\owrd_station_list.csv";   DestDir: "{app}\cfg";   Flags: ignoreversion
Source:  "..\cfg\mpoll.cbt";   DestDir: "{app}\cfg";   Flags: ignoreversion
Source:  "..\cfg\mpoll_inventory.txt";   DestDir: "{app}\cfg";   Flags: ignoreversion

; Hydromet tools specific  - below
Source:  "C:\utils\private\reclamationcgi.csv";   DestDir: "{app}\cfg";   Flags: ignoreversion
;ource:  "C:\utils\private\RuleCurves.xlsx";   DestDir: "{app}\cfg";   Flags: ignoreversion
Source:  "C:\utils\private\hydromet-tools-private.config";   DestDir: "{app}";   Flags: ignoreversion
Source:  "..\cfg\RatingTableTemplate.xls";   DestDir: "{app}\cfg";   Flags: ignoreversion
Source:  "..\cfg\snowgg_groups.csv";   DestDir: "{app}\cfg";   Flags: ignoreversion
Source:  "..\cfg\snowgg_groups_gp.csv";   DestDir: "{app}\cfg";   Flags: ignoreversion
Source:  "C:\HydrometTools\cfg\timeseries_gp.pdb";   DestDir: "{app}\cfg";   Flags: ignoreversion


                   
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{group}\HydrometTools"; Filename: "{app}\HydrometTools.exe";  WorkingDir: {app};
Name: "{commondesktop}\HydrometTools"; Filename: "{app}\HydrometTools.exe"; WorkingDir: {app}; Tasks: desktopicon

[Run]
Filename: "{app}\HydrometTools.exe"; Description: "{cm:LaunchProgram,HydrometTools}"; Flags: nowait postinstall skipifsilent

[Code]

function HaveDotNet4: boolean;
var ResultDWord:Cardinal;
begin
result:=RegQueryDWordValue(HKEY_LOCAL_MACHINE, 'Software\Microsoft\NET Framework Setup\NDP\v4\Full','Install', ResultDWord);
if ResultDWord = 1 then
begin
result:= True;
end;
end;

function InitializeSetup(): Boolean;

begin
if not HaveDotNet4 then
  begin
 MsgBox('WARNING: You do not have the Microsoft .NET Framework 4.0 installed.  HydrometTools requires the .NET Framework 4 ', mbInformation, MB_OK);
 result:=false;
 end ;
 result:= true;
end;


