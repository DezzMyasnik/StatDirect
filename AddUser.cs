using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
namespace StatDirect
{
    public partial class AddUser : Form
    {
        public AddUser()
        {
            InitializeComponent();
        }
        AccountList accountList = new AccountList();
        string acc_file = "accounts.json";
        private void AddUser_Load(object sender, EventArgs e)
        {
            RefreshAccounts();
        }

        void RefreshAccounts()
        {
            if (File.Exists(acc_file))
            {
                listBox1.Items.Clear();
                //FileStream fstr = new FileStream("accounts.json", FileMode.Open);
                var json = File.ReadAllText(acc_file);
                accountList = JsonConvert.DeserializeObject<AccountList>(json);
                foreach (var acc in accountList.accounts)
                {
                    listBox1.Items.Add(acc.Login);
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            UriBuilder url = new UriBuilder("https://oauth.yandex.ru/authorize?response_type=token&client_id=93f50636371e4a21a40479aef12de93d");
            webBrowser1.Url = url.Uri;
        }
        string token = string.Empty;
        private void button2_Click(object sender, EventArgs e)
        {
            var url = webBrowser1.Url.ToString(); //https://oauth.yandex.ru/verification_code#access_token=AQAAAAAaVKXOAASropAmhQkPoUzHtwnQ78yWRdc&token_type=bearer&expires_in=31525002
            token = url.Substring(url.IndexOf('=') + 1, url.Length - url.IndexOf('&') + 1);
            textBox2.Text = token;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var accc = new Accounts
            {
                Login = textBox1.Text,
                Token = token,
                Site = textBox3.Text

            };
            
            var list = new List<Accounts>();
            if (File.Exists(acc_file))
            {
                list = accountList.accounts;
                list.Add(accc);
            }
            else
            {
                list.Add(accc);
            }
            accountList = new AccountList
            {
                accounts = list
            };
                    
              
                var out_accountList = JsonConvert.SerializeObject(accountList);
                File.WriteAllText(acc_file, out_accountList);
                RefreshAccounts();

           

        }
    }
}
