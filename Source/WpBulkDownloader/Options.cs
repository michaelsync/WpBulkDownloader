using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WpBulkDownloader {
    public partial class Options : Form {
        public Options() {
            InitializeComponent();
        }

        private void label3_Click(object sender, EventArgs e) {

        }

        private void label2_Click(object sender, EventArgs e) {

        }

        private void Options_Load(object sender, EventArgs e) {
            string blogURL = Properties.Settings.Default.BlogURL; 
            string userName = Properties.Settings.Default.UserName ;
            string password = Properties.Settings.Default.Password ;
            if (blogURL != string.Empty)
                blogURLTextbox.Text = blogURL;
            if (userName != string.Empty)
                userNameTextbox.Text = userName;
            if (password != string.Empty)
                passwordTextbox.Text = password;
        }

        private void okButton_Click(object sender, EventArgs e) {
            if (blogURLTextbox.Text != string.Empty)
                Properties.Settings.Default.BlogURL = blogURLTextbox.Text;
            if (userNameTextbox.Text != string.Empty)
                Properties.Settings.Default.UserName = userNameTextbox.Text;
            if (passwordTextbox.Text != string.Empty)
                Properties.Settings.Default.Password  = passwordTextbox.Text;
            Properties.Settings.Default.Save();

            this.Close();
        }
    }
}

