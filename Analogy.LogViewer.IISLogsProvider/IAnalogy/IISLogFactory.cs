using Analogy.Interfaces;
using Analogy.Interfaces.Factories;
using System;
using System.Collections.Generic;

namespace Analogy.LogViewer.IISLogsProvider
{
    public class IISLogFactory : IAnalogyFactory
    {
        public static Guid AnalogyIISFactoryGuid { get; } = new Guid("AD7187B1-BD09-4BFE-A28B-3FD3DE879009");
        public Guid FactoryID { get; } = AnalogyIISFactoryGuid;
        public string Title { get; } = "IIS Parser";
        public IAnalogyDataProvidersFactory DataProviders { get; }
        public IAnalogyCustomActionsFactory Actions { get; }
        public IEnumerable<IAnalogyChangeLog> ChangeLog => IISLogsProvider.ChangeLog.GetChangeLog();
        public IEnumerable<string> Contributors { get; } = new List<string> { "Lior Banai" };
        public string About { get; } = "Analogy IIS Parser";

        public IISLogFactory()
        {
            DataProviders = new IISLogsDataProviderFactory();
            Actions = new AnalogyIISLogCustomActionFactory();
        }


    }


    public class AnalogyIISLogCustomActionFactory : IAnalogyCustomActionsFactory
    {
        public string Title { get; } = "Analogy IIS Built-In tools";
        public IEnumerable<IAnalogyCustomAction> Items { get; }

        public AnalogyIISLogCustomActionFactory()
        {
            Items = new List<IAnalogyCustomAction>(0);
        }
    }
}
