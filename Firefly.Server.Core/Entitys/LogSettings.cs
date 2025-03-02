using Firefly.Server.Core.Interfaces;
using NLog;

namespace Firefly.Server.Core.Entitys;
    public class LogSettings : ISettings
{

    public string LogLevel { get; set; } // TODO Enum. None, INFO, DEBUG, TRACE, I think I'm missing one?
    public string logFilePath { get; set; } // Default: FireflyServer_${shortdate}.log

    public string logArchivePath { get; set; } // Default: FireflyServer_LogArchive.{#}.zip // TODO: Check variables. Can we have a date here?

    public string archiveEvery { get; set; } // Default: Day - Can this be an Enum?

    public string archiveNumbering { get; set; } // Default: Rolling - Can this be an Enum?

    public int maxArchiveFiles { get; set; } // Default: 10

    public bool Validate(ref List<string> messages)
    {
        bool validationPassed = true;

        // TODO - Validation

        return validationPassed;
    }

    public void ApplySettingsToNLog()
    {
        GlobalDiagnosticsContext.Set("logFilePath", logFilePath);
        GlobalDiagnosticsContext.Set("logArchivePath", logArchivePath);
        GlobalDiagnosticsContext.Set("archiveEvery", archiveEvery);
        GlobalDiagnosticsContext.Set("archiveNumbering", archiveNumbering);
        GlobalDiagnosticsContext.Set("maxArchiveFiles", maxArchiveFiles);
    }
}
