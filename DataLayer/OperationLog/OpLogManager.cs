using System;
using System.IO;
using DataLayer.OperationLog.Operations;
using DataLayer.Utilities;

namespace DataLayer
{
    public class OpLogManager : IOpLogReader, IOpLogWriter
    {
        private readonly IFile olFile;
        private readonly IOperationSerializer serializer;
        private int writeOffset;
        private int readOffset;

        public OpLogManager(IFile olFile, IOperationSerializer serializer)
        {
            this.olFile = olFile;
            this.serializer = serializer;
        }

        public bool Read(out IOperation operation)
        {
            operation = null;
            using (var stream = olFile.GetStream())
            {
                stream.Seek(readOffset, SeekOrigin.Begin);
                try
                {
                    var read = serializer.Deserialize(stream);
                    operation = read;
                    readOffset += (int)stream.Position;
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }

        public void Write(IOperation operation)
        {
            using (var stream=olFile.GetStream())
            {
                var serializedOperation = serializer.Serialize(operation);
                stream.Seek(writeOffset,SeekOrigin.Begin);
                stream.Write(serializedOperation,0,serializedOperation.Length);
                writeOffset += serializedOperation.Length;
            }
        }
    }
}