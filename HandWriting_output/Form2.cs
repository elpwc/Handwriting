using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HandWriting_output
{
    public partial class Form2 : Form
    {
        public Form2(Image P)
        {
            InitializeComponent();
            BackgroundImage = P;
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
    }
}
