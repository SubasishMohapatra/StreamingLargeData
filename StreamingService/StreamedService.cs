using SharedLib;
using System.IO;
using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text;

namespace StreamingService
{
    public class StreamedService : IStreamedService
    {
        IEnumerable<Employee> employees;

        public StreamedService()
        {
            employees = Enumerable.Range(0,1000).Select(x=>
                    new Employee() { EmpID = ++x, Name = $"Employee{x}", Age = x % 60 });
        }


        public Stream Echo(Stream data)
        {
            return data;
        }

        public void UploadFile(Stream data)
        {
            using (var outputStream = new FileStream("FileUpload.jpg", FileMode.Create))
            {
                data.CopyTo(outputStream);
            }
        }

        public Stream DownloadFile(string query)
        {
            return new FileStream("DownloadFile.jpg", FileMode.Open, FileAccess.Read);
        }

        public Stream GetData()
        {
            var serialized = JsonConvert.SerializeObject(employees);
            var messageBytes = Encoding.UTF8.GetBytes(serialized);
            return new MemoryStream(messageBytes);
        }
    }
}
