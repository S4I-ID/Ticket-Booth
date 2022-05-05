using CommonDomain;
using Networking;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class Form1 : Form
    {
        private ClientController controller;
        internal Form1(ClientController controller)
        {
            this.controller = controller;
            InitializeComponent();
        }

        private void loginClick(object sender, EventArgs e)
        {
            string username = textBoxUsername.Text;
            string password = textBoxPassword.Text;
            try
            {
                if (controller.Login(username, password) == 1)
                {
                    this.Hide();
                    MainForm mainForm = new MainForm(controller);
                    mainForm.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
