using Firefly.Server.Core.Enums;
using Microsoft.Extensions.Configuration;
using NLog;
using NLog.Targets;

namespace Firefly.Server.Core.LocalConfig;
public class LogConfig : IConfig
{

    public LogLevel logLevel { get; set; } = LogLevel.Off;
    public string target { get; set; } = "";
    public string filePath { get; set; } = "";

    private string _archivePath = "";
    public string archivePath {
        get => _archivePath;
        set {
            // LocalSettings.ini takes % instead of # because hashes are considered comments in ini files.
            _archivePath = value.Replace("%", "#");
        }
    }

    public FileArchivePeriod archiveEvery { get; set; }

    public ArchiveNumberingMode archiveNumbering { get; set; }

    public int maxArchiveFiles { get; set; }

    private long _archiveAboveSize;
    public long archiveAboveSize {
        get => _archiveAboveSize;
        set {
            // archiveAboveSize is taken from the ini files in MB. Convert to Bytes for NLog
            _archiveAboveSize = value * Constants.MB_TO_BYTES;
        }
    }

    public string archiveDateFormat { get; set; } = "";

    private string[] _targets => target.Split(",");

    public bool Validate(ref List<string> messages) {
        bool validationPassed = true;

        foreach (var target in _targets) {
            if (target != "file" && target != "console"
                || target.Length == 0) {
                messages.Add($"Target of {target} is not an acceptable logging target. Acceptable logging targets are file or console.");
                validationPassed = false;
            }
        }

        if (filePath.Length <= 1) {
            messages.Add($"Log file path should be longer than 1 character.");
            validationPassed = false;
        }

        if (archivePath.Length <= 1) {
            messages.Add($"Log archive file path should be longer than 1 character.");
            validationPassed = false;
        }

        if (maxArchiveFiles < 1) {
            messages.Add($"Maximum archive files should be 1 or more.");
            validationPassed = false;
        }

        if (archiveAboveSize < Constants.MB_TO_BYTES * 10) {
            messages.Add($"Archive above size should be 10MB or more.");
            validationPassed = false;
        }



        return validationPassed;
    }

    public static LogConfig Build(IConfigurationRoot iniContent) {
        var data = new LogConfig();

        data.logLevel = LogLevel.FromString(iniContent[$"Logging:LogLevel"] ?? "Off");
        data.target = iniContent[$"Logging:Target"] ?? "";
        data.filePath = iniContent[$"Logging:FilePath"] ?? "";
        data.archiveEvery = Enum.Parse<FileArchivePeriod>(iniContent[$"Logging:ArchiveEvery"] ?? "None");
        data.archiveNumbering = Enum.Parse<ArchiveNumberingMode>(iniContent[$"Logging:ArchiveNumbering"] ?? "");
        data.archivePath = iniContent[$"Logging:ArchivePath"] ?? "";
        data.maxArchiveFiles = int.Parse(iniContent[$"Logging:MaxArchiveFiles"] ?? "-1");
        data.archiveAboveSize = long.Parse(iniContent[$"Logging:ArchiveAboveSize"] ?? "-1");
        data.archiveDateFormat = iniContent[$"Logging:ArchiveDateFormat"] ?? "";

        return data;
    }

    public static void ApplySettingsToNLog(LogConfig settingsToApply) {
        var loggingConfig = LogManager.Configuration;

        // Set the LogLevel
        foreach (var rule in loggingConfig.LoggingRules) {
            rule.SetLoggingLevels(settingsToApply.logLevel, LogLevel.Fatal);

            rule.Targets.Clear(); // Remove existing rules.
            Array.ForEach(settingsToApply._targets, target => rule.Targets.Add(loggingConfig.FindTargetByName(target)));
        }

        // Override nlog.config with data from provided instance of LogSettings
        var fileTarget = loggingConfig.FindTargetByName<FileTarget>("file");
        if (fileTarget != null) {
            fileTarget.FileName = settingsToApply.filePath;
            fileTarget.ArchiveFileName = settingsToApply.archivePath;

            fileTarget.ArchiveEvery = settingsToApply.archiveEvery;
            fileTarget.MaxArchiveFiles = settingsToApply.maxArchiveFiles;
            fileTarget.ArchiveNumbering = settingsToApply.archiveNumbering;
            fileTarget.ArchiveAboveSize = settingsToApply.archiveAboveSize;
            fileTarget.ArchiveDateFormat = settingsToApply.archiveDateFormat;

        }

        LogManager.ReconfigExistingLoggers(); // Apply changes
    }
}
