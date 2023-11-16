using Analogy.Interfaces;
using Analogy.Interfaces.Factories;
using Analogy.LogViewer.IISLogsProvider.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Analogy.LogViewer.IISLogsProvider
{
    public class IISLogFactory : Analogy.LogViewer.Template.PrimaryFactory
    {
        internal static Guid Id { get; } = new Guid("AD7187B1-BD09-4BFE-A28B-3FD3DE879009");
        public override Guid FactoryId { get; set; } = Id;
        public override string Title { get; set; } = "IIS Parser";
        public override IEnumerable<IAnalogyChangeLog> ChangeLog { get; set; } = IISLogsProvider.ChangeLog.GetChangeLog();
        public override IEnumerable<string> Contributors { get; set; } = new List<string> { "Lior Banai" };
        public override string About { get; set; } = "Analogy IIS Parser";
        public override Image? LargeImage { get; set; } = Resources.AnalogyIIS32x32;
        public override Image? SmallImage { get; set; } = Resources.AnalogyIIS16x16;
    }

    public class AnalogyIISLogCustomActionFactory : IAnalogyCustomActionsFactory
    {
        public Guid FactoryId { get; set; } = IISLogFactory.Id;
        public string Title { get; set; } = "Analogy IIS Built-In tools";
        public IEnumerable<IAnalogyCustomAction> Actions { get; } = new List<IAnalogyCustomAction>(0);
    }
}