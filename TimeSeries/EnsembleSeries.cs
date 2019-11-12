using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reclamation.Core;

namespace Reclamation.TimeSeries
{
	/// <summary>
	/// EnsembleSeries data is stored in a 'blob'
	/// in table name timeseries_blobs
	/// </summary>
	public class EnsembleSeries : Series
	{

		int _timeseries_blobs_id;
		int _member_length;
	  int _ensemble_member_index;
		DateTime _timeseries_start_date;

		/// ctor used by PiscesFactory
		public EnsembleSeries(TimeSeriesDatabase db, Reclamation.TimeSeries.TimeSeriesDatabaseDataSet.SeriesCatalogRow sr) : base(db, sr)
		{
			_timeseries_blobs_id = ConnectionStringUtility.GetIntFromConnectionString(ConnectionString, "timeseries_blobs.id");
			_member_length = ConnectionStringUtility.GetIntFromConnectionString(ConnectionString, "member_length");
			var strDate = ConnectionStringToken("timeseries_start_date", "");

			_timeseries_start_date = DateTime.Parse(strDate);
			_ensemble_member_index = ConnectionStringUtility.GetIntFromConnectionString(ConnectionString, "ensemble_member_index");

		}
		//constructor used before adding to Pisces Database.
		public EnsembleSeries(DateTime timeseries_start_date, int timeseries_blobs_id, 
			int member_length, int ensemble_member_index = 0) : base()
		{
			_timeseries_blobs_id = timeseries_blobs_id;
			_timeseries_start_date = timeseries_start_date;
			_ensemble_member_index = ensemble_member_index;
			_member_length = member_length;

		Provider = this.ToString();
			ConnectionString = "timeseries_blobs.id=" + _timeseries_blobs_id
				+ ";member_length=" + _member_length
				+ ";ensemble_member_index=" + _ensemble_member_index
				+ ";timeseries_start_date=" + _timeseries_start_date.ToString("yyyy-MM-dd HH:mm:ss");

		}

		static Dictionary<int, List<List<float>>> _cache = new Dictionary<int, List<List<float>>>();

		protected override void ReadCore(DateTime t1, DateTime t2)
		{
			Clear();
			if (m_db != null)
			{
				if (this.ScenarioName != "")
				{
					// TO DO.. scenarios...?
				}
				else
				{
					List<List<float>> allValues;

					if (_cache.ContainsKey(_timeseries_blobs_id))
					{
						allValues = _cache[_timeseries_blobs_id];
					}
					else
					{
						string sql = "select * from  timeseries_blob where id = "+ _timeseries_blobs_id;
						var table = m_db.Server.Table("timeseries_blob",sql);
						if (table.Rows.Count == 0)
						{
							return;
						}

						allValues = GetValues(table.Rows[0]);
						_cache.Add(_timeseries_blobs_id, allValues);
					}
					DateTime t = _timeseries_start_date;
					List<float> vals = allValues[_ensemble_member_index];
					for (int i = 0; i<vals.Count; i++)
					{
						Add(t, vals[i]);
						t = IncremetDate(t);

					}
				}

			}
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

			int member_count = Convert.ToInt32(row["member_count"]);
			int member_length = Convert.ToInt32(row["member_length"]);

			var rval = new List<List<float>>();
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
