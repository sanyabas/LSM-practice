using System;
using DataLayer.MemoryCopy;
using DataLayer.OperationLog.Operations;

namespace DataLayer.Warmup
{
    public class OpLogApplier : IOpLogApplier
    {
        private readonly IOpLogReader opLogReader;

        public OpLogApplier(IOpLogReader opLogReader)
        {
            this.opLogReader = opLogReader;
        }

        public void Apply(IMemTable memTable)
        {
            IOperation op;
            while (opLogReader.Read(out op))
            {
                var operation = op as Operation;
                memTable.Add(operation.Item);
            }
        }
    }
}