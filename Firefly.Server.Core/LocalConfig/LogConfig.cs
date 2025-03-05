using Firefly.Server.Core.Enums;
using Microsoft.Extensions.Configuration;
using NLog;
using NLog.Targets;

namespace Firefly.Server.Core.LocalConfig;
public class LogConfig : IConfig
{

    public LogLevel LogLevel { get; set; } = LogLevel.Off;
    private string[] Targets => Target.Split(",");
    public string Target { get; set; } = "";
    public string FilePath { get; set; } = "";

    private string _archivePath = "";
    public string ArchivePath {
        get => _archivePath;
        set {
            // LocalSettings.ini takes % instead of # because hashes are considered comments in ini files.
            _archivePath = value.Replace("%", "#");
        }
    }

    public FileArchivePeriod ArchiveEvery { get; set; }

    public ArchiveNumberingMode ArchiveNumbering { get; set; }

    public int MaxArchiveFiles { get; set; }

    private long _archiveAboveSize;
    public long ArchiveAboveSize {
        get => _archiveAboveSize;
        set {
            // archiveAboveSize is taken from the ini files in MB. Convert to Bytes for NLog
            _archiveAboveSize = value * Constants.MB_TO_BYTES;
        }
    }

    public string ArchiveDateFormat { get; set; } = "";

    public bool Validate(ref List<string> messages) {
        bool validationPassed = true;

        foreach (var target in Targets) {
            if (target != "file" && target != "console"
                || target.Length == 0) {
                messages.Add($"Target of {target} is not an acceptable logging target. Acceptable logging targets are file or console.");
                validationPassed = false;
            }
        }

        if (FilePath.Length <= 1) {
            messages.Add($"Log file path should be longer than 1 character.");
            validationPassed = false;
        }

        if (ArchivePath.Length <= 1) {
            messages.Add($"Log archive file path should be longer than 1 character.");
            validationPassed = false;
        }

        if (MaxArchiveFiles < 1) {
            messages.Add($"Maximum archive files should be 1 or more.");
            validationPassed = false;
        }

        if (ArchiveAboveSize < Constants.MB_TO_BYTES * 10) {
            messages.Add($"Archive above size should be 10MB or more.");
            validationPassed = false;
        }



        return validationPassed;
    }

    public static LogConfig Build(IConfigurationRoot iniContent) {
        var data = new LogConfig {
            LogLevel = LogLevel.FromString(iniContent[$"Logging:LogLevel"] ?? "Off"),
            Target = iniContent[$"Logging:Target"] ?? "",
            FilePath = iniContent[$"Logging:FilePath"] ?? "",
            ArchiveEvery = Enum.Parse<FileArchivePeriod>(iniContent[$"Logging:ArchiveEvery"] ?? "None"),
            ArchiveNumbering = Enum.Parse<ArchiveNumberingMode>(iniContent[$"Logging:ArchiveNumbering"] ?? ""),
            ArchivePath = iniContent[$"Logging:ArchivePath"] ?? "",
            MaxArchiveFiles = int.Parse(iniContent[$"Logging:MaxArchiveFiles"] ?? "-1"),
            ArchiveAboveSize = long.Parse(iniContent[$"Logging:ArchiveAboveSize"] ?? "-1"),
            ArchiveDateFormat = iniContent[$"Logging:ArchiveDateFormat"] ?? ""
        };

        

        return data;
    }

    public static void ApplySettingsToNLog(LogConfig settingsToApply) {
        var loggingConfig = LogManager.Configuration;

        // Set the LogLevel
        foreach (var rule in loggingConfig.LoggingRules) {
            rule.SetLoggingLevels(settingsToApply.LogLevel, LogLevel.Fatal);

            rule.Targets.Clear(); // Remove existing rules.
            Array.ForEach(settingsToApply.Targets, target => rule.Targets.Add(loggingConfig.FindTargetByName(target)));
        }

        // Override nlog.config with data from provided instance of LogSettings
        var fileTarget = loggingConfig.FindTargetByName<FileTarget>("file");
        if (fileTarget != null) {
            fileTarget.FileName = settingsToApply.FilePath;
            fileTarget.ArchiveFileName = settingsToApply.ArchivePath;

            fileTarget.ArchiveEvery = settingsToApply.ArchiveEvery;
            fileTarget.MaxArchiveFiles = settingsToApply.MaxArchiveFiles;
            fileTarget.ArchiveNumbering = settingsToApply.ArchiveNumbering;
            fileTarget.ArchiveAboveSize = settingsToApply.ArchiveAboveSize;
            fileTarget.ArchiveDateFormat = settingsToApply.ArchiveDateFormat;

        }

        LogManager.ReconfigExistingLoggers(); // Apply changes
    }
}
