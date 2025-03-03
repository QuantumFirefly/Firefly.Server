using Firefly.Server.Core.Interfaces;
using NLog;
using NLog.Targets;

namespace Firefly.Server.Core.Entitys;
    public class LogSettings : ISettings
{

    public LogLevel logLevel { get; set; } = LogLevel.Off;
    public string target { get; set; } = "";
    public string filePath { get; set; }

    private string _archivePath;
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
    public long archiveAboveSize
    {
        get => _archiveAboveSize;
        set
        {
            // archiveAboveSize is taken from the ini files in MB. Convert to Bytes for NLog
            _archiveAboveSize = value * Constants.MB_TO_BYTES;
        }
    }

    public string archiveDateFormat { get; set; }

    public bool Validate(ref List<string> messages)
    {
        bool validationPassed = true;

        foreach (var target in _targets)
        {
            if ((target != "file" && target != "console")
                || target.Length == 0)
            {
                messages.Add($"Target of {target} is not an acceptable logging target. Acceptable logging targets are file or console.");
                validationPassed = false;
            }
        }

        if (filePath.Length <= 1)
        {
            messages.Add($"Log file path should be longer than 1 character.");
            validationPassed = false;
        }

        if (archivePath.Length <= 1)
        {
            messages.Add($"Log archive file path should be longer than 1 character.");
            validationPassed = false;
        }

        if (maxArchiveFiles < 1)
        {
            messages.Add($"Maximum archive files should be 1 or more.");
            validationPassed = false;
        }

        if (archiveAboveSize < Constants.MB_TO_BYTES * 10)
        {
            messages.Add($"Archive above size should be 10MB or more.");
            validationPassed = false;
        }

        

        return validationPassed;
    }

    public void ApplySettingsToNLog()
    {
        // Set the LogLevel
        var loggingConfig = LogManager.Configuration;
        foreach (var rule in loggingConfig.LoggingRules)
        {
            rule.SetLoggingLevels(logLevel, LogLevel.Fatal);

            rule.Targets.Clear(); // Remove existing rules.
            Array.ForEach(_targets, target => rule.Targets.Add(loggingConfig.FindTargetByName(target)));
        }

        // Override nlog.config with data from LocalSettings.ini
        var fileTarget = loggingConfig.FindTargetByName<FileTarget>("file");
        if (fileTarget != null)
        {
            fileTarget.FileName = filePath;
            fileTarget.ArchiveFileName = archivePath;

            fileTarget.ArchiveEvery = archiveEvery;
            fileTarget.MaxArchiveFiles = maxArchiveFiles;
            fileTarget.ArchiveNumbering = archiveNumbering;
            fileTarget.ArchiveAboveSize = archiveAboveSize;
            fileTarget.ArchiveDateFormat = archiveDateFormat;

        }

        LogManager.ReconfigExistingLoggers(); // Apply changes
    }

    private string[] _targets => target.Split(",");

}
