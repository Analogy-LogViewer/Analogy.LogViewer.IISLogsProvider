using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Analogy.DataProviders.Extensions;
using Analogy.Interfaces;

namespace Analogy.LogViewer.IISLogsProvider
{
    public class IISFileParser
    {
        private Dictionary<string, (string value, string description)> Mapping;
        private Dictionary<string, Action<string, AnalogyLogMessage>> ActionMapping;
        private ILogParserSettings _logFileSettings;
        private Dictionary<int, string> columnIndexToName;
        private const string fieldHeader = "#Fields:";
        private string[] splitters;
        public IISFileParser(ILogParserSettings logParserSettings)
        {
            _logFileSettings = logParserSettings;
            splitters = new[] { logParserSettings.Splitter };
            Mapping = new Dictionary<string, (string value, string description)>
            {
                {"date", ("Date", "The date on which the activity occurred")},
                {"time", ("Time", "The time, in coordinated universal time (UTC), at which the activity occurred")},
                {"c-ip", ("Client IP Address", "The IP address of the client that made the request")},
                {"cs-username", ("User Name", "The name of the authenticated user who accessed your server. Anonymous users are indicated by a hyphen")},
                {"s-sitename", ("Service Name and Instance Number", "The Internet service name and instance number that was running on the client")},
                {"s-computername", ("Server Name", "The name of the server on which the log file entry was generated")},
                {"s-ip", ("Server IP Address", "The IP address of the server on which the log file entry was generated")},
                {"s-port", ("Server Port", "The server port number that is configured for the service")},
                {"cs-method", ("Method", "The requested action, for example, a GET method")},
                {"cs-uri-stem", ("URI Stem", "The target of the action, for example, Default.htm")},
                {"cs-uri-query", ("URI Query", "The query, if any that the client was trying to perform. A Universal Resource Identifier (URI) query is necessary only for dynamic pages")},
                {"sc-status", ("HTTP Status", "The HTTP status code")},
                {"sc-win32-status", ("Win32 Status", "The Windows status code")},
                {"sc-bytes", ("Bytes Sent", "The number of bytes that the server sent")},
                {"cs-bytes", ("Bytes Received", "The number of bytes that the server received")},
                {"time-taken", ("Time Taken", "The length of time that the action took, in milliseconds")},

                {"cs-version", ("Protocol Version", "The protocol version —HTTP or FTP —that the client used.")},
                {"cs-host", ("Host", "The host header name, if any")},
                {"cs(User-Agent)", ("User Agent", "The browser type that the client used")},
                {"cs(Cookie)", ("Cookie", "The content of the cookie sent or received if any.")},
                {"cs(Referer)", ("Referer", "The site that the user last visited. This site provided a link to the current site")},
                {"sc-substatus", ("Protocol Substatus", "The sub status error code")}

            };

            ActionMapping = new Dictionary<string, Action<string, AnalogyLogMessage>>
            {

                {"date", (val,m)=>
                    {
                        if (DateTime.TryParse(val, out DateTime dt))
                            m.Date = dt.Date;
                    }
                },
                {
                    "time",  (val,m)=>
                    {
                        if (DateTime.TryParse(val, out DateTime dt))
                            m.Date = m.Date.Date.Add(dt.TimeOfDay);
                    }

                },
                {"c-ip",  (val,m)=>
                    {
                        m.Text += val == "-" ? string.Empty : $"Client ip: {val + Environment.NewLine}";
                        m.Source += $"Client ip: {val}.";
                    }
                },
                {"cs-username",  (val,m)=>
                    {
                        m.Text += val == "-"
                            ? string.Empty
                            : $"{Mapping["cs-username"].value}: {val + Environment.NewLine}";
                        m.User = val;
                    }
                },
                {"s-sitename",  (val,m)=>m.Text +=val=="-" ?string.Empty : $"{Mapping["s-sitename"].value}: {val+Environment.NewLine}"},
                {"s-computername",  (val,m)=>m.Text +=val=="-" ?string.Empty : $"{Mapping["s-computername"].value}: {val+Environment.NewLine}"},
                {"s-ip",   (val,m)=>
                    {
                        m.Text += val == "-" ? string.Empty : $"{Mapping["s-ip"].value}: {val + Environment.NewLine}";
                        m.Source += $"Server ip: {val}.";
                    }
                },
                {"s-port",   (val,m)=>m.Text +=val=="-" ?string.Empty : $"{Mapping["s-port"].value}: {val+Environment.NewLine}"},
                {"cs-method",   (val,m)=>
                    {
                        m.Text += val == "-"
                            ? string.Empty
                            : $"{Mapping["cs-method"].value}: {val + Environment.NewLine}";
                        m.Category = val;
                    }
                },
                {"cs-uri-stem",   (val,m)=>
                    {
                        m.Text += val == "-"
                            ? string.Empty
                            : $"{Mapping["cs-uri-stem"].value}: {val + Environment.NewLine}";
                        m.MethodName = val;
                    }
                },
                {"cs-uri-query",   (val,m)=>m.Text +=val=="-" ?string.Empty : $"{Mapping["cs-uri-query"].value}: {val+Environment.NewLine}"},
                {"sc-status",   (val,m)=>
                    {
                        m.Text += val == "-"
                            ? string.Empty
                            : $"{Mapping["sc-status"].value}: {val + Environment.NewLine}";
                        if (int.TryParse(val,out int status))
                        {
                            if (status >= 100 && status <= 199)
                                m.Level = AnalogyLogLevel.Debug;
                            if (status >= 200 && status <= 299)
                                m.Level = AnalogyLogLevel.Event;
                            if (status >= 300 && status <= 399)
                                m.Level = AnalogyLogLevel.Event;
                            if (status >= 400 && status <= 499)
                                m.Level = AnalogyLogLevel.Error;
                            if (status >= 500 && status <= 599)
                                m.Level = AnalogyLogLevel.Error;
                        }
                    }
                },
                {"sc-win32-status",   (val,m)=>m.Text +=val=="-" ?string.Empty : $"{Mapping["sc-win32-status"].value}: {val+Environment.NewLine}"},
                {"sc-bytes",   (val,m)=>m.Text +=val=="-" ?string.Empty : $"{Mapping["sc-bytes"].value}: {val+Environment.NewLine}"},
                {"cs-bytes",   (val,m)=>m.Text +=val=="-" ?string.Empty : $"{Mapping["cs-bytes"].value}: {val+Environment.NewLine}"},
                {"time-taken",   (val,m)=>m.Text +=val=="-" ?string.Empty : $"{Mapping["time-taken"].value}: {val+Environment.NewLine}"},
                {"cs-version",  (val,m)=>m.Text +=val=="-" ?string.Empty : $"{Mapping["cs-version"].value}: {val+Environment.NewLine}"},
                {"cs-host",  (val,m)=>m.Text +=val=="-" ?string.Empty : $"{Mapping["cs-host"].value}: {val+Environment.NewLine}"},
                {"cs(User-Agent)",   (val,m)=>m.Text +=val=="-" ?string.Empty : $"{Mapping["cs(User-Agent)"].value}: {val+Environment.NewLine}"},
                {"cs(Cookie)",   (val,m)=>m.Text +=val=="-" ?string.Empty : $"{Mapping["cs(Cookie)"].value}: {val+Environment.NewLine}"},
                {"cs(Referer)",   (val,m)=>
                    {
                        m.Text += val == "-"
                            ? string.Empty
                            : $"{Mapping["cs(Referer)"].value}: {val + Environment.NewLine}";
                        m.Module =val;
                    }
                },
                {"sc-substatus",   (val,m)=>m.Text +=val=="-" ?string.Empty : $"{Mapping["sc-substatus"].value}: {val+Environment.NewLine}"}
                };


        }


