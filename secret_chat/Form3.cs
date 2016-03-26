using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using System.Management;
using System.Net;
using System.Xml.Linq;//для assm

namespace secret_chat
{
    delegate string MyDelegat(string metod, string token, string param);
    public partial class Form3 : Form
    {
        //int stat = 0;
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Shown(object sender, EventArgs e)
        {//узнать номер HDD
            //string key = string.Empty;
            if (Properties.Settings.Default.id == string.Empty)
            {
                MessageBox.Show("Для регистрации нужно выполнить вход в ВК");
                Close();
                return;
            }
            ManagementObjectSearcher searcher = new
                ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
            foreach (ManagementObject hdd in searcher.Get())
                try
                {
                    string key = string.Empty;
                    //MessageBox.Show(hdd["SerialNumber"].ToString());
                    //получаем номер харда
                    string key2 = (hdd["SerialNumber"].ToString());
                    //удаляем все буквы
                    foreach (char item in key2)
                    {
                        if (char.IsDigit(item))
                        {
                            key += item;
                        }
                    }
                    //получаем закодированный номер харда по id
                    key2 = new KD_Real.Kod().change(key2, Properties.Settings.Default.id, true);
                    //отправляем запрос на вк для получения кода для регистрации
                    MyDelegat delegat = new MyDelegat(GET);
                    //////////////////////////////////////////////////////////
                    //получаем код
                    textBox1.Text = delegat.Invoke("o.xml", Properties.Settings.Default.token, "id=" + new KD_Real.Kod().change(Properties.Settings.Default.id, key2, true) + "&num=" + new KD_Real.Kod().change(key, key, true));
                    XDocument doc = XDocument.Parse(textBox1.Text);
                    var t = textBox1.Text = doc.Element("response").Value;
                    //лезим в реестр//
                    var iReg = (string)Microsoft.Win32.Registry.GetValue(Microsoft.Win32.Registry.CurrentUser + "\\REND\\groop", "Rend", "-10");
                    if (iReg == "-10")
                    {
                        Microsoft.Win32.Registry.SetValue(Microsoft.Win32.Registry.CurrentUser + "\\REND\\groop", "Rend", new KD_Real.Kod().change(t, Properties.Settings.Default.id, false));
                    }

                    iReg = (string)Microsoft.Win32.Registry.GetValue(Microsoft.Win32.Registry.CurrentUser + "\\REND\\groop", "Bref", "-10");
                    if (iReg == "-10")
                    {
                        Microsoft.Win32.Registry.SetValue(Microsoft.Win32.Registry.CurrentUser + "\\REND\\groop", "Bref", "{3000-2589-1648-7895-0258}");
                    }
                    Guid g = Guid.NewGuid();
                    var ddd = g.ToString();
                    iReg = (string)Microsoft.Win32.Registry.GetValue(Microsoft.Win32.Registry.CurrentUser + "\\REND\\groop", "Droop", "-10");
                    if (iReg == "-10")
                    {
                        Microsoft.Win32.Registry.SetValue(Microsoft.Win32.Registry.CurrentUser + "\\REND\\groop", "Droop", "{3000-2589-1648-3587-0258}");
                    }

                    iReg = (string)Microsoft.Win32.Registry.GetValue(Microsoft.Win32.Registry.CurrentUser + "\\REND\\groop", "Drop", "-10");
                    if (iReg == "-10")
                    {
                        Microsoft.Win32.Registry.SetValue(Microsoft.Win32.Registry.CurrentUser + "\\REND\\groop", "Drop", "{" + t + "}");
                    }
                }
                catch
                {
                }
            /////////////////////////////////////////////
            //MessageBox.Show("Наличие подключения: "
            // + System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable().ToString()); 
            //контрольный код
            //       textBox1.Text = Assembly.GetExecutingAssembly().GetHashCode().ToString();//Environment.Version.ToString();//
            //       //узнать версию программы
            //       label3.Text = " Версия проги: " + Assembly.GetExecutingAssembly().GetName().Version.ToString() +
            //" Версия DLL: " + new KD_Real.Kod().version();
            //textBox2.Text = new KD_Real.Kod().change(key, "46455", true);
            ////лезим в реестр//
            //int iReg = (int)Microsoft.Win32.Registry.GetValue(Microsoft.Win32.Registry.CurrentUser + "\\REND\\groop", "Rend", -10);

            //if (iReg == -10)
            //{
            //    Microsoft.Win32.Registry.SetValue(Microsoft.Win32.Registry.CurrentUser + "\\REND\\groop", "Rend", 100110);
            //}
            //else
            //{
            //    if (iReg < 1)
            //    {
            //        if(this.stat == 1)Application.Exit();
            //    }
            //    else
            //    {
            //        Microsoft.Win32.Registry.SetValue(Microsoft.Win32.Registry.CurrentUser + "\\REND\\groop", "Rend", --iReg);
            //    }     
            //}
        }

        private string GET(string metod, string token, string param)// запрос с параметрами
        {
            //https://api.vk.com/method/'''METHOD_NAME'''?'''PARAMETERS'''&access_token='''ACCESS_TOKEN'''
            string Url = "https://api.vk.com/method/execute." + metod + "?" + param + "&access_token=" + token;
            WebRequest req = WebRequest.Create(Url);
            WebResponse resp = req.GetResponse();
            Stream stream = resp.GetResponseStream();
            StreamReader sr = new StreamReader(stream);
            string Out = sr.ReadToEnd();
            sr.Close();
            return Out;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://vk.com/public78543822");
            //System.Diagnostics.Process proc = System.Diagnostics.Process.Start("http://vk.com/public78543822");
            //proc.WaitForExit();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("Only DEMO, sorry");
        }
    }
}
