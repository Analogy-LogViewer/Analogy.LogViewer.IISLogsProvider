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
    public class IISLogsDataProviderFactory : LogViewer.Template.DataProvidersFactory
    {
        public override Guid FactoryId { get; set; } = IISLogFactory.AnalogyIISFactoryGuid;
        public override string Title { get; set; } = "IIS Logs Data Provider";

        public override IEnumerable<IAnalogyDataProvider> DataProviders { get; set; } = new List<IAnalogyDataProvider> { new AnalogyIISDataProvider() };

    }

    public class AnalogyIISDataProvider : Analogy.LogViewer.Template.OfflineDataProvider
    {
        public override string? OptionalTitle { get; set; } = "Analogy IIS Log Parser";

        public override Guid Id { get; set; } = new Guid("44688C02-3156-45B1-B916-08DB96BCD358");
        public override Image? LargeImage { get; set; } = null;
        public override Image? SmallImage { get; set; } = null;
        public override string FileOpenDialogFilters { get; set; } = "IIS log files|u_ex*.log";
        public override IEnumerable<string> SupportFormats { get; set; } = new[] { "u_ex*.log" };
        private ILogParserSettings LogParserSettings { get; set; }
        private IISFileParser IISFileParser { get; set; }
        private string iisFileSetting { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Analogy.LogViewer", "AnalogyIISSettings.json");

        public override async Task InitializeDataProviderAsync(IAnalogyLogger logger)
        {
            await base.InitializeDataProviderAsync(logger);
            LogManager.Instance.SetLogger(logger);
            if (File.Exists(iisFileSetting))
            {
                try
                {
                    LogParserSettings = JsonConvert.DeserializeObject<LogParserSettings>(iisFileSetting);
                }
                catch (Exception)
                {
                    LogParserSettings = new LogParserSettings();
                    LogParserSettings.IsConfigured = true;
                    LogParserSettings.SupportedFilesExtensions = new List<string> { "u_ex*.log" };
                }
            }
            else
            {
                LogParserSettings = new LogParserSettings();
                LogParserSettings.IsConfigured = true;
                LogParserSettings.SupportedFilesExtensions = new List<string> { "u_ex*.log" };

            }
            IISFileParser = new IISFileParser(LogParserSettings);
        }

        public override async Task<IEnumerable<AnalogyLogMessage>> Process(string fileName, CancellationToken token, ILogMessageCreatedHandler messagesHandler)
        {
            if (CanOpenFile(fileName))
                return await IISFileParser.Process(fileName, token, messagesHandler);
            return new List<AnalogyLogMessage>(0);

        }


        public override bool CanOpenFile(string fileName) => LogParserSettings.CanOpenFile(fileName);

        protected override List<FileInfo> GetSupportedFilesInternal(DirectoryInfo dirInfo, bool recursive)
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
