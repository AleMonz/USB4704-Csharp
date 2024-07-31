using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace USB4704Test
{
    public partial class MainForm : Form
    {

        //Flag for r/w data while loop
        public static bool loop = true;
        //Thread for loop
        static public Thread thread;

        public static int[] DataList1 = new int[16];

        public MainForm()
        {
            InitializeComponent();

            try
            {
                //Try to read and write on the device
                USB4704.StartReadingInputs();
                USB4704.StartWritingOutputs();
            }
            catch (Exception e) 
            { 
                MessageBox.Show(e.Message + "\r\nClosing the Program");
                this.Close();
            }

            thread = new Thread(UpdateData);
            thread.Start();

        }

        public void UpdateData()
        {
            Thread.Sleep(1000);
            Thread.Sleep(1000);
            while (loop)
            {
                for(int i = 0; i < DataList1.Length; i++)
                {
                    DataList1[i] = USB4704.DataList[i];
                }

                //Read the input values from the device
                cb_DO0.Invoke(new Action(() => cb_DO0.Checked = Convert.ToBoolean(DataList1[8])));
                cb_DO1.Invoke(new Action(() => cb_DO0.Checked = Convert.ToBoolean(DataList1[9])));
                cb_DO2.Invoke(new Action(() => cb_DO0.Checked = Convert.ToBoolean(DataList1[10])));
                cb_DO3.Invoke(new Action(() => cb_DO0.Checked = Convert.ToBoolean(DataList1[11])));
                cb_DO4.Invoke(new Action(() => cb_DO0.Checked = Convert.ToBoolean(DataList1[12])));
                cb_DO5.Invoke(new Action(() => cb_DO0.Checked = Convert.ToBoolean(DataList1[13])));
                cb_DO6.Invoke(new Action(() => cb_DO0.Checked = Convert.ToBoolean(DataList1[14])));
                cb_DO7.Invoke(new Action(() => cb_DO0.Checked = Convert.ToBoolean(DataList1[15])));


                //Write the output values to the device
                DataList1[0] = Convert.ToInt32(cb_DI0.Checked);
                DataList1[1] = Convert.ToInt32(cb_DI1.Checked);
                DataList1[2] = Convert.ToInt32(cb_DI2.Checked);
                DataList1[3] = Convert.ToInt32(cb_DI3.Checked);
                DataList1[4] = Convert.ToInt32(cb_DI4.Checked);
                DataList1[5] = Convert.ToInt32(cb_DI5.Checked);
                DataList1[6] = Convert.ToInt32(cb_DI6.Checked);
                DataList1[7] = Convert.ToInt32(cb_DI7.Checked);

                Thread.Sleep(100);

            }
        }

        private void btn_Stop_Click(object sender, EventArgs e)
        {
            loop = false;

            btn_Stop.Enabled = false;
            btn_Stop.Text = "Closing threads";

            Thread.Sleep(1000);

            thread.Abort();

            Thread.Sleep(1000);

            this.Close();

        }
    }
}
