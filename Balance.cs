using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatDirect
{
    //  string postData = "{ \"method\": \"GetBalance\", \"param\":  [265577,265578,265579], \"token\":\"" + token+"\", \"locale\": \"ru\"}";

    public class Balance
    {
        public string method { get; set; }
        public long[] param { get; set; }
        public string token { get; set; }
        public string locale { get; set; }
        
    }
    public class GetBalance
    {
        public ResponseBalance[] data { get; set; }
    }
    public class ResponseBalance
    {
        public long CampaignID { get; set; }
        public float Rest { get; set; }
        public float Sum { get; set; }

        public float? SumAvailableForTransfer { get; set; }
    }

    public class GetOwnBalance
    {
        public string method { get; set; }
        public OwnBalance param { get; set; }
        public string token { get; set; }
        public string locale { get; set; }
    }
    public class OwnBalance
    {
        public string Action { get; set; }
    }
    public class OwnBalanceResponse
    {
        public BalanceResponse data { get; set; }

    }
    public class BalanceResponse
    {
        public AccountActionResult[] ActionResult { get; set; }
        public AccountsInfo[] Accounts { get; set; }
    }
    public class AccountsInfo
    {
        public string Login { get; set; }
        public string Amount { get; set; }
        public long AccountID { get; set; }
    }
    public class AccountActionResult
    {
        public int AccountID { get; set; }
        public string Login { get; set; }
        public Error[] Errors { get; set; }
    }
    public class Error
    {
        public int FaultCode { get; set; }
        public string FaultString { get; set; }
        public string FaultDetail { get; set; }
    }
}
