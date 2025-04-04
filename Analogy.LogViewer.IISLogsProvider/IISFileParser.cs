using Analogy.Interfaces;
using Analogy.Interfaces.DataTypes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Analogy.LogViewer.IISLogsProvider
{
    public class IISFileParser
    {
        private Dictionary<string, (string Value, string Description)> Mapping;
        private Dictionary<string, Action<string, AnalogyLogMessage>> ActionMapping;
        private ILogParserSettings _logFileSettings;
        private Dictionary<int, string> columnIndexToName;
        private const string fieldHeader = "#Fields:";
        private string[] splitters = [" "];

        private Action<string, string, AnalogyLogMessage> CustomPropertyAppender = (key, val, m) =>
        {
            m.Text += val == "-" ? string.Empty : $"{Environment.NewLine} {key}:{val}";
            m.AddOrReplaceAdditionalProperty(key, val);
        };
        public IISFileParser(ILogParserSettings logParserSettings)
        {
            _logFileSettings = logParserSettings;

            Mapping = new Dictionary<string, (string Value, string Description)>
(StringComparer.Ordinal)
            {
                { "date", ("Date", "The date on which the activity occurred")},
                { "time", ("Time", "The time, in coordinated universal time (UTC), at which the activity occurred")},
                { "c-ip", ("Client IP Address", "The IP address of the client that made the request")},
                { "cs-username", ("User Name", "The name of the authenticated user who accessed your server. Anonymous users are indicated by a hyphen")},
                { "s-sitename", ("Service Name and Instance Number", "The Internet service name and instance number that was running on the client")},
                { "s-computername", ("Server Name", "The name of the server on which the log file entry was generated")},
                { "s-ip", ("Server IP Address", "The IP address of the server on which the log file entry was generated")},
                { "s-port", ("Server Port", "The server port number that is configured for the service")},
                { "cs-method", ("Method", "The requested action, for example, a GET method")},
                { "cs-uri-stem", ("URI Stem", "The target of the action, for example, Default.htm")},
                { "cs-uri-query", ("URI Query", "The query, if any that the client was trying to perform. A Universal Resource Identifier (URI) query is necessary only for dynamic pages")},
                { "sc-status", ("HTTP Status", "The HTTP status code")},
                { "sc-win32-status", ("Win32 Status", "The Windows status code")},
                { "sc-bytes", ("Bytes Sent", "The number of bytes that the server sent")},
                { "cs-bytes", ("Bytes Received", "The number of bytes that the server received")},
                { "time-taken", ("Time Taken", "The length of time that the action took, in milliseconds")},

                { "cs-version", ("Protocol Version", "The protocol version —HTTP or FTP —that the client used.")},
                { "cs-host", ("Host", "The host header name, if any")},
                { "cs(User-Agent)", ("User Agent", "The browser type that the client used")},
                { "cs(Cookie)", ("Cookie", "The content of the cookie sent or received if any.")},
                { "cs(Referer)", ("Referer", "The site that the user last visited. This site provided a link to the current site")},
                { "sc-substatus", ("Protocol Substatus", "The sub status error code")},
            };

            ActionMapping = new Dictionary<string, Action<string, AnalogyLogMessage>>
(StringComparer.Ordinal)
            {
                {
                    "date", (val, m)=>
                    {
                        if (DateTimeOffset.TryParse(val, out DateTimeOffset dt))
                        {
                            m.Date = dt;
                        }
                    }
                },
                {
                    "time",  (val, m) =>
                    {
                        if (DateTimeOffset.TryParse(val, out DateTimeOffset dt))
                        {
                            m.Date = dt;
                        }
                    }
                },
                {
                    "c-ip",  (val, m)=>
                    {
                        m.Text += val == "-" ? string.Empty : $"Client ip: {val + Environment.NewLine}";
                        m.Source += $"Client ip: {val}.";
                        m.AddOrReplaceAdditionalProperty("c-ip", $"Client ip: {val}.", StringComparer.Ordinal);
                    }
                },
                {
                    "cs-username",  (val, m)=>
                    {
                        m.Text += val == "-"
                            ? string.Empty
                            : $"{Mapping["cs-username"].Value}: {val + Environment.NewLine}";
                        m.User = val;
                    }
                },
                {
                    "s-sitename",  (val, m)=>
                    {
                        m.Text += val == "-"
                            ? string.Empty
                            : $"{Mapping["s-sitename"].Value}: {val + Environment.NewLine}";
                        m.AddOrReplaceAdditionalProperty("s-sitename", val, StringComparer.Ordinal);
                    }
                },
                {
                    "s-computername",  (val, m)=>
                    {
                        m.Text += val == "-"
                            ? string.Empty
                            : $"{Mapping["s-computername"].Value}: {val + Environment.NewLine}";
                        m.AddOrReplaceAdditionalProperty("s-computername", val, StringComparer.Ordinal);
                    }
                },
                {
                    "s-ip",   (val, m)=>
                    {
                        m.Text += val == "-" ? string.Empty : $"{Mapping["s-ip"].Value}: {val + Environment.NewLine}";
                        m.Source += $"Server ip: {val}.";
                        m.AddOrReplaceAdditionalProperty("s-ip", $"Server ip: {val}.", StringComparer.Ordinal);
                    }
                },
                {
                    "s-port",   (val, m)=>
                    {
                        m.Text += val == "-" ? string.Empty : $"{Mapping["s-port"].Value}: {val + Environment.NewLine}";
                        m.AddOrReplaceAdditionalProperty("s-port", val, StringComparer.Ordinal);
                    }
                },
                {
                    "cs-method",   (val, m)=>
                    {
                        m.Text += val == "-"
                            ? string.Empty
                            : $"{Mapping["cs-method"].Value}: {val + Environment.NewLine}";
                        m.MethodName = val;
                        m.AddOrReplaceAdditionalProperty("cs-method", val, StringComparer.Ordinal);
                    }
                },
                {
                    "cs-uri-stem",   (val, m)=>
                    {
                        m.Text += val == "-"
                            ? string.Empty
                            : $"{Mapping["cs-uri-stem"].Value}: {val + Environment.NewLine}";
                        m.AddOrReplaceAdditionalProperty("cs-uri-stem", val, StringComparer.Ordinal);
                    }
                },
                {
                    "cs-uri-query",   (val, m)=>
                    {
                        m.Text += val == "-"
                            ? string.Empty
                            : $"{Mapping["cs-uri-query"].Value}: {val + Environment.NewLine}";
                        m.AddOrReplaceAdditionalProperty("cs-uri-query", val, StringComparer.Ordinal);
                    }
                },
                {
                    "sc-status",   (val, m)=>
                    {
                        m.AddOrReplaceAdditionalProperty("sc-status", val, StringComparer.Ordinal);
                        m.Text += val == "-"
                            ? string.Empty
                            : $"{Mapping["sc-status"].Value}: {val + Environment.NewLine}";
                        if (int.TryParse(val, out int status))
                        {
                            if (status is >= 100 and <= 199)
                            {
                                m.Level = AnalogyLogLevel.Debug;
                            }

                            if (status is >= 200 and <= 299)
                            {
                                m.Level = AnalogyLogLevel.Information;
                            }

                            if (status is >= 300 and <= 399)
                            {
                                m.Level = AnalogyLogLevel.Information;
                            }

                            if (status is >= 400 and <= 499)
                            {
                                m.Level = AnalogyLogLevel.Error;
                            }

                            if (status is >= 500 and <= 599)
                            {
                                m.Level = AnalogyLogLevel.Error;
                            }
                        }
                    }
                },
                {
                    "sc-win32-status",   (val, m)=>
                    {
                        m.AddOrReplaceAdditionalProperty("sc-win32-status", val, StringComparer.Ordinal);
                        m.Text += val == "-"
                            ? string.Empty
                            : $"{Mapping["sc-win32-status"].Value}: {val + Environment.NewLine}";
                    }
                },
                {
                    "sc-bytes",   (val, m)=>
                    {
                        m.AddOrReplaceAdditionalProperty("sc-bytes", val, StringComparer.Ordinal);
                        m.Text += val == "-"
                            ? string.Empty
                            : $"{Mapping["sc-bytes"].Value}: {val + Environment.NewLine}";
                    }
                },
                {
                    "cs-bytes",   (val, m)=>
                    {
                        m.AddOrReplaceAdditionalProperty("cs-bytes", val, StringComparer.Ordinal);
                        m.Text += val == "-"
                            ? string.Empty
                            : $"{Mapping["cs-bytes"].Value}: {val + Environment.NewLine}";
                    }
                },
                {
                    "time-taken",   (val, m)=>
                    {
                        m.AddOrReplaceAdditionalProperty("time-taken", val, StringComparer.Ordinal);
                        m.Text += val == "-"
                            ? string.Empty
                            : $"{Mapping["time-taken"].Value}: {val + Environment.NewLine}";
                    }
                },
                {
                    "cs-version",  (val, m)=>
                    {
                        m.AddOrReplaceAdditionalProperty("cs-version", val, StringComparer.Ordinal);
                        m.Text += val == "-"
                            ? string.Empty
                            : $"{Mapping["cs-version"].Value}: {val + Environment.NewLine}";
                    }
                },
                {
                    "cs-host",  (val, m)=>
                    {
                        m.AddOrReplaceAdditionalProperty("cs-host", val, StringComparer.Ordinal);
                        m.Text += val == "-"
                            ? string.Empty
                            : $"{Mapping["cs-host"].Value}: {val + Environment.NewLine}";
                    }
                },
                {
                    "cs(User-Agent)",   (val, m)=>
                    {
                        m.AddOrReplaceAdditionalProperty("cs(User-Agent)", val, StringComparer.Ordinal);
                        m.Text += val == "-"
                            ? string.Empty
                            : $"{Mapping["cs(User-Agent)"].Value}: {val + Environment.NewLine}";
                    }
                },
                {
                    "cs(Cookie)",   (val, m)=>
                    {
                        m.AddOrReplaceAdditionalProperty("cs(Cookie)", val, StringComparer.Ordinal);
                        m.Text += val == "-"
                            ? string.Empty
                            : $"{Mapping["cs(Cookie)"].Value}: {val + Environment.NewLine}";
                    }
                },
                {
                    "cs(Referer)",   (val, m)=>
                    {
                        m.AddOrReplaceAdditionalProperty("cs(Referer)", val, StringComparer.Ordinal);
                        m.Text += val == "-"
                            ? string.Empty
                            : $"{Mapping["cs(Referer)"].Value}: {val + Environment.NewLine}";
                        m.Module =val;
                    }
                },
                {
                    "sc-substatus",   (val, m)=>
                    {
                        m.AddOrReplaceAdditionalProperty("sc-substatus", val, StringComparer.Ordinal);
                        m.Text += val == "-"
                            ? string.Empty
                            : $"{Mapping["sc-substatus"].Value}: {val + Environment.NewLine}";
                    }
                },
            };
        }

        public async Task<IEnumerable<IAnalogyLogMessage>> Process(string fileName, CancellationToken token, ILogMessageCreatedHandler messagesHandler)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                AnalogyLogMessage empty = new AnalogyLogMessage($"File is null or empty. Aborting.",
                    AnalogyLogLevel.Critical, AnalogyLogClass.General, "Analogy", "None")
                {
                    Source = "Analogy",
                    Module = System.Diagnostics.Process.GetCurrentProcess().ProcessName,
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
                    Module = System.Diagnostics.Process.GetCurrentProcess().ProcessName,
                };
                messagesHandler.AppendMessage(empty, Utils.GetFileNameAsDataSource(fileName));
                return new List<AnalogyLogMessage> { empty };
            }
            List<AnalogyLogMessage> messages = new List<AnalogyLogMessage>();
            try
            {
                long count = 0;
                using var stream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using var reader = new StreamReader(stream);
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
                    messagesHandler.AppendMessage(entry, Utils.GetFileNameAsDataSource(fileName));
                    count++;
                    messagesHandler.ReportFileReadProgress(new AnalogyFileReadProgress(AnalogyFileReadProgressType.Incremental, 1, count, count));
                }

                return messages;
            }
            catch (Exception e)
            {
                AnalogyLogMessage empty = new AnalogyLogMessage($"Error occured processing file {fileName}. Reason: {e.Message}",
                    AnalogyLogLevel.Critical, AnalogyLogClass.General, "Analogy", "None")
                {
                    Source = "Analogy",
                    Module = System.Diagnostics.Process.GetCurrentProcess().ProcessName,
                };
                messagesHandler.AppendMessage(empty, Utils.GetFileNameAsDataSource(fileName));
                return new List<IAnalogyLogMessage> { empty };
            }
        }

        private AnalogyLogMessage HandleHeaderMessage(string line, string filename)
        {
            var m = new AnalogyLogMessage("Header: " + line, AnalogyLogLevel.Information, AnalogyLogClass.General, filename,
                "", "IIS");
            m.Date = DateTimeOffset.MinValue;

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
                if (ActionMapping.TryGetValue(field, out var action))
                {
                    action(value, m);
                }
                else //custom
                {
                    CustomPropertyAppender(field, value, m);
                }
            }

            return m;
        }
    }
}