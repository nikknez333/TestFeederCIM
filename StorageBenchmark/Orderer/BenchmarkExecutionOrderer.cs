using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

namespace StorageBenchmark.Orderer
{
    public class BenchmarkExecutionOrderer : IOrderer
    {
        public static readonly IOrderer Instance = new BenchmarkExecutionOrderer();

        public bool SeparateLogicalGroups => throw new NotImplementedException();

        public IEnumerable<BenchmarkCase> GetExecutionOrder(ImmutableArray<BenchmarkCase> benchmarksCase, IEnumerable<BenchmarkLogicalGroupRule> order = null)
        {
            return benchmarksCase.OrderBy(benchmarkCase => GetBenchmarkOrder(benchmarkCase));
        }

        public int GetBenchmarkOrder(BenchmarkCase benchmarkCase)
        {
            string benchmarkMethodName = benchmarkCase.Descriptor.WorkloadMethod.Name;

            switch(benchmarkMethodName)
            {
                case "AddRDFToDB":
                    return 0;
                case "ReadTriplesForGraph":
                    return 1;
                case "SQLQueryNoJoin":
                    return 2;
                case "SQLQueryWithJoin":
                    return 3;
                case "UpdateRDFInDB":
                    return 4;
                case "DeleteTriplesFromGraph":
                    return 5;
                case "DeleteGraphAndTriples":
                    return 6;
                default:
                    return int.MaxValue;
            }
        }

        public string GetHighlightGroupKey(BenchmarkCase benchmarkCase)
        {
            throw new NotImplementedException();
        }

        public string GetLogicalGroupKey(ImmutableArray<BenchmarkCase> allBenchmarksCases, BenchmarkCase benchmarkCase)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IGrouping<string, BenchmarkCase>> GetLogicalGroupOrder(IEnumerable<IGrouping<string, BenchmarkCase>> logicalGroups, IEnumerable<BenchmarkLogicalGroupRule> order = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BenchmarkCase> GetSummaryOrder(ImmutableArray<BenchmarkCase> benchmarksCases, Summary summary)
        {
            throw new NotImplementedException();
        }
    }
}
