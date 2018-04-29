using Newtonsoft.Json;
using SharedLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

[DataContract()]
public class CustomStream : Stream
{
    //#endregion
    [DataMember]
    public Func<int, List<Employee>> GetRecords { get; set; }

    public CustomStream(Func<int, List<Employee>> getRecords)
    {
        this.GetRecords = getRecords;
    }

    long totalBytesRead = 0;
    private int recordStartPosition = 0;
    private bool canRead = true;

    public override bool CanRead
    {
        get { return canRead; }
    }
    public override bool CanSeek
    {
        get { return false; }
    }
    public override bool CanWrite
    {
        get { return false; }
    }
    public override long Length
    {
        get { throw new NotImplementedException(); }
    }
    public override long Position
    {
        get
        {
            return totalBytesRead;
        }
        set
        {
            totalBytesRead = value;
        }
    }
    public override void Flush()
    {        
    }
    public override int Read(byte[] buffer, int offset, int count)
    {
        if (count == 256) return count;
        var records = this.GetRecords(recordStartPosition);
        if (records.Count <= 0) return count;
        if (records.Count <= Consts.BatchSize)
        {
            records.Add(new Employee() { EmpID = -1, Name = "", Age = -1 });           
        }
        var serialized = JsonConvert.SerializeObject(records);
        var tempBuffer = Encoding.UTF8.GetBytes(serialized);
        if (tempBuffer.Length <= buffer.Length)
        {
            recordStartPosition += Consts.BatchSize;
            tempBuffer.CopyTo(buffer, offset);
            totalBytesRead += tempBuffer.Length;
        }
        return count;
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        throw new NotImplementedException();
    }
    public override void SetLength(long value)
    {
        throw new NotImplementedException();
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        throw new NotImplementedException();
    }
}

