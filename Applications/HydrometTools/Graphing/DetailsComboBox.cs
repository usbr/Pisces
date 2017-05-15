using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections.Specialized;
using HydrometTools.Properties;

namespace HydrometTools.Graphing
{

    public partial class DetailsComboBox : UserControl
    {
        public DetailsComboBox()
        {
            InitializeComponent();
            comboBoxGraphList.SelectedIndexChanged += new EventHandler(comboBoxGraphList_SelectedIndexChanged);
        }


        public string SelectedItem
        {
            get {
                if (comboBoxGraphList.SelectedIndex >= 0)
                    return sc[comboBoxGraphList.SelectedIndex].Trim();
                return ""; 
            }
        }


        public override string Text
        {
            get
            {
                return labelText.Text;
            }
            set
            {
                labelText.Text = value;
            }
        }


        private StringCollection sc = new StringCollection();

        public StringCollection Items
        {
            get
            {
                if (sc == null)
                    sc = new StringCollection();
                return sc;
            }
            set
            {
                this.sc = value;
                LoadSettings();
            }
        }

        string[] Prefix
        {
            get {
                List<string> rval = new List<string>();
                foreach (var item in sc)
                {
                    var s = item.Split(',');
                    if (s.Length > 0)
                        rval.Add(s[0].Trim());
                    else
                    rval.Add("");
                }

                return rval.ToArray();
            }
        }

        public event EventHandler<EventArgs> ItemsChanged;

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            EditStringCollection es = new EditStringCollection();
            string[] items = new string[sc.Count];
            sc.CopyTo(items, 0);

            es.Items =  items;
            if (es.ShowDialog() == DialogResult.OK)
            {
                sc.Clear();
                sc.AddRange(es.Items);
                LoadSettings();
                ItemsChanged(this, EventArgs.Empty);

            }
        }

        public event EventHandler SelectedIndexChanged;

        void comboBoxGraphList_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedIndexChanged(this, EventArgs.Empty);    
        }


        void LoadSettings()
        {
            int selIndex = comboBoxGraphList.SelectedIndex;
            this.comboBoxGraphList.Items.Clear();
            this.comboBoxGraphList.Items.AddRange(Prefix);
            if (comboBoxGraphList.Items.Count > 0 && selIndex < comboBoxGraphList.Items.Count)
                comboBoxGraphList.SelectedIndex = selIndex;
        }
    }
}
