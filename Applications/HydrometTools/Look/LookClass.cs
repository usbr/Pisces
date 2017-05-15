using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.ComponentModel;

namespace Look
{
    class LookClass
    {
        public static DataGridView AddCheckBox(DataGridView data, DataTable tbl)
        {
                DataGridViewCheckBoxColumn col = new DataGridViewCheckBoxColumn(false);
                col.Name = "check";
                col.HeaderText = "select";
                col.FalseValue = false;
                col.TrueValue = true;
                data.Columns.Add(col);
                data.Columns["check"].DisplayIndex = 0;
                return data;
        }

    }
}
