using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace illuminati
{
    public partial class Form2 : Form
    {
        public Form2(Form1 _form1)
        {
            InitializeComponent();

            this.Owner = _form1;
        }
    }
}
