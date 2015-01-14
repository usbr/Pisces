using Reclamation.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Pisces
{
    public partial class ServerDatabaseDialog : Form
    {
        
        public ServerDatabaseDialog()
        {
            InitializeComponent();
            this.comboBoxDbType.SelectedIndex = 0;
        }

        public string ServerName
        {
            get
            {
                var t = this.textBoxServerDatabase.Text.Split(':');
                return t[0];
            }
        }

        public DatabaseType DatabaseType
        {
            get
            {
                if (this.comboBoxDbType.SelectedItem.ToString().ToLower()
                    == "mysql")
                    return DatabaseType.MySQL;

                return DatabaseType.PostgreSql;
            }
        }
        public string DatabaseName
        {
            get
            {
                var t = this.textBoxServerDatabase.Text.Split(':');
                if( t.Length ==2)
                return t[1];

                return "";
            }
        }
    }
}
