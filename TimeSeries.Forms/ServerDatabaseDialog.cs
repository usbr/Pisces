using Reclamation.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Reclamation.TimeSeries.Forms
{
    public partial class ServerDatabaseDialog : Form
    {
        
        public ServerDatabaseDialog()
        {
            InitializeComponent();
            this.comboBox1.SelectedIndex = 0;
        }

        //lrgs1:timeseries

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

                return DatabaseType.PostgreSql;
            }
        }
        public string DatabaseName
        {
            get
            {
                // serverip:database #mysql
                var t = this.comboBox1.Text.Split(':', '#');

                if( t.Length >=2)
                return t[1];

                return "";
            }
        }
    }
}
