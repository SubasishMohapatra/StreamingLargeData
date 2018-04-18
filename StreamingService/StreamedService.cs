using SharedLib;
using System.IO;

namespace StreamingService
{
    public class StreamedService : IStreamedService
    {
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
    }
}
