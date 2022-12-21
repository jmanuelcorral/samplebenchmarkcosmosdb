using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchcosmoscli.Config
{
    public class RequestChargeColumn : IColumn
    {


        public string Id => nameof(RequestChargeColumn);
        public string ColumnName => "RequestCharge";

        public bool IsDefault(Summary summary, BenchmarkCase benchmarkCase) => false;
        public string GetValue(Summary summary, BenchmarkCase benchmarkCase) => GetValue(summary, benchmarkCase, SummaryStyle.Default);

        public bool IsAvailable(Summary summary) => true;
        public bool AlwaysShow => true;
        public ColumnCategory Category => ColumnCategory.Metric;
        public int PriorityInCategory => 0;
        public bool IsNumeric => true;
        public UnitType UnitType => UnitType.Size;
        public string Legend => $"Custom '{ColumnName}' Request Charge column";
        public string GetValue(Summary summary, BenchmarkCase benchmarkCase, SummaryStyle style)
        {

            var benchmarkName = benchmarkCase.Descriptor.WorkloadMethod.Name.ToLower();
            var parameter = benchmarkCase.Parameters.Items.FirstOrDefault(x => x.Name == "N");
            if (parameter == null)
            {
                return "no parameter";
            }
            var N = Convert.ToInt32(parameter.Value);
            var filename = $"rtus-size.{benchmarkName}.{N}.txt";
            return File.Exists(filename) ? File.ReadAllText(filename) : "no file";


        }
        public override string ToString() => ColumnName;
    }
}
