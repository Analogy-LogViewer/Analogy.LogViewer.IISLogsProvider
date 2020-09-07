using Analogy.Interfaces;
using Analogy.Interfaces.Factories;
using System;
using System.Collections.Generic;
using System.Drawing;
using Analogy.LogViewer.IISLogsProvider.Properties;

namespace Analogy.LogViewer.IISLogsProvider
{
    public class IISLogFactory : IAnalogyFactory
    {
        internal static Guid AnalogyIISFactoryGuid { get; } = new Guid("AD7187B1-BD09-4BFE-A28B-3FD3DE879009");
        public Guid FactoryId { get; set; } = AnalogyIISFactoryGuid;
        public string Title { get; set; } = "IIS Parser";
        public IEnumerable<IAnalogyChangeLog> ChangeLog { get; set; } = IISLogsProvider.ChangeLog.GetChangeLog();
        public IEnumerable<string> Contributors { get; set; } = new List<string> { "Lior Banai" };
        public string About { get; set; } = "Analogy IIS Parser";
        public Image LargeImage { get; set; } = Resources.AnalogyIIS32x32;
        public Image SmallImage { get; set; } = Resources.AnalogyIIS16x16;
    }

    public class AnalogyIISLogCustomActionFactory : IAnalogyCustomActionsFactory
    {
        public Guid FactoryId { get; set; } = IISLogFactory.AnalogyIISFactoryGuid;
        public string Title { get; set; } = "Analogy IIS Built-In tools";
        public IEnumerable<IAnalogyCustomAction> Actions { get; } = new List<IAnalogyCustomAction>(0);
    }
}
