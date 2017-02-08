using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Reclamation.Core;

namespace HydrometNotifications.UserInterface
{
    public partial class NotificationMain : UserControl
    {
        public NotificationMain()
        {
            InitializeComponent();
        }

        private void NotificationMain_Load(object sender, EventArgs e)
        {
            ReadGroups();
        }

        private void ReadGroups()
        {
            var pw = UserPreference.Lookup("timeseries_database_password");
            pw = StringCipher.Decrypt(pw, "");

            var svr = PostgreSQL.GetPostgresServer("hydromet", password:pw);
            HydrometNotifications.AlarmDataSet.DB = svr;
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource =
             HydrometNotifications.AlarmDataSet.GetGroups();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;
            DataGridViewCell cell = (DataGridViewCell) dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
            var group = cell.Value.ToString();
            OpenGroup(group);
        }

        private void OpenGroup(string group)
        {
            var detail = new NotificationDetail();
            detail.SetGroup(group);
            detail.Parent = this;
            detail.BringToFront();
            detail.Dock = DockStyle.Fill;
            detail.Visible = true;
            detail.OnClose += detail_OnClose;
        }

        void detail_OnClose(object sender, EventArgs e)
        {
            ReadGroups();
        }

        private void buttonNewGroup_Click(object sender, EventArgs e)
        {
            OpenGroup(this.textBoxNewGroup.Text);
        }
    }
}
