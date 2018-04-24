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
    [ServiceKnownType(typeof(CustomStream))]
    [ServiceKnownType(typeof(Employee))]
    public interface IStreamedService
    {
        //[OperationContract]
        //Stream Echo(Stream data);
        //[OperationContract]
        //Stream DownloadFile(string query);
        //[OperationContract(IsOneWay = true)]
        //void UploadFile(Stream data);
        [OperationContract]
        Stream GetData();
    }
}
