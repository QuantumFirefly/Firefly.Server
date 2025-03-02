using Firefly.Server.Core.Interfaces;
using NLog;
using NLog.Targets;

namespace Firefly.Server.Core.Entitys;
    public class LogSettings : ISettings
{

    public LogLevel LogLevel { get; set; } = LogLevel.Off;
    public string target { get; set; } = "File";
    public string filePath { get; set; }

    public string archivePath { get; set; }

    public FileArchivePeriod archiveEvery { get; set; } // Default: Day

    public ArchiveNumberingMode archiveNumbering { get; set; }

    public int maxArchiveFiles { get; set; } // Default: 10

    public long archiveAboveSize { get; set; }

    public string archiveDateFormat { get; set; }

    // This setting is important for large corporates who need to ensure integrity of user data.
    public bool redactPIIFromLogs { get; set; } = false; // PII - Personally identifying information. 
    // TODO - Move this to be stored in database.

    public bool Validate(ref List<string> messages)
    {
        bool validationPassed = true;

        // TODO - Validation

        return validationPassed;
    }

    public void ApplySettingsToNLog()
    {
        GlobalDiagnosticsContext.Set("filePath", filePath);
        GlobalDiagnosticsContext.Set("archivePath", archivePath);
        GlobalDiagnosticsContext.Set("archiveEvery", archiveEvery);
        GlobalDiagnosticsContext.Set("archiveNumbering", archiveNumbering);
        GlobalDiagnosticsContext.Set("maxArchiveFiles", maxArchiveFiles);
        GlobalDiagnosticsContext.Set("archiveAboveSize", archiveAboveSize);
        GlobalDiagnosticsContext.Set("archiveDateFormat", archiveDateFormat);
    }
}
