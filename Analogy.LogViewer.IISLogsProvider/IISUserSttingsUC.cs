using System.Runtime;
using System.Windows.Forms;

namespace Analogy.LogViewer.IISLogsProvider
{
    public partial class IISUserSettingsUC : UserControl
    {
        public IISUserSettingsUC()
        {
            InitializeComponent();
        }

        public void SaveSettings()
        {
            UserSettingsManager.UserSettings.Save();
        }

        private void IISUserSettingsUC_Load(object sender, System.EventArgs e)
        {
            cbGeoService.Checked = UserSettingsManager.UserSettings.Settings.UseGeoLocationService;
            if (UserSettingsManager.UserSettings.Settings.UseApplicationFolderForSettings)
            {
                rbtnApplicationFolder.Checked = true;
            }
            else
            {
                rbtnPerUser.Checked = true;
            }
        }

        private void btnSave_Click(object sender, System.EventArgs e)
        {
            UserSettingsManager.UserSettings.Settings.UseApplicationFolderForSettings = rbtnApplicationFolder.Checked;
            UserSettingsManager.UserSettings.Settings.UseGeoLocationService = cbGeoService.Checked;
            UserSettingsManager.UserSettings.Save();
        }
    }
}