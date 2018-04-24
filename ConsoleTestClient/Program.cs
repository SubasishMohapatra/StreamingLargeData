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
                     do
                     {
                         bytesRead = stream.Read(messageBuffer, 0, messageBuffer.Length);
                         if (bytesRead <= 0 || bytesRead == 256) break;
                         messageChunk = Encoding.UTF8.GetString(messageBuffer).Replace("\0", "").Split(']')[0];//remove null objects
                         if (!String.IsNullOrEmpty(messageChunk))
                         {
                             if (messageChunk.EndsWith("}}"))
                                 messageChunk = messageChunk.Replace("}}", "}");
                             var clientMessage = JsonConvert.DeserializeObject<List<Employee>>(messageChunk+"]");
                             clientMessage.ForEach(x => Console.WriteLine($"EmpID:{x.EmpID} Name:{x.Name} Age:{x.Age}"));
                         }
                         messageBuffer = new byte[messageBuffer.Length];
                     } while (bytesRead > 0);
                 });
            mainThread.Start();
            mainThread.Join();
        }
    }
}
