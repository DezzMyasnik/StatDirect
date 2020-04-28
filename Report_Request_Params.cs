using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatDirect
{
    public class GetCoast
    {
        public Params_Coast @params { get; set; }
    }
    public class Params_Coast
    {

        public SelectionCriteriaReport SelectionCriteria { get; set; }
        public string[] FieldNames { get; set; }
        public Page Page { get; set; }
        public List<OrderByStruct> OrderBy { get; set; }
        public string ReportName { get; set; }
        public string ReportType { get; set; }
        public string DateRangeType { get; set; }
        public string Format { get; set; }
        public string IncludeVAT { get; set; }
        public string IncludeDiscount { get; set; }
    }
    public class Page
    {
        public int Limit { get; set; }
    }
    public class OrderByStruct
    {
        public string Field { get; set; }
        public string SortOrder { get; set; }

    }
    public class FilterData
    {
        public string Field { get; set; }
        public string Operator { get; set; }
        public string[] Values { get; set; }
    }
    public class SelectionCriteriaReport
    {
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public List<FilterData> Filter { get; set; }
    }
    public partial class Report_Request_Params
    {
       
    }
}
