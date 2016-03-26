using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.IO;

namespace secret_chat
{
    public partial class Form2 : Form
    {
        class timer1 :  System.Windows.Forms.Timer
        {
            public int step { get; set; }
            public timer1()
            {
                this.Interval = 1000; 
                step = 30; 
            }
        }
        timer1 _time = new timer1();
        public Form2()
        {
            InitializeComponent();
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            _time.Enabled = true;
            _time.Tick+=new EventHandler(this.timer1_Tick);
        }



        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            this.progressBar1.Visible = this._time.Enabled = this.label1.Visible = false;
            //http://REDIRECT_URI#access_token=533bacf01e11f55b536a565b57531ad114461ae8736d6506a3&expires_in=86400&user_id=8492
            //http://REDIRECT_URI?error=access_denied&error_description=The+user+or+authorization+server+denied+the+request.
            string uri = webBrowser1.Url.ToString();
            string text = webBrowser1.DocumentText;
            if (webBrowser1.DocumentTitle == "ВКонтакте | Вход")
            {
                return;
            }
            if (webBrowser1.DocumentTitle == "ВКонтакте | Разрешение доступа")
            {
                return;
            }
            int i = text.IndexOf(@"<a class=""top_nav_link"" id=""logout_link"" href=""https://login.vk.com/?act=logout&hash=");
            if (i != -1)
            {
                char[] strstr = new char[18];
                text.CopyTo(i + 85, strstr, 0, 18);
                string str = string.Empty;
                foreach (var item in strstr)
                {
                    str += item;
                }
                Properties.Settings.Default.LogOut = str;
                string logout = "https://login.vk.com/?act=logout&hash=" + Properties.Settings.Default.LogOut + "&_origin=http://vk.com";
                webBrowser1.Navigate(logout);
                Properties.Settings.Default.Reset();
                return;
            }
            //string token;
            dynamic token;
            string id;
            try
            {
                if (uri.Split('/')[3][0] == 'i')
                {
                    //string str = Properties.Settings.Default.LogOut;
                    Properties.Settings.Default.Reset();
                   // Properties.Settings.Default.LogOut = str;
                    Close();
                    return;
                }
                token = uri.Split('#')[1];

                if (token[0] == 'a')
                {
                    token = token.Split('&')[0];
                    token = token.Split('=')[1];
                }
                
                id = uri.Split('=')[3];
               // MessageBox.Show(id + "  " + token);
                Properties.Settings.Default.id = id;
                Properties.Settings.Default.token = token;
                Properties.Settings.Default.auth = true;
                string Url = "https://api.vk.com/method/stats.trackVisitor?access_token=" + token;
             WebRequest req = WebRequest.Create(Url);
             //WebRequest req = WebRequest.Create("https://api.vk.com/method/messages.send?user_id=20056500&message=привет из проги&access_token=" + token);
req.GetResponse();
this.DialogResult = System.Windows.Forms.DialogResult.OK;
Close();
            }
            catch
            {
                //MessageBox.Show("catch");
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
               Close();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (_time.step > 0)
            {
                this._time.step --;
                this.progressBar1.Value += 10;
                return;
            }
            _time.Enabled = false;
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
    }
}
