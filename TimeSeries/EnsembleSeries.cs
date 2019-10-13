using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reclamation.TimeSeries
{
    /// <summary>
    /// EnsembleSeries data is stored in a 'blob'
    /// in table name timeseries_blobs
    /// </summary>
    class EnsembleSeries : Series
    {
        public EnsembleSeries(TimeSeriesDatabase db, Reclamation.TimeSeries.TimeSeriesDatabaseDataSet.SeriesCatalogRow sr) : base(db, sr)
        {

        }

        protected override void ReadCore(DateTime t1, DateTime t2)
        {
            if (m_db != null)
            {
                if (this.ScenarioName != "")
                {
                    // TO DO.. scenarios...?
                }
                else
                {
                    int id = Convert.ToInt32(this.ConnectionStringToken("timeseries_blobs.id"));

                    string sql = "select * from  timeseries_blob where id = id";
                    var tbl = m_db.Server.Table(sql, "timeseries_blob");

                    
                    ///  Table = m_db.ReadTimeSeriesTable(ID, t1, t2);
                }


            }
        }

        private static List<DateTime> GetTimes(DataRow row)
        {
            var rval = new List<DateTime>();
            DateTime t = Convert.ToDateTime(row["timeseries_start_date"]);
            int count = Convert.ToInt32(row["member_length"]);
            for (int i = 0; i < count; i++)
            {
                rval.Add(t);
                t = t.AddHours(1); // hardcode hourly
            }
            return rval;
        }

        //https://stackoverflow.com/questions/7013771/decompress-byte-array-to-string-via-binaryreader-yields-empty-string
        static byte[] Decompress(byte[] data)
        {
            using (var compressedStream = new MemoryStream(data))
            using (var zipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
            using (var resultStream = new MemoryStream())
            {
                zipStream.CopyTo(resultStream);
                return resultStream.ToArray();
            }
        }

        private static List<List<float>> GetValues(DataRow row)
        {
            int compressed = Convert.ToInt32(row["compressed"]);
            var rval = new List<List<float>>();
            int member_count = Convert.ToInt32(row["member_count"]);
            int member_length = Convert.ToInt32(row["member_length"]);

            byte[] byte_values = (byte[])row["byte_value_array"];

            if (compressed != 0)
            {
                byte_values = Decompress(byte_values);
            }

            var numBytesPerMember = byte_values.Length / member_count;

            for (int i = 0; i < member_count; i++)
            {
                var floatValues = new float[member_length];
                Buffer.BlockCopy(byte_values, i * numBytesPerMember, floatValues, 0, numBytesPerMember);
                var values = new List<float>();
                values.AddRange(floatValues);
                rval.Add(values);
            }

            return rval;
        }
    }
}
