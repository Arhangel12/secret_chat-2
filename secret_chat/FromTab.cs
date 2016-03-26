using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace secret_chat
{
    public partial class FromTab : UserControl
    {
        public FromTab()
        {
            InitializeComponent();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
          richTextBox1.SelectionStart = richTextBox1.TextLength;
          if (DateTime.Now > DateTime.Parse("15.04.2016")) richTextBox1.BackColor = System.Drawing.Color.Black;
          richTextBox1.ScrollToCaret();
        }
    }
}