        public async Task<IEnumerable<AnalogyLogMessage>> Process(string fileName, CancellationToken token, ILogMessageCreatedHandler messagesHandler)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                AnalogyLogMessage empty = new AnalogyLogMessage($"File is null or empty. Aborting.",
                    AnalogyLogLevel.Critical, AnalogyLogClass.General, "Analogy", "None")
                {
                    Source = "Analogy",
                    Module = System.Diagnostics.Process.GetCurrentProcess().ProcessName
                };
                messagesHandler.AppendMessage(empty, Utils.GetFileNameAsDataSource(fileName));
                return new List<AnalogyLogMessage> { empty };
            }
            if (!_logFileSettings.CanOpenFile(fileName))
            {
                AnalogyLogMessage empty = new AnalogyLogMessage($"File {fileName} Is not supported or not configured correctly in the windows settings",
                    AnalogyLogLevel.Critical, AnalogyLogClass.General, "Analogy", "None")
                {
                    Source = "Analogy",
                    Module = System.Diagnostics.Process.GetCurrentProcess().ProcessName
                };
                messagesHandler.AppendMessage(empty, Utils.GetFileNameAsDataSource(fileName));
                return new List<AnalogyLogMessage> { empty };
            }
            List<AnalogyLogMessage> messages = new List<AnalogyLogMessage>();
            try
            {
                using (var stream = File.OpenRead(fileName))
                {
                    using (var reader = new StreamReader(stream))
                    {
                        while (!reader.EndOfStream)
                        {
                            var line = await reader.ReadLineAsync();
                            if (line.StartsWith("#Software:", StringComparison.CurrentCultureIgnoreCase) ||
                                line.StartsWith("#Version:", StringComparison.CurrentCultureIgnoreCase) ||
                                line.StartsWith("#Date:", StringComparison.CurrentCultureIgnoreCase) ||
                                line.StartsWith(fieldHeader, StringComparison.CurrentCultureIgnoreCase))
                            {
                                var headerMsg = HandleHeaderMessage(line, fileName);
                                messagesHandler.AppendMessage(headerMsg, Utils.GetFileNameAsDataSource(fileName));
                                messages.Add(headerMsg);
                                continue;
                            }
                            var items = line.Split(splitters, StringSplitOptions.None);
                            var entry = Parse(items);
                            entry.FileName = fileName;
                            messages.Add(entry);
                        }
                    }
                }
                messagesHandler.AppendMessages(messages, fileName);
                return messages;
            }
            catch (Exception e)
            {
                AnalogyLogMessage empty = new AnalogyLogMessage($"Error occured processing file {fileName}. Reason: {e.Message}",
                    AnalogyLogLevel.Critical, AnalogyLogClass.General, "Analogy", "None")
                {
                    Source = "Analogy",
                    Module = System.Diagnostics.Process.GetCurrentProcess().ProcessName
                };
                messagesHandler.AppendMessage(empty, Utils.GetFileNameAsDataSource(fileName));
                return new List<AnalogyLogMessage> { empty };
            }
        }

        private AnalogyLogMessage HandleHeaderMessage(string line, string filename)
        {
            var m = new AnalogyLogMessage("Header: " + line, AnalogyLogLevel.Event, AnalogyLogClass.General, filename,
                "", "IIS");
            m.Date = DateTime.MinValue;

            if (line.StartsWith(fieldHeader, StringComparison.CurrentCultureIgnoreCase))
            {
                line = line.Remove(0, fieldHeader.Length);
                //generate map
                var items = line.Split(splitters, StringSplitOptions.RemoveEmptyEntries).ToList();
                columnIndexToName = items.ToDictionary(itm => items.IndexOf(itm), itm => itm);
            }
            return m;
        }

        private AnalogyLogMessage Parse(string[] items)
        {
            AnalogyLogMessage m = new AnalogyLogMessage();
            for (var index = 0; index < items.Length; index++)
            {
                string value = items[index];
                string field = columnIndexToName[index];
                ActionMapping[field](value, m);
            }

            return m;
        }
    }
}
