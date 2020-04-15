using Analogy.Interfaces;
using Analogy.Interfaces.DataTypes;
using Analogy.Interfaces.Factories;
using Analogy.LogViewer.IISLogsProvider.Managers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Analogy.LogViewer.IISLogsProvider
{
    public class IISLogsDataProviderFactory : IAnalogyDataProvidersFactory
    {
        public Guid FactoryId { get; } = IISLogFactory.AnalogyIISFactoryGuid;
        public string Title { get; } = "Analogy IIS Logs Data Provider";

        public IEnumerable<IAnalogyDataProvider> DataProviders { get; } =
            new List<IAnalogyDataProvider> { new AnalogyIISDataProvider() };

    }

    public class AnalogyIISDataProvider : IAnalogyOfflineDataProvider
    {
        public string OptionalTitle { get; } = "Analogy IIS Log Parser";

        public Guid ID { get; } = new Guid("44688C02-3156-45B1-B916-08DB96BCD358");

        public bool CanSaveToLogFile { get; } = false;
        public string FileOpenDialogFilters { get; } = "IIS log files|u_ex*.log";
        public string FileSaveDialogFilters { get; } = string.Empty;
        public IEnumerable<string> SupportFormats { get; } = new[] { "u_ex*.log" };
        public string InitialFolderFullPath { get; } = Environment.CurrentDirectory;
        public bool DisableFilePoolingOption { get; } = false;
        private ILogParserSettings LogParserSettings { get; set; }
        private IISFileParser IISFileParser { get; set; }
        private string NLogFileSetting { get; } = "AnalogyIISSettings.json";
        public bool UseCustomColors { get; set; } = false;
        public IEnumerable<(string originalHeader, string replacementHeader)> GetReplacementHeaders()
            => Array.Empty<(string, string)>();

        public (Color backgroundColor, Color foregroundColor) GetColorForMessage(IAnalogyLogMessage logMessage)
            => (Color.Empty, Color.Empty);

        public Task InitializeDataProviderAsync(IAnalogyLogger logger)
        {
            LogManager.Instance.SetLogger(logger);
            if (File.Exists(NLogFileSetting))
            {
                try
                {
                    LogParserSettings = JsonConvert.DeserializeObject<LogParserSettings>(NLogFileSetting);
                }
                catch (Exception)
                {
                    LogParserSettings = new LogParserSettings();
                    LogParserSettings.Splitter = " ";
                    LogParserSettings.SupportedFilesExtensions = new List<string> { "u_ex*.log" };
                }
            }
            else
            {
                LogParserSettings = new LogParserSettings();
                LogParserSettings.Splitter = " ";
                LogParserSettings.SupportedFilesExtensions = new List<string> { "u_ex*.log" };

            }
            IISFileParser = new IISFileParser(LogParserSettings);
            return Task.CompletedTask;
        }

        public void MessageOpened(AnalogyLogMessage message)
        {
            //do nothing
        }



        public async Task<IEnumerable<AnalogyLogMessage>> Process(string fileName, CancellationToken token, ILogMessageCreatedHandler messagesHandler)
        {
            if (CanOpenFile(fileName))
                return await IISFileParser.Process(fileName, token, messagesHandler);
            return new List<AnalogyLogMessage>(0);

        }

        public IEnumerable<FileInfo> GetSupportedFiles(DirectoryInfo dirInfo, bool recursiveLoad)
        => GetSupportedFilesInternal(dirInfo, recursiveLoad);

        public Task SaveAsync(List<AnalogyLogMessage> messages, string fileName)
        {
            throw new NotSupportedException("Saving is not supported for iis log");
        }

        public bool CanOpenFile(string fileName) => LogParserSettings.CanOpenFile(fileName);

        public bool CanOpenAllFiles(IEnumerable<string> fileNames) => fileNames.All(CanOpenFile);

        public static List<FileInfo> GetSupportedFilesInternal(DirectoryInfo dirInfo, bool recursive)
        {
            List<FileInfo> files = dirInfo.GetFiles("u_ex*.log").ToList();
            if (!recursive)
                return files;
            try
            {
                foreach (DirectoryInfo dir in dirInfo.GetDirectories())
                {
                    files.AddRange(GetSupportedFilesInternal(dir, true));
                }
            }
            catch (Exception)
            {
                return files;
            }

            return files;
        }
    }

    public class AnalogyIISUserSettings //: IAnalogyDataProviderSettings
    {
        public Task SaveSettingsAsync()
        {
            return Task.CompletedTask;
        }

        public string Title { get; } = "IIS Parser Settings";
        public UserControl DataProviderSettings { get; } = new IISUserSettingsUC();
        public Image SmallImage { get; } = Properties.Resources.AnalogyIIS16x16;
        public Image LargeImage { get; } = Properties.Resources.AnalogyIIS32x32;
        public Guid FactoryId { get; set; } = IISLogFactory.AnalogyIISFactoryGuid;
    }

}
