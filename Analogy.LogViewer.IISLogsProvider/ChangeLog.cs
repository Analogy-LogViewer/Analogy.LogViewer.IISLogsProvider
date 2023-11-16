using Analogy.Interfaces;
using System;
using System.Collections.Generic;

namespace Analogy.LogViewer.IISLogsProvider
{
    public static class ChangeLog
    {
        public static IEnumerable<AnalogyChangeLog> GetChangeLog()
        {
            yield return new AnalogyChangeLog("IIS Data Provider: Initial release", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2019, 11, 14), "");
        }
    }
}