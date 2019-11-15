using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Analogy.DataProviders.Extensions;
using Analogy.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Analogy.LogViewer.IISLogsProvider.UnitTests
{
    [TestClass]
    public class UnitTest1:ILogMessageCreatedHandler
    {

        public bool ForceNoFileCaching { get; set; } = true;
        public bool DoNotAddToRecentHistory { get; set; } = true;
        private LogParserSettings LogParserSettings { get; set; }
        private CancellationTokenSource CancellationTokenSource { get; set; }
        private string filename = @"d:\u_ex190909.log";
        private List<AnalogyLogMessage> messages;
        [TestMethod]
        public async Task TestMethod1()
        {
            CancellationTokenSource=new CancellationTokenSource();
            messages=new  List<AnalogyLogMessage>();
            LogParserSettings = new LogParserSettings();
            LogParserSettings.Splitter = " ";
            LogParserSettings.IsConfigured = true;
            LogParserSettings.SupportedFilesExtensions = new List<string> { "u_ex*.log" };
            IISFileParser p = new IISFileParser(LogParserSettings);

            await p.Process(filename, CancellationTokenSource.Token,this);
        }

        public void AppendMessage(AnalogyLogMessage message, string dataSource)
        {
            messages.Add(message);
        }

        public void AppendMessages(List<AnalogyLogMessage> messages, string dataSource)
        {
            this.messages.AddRange(messages);
        }

    }
}
