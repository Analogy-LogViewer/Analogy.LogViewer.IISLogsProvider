using Analogy.Interfaces;
using Analogy.Interfaces.DataTypes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Analogy.LogViewer.IISLogsProvider.UnitTests
{
    [TestClass]
    public class UnitTest1 : ILogMessageCreatedHandler
    {
        public void ReportFileReadProgress(AnalogyFileReadProgress progress)
        {
            //noop
        }

        public bool ForceNoFileCaching { get; set; } = true;
        public bool DoNotAddToRecentHistory { get; set; } = true;
        private LogParserSettings LogParserSettings { get; set; }
        private CancellationTokenSource CancellationTokenSource { get; set; }
        private string filename = "u_ex_Test.log";
        private List<AnalogyLogMessage> messages = new List<AnalogyLogMessage>();
        [TestMethod]
        public async Task TestMethod1()
        {
            messages.Clear();
            CancellationTokenSource = new CancellationTokenSource();
            LogParserSettings = new LogParserSettings();
            LogParserSettings.IsConfigured = true;
            LogParserSettings.SupportedFilesExtensions = new List<string> { "u_ex*.log" };
            IISFileParser p = new IISFileParser(LogParserSettings);

            var allMessages = (await p.Process(filename, CancellationTokenSource.Token, this)).ToList();
            Assert.IsTrue(allMessages.Count == 20 && allMessages.Count == messages.Count);
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
