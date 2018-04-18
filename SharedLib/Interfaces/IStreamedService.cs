using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SharedLib
{
    [ServiceContract]
    public interface IStreamedService
    {
        [OperationContract]
        Stream Echo(Stream data);
        [OperationContract]
        Stream DownloadFile(string query);
        [OperationContract(IsOneWay = true)]
        void UploadFile(Stream data);
    }
}
