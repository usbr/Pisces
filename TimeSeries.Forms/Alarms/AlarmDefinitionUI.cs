using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Reclamation.TimeSeries.Alarms;
using Reclamation.Core;
using System.Configuration;
using DgvFilterPopup;

namespace Reclamation.TimeSeries.Forms.Alarms
{
    public partial class AlarmDefinitionUI : UserControl
    {
        AlarmDataSet m_ds;
        AlarmDataSet.alarm_definitionDataTable alarm_definition;

        public AlarmDefinitionUI()
        {
            InitializeComponent();
        }
        public AlarmDefinitionUI(AlarmDataSet ds)
        {
            InitializeComponent();
            m_ds = ds;
            //RefreshDefinition();
        }

        DgvFilterManager filterManager = new DgvFilterManager();
        private void RefreshDefinition() 
        {
            filterManager = null;
            filterManager = new DgvFilterManager();

            alarm_definition = m_ds.GetAlarmDefinition();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.DataSource = new DataTable();
           dataGridView1.DataSource = alarm_definition;
           dataGridView1.Columns["id"].ReadOnly = true;//.Visible = false;


           DataGridViewComboBoxColumn c = new DataGridViewComboBoxColumn();
            
           c.DataSource = alarm_definition;
           c.Name = alarm_definition.listColumn.ColumnName;
           c.ValueMember = "list";
           c.DisplayMember = "list";

           c.DataSource = m_ds.GetList();
           c.DataPropertyName = "list";
           dataGridView1.Columns["list"].Visible = false;
           dataGridView1.Columns.Insert(1, c);

           filterManager.DataGridView = dataGridView1;

           dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

        }


        private void AlarmDefinition_Load(object sender, EventArgs e)
        {
            RefreshDefinition();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {

            m_ds.SaveTable(alarm_definition);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void dataGridView1_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            RefreshUi();
        }
        private void RefreshUi()
        {
            Enabling();
        }


        private void Enabling()
        {
            var sr = dataGridView1.SelectedRows;
            this.buttonTest.Enabled = sr.Count == 1;
            this.textBoxValue.Enabled = sr.Count == 1;
        }

        private void buttonTest_Click(object sender, EventArgs e)
        {
           
         DataRowView currentDataRowView = (DataRowView)dataGridView1.CurrentRow.DataBoundItem;
         var alarm = (AlarmDataSet.alarm_definitionRow)currentDataRowView.Row;

          m_ds.CreateAlarm(alarm,new Point(DateTime.Now,Convert.ToDouble(textBoxValue.Text)));

        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            RefreshDefinition();
        }
    }
}
