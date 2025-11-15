using Analogy.Interfaces.DataTypes;
using Analogy.Interfaces.WinForms.DataTypes;
using Analogy.LogViewer.IISLogsProvider.Properties;
using Analogy.LogViewer.Template.WinForms;
using Microsoft.Extensions.Logging;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Analogy.LogViewer.IISLogsProvider.IAnalogy
{
    internal class UserSettingsFactory : TemplateUserSettingsFactoryWinForms
    {
        public override Guid FactoryId { get; set; } = IISLogFactory.Id;
        public override Guid Id { get; set; } = new Guid("fe9d38dc-dd31-4f15-8aee-acb7f7e9085b");
        public override UserControl DataProviderSettings { get; set; }
        public override string Title { get; set; } = "Example User Settings";
        public override Image? SmallImage { get; set; } = Resources.iis_Icon_16x16;
        public override Image? LargeImage { get; set; } = Resources.iis_Icon_32x32;
        public override AnalogyToolTip ToolTip { get; set; } = new AnalogyToolTipWithImages("IIS settings", "Settings",
            "IIS settings", Resources.iis_Icon_16x16, Resources.iis_Icon_32x32);
        public override void CreateUserControl(ILogger logger)
        {
            DataProviderSettings = new IISUserSettingsUC();
        }

        public override Task SaveSettingsAsync()
        {
            ((IISUserSettingsUC)DataProviderSettings)?.SaveSettings();
            return Task.CompletedTask;
        }
    }
}