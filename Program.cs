using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace csharp
{

    public class InDefensiveCopyTest
    {
        public struct FairlyLargeStruct
        {
            private readonly long l1, l2, l3, l4;
            public int N { get; set; }
            public FairlyLargeStruct(int n) : this() => N = n;
        }


        private readonly int[] _data = Enumerable.Range(1, 100_000).ToArray();

        [Benchmark]
        public int AggregatePassedByValue()
        {
            var x = new FairlyLargeStruct(42);
            var y = DoAggregate(x);
            return x.N * y;

            int DoAggregate(FairlyLargeStruct largeStruct)
            {
                int result = 0;
                foreach (int n in _data)
                    result += n + largeStruct.N;
                return result;
            }
        }

        [Benchmark]
        public int AggregatePassedByRef()
        {
            var x = new FairlyLargeStruct(42);
            var y = DoAggregate(ref x);
            return x.N * y;

            int DoAggregate(ref FairlyLargeStruct largeStruct)
            {
                int result = 0;
                foreach (int n in _data)
                    result += n + largeStruct.N;
                return result;
            }
        }


        [Benchmark]
        public int AggregatePassedByIn()
        {
            var x = new FairlyLargeStruct(42);
            var y = DoAggregate(x);
            return x.N * y;

            int DoAggregate(in FairlyLargeStruct largeStruct)
            {
                int result = 0;
                foreach (int n in _data)
                    result += n + largeStruct.N;
                return result;
            }
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<InDefensiveCopyTest>();
        }
    }
}
