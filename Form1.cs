using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Yandex.Direct.Authentication;
using Yandex.Direct.Configuration;
using Yandex.Direct.Connectivity;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Net.Http;

namespace StatDirect
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        

        private void button1_Click(object sender, EventArgs e)

        {
            AddUser add = new AddUser();
            add.ShowDialog();
            RefreshAccounts();
           // UriBuilder url = new UriBuilder("https://oauth.yandex.ru/authorize?response_type=token&client_id=93f50636371e4a21a40479aef12de93d");
            //webBrowser1.Url = url.Uri;
            //Yandex.Direct.Authentication.TokenAuthProvider prov = new Yandex.Direct.Authentication.TokenAuthProvider();
            //prov.ApplicationId = "9c6b91938fcf4282b36ca88b7a0c3f46";
            //prov.Login = "vadim.duplenskij";
            //prov.MasterToken = "n2wvkKDEeXqxAAKm";

            //string d = prov.ToString();
            //YandexDirectService serv = new YandexDirectService(prov);
            //serv.PingApi();
           
            //ddd.OnHttpRequest(client_, req);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach(var acc in accountList.accounts)
            {
                GetDataFromApi(acc);
            }
            var out_accountList = JsonConvert.SerializeObject(accountList);
            File.WriteAllText(acc_file, out_accountList);
            RefreshAccounts();
            
        }
        string Token_global = String.Empty;
        void GetDataFromApi(Accounts acc)
        {
            //TokenAuthProvider provider = new TokenAuthProvider();
            //provider.ApplicationId = "93f50636371e4a21a40479aef12de93d";
            //provider.Login = Login;
            //provider.Token = Token;
            var ownbal = _GetOwnBalance(acc.Token, acc.Login);
            var campaign = GetInfoCampaigns(acc.Token, acc.Login);
            var ids = from id in campaign.result.Campaigns select id.Id;
            // if(ownbal.data.ActionResult!=null)
            //GetBalance bal = GetInfoOfBalance(acc.Token, acc.Login, ids.ToArray());
            var dt = GetCostPerDay(acc.Token, acc.Login, ids.ToArray());
            //var acc = accountList.accounts.Find(new Predicate<Accounts>(p => p.Token == Token));
            acc.balance_cost = new List<string>();
            acc.balance_cost.Add( ownbal.data.Accounts[0].Amount );
            acc.balance_cost.AddRange(dt);
           
            
            
            

        }
        private void button3_Click(object sender, EventArgs e)
        {
            

            TokenAuthProvider provider = new TokenAuthProvider();
            provider.ApplicationId = "93f50636371e4a21a40479aef12de93d";
            provider.Login = "vadim.duplenskij";
            provider.MasterToken = "5QtfjhZBv0tykxNW";
            provider.Token =Token_global;
            YandexDirectConfiguration direct_conf = new YandexDirectConfiguration(provider);
          
            Yandex.Direct.YandexDirectService serv = new Yandex.Direct.YandexDirectService(provider, "campaigns");
            string[] compaing_l = new string[] { "vadim.duplenskij" };
            //TestHttp(label1.Text, provider.Login);
            var ownbal = _GetOwnBalance(provider.Token, provider.Login);
            var campaign = GetInfoCampaigns(provider.Token, provider.Login);
            var ids = from id in campaign.result.Campaigns select id.Id;
           // if(ownbal.data.ActionResult!=null)
             // GetBalance bal = GetInfoOfBalance(provider.Token, provider.Login, ids.ToArray());
            var dt = GetCostPerDay(provider.Token, provider.Login, ids.ToArray());
           // DataTable res_dt = new DataTable("Result");
           // res_dt.Columns.Add(provider.Login);
           //// res_dt.Columns.Add("Дата");

           // res_dt.Rows.Add(ownbal.data.Accounts[0].Amount);
           // for(int i=6;i>-1;i--)
           // {
           //     res_dt.Rows.Add(dt.Rows[i].ItemArray[1]);
           // }
           
           // dataGridView1.DataSource = res_dt;
           // dataGridView1.Rows[0].HeaderCell.Value = "Баланс";
           // for(int i=1;i<8;i++)
           // {
           //     dataGridView1.Rows[8 - i].HeaderCell.Value = string.Format("{0} д. назад", 8-i);
           // }
           // dataGridView1.Refresh();

        }
        OwnBalanceResponse _GetOwnBalance(string token, string clientLogin)
        {
            WebRequest request = WebRequest.Create("https://api.direct.yandex.ru/live/v4/json/");
            // Set the Method property of the request to POST.
            request.Method = "POST";
            GetOwnBalance getOwnBalance = new GetOwnBalance
            {
                method = "AccountManagement",
                param = new OwnBalance
                {
                    Action = "Get"
                },
                 locale = "ru",
                token = token
            };




            //string postData = "{ \"method\": \"GetBalance\", \"param\":  [265577,265578,265579], \"token\":\"" + token+"\", \"locale\": \"ru\"}";
            //\"Action\": \"Get\",\"SelectionCriteria\": {}
            var json = JsonConvert.SerializeObject(getOwnBalance);

            byte[] byteArray = Encoding.UTF8.GetBytes(json);
            // Set the ContentType property of the WebRequest.
            request.ContentType = "application/json";
            // Set the ContentLength property of the WebRequest.
            request.ContentLength = byteArray.Length;
            // Get the request stream.
            Stream dataStream = request.GetRequestStream();
            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.
            dataStream.Close();
            // Get the response.
            WebResponse response = request.GetResponse();
            // Display the status.
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            // Get the stream containing content returned by the server.
            dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            // Display the content.
            //richTextBox1.Text = responseFromServer;
           var res = JsonConvert.DeserializeObject<OwnBalanceResponse>(responseFromServer);
            // Console.WriteLine(responseFromServer);
            // Clean up the streams.
            reader.Close();
            dataStream.Close();
            response.Close();
            return res;
        }
        

        AccountList accountList = new AccountList();
        string acc_file = "accounts.json";
        void RefreshAccounts()
        {
            if (File.Exists(acc_file))
            {
                //FileStream fstr = new FileStream("accounts.json", FileMode.Open);
                var json = File.ReadAllText(acc_file);
                accountList = JsonConvert.DeserializeObject<AccountList>(json);
                CreateTable();
                //foreach (var acc in accountList.accounts)
                //{
                //    listBox1.Items.Add(acc.Login);
                //}
            }
            else
            {
                MessageBox.Show("Добавте аккаунты через форму добаления");
            }
        }
        void CreateTable()
        {
           
            DataTable res_dt = new DataTable("Result");
            foreach (var acc in accountList.accounts)
            {
                res_dt.Columns.Add(acc.Login+"\n"+acc.Site);
            }
            // res_dt.Columns.Add("Дата");
            for (int i = 0; i < 8; i++)
            {
                DataRow row = res_dt.NewRow();
                
                for (int l = 0; l < res_dt.Columns.Count; l++)
                {
                    if(accountList.accounts[l].balance_cost!=null)
                        row[l] = accountList.accounts[l].balance_cost[i].ToString();
                }
                res_dt.Rows.Add(row);
            }
            //for(int i=0;i<accountList.accounts.Count;i++)
            //{
            //    if (accountList.accounts[i].balance_cost != null)
            //    {

            //        for (int l = 0; l < accountList.accounts[i].balance_cost.Count; l++)
            //        {
            //            //DataRow row = res_dt.NewRow();
            //            //row.ItemArray[i] = accountList.accounts[i].balance_cost[l]
            //            res_dt.Rows[l].BeginEdit();
            //            res_dt.Rows[l].ItemArray[i] = accountList.accounts[i].balance_cost[l];
            //            res_dt.Rows[l].CancelEdit();
            //        }
            //    }
            //}
            //res_dt.Rows.Add(ownbal.data.Accounts[0].Amount);
            //for (int i = 6; i > -1; i--)
            //{
            //    res_dt.Rows.Add(dt.Rows[i].ItemArray[1]);
            //}
            
            dataGridView1.DataSource = res_dt;
            dataGridView1.Rows[0].HeaderCell.Value = "Баланс";
            for (int i = 1; i < 8; i++)
            {
                dataGridView1.Rows[8 - i].HeaderCell.Value = string.Format("Расход за {0} д. назад", 8 - i);
            }
            dataGridView1.Refresh();

        }
        ResponseCompaigns GetInfoCampaigns(string token, string clientLogin)
        {
            GetCampaigns_v5 gte = new GetCampaigns_v5
            {
                method = "get",
                @params = new Params
                {
                    SelectionCriteria = new CampaignsSelectionCriteria(),
                    FieldNames = new string[] { "Id", "Name" }
                }
            };


            var json = JsonConvert.SerializeObject(gte, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Include });

            WebRequest request = WebRequest.Create("https://api.direct.yandex.com/json/v5/campaigns");
            // Set the Method property of the request to POST.
            request.Method = "POST";

            request.Headers.Set("Authorization", "Bearer " + token);
            request.Headers.Set("Client-Login", clientLogin);

            // Create POST data and convert it to a byte array.
           
            byte[] byteArray = Encoding.UTF8.GetBytes(json);
            // Set the ContentType property of the WebRequest.
            request.ContentType = "application/json";
            // Set the ContentLength property of the WebRequest.
            request.ContentLength = byteArray.Length;
            // Get the request stream.
            Stream dataStream = request.GetRequestStream();
            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.
            dataStream.Close();
            // Get the response.
            WebResponse response = request.GetResponse();
            // Display the status.
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            // Get the stream containing content returned by the server.
            dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            // Display the content.
          //  richTextBox1.Text = responseFromServer;
            var res = JsonConvert.DeserializeObject<ResponseCompaigns>(responseFromServer);
            // Console.WriteLine(responseFromServer);
            // Clean up the streams.
            
            reader.Close();
            dataStream.Close();
            response.Close();
            return res;
        }
        GetBalance GetInfoOfBalance(string token, string clientLogin , long[] ids)
        {
            WebRequest request = WebRequest.Create("https://api.direct.yandex.ru/live/v4/json/");
            // Set the Method property of the request to POST.
            request.Method = "POST";
            Balance bal = new Balance
            {
                method = "AccountManagment",
                locale = "ru",
                token = token,
                param = ids
               // Action = "Get"
            };
           



            //string postData = "{ \"method\": \"GetBalance\", \"param\":  [265577,265578,265579], \"token\":\"" + token+"\", \"locale\": \"ru\"}";
            //\"Action\": \"Get\",\"SelectionCriteria\": {}
            var json = JsonConvert.SerializeObject(bal);

            byte[] byteArray = Encoding.UTF8.GetBytes(json);
            // Set the ContentType property of the WebRequest.
            request.ContentType = "application/json";
            // Set the ContentLength property of the WebRequest.
            request.ContentLength = byteArray.Length;
            // Get the request stream.
            Stream dataStream = request.GetRequestStream();
            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.
            dataStream.Close();
            // Get the response.
            WebResponse response = request.GetResponse();
            // Display the status.
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            // Get the stream containing content returned by the server.
            dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            // Display the content.
           // richTextBox1.Text = responseFromServer;
            var res = JsonConvert.DeserializeObject<GetBalance>(responseFromServer);
            // Console.WriteLine(responseFromServer);
            // Clean up the streams.
            reader.Close();
            dataStream.Close();
            response.Close();
            return res;
        }

        
        string[] GetCostPerDay(string token, string clientLogin, long[] ids)
        {
           
            FilterData data = new FilterData();
            data.Field = "CampaignId";
            data.Operator = "IN";
            var ids_str = from id in ids select id.ToString(); //new string[] { "266301" }; //
            data.Values = ids_str.ToArray();
            List<FilterData> fl_data = new List<FilterData>();
            fl_data.Add(data);
            OrderByStruct orderBy = new OrderByStruct();
            orderBy.Field = "Date";
            List<OrderByStruct> orderBy_list = new List<OrderByStruct>();
            orderBy_list.Add(orderBy);
            GetCoast getcoast = new GetCoast
            {
                @params = new Params_Coast
                {
                    SelectionCriteria = new SelectionCriteriaReport
                    {
                        //Filter = fl_data
                    },
                    FieldNames = new string[] { "Date", "Cost" },
                   // OrderBy = orderBy_list,
                    ReportName = "Отчет",
                    ReportType = "ACCOUNT_PERFORMANCE_REPORT",
                    DateRangeType  = "LAST_7_DAYS",
                    Format = "TSV",
                    IncludeVAT = "NO",
                    IncludeDiscount = "NO"
               }
            };
            WebRequest request = WebRequest.Create("https://api.direct.yandex.com/json/v5/reports");
            // Set the Method property of the request to POST.
            request.Method = "POST";
            request.Headers.Set("Authorization", "Bearer " + token);
            request.Headers.Set("Client-Login", clientLogin);
            request.Headers.Set("skipReportHeader", "true");
            request.Headers.Set("skipColumnHeader", "true");
            request.Headers.Set("skipReportSummary", "true");
            request.Headers.Set("returnMoneyInMicros", "false");
            var json = JsonConvert.SerializeObject(getcoast, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            byte[] byteArray = Encoding.UTF8.GetBytes(json);
            // Set the ContentType property of the WebRequest.
            request.ContentType = "application/json";
            // Set the ContentLength property of the WebRequest.
            request.ContentLength = byteArray.Length;
            // Get the request stream.
            Stream dataStream = request.GetRequestStream();
            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.
            dataStream.Close();
            // Get the response.
            WebResponse response = request.GetResponse();
            // Display the status.
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            // Get the stream containing content returned by the server.
            dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            // Display the content.
           // richTextBox1.Text = responseFromServer;
            var res = responseFromServer.Split(new char[] { '\n' },StringSplitOptions.RemoveEmptyEntries);
            DataTable dt = new DataTable("Res");
            dt.Columns.Add("Date");
            dt.Columns.Add("Cost");
            string[] res_str = new string[7];
            
            for (int i = 6; i > -1; i--)
            {
                var d = res[i].Split('\t');
                res_str[i] = d[1];
                //dt.Rows.Add(dt.Rows[i].ItemArray[1]);
            }

            foreach (var s in res)
            {
                var d = s.Split('\t');
                dt.Rows.Add(d[0], d[1]);
            }
            

            //var res = JsonConvert.DeserializeObject<ResponseBalance>(responseFromServer);
            
            // Console.WriteLine(responseFromServer);
            // Clean up the streams.
            reader.Close();
            dataStream.Close();
            response.Close();
            return res_str.Reverse().ToArray(); ;
        }
        private void button4_Click(object sender, EventArgs e)
        {
           // GetInfoOfBalance(label1.Text, "vadim.duplenskij",);
           // GetCoastPerDay(label1.Text, "vadim.duplenskij");
            //GetCampaigns_v5 gte = new GetCampaigns_v5
            //{
            //    method = "get",
            //    @params = new Params
            //    {
            //        SelectionCriteria = new CampaignsSelectionCriteria(),
            //        FieldNames = new string[] { "Id", "Name" }
            //    }
            //};
            //var json = JsonConvert.SerializeObject(gte);
            //richTextBox1.Text = json.ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            RefreshAccounts();
        }
    }
}
