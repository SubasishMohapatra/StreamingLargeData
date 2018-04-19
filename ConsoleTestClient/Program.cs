using SharedLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StreamingService;
using Newtonsoft.Json;

namespace ConsoleTestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var stream = new ServiceClient<IStreamedService>().Execute(d => d.GetData());
            StringBuilder messageBuilder = new StringBuilder();
            string messageChunk = string.Empty;
            byte[] messageBuffer = new byte[1024];
            int bytesRead=0;
            do
            {
                bytesRead = stream.Read(messageBuffer, 0, messageBuffer.Length);
                messageChunk = Encoding.UTF8.GetString(messageBuffer).Replace("\0","");//remove null objects
                messageBuilder.Append(messageChunk);
                messageBuffer = new byte[messageBuffer.Length];
            } while (bytesRead > 0);
            var clientMessage = JsonConvert.DeserializeObject<List<Employee>>(messageBuilder.ToString());

            //int counter = 0;
            //byte[] messageBytes;
            //string serialized;
            //IEnumerable<Employee> empSubset;
            //using (MemoryStream stream = new MemoryStream())
            //{
            //    while (1 == 1)
            //    {
            //        var min = counter;
            //        var max = (counter + 1000) <= employees.Count() ? counter + 1000 : employees.Count() - counter;
            //        empSubset = employees.Where(x => x.EmpID > min && x.EmpID <= max);
            //        counter += 1000;
            //        serialized = JsonConvert.SerializeObject(empSubset);
            //        messageBytes = Encoding.UTF8.GetBytes(serialized);
            //        stream.Write(messageBytes, 0, messageBytes.Length);
            //    }
            //}

        }
    }
}
