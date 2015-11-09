#define AppName "Pisces"
#define SrcApp ".\bin\x86\debug\Pisces.exe"
#define FileVerStr GetStringFileInfo(SrcApp, "ProductVersion")

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

DefaultDirName={sd}\Pisces2
DefaultGroupName=Pisces

Compression=lzma
SolidCompression=yes
PrivilegesRequired=lowest

SetupIconFile=".\images\Fish_icon_3.ico"



[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked


[Files]
Source:  ".\bin\debug\Pisces.exe";   DestDir: "{app}";   Flags: ignoreversion
Source:  ".\bin\debug\plugins.txt";   DestDir: "{app}";   Flags: ignoreversion
Source:  ".\bin\debug\Pisces.exe.config";   DestDir: "{app}";   Flags: ignoreversion
Source:  "..\private.config";   DestDir: "{app}";   Flags: ignoreversion
Source:  ".\bin\debug\images\*";   DestDir: "{app}\images";   Flags: ignoreversion
Source:  ".\bin\debug\Aga.Controls.dll";   DestDir: "{app}";   Flags: ignoreversion
Source:  ".\bin\debug\ICSharpCode.SharpZipLib.dll";   DestDir: "{app}";   Flags: ignoreversion
Source:  ".\bin\debug\Reclamation.Core.dll";   DestDir: "{app}";   Flags: ignoreversion
Source:  ".\bin\debug\Reclamation.TimeSeries.dll";   DestDir: "{app}";   Flags: ignoreversion
Source:  ".\bin\debug\Reclamation.TimeSeries.Excel.dll";   DestDir: "{app}";   Flags: ignoreversion
Source:  ".\bin\debug\Reclamation.TimeSeries.Modsim.dll";   DestDir: "{app}";   Flags: ignoreversion
Source:  ".\bin\debug\Reclamation.TimeSeries.Forms.dll";   DestDir: "{app}";   Flags: ignoreversion
Source:  ".\bin\debug\Reclamation.TimeSeries.OracleHdb.dll";   DestDir: "{app}";   Flags: ignoreversion
Source:  ".\bin\debug\Reclamation.TimeSeries.ArmyCorps.dll";   DestDir: "{app}";   Flags: ignoreversion
;Source:  ".\bin\debug\Reclamation.TimeSeries.Urgsim.dll";   DestDir: "{app}";   Flags: ignoreversion
Source:  ".\bin\debug\SpreadsheetGear.dll";   DestDir: "{app}";   Flags: ignoreversion
Source:  ".\bin\debug\SQLite.Interop.dll";   DestDir: "{app}";   Flags: ignoreversion
Source:  ".\bin\debug\System.Data.SQLite.dll";   DestDir: "{app}";   Flags: ignoreversion
Source:  ".\bin\debug\TeeChart.dll";   DestDir: "{app}";   Flags: ignoreversion
Source:  ".\bin\debug\hdb-poet.exe";   DestDir: "{app}";   Flags: ignoreversion
;Source:  ".\bin\debug\Tamir.SharpSSH.dll";   DestDir: "{app}";   Flags: ignoreversion
Source:  ".\bin\debug\DSSUTL.EXE";   DestDir: "{app}";   Flags: ignoreversion
Source:  ".\bin\debug\Reclamation.TimeSeries.Graphing.dll";   DestDir: "{app}";   Flags: ignoreversion
Source:  ".\bin\debug\Npgsql.dll";   DestDir: "{app}";   Flags: ignoreversion
Source:  ".\bin\debug\Mono.Security.dll";   DestDir: "{app}";   Flags: ignoreversion
Source:  ".\bin\debug\DgvFilterPopup.dll";   DestDir: "{app}";   Flags: ignoreversion

; ORACLE
Source:  "..\PaidThirdParty\Devart.Data.Oracle.dll";   DestDir: "{app}";   Flags: ignoreversion
Source:  "..\PaidThirdParty\Devart.Data.dll";   DestDir: "{app}";   Flags: ignoreversion

Source:  "t:\PN6200\Hydromet\ConfigurationData\gp\*.*";   DestDir: "{app}\gp";   Flags: ignoreversion
Source:  "t:\PN6200\Hydromet\ConfigurationData\yak\*.*";   DestDir: "{app}\yak";   Flags: ignoreversion
Source:  "t:\PN6200\Hydromet\ConfigurationData\site.csv";   DestDir: "{app}";   Flags: ignoreversion
Source:  "t:\PN6200\Hydromet\ConfigurationData\pcode.csv";   DestDir: "{app}";   Flags: ignoreversion
Source:  "t:\PN6200\Hydromet\ConfigurationData\site1.csv";   DestDir: "{app}";   Flags: ignoreversion

Source:  "t:\PN6200\Hydromet\ConfigurationData\nwcc_inventory.csv";   DestDir: "{app}";   Flags: ignoreversion
Source:  "t:\PN6200\Hydromet\ConfigurationData\snotel_site_list2.csv";   DestDir: "{app}";   Flags: ignoreversion
Source:  "t:\PN6200\Hydromet\ConfigurationData\boise_arc.dat";   DestDir: "{app}";   Flags: ignoreversion

; Modsim
Source:  ".\bin\debug\alglibnet2.dll";   DestDir: "{app}";   Flags: ignoreversion
Source:  ".\bin\debug\libsim.dll";   DestDir: "{app}";   Flags: ignoreversion
Source:  ".\bin\debug\XYFile.dll";   DestDir: "{app}";   Flags: ignoreversion
Source:  ".\bin\debug\A2CM.dll";   DestDir: "{app}";   Flags: ignoreversion
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


function IsDotNetDetected(version: string; service: cardinal): boolean;
// Indicates whether the specified version and service pack of the .NET Framework is installed.
//
// version -- Specify one of these strings for the required .NET Framework version:
//    'v1.1.4322'     .NET Framework 1.1
//    'v2.0.50727'    .NET Framework 2.0
//    'v3.0'          .NET Framework 3.0
//    'v3.5'          .NET Framework 3.5
//    'v4\Client'     .NET Framework 4.0 Client Profile
//    'v4\Full'       .NET Framework 4.0 Full Installation
//
// service -- Specify any non-negative integer for the required service pack level:
//    0               No service packs required
//    1, 2, etc.      Service pack 1, 2, etc. required
var
    key: string;
    install, serviceCount: cardinal;
    success: boolean;
begin
    key := 'SOFTWARE\Microsoft\NET Framework Setup\NDP\' + version;
    // .NET 3.0 uses value InstallSuccess in subkey Setup
    if Pos('v3.0', version) = 1 then begin
        success := RegQueryDWordValue(HKLM, key + '\Setup', 'InstallSuccess', install);
    end else begin
        success := RegQueryDWordValue(HKLM, key, 'Install', install);
    end;
    // .NET 4.0 uses value Servicing instead of SP
    if Pos('v4', version) = 1 then begin
        success := success and RegQueryDWordValue(HKLM, key, 'Servicing', serviceCount);
    end else begin
        success := success and RegQueryDWordValue(HKLM, key, 'SP', serviceCount);
    end;
    result := success and (install = 1) and (serviceCount >= service);
end;

function InitializeSetup(): Boolean;
begin
    if not IsDotNetDetected('v4\Full', 0) then begin
        MsgBox('MyApp requires Microsoft .NET Framework 4.0 '#13#13
            'Please use Windows Update to install this version,'#13
            'and then re-run the Pisces setup program.', mbInformation, MB_OK);
        result := false;
    end else
        result := true;
end;

