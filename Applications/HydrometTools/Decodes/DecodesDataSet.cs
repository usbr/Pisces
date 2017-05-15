using System.Data;
using System.Linq;
namespace HydrometTools.Decodes
{
    
    
    public partial class DecodesDataSet {
        partial class equipmentpropertyDataTable
        {
        }

        
        public partial class unitconverterDataTable :  global::System.Data.TypedTableBase<unitconverterRow> 
        {

            public int GetNextID()
            {
                if( this.Rows.Count ==0)
                return 1;

                int max =this.AsEnumerable().Select(row => row.id).Max();
                return max + 1;

            }
        }

    }
}
