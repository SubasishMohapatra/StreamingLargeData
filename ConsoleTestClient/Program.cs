using SharedLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StreamingService;
using Newtonsoft.Json;
using System.Threading;
using System.Diagnostics;

namespace ConsoleTestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var mainThread = new Thread(() =>
                 {
                     var stream = new ServiceClient<IStreamedService>().Execute<Stream>(d => d.GetData());
                     string messageChunk = string.Empty;
                     byte[] messageBuffer = new byte[1024];
                     int bytesRead = 0;
                     string unparsedData = string.Empty;
                     Debug.Flush();
                     do
                     {
                         bytesRead = stream.Read(messageBuffer, 0, messageBuffer.Length);
                         if (bytesRead <= 0 || bytesRead == 256) break;
                         messageChunk = Encoding.UTF8.GetString(messageBuffer).Replace("\0", "").Split(']')[0];//remove null objects
                         if (!String.IsNullOrEmpty(messageChunk))
                         {
                             if (messageChunk.EndsWith("}}"))
                                 messageChunk = messageChunk.Replace("}}", "}");
                             try
                             {
                                 string objectQuery = "";
                                 var readAll = false;
                                 unparsedData = ParseAndPrepareMessage(messageChunk, unparsedData, out objectQuery);
                                 var clientMessage = JsonConvert.DeserializeObject<List<Employee>>(objectQuery);
                                 if (clientMessage.Any(x => x.EmpID == -1))
                                 {
                                     readAll = true;
                                     clientMessage=clientMessage.TakeWhile(x => x.EmpID != -1).ToList();
                                 }
                                 clientMessage.ForEach(x =>
                                 {
                                     var output = $"EmpID:{x.EmpID} Name:{x.Name} Age:{x.Age}";
                                     Console.WriteLine(output);
                                     Trace.WriteLine(output);
                                 });
                                 if (readAll) break;
                             }
                             catch (Exception ex)
                             {
                                 break;
                             }
                         }
                         messageBuffer = new byte[messageBuffer.Length];
                     } while (bytesRead > 0 || stream.CanRead);
                 });
            mainThread.Start();
            mainThread.Join();
        }

        private static string ParseAndPrepareMessage(string incoming, string prepend, out string output)
        {
            //incoming = incoming.Replace("[", "").Replace("]", "");
            //int indexOfLastItem = incoming.LastIndexOf("}") + 1;
            //var unparsedData = indexOfLastItem != incoming.Length ? incoming.Substring(indexOfLastItem + 1, incoming.Length - indexOfLastItem - 1) : ""; //-1 is for the comma
            //output = $"[{(prepend + incoming.Substring(0, indexOfLastItem))}]";
            //return unparsedData;

            //incoming = incoming.Replace("[", "").Replace("]", "");
            //incoming = incoming.SkipWhile(x => x != '{').ToString();
            //int indexOfLastItemClosingBracket = incoming.LastIndexOf("}") + 1;
            //var unparsedData = incoming.Substring(indexOfLastItemClosingBracket + 1);
            //output = $"[{(prepend + incoming.Substring(0, indexOfLastItemClosingBracket))}]";
            //return unparsedData;

            incoming = incoming.Replace("[", "").Replace("]", "");
            int indexOfFirstItemStartingBracket = incoming.IndexOf("{");
            int indexOfLastItemClosingBracket = incoming.LastIndexOf("}") + 1;
            var append = incoming.Substring(0, indexOfLastItemClosingBracket);
            var unparsedData = incoming.Substring(indexOfLastItemClosingBracket).IndexOf("{") != -1 ? incoming.Substring(indexOfLastItemClosingBracket + 1) : "";
            output = $"[{new string((prepend + append).SkipWhile(x => x != '{').ToArray())}]";
            return unparsedData;

        }
    }
}
