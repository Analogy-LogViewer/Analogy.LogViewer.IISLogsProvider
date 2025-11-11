using Analogy.Interfaces;
using Analogy.Interfaces.WinForms;
using Analogy.LogViewer.IISLogsProvider.IAnalogy;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Analogy.LogViewer.IISLogsProvider
{
    public class IISLogsDataProviderFactory : LogViewer.Template.DataProvidersFactoryWinForms
    {
        public override Guid FactoryId { get; set; } = IISLogFactory.Id;
        public override string Title { get; set; } = "IIS Logs Data Provider";

        public override IEnumerable<IAnalogyDataProviderWinForms> DataProviders { get; set; } = new List<IAnalogyDataProviderWinForms> { new AnalogyIISDataProvider() };
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