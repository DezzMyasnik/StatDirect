using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace StatDirect
{
    internal class GetCampaigns_v5
    {
        public string method { get; set; }
        public Params @params { get; set; }

    }
    internal class Params
    {
        [JsonProperty(PropertyName = "SelectionCriteria", DefaultValueHandling = DefaultValueHandling.Populate)]
        
        public CampaignsSelectionCriteria SelectionCriteria { get; set; }
        public string[] FieldNames { get; set; }
    }
    internal class CampaignsSelectionCriteria
    {
        //public long[] Ids { get; set; }
        //public string Types { get; set; }
        //public string States { get; set; }
        //public string Statuses { get; set; }
        //public string StatusesPayment { get; set; }


    }
    
}
