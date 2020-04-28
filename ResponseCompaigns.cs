using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatDirect
{
    public class ResponseCompaigns
    {
        public RespCampaing result { get; set; }
    }
    public class RespCampaing
    {
        public Campaings[] Campaigns { get; set; }
    }
    public class Campaings
    {
        public string Name { get; set; }
        public long Id { get; set; }
    }
}
