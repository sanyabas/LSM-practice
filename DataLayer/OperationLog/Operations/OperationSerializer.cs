using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DataLayer.DataModel;
using DataLayer.OperationLog.Operations;

namespace DataLayer
{
    //учесть что надо прыгать по определённому офсету в бинарно сериализованном файле
    // ветки с решенными этапами локально держат

    public class OperationSerializer : IOperationSerializer
    {
        public byte[] Serialize(IOperation operation)
        {
            var opOperation = operation as Operation;
            var keyBytes = Encoding.UTF8.GetBytes(opOperation.Item.Key);
            var valueBytes = (opOperation.Item.Value != null) ? Encoding.UTF8.GetBytes(opOperation.Item.Value) : new byte[0];
            var keyLength = (byte)keyBytes.Length;
            var valueLength = (byte)valueBytes.Length;
            var serialized = new List<byte> { keyLength };
            serialized.AddRange(keyBytes);
            serialized.Add(valueLength);
            serialized.AddRange(valueBytes);
            return serialized.ToArray();
        }

        public IOperation Deserialize(Stream opLogStream)
        {
            var keyLength = opLogStream.ReadByte();
            var keyBytes = new byte[keyLength];
            var read = opLogStream.Read(keyBytes, 0, keyLength);
            var key = Encoding.UTF8.GetString(keyBytes);
            var valueLength = opLogStream.ReadByte();
            Item item;
            if (valueLength != 0)
            {
                var valueBytes = new byte[valueLength];
                opLogStream.Read(valueBytes, 0, valueLength);
                var value = Encoding.UTF8.GetString(valueBytes);
                item = Item.CreateItem(key, value);
            }
            else
            {
                item = Item.CreateTombStone(key);
            }
            return new Operation(item);
        }
    }
}