using SharedLib;
using System.IO;
using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text;
using System.ServiceModel;

namespace StreamingService
{
    [ServiceBehavior]
    public class StreamedService : IStreamedService
    {
        IEnumerable<Employee> employees;
        
        public StreamedService()
        {
            employees = Enumerable.Range(0,1000).Select(x=>
                    new Employee() { EmpID = ++x, Name = $"Employee{x}", Age = x % 61 });
        }


        //public Stream Echo(Stream data)
        //{
        //    return data;
        //}

        //public void UploadFile(Stream data)
        //{
        //    using (var outputStream = new FileStream("FileUpload.jpg", FileMode.Create))
        //    {
        //        data.CopyTo(outputStream);
        //    }
        //}

        //public Stream DownloadFile(string query)
        //{
        //    return new FileStream("DownloadFile.jpg", FileMode.Open, FileAccess.Read);
        //}
        [ServiceKnownType("GetData", typeof(StreamedService))]
        [OperationBehavior]
        public Stream GetData()
        {
            try
            {
                //return new MemoryStream();
                Func<int, List<Employee>> getRecords = (start) =>
                {
                    var records = new List<Employee>();
                    records = employees.Where(x => x.EmpID > start && x.EmpID <= (start + Consts.BatchSize)).ToList();
                    return records;
                };
                return new CustomStream(getRecords);
            }
            catch(Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }
    }
}
