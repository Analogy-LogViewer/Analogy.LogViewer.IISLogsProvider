using Analogy.LogViewer.Template.Managers;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace Analogy.LogViewer.IISLogsProvider
{
    public class UserSettingsManager
    {
        private static readonly Lazy<UserSettingsManager> _instance =
             new Lazy<UserSettingsManager>(() => new UserSettingsManager());
        public static UserSettingsManager UserSettings { get; set; } = _instance.Value;
        private string LocalSettingFileName { get; } = "AnalogyIIsSettings.json";

        public string IisPerUserFileSetting => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Analogy.LogViewer", LocalSettingFileName);
        public IISUserSettings Settings { get; set; }

        public UserSettingsManager()
        {
            //check if local file exist:
            var loaded = LoadFileSettings(LocalSettingFileName, true);
            if (!loaded)
            {
                LoadFileSettings(IisPerUserFileSetting, false);
            }
        }

        private bool LoadFileSettings(string localSettingFileName, bool optional)
        {
            if (File.Exists(localSettingFileName))
            {
                try
                {
                    var settings = new JsonSerializerSettings
                    {
                        ObjectCreationHandling = ObjectCreationHandling.Replace,
                    };
                    string data = File.ReadAllText(localSettingFileName);
                    Settings = JsonConvert.DeserializeObject<IISUserSettings>(data, settings);
                    if (string.IsNullOrEmpty(Settings.GeolocationServiceApi))
                    {
                        Settings.GeolocationServiceApi = "http://ip-api.com/json/";
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    LogManager.Instance?.LogWarning($"Error loading user setting file: {ex.Message}", "Analogy Serilog Parser");
                    Settings = new IISUserSettings();
                    return true;
                }
            }
            else
            {
                if (!optional)
                {
                    Settings = new IISUserSettings();
                    return false;
                }
            }

            return false;
        }

        public void Save()
        {
            try
            {
                if (Settings.UseApplicationFolderForSettings)
                {
                    File.WriteAllText(LocalSettingFileName, JsonConvert.SerializeObject(Settings));
                }
                else
                {
                    if (File.Exists(LocalSettingFileName))
                    {
                        try
                        {
                            File.Delete(LocalSettingFileName);
                        }
                        catch (Exception e)
                        {
                            LogManager.Instance.LogError($"Error deleting local file: {e.Message}");
                        }
                    }
                    File.WriteAllText(IisPerUserFileSetting, JsonConvert.SerializeObject(Settings));
                }
            }
            catch (Exception e)
            {
                LogManager.Instance.LogError(e, "Error saving settings: " + e.Message);
            }
        }
    }
}