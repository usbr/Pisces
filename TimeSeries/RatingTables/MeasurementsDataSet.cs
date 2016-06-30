using System.Collections;
using System.Linq;

namespace Reclamation.TimeSeries.RatingTables {
    
    
    public partial class MeasurementsDataSet {

        public partial class measurementDataTable : global::System.Data.TypedTableBase<measurementRow>
        {

            public int NextID()
            {
                if (this.Rows.Count == 0)
                    return 1;

                int max = this.AsEnumerable().Select(row => row.id).Max();
                return max + 1;

            }
        }
        

    }
}
