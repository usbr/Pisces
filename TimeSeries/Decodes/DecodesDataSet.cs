using System.Data;
using System.Linq;
namespace Reclamation.TimeSeries.Decodes {
    
    
    public partial class DecodesDataSet {

        
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
