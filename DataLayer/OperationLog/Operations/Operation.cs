using DataLayer.DataModel;

namespace DataLayer.OperationLog.Operations
{
    public class Operation : IOperation
    {
        public Operation(Item item)
        {
            Item = item;
        }

        public Item Item { get; }
    }
}