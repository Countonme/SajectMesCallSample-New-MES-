using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SajectMesCallSample
{
    public partial class Form1 : Form
    {
 
        public Form1()
        {
            InitializeComponent();
            this.button2.Click += Button2_Click;
            this.FormClosing += Form1_FormClosing;


        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            MES_Service.MesDisconnect();
            Environment.Exit(0 );   
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            int command = int.Parse(textBox1.Text);
            string f_data=textBox2.Text;
           f_data= MES_Service.MesSendSample(command, f_data);
            richTextBox1.Text+=(f_data)+"\r";
        }
        private void button1_Click(object sender, EventArgs e)
        {
            MES_Service.MesConnect();
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MES_Service.MesDisconnect();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {

        }
    }
}
