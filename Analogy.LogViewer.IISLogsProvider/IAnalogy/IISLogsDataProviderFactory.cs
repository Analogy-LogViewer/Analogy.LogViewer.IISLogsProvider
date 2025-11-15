using Analogy.Interfaces;
using Analogy.LogViewer.IISLogsProvider.IAnalogy;
using Analogy.LogViewer.IISLogsProvider.Properties;
using Analogy.LogViewer.Template.WinForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Analogy.LogViewer.IISLogsProvider
{
    public class IISLogsDataProviderFactory : DataProvidersFactoryWinForms
    {
        public override Guid FactoryId { get; set; } = IISLogFactory.Id;
        public override string Title { get; set; } = "IIS Logs Data Provider";

        public override IEnumerable<IAnalogyDataProvider> DataProviders { get; set; } = new List<IAnalogyDataProvider> { new AnalogyIISDataProvider() };

        public IISLogsDataProviderFactory()
        {
            SmallImages[new Guid("44688C02-3156-45B1-B916-08DB96BCD358")] = Resources.AnalogyIIS16x16;
            LargeImages[new Guid("44688C02-3156-45B1-B916-08DB96BCD358")] = Resources.AnalogyIIS32x32;
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
        public Guid FactoryId { get; set; } = IISLogFactory.Id;
    }
}