using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatDirect
{
    public class AccountList
    {
        public List<Accounts> accounts { get; set; }
    }
    public class Accounts
    {
        public string Login { get; set; }
        public string Token { get; set; }
        public string Site { get; set; }
        public List<string> balance_cost { get; set; }
    }
}
