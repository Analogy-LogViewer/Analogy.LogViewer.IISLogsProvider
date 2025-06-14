using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace Analogy.LogViewer.IISLogsProvider
{
    public class IISUserSettings
    {
        public bool UseApplicationFolderForSettings { get; set; }
        public bool UseGeoLocationService { get; set; }
        public string GeolocationServiceApi { get; set; } = "http://ip-api.com/json/";
    }
}