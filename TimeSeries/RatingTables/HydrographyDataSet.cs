using System.Collections;
using System.Linq;
using System.Data;

namespace Reclamation.TimeSeries.RatingTables {
    
    
    public partial class HydrographyDataSet {

        TimeSeriesDatabase m_db;
        public HydrographyDataSet CreateInstance(TimeSeriesDatabase db)
        {
            HydrographyDataSet rval = new HydrographyDataSet();
            m_db = db;
            return rval;
        }

        public partial class measurementDataTable :  TypedTableBase<measurementRow>
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

 
