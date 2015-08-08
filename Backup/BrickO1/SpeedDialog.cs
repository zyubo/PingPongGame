using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BrickO1
{
    public partial class SpeedDialog : Form

    {
        public int Speed = 250;
        
        public SpeedDialog()
        {
            InitializeComponent();
        }

        private void SlowRadio_CheckedChanged(object sender, EventArgs e)
        {

        }



        private void button1_Click(object sender, System.EventArgs e)
        {
            if (SlowRadio.Checked)
                Speed = 100;
            else if (MediumRadio.Checked)
                Speed = 100;
            else
                Speed = 50;

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void SpeedDialog_Load(object sender, EventArgs e)
        {

        }

        
    }
}
