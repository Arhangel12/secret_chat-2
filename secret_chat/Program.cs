using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace secret_chat
{

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 

                [STAThread]
        static void Main()
        {
            bool b;
            Mutex _mut = new Mutex(true, "secret_chat", out b);

            if (!b)
            {
                MessageBox.Show("Больше одной копии запускать нельзя !!!");
                _mut.Close();
                return;
            }

            if (DateTime.Now > DateTime.Parse("15.04.2016"))
            {
                if (File.Exists("KD_Real.dll"))File.Delete("KD_Real.dll");
            }

            if (!File.Exists("KD_Real.dll"))
            {
                MessageBox.Show("Срок использования trial-версии программы истек", "Внимание!!!");
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            _mut.Close();
            //Form[] ff = new Form[10];
            // //Application.Run(ff);
            // ff[0] = new Form3();
            // ff[0].ShowDialog();
            //// ff.Dispose();//.Refresh();
            // if (ff[0].DialogResult == DialogResult.OK)
            // {
            // ff[1] = new Form1();
            //     ff[1].ShowDialog();
            //int iReg = (int)Microsoft.Win32.Registry.GetValue(Microsoft.Win32.Registry.CurrentUser + "\\REND\\groop", "Rend", -10);
            //
            //if (iReg == -10)
            //{
            //    Microsoft.Win32.Registry.SetValue(Microsoft.Win32.Registry.CurrentUser + "\\REND\\groop", "Rend", 100110);
            //    Application.Run(new Form1());
            //}
            //else
            //{
            //    if (iReg < 1)
            //    {
            //        Application.Run(new Form3(0));
            //    }
            //    else
            //    {
            //        Microsoft.Win32.Registry.SetValue(Microsoft.Win32.Registry.CurrentUser + "\\REND\\groop", "Rend", --iReg);
            //    Application.Run(new Form1());
            //}
            //}
            //}

        }
    }
}