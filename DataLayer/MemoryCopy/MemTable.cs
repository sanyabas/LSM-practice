using System.Collections.Generic;
using System.Linq;
using DataLayer.DataModel;
using DataLayer.OperationLog.Operations;

namespace DataLayer.MemoryCopy
{
    public class MemTable : IMemTable
    {
        private readonly IOpLogWriter opLogWriter;
        private List<Item> items;

        public MemTable(IOpLogWriter opLogWriter)
        {
            this.opLogWriter = opLogWriter;
            items=new List<Item>();
        }

        public void Add(Item item)
        {
            items.Add(item);
            opLogWriter.Write(new Operation(item));
        }

        public Item Get(string key) => items.Last(item => item.Key==key);
    }
}