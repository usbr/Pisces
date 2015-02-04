using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Reclamation.TimeSeries.Forms.MetaData
{
    public partial class DataRowEditor : UserControl
    {
        public DataRowEditor()
        {
            InitializeComponent();
        }

        public void SetDataRow(DataRow row)
        {
            var wrapper = new RowWrapper(row);
            propertyGrid1.SelectedObject = wrapper;
            //    wrapper.Exclude.Add("ID");
     //   wrapper.Exclude.Add("Bar");

        }
    }

[TypeConverter(typeof(RowWrapper.RowWrapperConverter))]
class RowWrapper
{
    private readonly List<string> exclude = new List<string>();
    public List<string> Exclude { get { return exclude; } }
    private readonly DataRowView rowView;
    public RowWrapper(DataRow row)
    {
        DataView view = new DataView(row.Table);
        foreach (DataRowView tmp in view)
        {
            if (tmp.Row == row)
            {
                rowView = tmp;
                break;
            }
        }
    }
    static DataRowView GetRowView(object component)
    {
        return ((RowWrapper)component).rowView;
    }
    class RowWrapperConverter : TypeConverter
    {
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        public override PropertyDescriptorCollection GetProperties(
            ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            RowWrapper rw = (RowWrapper)value;
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(
                GetRowView(value), attributes);
            List<PropertyDescriptor> result = new List<PropertyDescriptor>(props.Count);
            foreach (PropertyDescriptor prop in props)
            {
                if (rw.Exclude.Contains(prop.Name)) continue;
                result.Add(new RowWrapperDescriptor(prop));
            }
            return new PropertyDescriptorCollection(result.ToArray());
        }
    }
    class RowWrapperDescriptor : PropertyDescriptor
    {
        static Attribute[] GetAttribs(AttributeCollection value)
        {
            if (value == null) return null;
            Attribute[] result = new Attribute[value.Count];
            value.CopyTo(result, 0);
            return result;
        }
        readonly PropertyDescriptor innerProp;
        public RowWrapperDescriptor(PropertyDescriptor innerProperty)
            : base(
                innerProperty.Name, GetAttribs(innerProperty.Attributes))
        {
            this.innerProp = innerProperty;
        }


        public override bool ShouldSerializeValue(object component)
        {
            return innerProp.ShouldSerializeValue(GetRowView(component));
        }
        public override void ResetValue(object component)
        {
            innerProp.ResetValue(GetRowView(component));
        }
        public override bool CanResetValue(object component)
        {
            return innerProp.CanResetValue(GetRowView(component));
        }
        public override void SetValue(object component, object value)
        {
            innerProp.SetValue(GetRowView(component), value);
        }
        public override object GetValue(object component)
        {
            return innerProp.GetValue(GetRowView(component));
        }
        public override Type PropertyType
        {
            get { return innerProp.PropertyType; }
        }
        public override Type ComponentType
        {
            get { return typeof(RowWrapper); }
        }
        public override bool IsReadOnly
        {
            get { return innerProp.IsReadOnly; }
        }
    }
}




}
