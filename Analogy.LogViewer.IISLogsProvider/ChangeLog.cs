using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Analogy.Interfaces;

namespace Analogy.LogViewer.IISLogsProvider
{
    public static class ChangeLog
    {
        public static IEnumerable<AnalogyChangeLog> GetChangeLog()
        {
            yield return new AnalogyChangeLog("IIS Data Provider: Initial release", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2019, 11, 14));

        }
    }
}
