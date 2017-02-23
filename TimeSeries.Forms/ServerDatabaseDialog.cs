using Reclamation.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections.Specialized;
using System.IO;

namespace Reclamation.TimeSeries.Forms
{
    public partial class ServerDatabaseDialog : Form
    {
        StringCollection dbList = Properties.Settings.Default.DatabaseList;
        string clearItems = string.Empty;
        int currentIdx = 0;
        TextFileCredentials credentials;

        public ServerDatabaseDialog()
        {
         var fn = Path.Combine(FileUtility.GetExecutableDirectory(), "pisces_login.txt");
            if (!File.Exists(fn))
                File.Create(fn);

           credentials = new TextFileCredentials(fn);


            InitializeComponent();

            //add dashes to start and end of clear items to roughly the
            //length of combobox
            string dashes = new string('-', 30);
            clearItems = dashes + "  clear items  " + dashes;

            LoadDatabaseList();
            this.labelUserName.Text = WindowsUtility.GetShortUserName().ToLower();
        }

        private void LoadDatabaseList()
        {
            if (dbList.Count == 0)
            {
                return;
            }

            if (!dbList.Contains(clearItems))
            {
                dbList.Add(clearItems);
            }

            string[] items = new string[dbList.Count];
            dbList.CopyTo(items, 0);
            this.comboBox1.Items.Clear();
            comboBox1.Items.AddRange(items);

            var s = UserPreference.Lookup("SelectedDatabaseServer");
            
            comboBox1.SelectedItem = s;
            comboBox1_SelectionChangeCommitted(comboBox1, EventArgs.Empty);
        }

        public string ServerName
        {
            get
            {
                var t = this.comboBox1.Text.Split(':');
                return t[0];
            }
        }

        public DatabaseType DatabaseType
        {
            get
            {
                if (  this.comboBox1.Text.IndexOf("#mysql") >=0)
                    return DatabaseType.MySQL;
                if (this.comboBox1.Text.IndexOf("#sqlserver") >= 0)
                    return Core.DatabaseType.SqlServer;

                return DatabaseType.PostgreSql;
            }
        }
        public string DatabaseName
        {
            get
            {
                // server:database #mysql
                var t = this.comboBox1.Text.Split(':', '#');

                if( t.Length >=2)
                return t[1];

                return "";
            }
        }
        public string Password
        {
            get
            {
                var t = this.textBoxPassword.Text;

                if (t != "")
                    return t;

                return "";
            }
        }

        private void save_Click(object sender, EventArgs e)
        {
            if (!dbList.Contains(this.comboBox1.Text))
            {
                string val = comboBox1.Text;
                if( this.comboBox1.SelectedItem != null)
                      val = comboBox1.Text;
                dbList.Insert(0,val );
                Properties.Settings.Default.Save();
            }

            credentials.Save(comboBox1.Text.Trim(), textBoxPassword.Text);
            UserPreference.Save("SelectedDatabaseServer", comboBox1.Text.Trim());
        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            var cb = sender as ComboBox;
            if (cb == null || cb.SelectedItem == null)
            {
                return;
            }

            string sel = cb.SelectedItem.ToString().Trim();
            if (sel == clearItems)
            {
                var msg = "OK to clear database list?";
                var result = MessageBox.Show(msg, "Clear Database List", MessageBoxButtons.OKCancel);
                if (result == DialogResult.OK)
                {
                    dbList.Clear();
                    cb.Items.Clear();
                    Properties.Settings.Default.Save();
                }
                else
                {
                    cb.SelectedIndex = currentIdx;
                }
            }

            if( credentials.Contains( sel))
            {
                textBoxPassword.Text = credentials.GetPassword(sel);
            }
            else
            {
                textBoxPassword.Text = "";
            }
        }

        private void comboBox1_DropDown(object sender, EventArgs e)
        {
            var cb = sender as ComboBox;
            if (cb == null)
            {
                return;
            }

            currentIdx = cb.SelectedIndex;
            LoadDatabaseList();
        }

        private void checkBoxHide_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxHide.Checked)
                this.textBoxPassword.PasswordChar = '*';
            else
                this.textBoxPassword.PasswordChar = '\0';
        }
    }
}
