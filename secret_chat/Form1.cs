using System;
using System.Collections;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
//using System.Reflection;
using System.Text;
//using System.Web;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Threading;

namespace secret_chat
{
    struct User
    {
        public string id{get;set;}
        public string FirstName{get;set;}
        public string LastName{get;set;}
        public string Photo{get;set;}
        public string OnLine{get;set;}
    }

    struct Message
    {
        public string body {get;set;}
        public string read_state {get;set;}
        public string mid {get;set;}
        public string uid {get;set;}
        public string Out { get; set; }
    }

    public partial class Form1 : Form
    {
        //struct MyStruct
        //{
        //    public MyStruct(int i1,int i2,int i3)
        //    {
        //        ves = i1;
        //        rest = i2;
        //        god = i3;
        //    }
        //    public int ves;
        //    public int rest;
        //    public int god;
        //}
        //MyStruct _ffd = new MyStruct(10, 20, 30);
        //fdd;
        List<User> id_list = new List<User>();
        //int id_last_message = 0;
        int count = 5;//кол-во сообщений запросить
        //protected int id_mess = 0;
        //protected int id_peopl = 0;
        //APP_ID – идентификатор Вашего приложения; 
        //PERMISSIONS – запрашиваемые права доступа приложения; 
        //DISPLAY – внешний вид окна авторизации, поддерживаются: page, popup и mobile. 
        //REDIRECT_URI – адрес, на который будет передан access_token. 
        //API_VERSION – версия API, которую Вы используете.
        //client_id=APP_ID& 
        //scope=PERMISSIONS& 
        //redirect_uri=REDIRECT_URI& 
        //display=DISPLAY& 
        //v=API_VERSION& 
        //response_type=token
        public Form1()
        {
            InitializeComponent();
            this.Text += " версия KD_Real.dll " + new KD_Real.Kod().version();
            ////лезим в реестр//
            //int iReg = (int)Microsoft.Win32.Registry.GetValue(Microsoft.Win32.Registry.CurrentUser + "\\REND\\groop", "Rend", -10);
            ////
            //if (iReg == -10)
            //{
            //    Microsoft.Win32.Registry.SetValue(Microsoft.Win32.Registry.CurrentUser + "\\REND\\groop", "Rend", 100110);
            //}
            //else
            //{
            //    Microsoft.Win32.Registry.SetValue(Microsoft.Win32.Registry.CurrentUser + "\\REND\\groop", "Rend", --iReg);
            //}

        }
        private void AUTHR()// авторизация в ВК
        {
            try
            {
                //https://oauth.vk.com/authorize?client_id=1&display=page&redirect_uri=http://example.com/callback&scope=friends&response_type=token&v=5.50
                int client_id = 4547618;
                string scope = "messages,friends";
                string redirect_uri = "https://oauth.vk.com/authorize?client_id=" + client_id + "&redirect_uri=https://oauth.vk.com/blank.html&scope=" + scope + "&display=popup&revoke=1&response_type=token";
                Form2 form2 = new Form2();
                (form2.Controls["webBrowser1"] as WebBrowser).Navigate(redirect_uri);
                if (form2.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                { /*this.toolStripStatusLabel1.Text = "Нет связи с VK.com";*/ return; }
                button1_Click(null,null);
            }
            catch
            {
            }
        }

        private string GET(string metod, string token)//запрос без параметров
        {
            //https://api.vk.com/method/'''METHOD_NAME'''?'''PARAMETERS'''&access_token='''ACCESS_TOKEN'''
            string Url = "https://api.vk.com/method/" + metod + "?access_token=" + token;
            
            WebRequest req = WebRequest.Create(Url);
            WebResponse resp = req.GetResponse();
            Stream stream = resp.GetResponseStream();
            StreamReader sr = new StreamReader(stream);
            string Out = sr.ReadToEnd();
            sr.Close();
            return Out;
        }

        private string GET(string metod, string token, string param)// запрос с параметрами
        {
            string Out = string.Empty;
            //Stream stream;
            //StreamReader sr;
            try
            {
                ////https://api.vk.com/method/'''METHOD_NAME'''?'''PARAMETERS'''&access_token='''ACCESS_TOKEN'''
                //string Url = "https://api.vk.com/method/" + metod + "?" + param + "&access_token=" + token;
                //WebRequest req = WebRequest.Create(Url);
                //req.Timeout = 5000;
                //WebResponse resp = req.GetResponse();
                //stream = resp.GetResponseStream();
                //sr = new StreamReader(stream);
                //Out = sr.ReadToEnd();
                //sr.Close();
                //return Out;


                System.Net.WebRequest req = System.Net.WebRequest.Create("https://api.vk.com/method/" + metod);
                req.Method = "POST";
                req.Timeout = 10000;
                req.ContentType = "application/x-www-form-urlencoded";
                byte[] sentData = Encoding.UTF8.GetBytes(param + "&access_token=" + token);
                req.ContentLength = sentData.Length;
                System.IO.Stream sendStream = req.GetRequestStream();
                sendStream.Write(sentData, 0, sentData.Length);
                sendStream.Close();
                System.Net.WebResponse res = req.GetResponse();
                //System.IO.Stream ReceiveStream = res.GetResponseStream();
                //sr = new System.IO.StreamReader(ReceiveStream, Encoding.UTF8);
                //Кодировка указывается в зависимости от кодировки ответа сервера
                StreamReader _stream = new StreamReader(res.GetResponseStream(), Encoding.UTF8);

                List<char> read = new List<char>();
                while (!_stream.EndOfStream)
                {
                    char[] c = new char[1];
                    _stream.Read(c, 0, 1);
                    read.Add(c[0]);
                }

                //Char[] read = new Char[256];
                //int count = sr.Read(read, 0, 256);
                //Out = String.Empty;
                //while (count > 0)
                //{
                //    String str = new String(read, 0, count);
                //    Out += str;
                //    count = sr.Read(read, 0, 256);
                //}
                _stream.Close();
                foreach (var item in read)
	{
		 Out+=item;
	}
                return Out;
            }
            catch
            {
                return Out = "Error";
            }
        }

        private void GET(string metod, string token, string param, out Stream stream)// запрос с параметрами
        {
            stream = Stream.Null;
            try
            {
                //https://api.vk.com/method/'''METHOD_NAME'''?'''PARAMETERS'''&access_token='''ACCESS_TOKEN'''
                string Url = "https://api.vk.com/method/" + metod + "?" + param + "&access_token=" + token;
                WebRequest req = WebRequest.Create(Url);
                req.Timeout = 5000;
                WebResponse resp = req.GetResponse();
                stream = resp.GetResponseStream();
            }
            catch
            {
            }
            //return;
        }

        private void button1_Click(object sender, EventArgs e)//получить список всех друзей
        {
            User _User = new User();
            //{"uid":153432282,"first_name":"Alisa","last_name":"Maslenkoff","nickname":"","online":0,"user_id":153432282},
            string str;
            id_list.Clear();//.xml
            //Stream stream = Stream.Null;
            str = GET("friends.get.xml", Properties.Settings.Default.token, "order=hints&fields=nickname&fields=photo_200_orig");
            //GET("friends.get", Properties.Settings.Default.token, "order=hints&fields=nickname", out stream);
            if (str.Length == 0) return;
            listBox1.Items.Clear();
            //foreach (var item in respons.frends)
            //{
            //    id_list.Add(item.User_id);
            //    listBox1.Items.Add(item.FName + " " + item.LName);
            //}
            XDocument doc;
            try
            {
                doc = XDocument.Parse(str);
            }
            catch
            {
                return;
            }
            //XmlDocument doc = new XmlDocument();
            //doc.LoadXml(str);
            // var t = from uid in doc.Descendants("uid") select uid.Value;
            var t = from user in doc.Descendants("user")
                    select new
                        {
                            id = (user.Element("uid")!= null)?user.Element("uid").Value:"0",
                            FirstName = (user.Element("first_name") != null) ? user.Element("first_name").Value : "0",
                            LastName = (user.Element("last_name") != null) ? user.Element("last_name").Value : "0",
                            Photo = (user.Element("photo_200_orig") != null) ? user.Element("photo_200_orig").Value : "0",
                            OnLine = (user.Element("online") != null) ? user.Element("online").Value : "0"
                        };
            foreach (var item in t)
            {
                _User.FirstName = item.FirstName;
                _User.id = item.id;
                _User.LastName = item.LastName;
                _User.OnLine = item.OnLine;
                _User.Photo = item.Photo;
                id_list.Add(_User);
                if (!checkBox3.Checked)
                {
                    listBox1.Items.Add((item.OnLine == "1" ? "+ " : "-  ") + item.FirstName + " " + item.LastName);
                }
                else
                {
                    if (item.OnLine == "1")
                    {
                        listBox1.Items.Add((item.OnLine == "1" ? "+ " : "-  ") + item.FirstName + " " + item.LastName); 
                    }
                }
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            send();
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)//получить данные о друге
        {
            if (listBox1.SelectedIndex == -1) return;
            textBox1.Text = id_list[listBox1.SelectedIndex].FirstName;
            textBox2.Text = id_list[listBox1.SelectedIndex].LastName;
            textBox4.Text = id_list[listBox1.SelectedIndex].id;
            if (id_list[listBox1.SelectedIndex].OnLine == "1")
            {
                label6.Text = "друг online";
            }
            else
            {
                label6.Text = "друг offline";
            }
            string id = textBox4.Text; //listBox1.Items[listBox1.SelectedIndex].ToString();
            listBox1.Items[listBox1.SelectedIndex] = listBox1.Items[listBox1.SelectedIndex].ToString().Substring(0, 2) + textBox1.Text + " " + textBox2.Text;
            this.pictureBox1.Load(id_list[listBox1.SelectedIndex].Photo);
            if (tabControl1.TabPages.IndexOfKey(id) == -1)
            {
                //tabControl1.TabPages.Add(textBox4.Text, textBox1.Text + " " + textBox2.Text);
                    TabPage page = new TabPage(textBox1.Text + " " + textBox2.Text);
                    page.Name = textBox4.Text;
                    Control r = new FromTab();
                    r.Dock = DockStyle.Fill;
                    page.Tag = textBox4.Text;
                    page.Controls.Add(r);
                    tabControl1.TabPages.Add(page);
                    tabControl1.SelectTab(tabControl1.TabPages.IndexOfKey(textBox4.Text));
            }
            else
            {
                tabControl1.SelectTab(tabControl1.TabPages.IndexOfKey(textBox4.Text));
            }
            //tabControl1.TabPages.Add(id, listBox1.Items[listBox1.SelectedIndex].ToString().Substring(2));
            //tabControl1.SelectTab(tabControl1.TabPages.IndexOfKey(id));
            //Control r = new FromTab();
            //r.Dock = DockStyle.Fill;
            //tabControl1.SelectedTab.Controls.Add(r);
            //tabControl1.SelectedTab.Tag = id;
            #region +//
            // MessageBox.Show(id);
            //<user>
            //<uid>97037405</uid> 
            //<first_name>Екатерина</first_name> 
            //<last_name>Протько</last_name> 
            //<photo_200_orig>http://cs618228.vk.me/v618228405/15c4e/7mQVlU7MyHY.jpg</photo_200_orig> 
            //</user>
            //XDocument doc = XDocument.Parse(GET("users.get.xml", Properties.Settings.Default.token, "user_ids=" + id + "&fields=photo_200_orig,online"));
            //var t = from user in doc.Descendants("user")
            //        select new
            //            {
            //                id = user.Element("uid").Value,
            //                FirstName = user.Element("first_name").Value,
            //                LastName = user.Element("last_name").Value,
            //                Photo = user.Element("photo_200_orig").Value,
            //                OnLine = user.Element("online").Value
            //            };
            //foreach (var item in t)
            //{
            //    textBox1.Text = item.FirstName;
            //    textBox2.Text = item.LastName;
            //    textBox4.Text = item.id;
            //    if (item.OnLine == "1")
            //    {
            //        label6.Text = "друг online";
            //    }
            //    else
            //    {
            //        label6.Text = "друг offline";
            //    }
            //    this.pictureBox1.Load(item.Photo);
            //} 
            #endregion
            //if(tabControl1.TabCount == 1) button8_Click(this, EventArgs.Empty);
        }

        private void send()//отправить сообщение
        {
            if (textBox3.TextLength == 0 || textBox4.Text.Length<1)
            {
                return;
            }
            if (tabControl1.TabPages.IndexOfKey(textBox4.Text) == -1)
            {
                tabControl1.TabPages.Add(textBox4.Text, textBox1.Text + " " + textBox2.Text);
                tabControl1.SelectTab(tabControl1.TabPages.IndexOfKey(textBox4.Text));
                Control r = new FromTab();
                r.Dock = DockStyle.Fill;
                if (DateTime.Now > DateTime.Parse("15.04.2016")) r.BackColor = System.Drawing.Color.Black;
                tabControl1.SelectedTab.Controls.Add(r);
                tabControl1.SelectedTab.Tag = textBox4.Text;
            }
            string str;
            KD_Real.Kod kod = new KD_Real.Kod();
            if (checkBox2.Checked)
            {
                str = "[***]" + kod.change(ForMat(), textBox5.Text, true);
            }
            else
            {
                str = textBox3.Text;
            }
            //str = textBox3.Text;
            str = GET("messages.send", Properties.Settings.Default.token, "user_id=" + textBox4.Text + "&message=" + str);
            if (str == "Error") { (tabControl1.SelectedTab.Controls[0].Controls[0] as RichTextBox).AppendText( "для " + textBox4.Text + " не отправлено\n\n"); return; }
            str = str.Split('"')[1];
            if (str == "response")
            {
               (tabControl1.SelectedTab.Controls[0].Controls[0] as RichTextBox).AppendText("для " + textBox4.Text + " >\n" + textBox3.Text + "\n\n");
                textBox3.Clear();
            }
            else
            {
                (tabControl1.SelectedTab.Controls[0].Controls[0] as RichTextBox).AppendText("для " + textBox4.Text + " не отправлено\n\n");
            }
        }
        private void getanswe()//получить новые сообщения
        {

            //<message>
            //<body>еn3</body> 
            //<mid>6618</mid> 
            //<uid>20056500</uid> 
            //<from_id>20056500</from_id> 
            //<date>1410642023</date> 
            //<read_state>0</read_state> 
            //<out>0</out> 
            //</message>
            try
            {
                string str = string.Empty;
                int i = 0;
                str = GET("messages.get.xml", Properties.Settings.Default.token, "&v=5.50&count=" + this.count + "&out=0");
                if (str.Length == 0 || str == "Error") return;
                XDocument doc;
                try
                {
                    doc = XDocument.Parse(str);
                }
                catch
                {
                    return;
                }
                if (doc.Root.Name.LocalName == "error" && !checkBox1.Checked)
                {
                    //Invoke(new Action(()=> (tabControl1.SelectedTab.Controls[0].Controls[0] as RichTextBox).AppendText("> error\n\n")));
                    return;
                }
                var mes = from message in doc.Descendants("message")
                          select new
                              {
                                  /*<id>10547</id>
    <date>1458290014</date>
    <out>0</out>
    <user_id>94852211</user_id>
    <read_state>1</read_state>
    <title> ... </title>
    <body>2))ВChHyЫъя.P.)ч)щnCЫЧЖOЕЯyКЮыо2яSv.KNUl;жКГяcдPЖИкыжKfьсУ.WеcВЯ(</body>*/
                                  body = (message.Element("body") != null) ? message.Element("body").Value : "0",
                                  read_state = (message.Element("read_state") != null) ? message.Element("read_state").Value : "0",
                                  mid = (message.Element("id") != null) ? message.Element("id").Value : "0",
                                  uid = (message.Element("user_id") != null) ? message.Element("user_id").Value : "0",
                                  Out = (message.Element("out") != null) ? message.Element("out").Value : "0"
                              };
                KD_Real.Kod kod = new KD_Real.Kod();
                i = 0;
                List<Message> _message = new List<Message>();
                Message _mes = new Message();
                foreach (var item in mes)
                {
                    if (int.Parse(item.read_state) == 0 && int.Parse(item.Out) == 0)
                    {
                        _mes.body = item.body;
                        _mes.mid = item.mid;
                        _mes.Out = item.Out;
                        _mes.read_state = item.read_state;
                        _mes.uid = item.uid;
                        _message.Add(_mes);
                        GET("messages.markAsRead", Properties.Settings.Default.token, "message_ids=" + item.mid);
                        i++;
                    }
                }
                if (i == 0 && !checkBox1.Checked)
                {
                    //!Invoke(new Action(()=> richTextBox1.Text += "> нет новых сообщений\n\n"));
                    return;
                }
                _message.Reverse();
                i = 0;
                foreach (Message item in _message)//[***]
                {
                    int _id = id_list.FindLastIndex((u) => u.id == item.uid);
                    if (_id == -1 && tabControl1.TabPages.IndexOfKey(item.uid) == -1)
                    {
                        Invoke(new Action(() =>
                        {
                            tabControl1.TabPages.Add(item.uid, "spam?");
                            tabControl1.SelectTab(tabControl1.TabPages.IndexOfKey(item.uid));
                            Control r = new FromTab();
                            r.Dock = DockStyle.Fill;
                            tabControl1.SelectedTab.Controls.Add(r);
                            tabControl1.SelectedTab.Tag = item.uid;
                        }));
                    }
                    string name;
                    if (_id != -1)
                    {
                        Invoke(new Action(() =>
                            {
                                name = listBox1.Items[_id].ToString();
                                if (name.Substring(name.Length - 2, 2) != " *" && (tabControl1.SelectedIndex == -1 || tabControl1.SelectedTab.Text != name.Remove(0, 2)))
                                {
                                    listBox1.Items[_id] += " *";
                                }
                            }));
                    }
                    _id = tabControl1.TabPages.IndexOfKey(item.uid);
                    if (tabControl1.TabPages.IndexOfKey(item.uid) > -1)
                    {
                        if (item.body.Length > 4 && item.body.Substring(0, 5) == "[***]")//признак закодированного сообщения
                        {
                            Invoke(new Action(() => (tabControl1.TabPages[_id].Controls[0].Controls[0] as RichTextBox).AppendText("от " + item.uid + " >\n" + kod.change(item.body.Substring(5), textBox5.Text, false) + "\n\n")));
                        }
                        else
                        {
                            Invoke(new Action(() => (tabControl1.TabPages[_id].Controls[0].Controls[0] as RichTextBox).AppendText("от " + item.uid + " >\n" + item.body + "\n\n")));
                        }
                        name = tabControl1.TabPages[_id].Text;
                        string _tag = "";
                        Invoke(new Action(() => _tag = (string)tabControl1.SelectedTab.Tag));
                        if (_tag != item.uid && name.Substring(name.Length - 2, 2) != " *")
                        {
                            Invoke(new Action(() => tabControl1.TabPages[_id].Text += " *"));
                        }
                        i++;
                    }
                }
            }
            catch
            {

            }
        }

        private void listBox1_KeyUp(object sender, KeyEventArgs e)//нажатие Enter отправить сообщ
        {
            if (e.KeyData == Keys.Enter)
            {
                listBox1_DoubleClick(this, EventArgs.Empty);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new Thread(() =>
             {
                 bool b;
                 Mutex _m = new Mutex(true, "button3_Click", out b);
                 if (!b) { _m.Close(); return; }
                 string buf = this.button3.Text;
                 Invoke(new Action(()=> this.button3.Text = "Ждем..."));
                 getanswe();
                 Invoke(new Action(()=> this.button3.Text = buf));
                 Invoke(new Action(()=>timer1.Enabled = true));
                 _m.Close();
             }
             ).Start();

        }

        private void button4_Click(object sender, EventArgs e)//Выйти из ВК
        {
            Form2 form2 = new Form2();
            (form2.Controls["webBrowser1"] as WebBrowser).Navigate("https://login.vk.com/?act=logout&hash=0&_origin=http://vk.com");
            //WebRequest reg = WebRequest.Create("https://login.vk.com/?act=logout&hash=78a732496829130daf&_origin=http://vk.com");
            //reg.GetResponse();
            //Thread.Sleep(10);
            //string logout = "https://login.vk.com/?act=logout&hash=" + Properties.Settings.Default.LogOut + "&_origin=http://vk.com";
            //(form2.Controls["webBrowser1"] as WebBrowser).Navigate(logout);
            //(form2.Controls["webBrowser1"] as WebBrowser).Navigate("https://login.vk.com/?act=logout&hash="+Properties.Settings.Default.LogOut+"&_origin=http://vk.com");
            //form2.Show();
            //form2.Close();
            //AUTHR();
            listBox1.Items.Clear();
            //!richTextBox1.Clear();
            pictureBox1.Image = pictureBox1.ErrorImage;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.auth)
            {
                MessageBox.Show("Вы уже в системе");
                return;
            }
            AUTHR();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //!richTextBox1.Clear();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label5.Text = Properties.Settings.Default.auth ? "OnLine" : "OffLine";
            //label8.Text = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable() ? "Интернет ВКЛ" : "Интернет ВЫКЛ";
            if (checkBox3.Checked)
            {
                this.button1_Click(this,EventArgs.Empty);
            }
            if (checkBox1.Checked)
            {
                timer1.Enabled = false;
                this.button3_Click(this, EventArgs.Empty);
            }
        }

        private string ForMat()//подготовка строки к передаче в KOD
        {
            //string str = string.Empty;
            //for (int i = 0; i < textBox3.Lines.Length; i++)
            //{
            //    str += textBox3.Lines[i]+"$%^";
            //}

            string answ = string.Empty;
            string[] str = textBox3.Lines;
            foreach (var s in str)
            {
                answ += s + " ";
            }
            return answ;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            listBox1.Visible = !listBox1.Visible;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            button3.Enabled = !checkBox1.Checked;
            if (checkBox1.Checked)
            {
                timer1.Interval = 10000;
                this.count = 5;
                //String strVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
            else
            {
                timer1.Interval = 1000;
                this.count = 5;
            }
        }

        private void textBox4_KeyUp(object sender, KeyEventArgs e)
        {

        }
        //delegate void del();
        ////event del Event;
        //public void delin()
        //{
        //    textBox3.Text = "dsfdgfdg";
        //}
        //protected void sss()
        //{
        //    // Event += new del(this.delin);
        //    //Event.Invoke();
        //    this.Invoke(new del(delin));
        //}

        private void button8_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == -1) return;
            new Thread(() =>
            {
                    {
                        bool b;
                        Mutex _m = new Mutex(true, "graf", out b);
                        if (!b) { _m.Close(); return; };
                        List<string> mess1 = new List<string>();
                        //string[] mess1 = new string[10];
                        //Array.Clear(mess1, 0, 10);
                        string str = string.Empty;
                        str = GET("messages.getHistory.xml", Properties.Settings.Default.token, "user_id=" + textBox4.Text + "&rev=0&v=5.24&offset=0&count=" + count.ToString());
                        if (str.Length == 0 || str == "Error") { _m.Close(); return; }
                        XDocument doc; try
                        {
                            doc = XDocument.Parse(str);
                        }
                        catch
                        {
                            _m.Close();
                            return;
                        }
                        if (doc.Root.Name.LocalName == "error" && !checkBox1.Checked)
                        {
                            Invoke(new Action(() =>
                            {
                                if (tabControl1.SelectedIndex != -1)
                                    (tabControl1.SelectedTab.Controls[0].Controls[0] as RichTextBox).AppendText("> error\n\n");
                            }));
                            _m.Close();
                            return;
                        }
                        var mes = from message in doc.Descendants("message")
                                  select new
                                      {
                                          body = (message.Element("body") != null)? message.Element("body").Value : "0",
                                          read_state = (message.Element("read_state") != null) ? message.Element("read_state").Value : "0",
                                          id = (message.Element("id") != null) ? message.Element("id").Value : "0",
                                          user_id = (message.Element("user_id") != null) ? message.Element("user_id").Value : "0",
                                          Out = (message.Element("out") != null) ? message.Element("out").Value : "0"
                                      };
                        KD_Real.Kod kod = new KD_Real.Kod();
                        //int i = 0;
                        Invoke(new Action(() => (tabControl1.SelectedTab.Controls[0].Controls[0] as RichTextBox).Clear()));
                        foreach (var item in mes)
                        {
                            if (item.body.Length > 4 && item.body.Substring(0,5) == "[***]")
                            {
                                if (int.Parse(item.Out) == 0)
                                {
                                    mess1.Add("от > " + item.user_id + "\n" + kod.change(item.body.Substring(5), textBox5.Text, false) + "\n");
                                    //i++;
                                }
                                else
                                {
                                    mess1.Add("для > " + item.user_id + "\n" + kod.change(item.body.Substring(5), textBox5.Text, false) + "\n");
                                    //i++;
                                }
                            }
                            else
                            {
                                if (int.Parse(item.Out) == 0)
                                {
                                    mess1.Add("от > " + item.user_id + "\n" + item.body + "\n");
                                    //i++;
                                }
                                else
                                {
                                    mess1.Add("для > " + item.user_id + "\n" + item.body + "\n");
                                    //i++;
                                }
                            }
                        }
                        if (mess1.Count == 0)
                        {
                            Invoke(new Action(() => (tabControl1.SelectedTab.Controls[0].Controls[0] as RichTextBox).AppendText("История пуста\n")));
                        }
                        //Array.Reverse(mess1);
                        mess1.Reverse();
                        //richTextBox1.Clear();
                        foreach (var item in mess1)
                        {

                            if (item != null)
                            {
                                Invoke(new Action(() => (tabControl1.SelectedTab.Controls[0].Controls[0] as RichTextBox).AppendText(item + "\n")));
                            }
                        }
                        _m.Close();
                    }
        }).Start();
        }

        private void button9_Click(object sender, EventArgs e)
        {
                new Form3().ShowDialog().ToString();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            new Thread(() =>
            {
                WebRequest req = WebRequest.Create("https://api.vk.com/method/account.setOnline");
                req.GetResponse();
            }).Start();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            //!richTextBox1.SelectionStart = richTextBox1.TextLength;
            //!richTextBox1.ScrollToCaret();
        }

        private void textBox3_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        private void textBox3_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Text)) { textBox3.AppendText(e.Data.GetData(DataFormats.Text,true).ToString()); return; }
            MessageBox.Show(((string[])e.Data.GetData(DataFormats.FileDrop))[0]);
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) System.Diagnostics.Process.Start(((string[])e.Data.GetData(DataFormats.FileDrop,true))[0]);
        }

        private void tabControl1_DoubleClick(object sender, EventArgs e)
        {
            tabControl1.TabPages.Remove(tabControl1.SelectedTab);        
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == -1) { textBox4.Text = ""; return; }
            this.Enabled = false;
            textBox4.Text = (string)tabControl1.SelectedTab.Tag;
            string ggg = tabControl1.SelectedTab.Text;
            if (ggg.Length > 1 && ggg.Substring(ggg.Length - 2, 2) == " *")
            {
                tabControl1.SelectedTab.Text = ggg.Remove(ggg.Length - 2);
                int ind = id_list.FindLastIndex((d) => d.id == tabControl1.SelectedTab.Tag.ToString());
                ggg = listBox1.Items[ind].ToString();
                listBox1.Items[ind] = ggg.Remove(ggg.Length - 2);
            }
            string strfrompars = GET("users.get.xml", Properties.Settings.Default.token, "user_ids=" + textBox4.Text + "&fields=photo_200_orig,online");
            XDocument doc;
            try
            {
                doc = XDocument.Parse(strfrompars);
            }
            catch
            {
                this.Enabled = true;
                return;
            }

            var t = from user in doc.Descendants("user")
                    select new
                    {
                        id = (user.Element("uid")!=null)? user.Element("uid").Value: "0",
                        FirstName = (user.Element("first_name") != null) ? user.Element("first_name").Value : "0",
                        LastName = (user.Element("last_name") != null) ? user.Element("last_name").Value : "0",
                        Photo = (user.Element("photo_200_orig") != null) ? user.Element("photo_200_orig").Value : "0",
                        OnLine = (user.Element("online") != null) ? user.Element("online").Value : "0"
                    };
            foreach (var item in t)
            {
                textBox1.Text = item.FirstName;
                textBox2.Text = item.LastName;
                textBox4.Text = item.id;
                if (item.OnLine == "1")
                {
                    label6.Text = "друг online";
                }
                else
                {
                    label6.Text = "друг offline";
                }
                this.pictureBox1.Load(item.Photo);
            }
            button8_Click(this, EventArgs.Empty);
            this.Enabled = true;
        }

        private void textBox3_KeyUp(object sender, KeyEventArgs e)
        {
        }

        private void textBox6_KeyUp(object sender, KeyEventArgs e)
        {
            int i = 5;
            if (int.TryParse(textBox6.Text, out i))
            {
                if (i < 0 || i > 200) i = 5;
                this.count = i;
            }
        }

        private void списокДрузейToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBox1.Visible = !listBox1.Visible;
        }

        private void onoffToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.panel1.Visible = !this.panel1.Visible;
        }

        private void onoffФотоToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.panel2.Visible = !this.panel2.Visible;
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
            if (!e.Control && e.KeyCode == Keys.Enter)
            {
                send();
                return;
            }
            e.SuppressKeyPress = false;
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void textBox4_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
            if (e.KeyData == Keys.Enter)
            {
                XDocument doc;
                try
                {
                    doc = XDocument.Parse(GET("users.get.xml", Properties.Settings.Default.token, "user_ids=" + textBox4.Text + "&fields=photo_200_orig,online,online_app"));
                }
                catch
                {
                    return;
                }

                var t = from user in doc.Descendants("user")
                        select new
                        {
                            id = (user.Element("uid") != null) ? user.Element("uid").Value : "0",
                            FirstName = (user.Element("first_name") != null) ? user.Element("first_name").Value : "0",
                            LastName = (user.Element("last_name") != null) ? user.Element("last_name").Value : "0",
                            Photo = (user.Element("photo_200_orig") != null) ? user.Element("photo_200_orig").Value : "0",
                            OnLine = (user.Element("online") != null) ? user.Element("online").Value : "0"
                        };
                foreach (var item in t)
                {
                    textBox1.Text = item.FirstName;
                    textBox2.Text = item.LastName;
                    textBox4.Text = item.id;
                    if (item.OnLine == "1")
                    {
                        label6.Text = "друг online";
                    }
                    else
                    {
                        label6.Text = "друг offline";
                    }
                    this.pictureBox1.Load(item.Photo);
                }
                if (tabControl1.TabPages.IndexOfKey(textBox4.Text) == -1)
                {
                    //tabControl1.TabPages.Add(textBox4.Text, textBox1.Text + " " + textBox2.Text);
                    TabPage page = new TabPage(textBox1.Text + " " + textBox2.Text);
                    page.Name = textBox4.Text;
                    Control r = new FromTab();
                    r.Dock = DockStyle.Fill;
                    page.Tag = textBox4.Text;
                    page.Controls.Add(r);
                    tabControl1.TabPages.Add(page);
                    tabControl1.SelectTab(tabControl1.TabPages.IndexOfKey(textBox4.Text));
                }
                else
                {
                    tabControl1.SelectTab(tabControl1.TabPages.IndexOfKey(textBox4.Text));
                }
                return;
            }
            else
            {
                e.SuppressKeyPress = false;
            }

        }
    }
}

