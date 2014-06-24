// Karl Tarbet  
// March 9, 2007
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Reclamation.Core;

//http://waterdata.usgs.gov/nwis/nwisman/?site_no=13032500&agency_cd=USGS

namespace Reclamation.TimeSeries.Usgs
{
    public enum UsgsDailyParameter
    {
      DailyMeanDischarge,
      DailyMeanTemperature,
      DailyMaxTemperature,
      DailyMinTemperature
    }

    /// <summary>
    /// Retreives inventory from usgs NWIS server
    /// </summary>
    public class UsgsInventory// : IDisposable
    {

        //static string s_gzipURL = @"http://waterdata.usgs.gov/nwis/inventory?search_site_no=012&search_site_no_match_type=beginning&sort_key=site_no&group_key=NONE&format=sitefile_output&sitefile_output_format=rdb_gz&column_name=agency_cd&column_name=site_no&column_name=station_nm&column_name=lat_va&column_name=long_va&column_name=dec_lat_va&column_name=dec_long_va&column_name=coord_meth_cd&column_name=coord_acy_cd&column_name=coord_datum_cd&column_name=dec_coord_datum_cd&column_name=district_cd&column_name=state_cd&column_name=county_cd&column_name=country_cd&column_name=land_net_ds&column_name=map_nm&column_name=map_scale_fc&column_name=alt_va&column_name=alt_meth_cd&column_name=alt_acy_va&column_name=alt_datum_cd&column_name=huc_cd&column_name=basin_cd&column_name=topo_cd&column_name=station_type_cd&column_name=agency_use_cd&column_name=data_types_cd&column_name=instruments_cd&column_name=construction_dt&column_name=inventory_dt&column_name=drain_area_va&column_name=contrib_drain_area_va&column_name=tz_cd&column_name=local_time_fg&column_name=reliability_cd&column_name=gw_file_cd&column_name=gw_type_cd&column_name=nat_aqfr_cd&column_name=aqfr_cd&column_name=aqfr_type_cd&column_name=well_depth_va&column_name=hole_depth_va&column_name=depth_src_cd&column_name=project_no&column_name=rt_bol&column_name=discharge_begin_date&column_name=discharge_end_date&column_name=discharge_count_nu&column_name=peak_begin_date&column_name=peak_end_date&column_name=peak_count_nu&column_name=qw_begin_date&column_name=qw_end_date&column_name=qw_count_nu&column_name=gw_begin_date&column_name=gw_end_date&column_name=gw_count_nu&list_of_search_criteria=search_site_no";
        //static string s_basicURL = @"http://waterdata.usgs.gov/nwis/inventory?search_site_no=012&search_site_no_match_type=beginning&sort_key=site_no&group_key=NONE&format=sitefile_output&sitefile_output_format=rdb&column_name=agency_cd&column_name=site_no&column_name=station_nm&column_name=lat_va&column_name=long_va&column_name=dec_lat_va&column_name=dec_long_va&column_name=coord_meth_cd&column_name=coord_acy_cd&column_name=coord_datum_cd&column_name=dec_coord_datum_cd&column_name=district_cd&column_name=state_cd&column_name=county_cd&column_name=country_cd&column_name=land_net_ds&column_name=map_nm&column_name=map_scale_fc&column_name=alt_va&column_name=alt_meth_cd&column_name=alt_acy_va&column_name=alt_datum_cd&column_name=huc_cd&column_name=basin_cd&column_name=topo_cd&column_name=station_type_cd&column_name=agency_use_cd&column_name=data_types_cd&column_name=instruments_cd&column_name=construction_dt&column_name=inventory_dt&column_name=drain_area_va&column_name=contrib_drain_area_va&column_name=tz_cd&column_name=local_time_fg&column_name=reliability_cd&column_name=gw_file_cd&column_name=gw_type_cd&column_name=nat_aqfr_cd&column_name=aqfr_cd&column_name=aqfr_type_cd&column_name=well_depth_va&column_name=hole_depth_va&column_name=depth_src_cd&column_name=project_no&column_name=rt_bol&column_name=discharge_begin_date&column_name=discharge_end_date&column_name=discharge_count_nu&column_name=peak_begin_date&column_name=peak_end_date&column_name=peak_count_nu&column_name=qw_begin_date&column_name=qw_end_date&column_name=qw_count_nu&column_name=gw_begin_date&column_name=gw_end_date&column_name=gw_count_nu&list_of_search_criteria=search_site_no";
        static string s_exactMatch = @"http://waterdata.usgs.gov/nwis/inventory?search_site_no=13010065&search_site_no_match_type=exact&sort_key=site_no&group_key=NONE&format=sitefile_output&sitefile_output_format=rdb&column_name=agency_cd&column_name=site_no&column_name=station_nm&column_name=lat_va&column_name=long_va&column_name=dec_lat_va&column_name=dec_long_va&column_name=coord_meth_cd&column_name=coord_acy_cd&column_name=coord_datum_cd&column_name=dec_coord_datum_cd&column_name=district_cd&column_name=state_cd&column_name=county_cd&column_name=country_cd&column_name=land_net_ds&column_name=map_nm&column_name=map_scale_fc&column_name=alt_va&column_name=alt_meth_cd&column_name=alt_acy_va&column_name=alt_datum_cd&column_name=huc_cd&column_name=basin_cd&column_name=topo_cd&column_name=station_type_cd&column_name=agency_use_cd&column_name=data_types_cd&column_name=instruments_cd&column_name=construction_dt&column_name=inventory_dt&column_name=drain_area_va&column_name=contrib_drain_area_va&column_name=tz_cd&column_name=local_time_fg&column_name=reliability_cd&column_name=gw_file_cd&column_name=gw_type_cd&column_name=nat_aqfr_cd&column_name=aqfr_cd&column_name=aqfr_type_cd&column_name=well_depth_va&column_name=hole_depth_va&column_name=depth_src_cd&column_name=project_no&column_name=rt_bol&column_name=discharge_begin_date&column_name=discharge_end_date&column_name=discharge_count_nu&column_name=peak_begin_date&column_name=peak_end_date&column_name=peak_count_nu&column_name=qw_begin_date&column_name=qw_end_date&column_name=qw_count_nu&column_name=gw_begin_date&column_name=gw_end_date&column_name=gw_count_nu&list_of_search_criteria=search_site_no";

        UsgsRDBFile inventoryTable;

        /*  Example Inventory
    #
#
# US Geological Survey
# retrieved: 2007-03-09 10:33:58 EST
# URL: http://nwis.waterdata.usgs.gov/nwis/inventory
#
# The Site File stores location and general information about ground water,
# surface water, and meteorological sites
# for sites in USA.
#
# The following selected fields are included in this output:
#
#  agency_cd       -- Agency
#  site_no         -- Site identification number
#  station_nm      -- Site name
#  lat_va          -- DMS latitude
#  long_va         -- DMS longitude
#  dec_lat_va      -- Decimal latitude
#  dec_long_va     -- Decimal longitude
#  coord_meth_cd   -- Latitude-longitude method
#  coord_acy_cd    -- Latitude-longitude accuracy
#  coord_datum_cd  -- Latitude-longitude datum
#  dec_coord_datum_cd -- Decimal Latitude-longitude datum
#  district_cd     -- District code
#  state_cd        -- State code
#  county_cd       -- County code
#  country_cd      -- Country code
#  land_net_ds     -- Land net location description
#  map_nm          -- Name of location map
#  map_scale_fc    -- Scale of location map
#  alt_va          -- Altitude of Gage/land surface
#  alt_meth_cd     -- Method altitude determined
#  alt_acy_va      -- Altitude accuracy
#  alt_datum_cd    -- Altitude datum
#  huc_cd          -- Hydrologic unit code
#  basin_cd        -- Drainage basin code
#  topo_cd         -- Topographic setting code
#  station_type_cd -- Site type code
#  agency_use_cd   -- Agency use of site code
#  data_types_cd   -- Flags for the type of data collected
#  instruments_cd  -- Flags for instruments at site
#  construction_dt -- Date of first construction
#  inventory_dt    -- Date site established or inventoried
#  drain_area_va   -- Drainage area
#  contrib_drain_area_va -- Contributing drainage area
#  tz_cd           -- Mean Greenwich time offset
#  local_time_fg   -- Local standard time flag
#  reliability_cd  -- Data reliability code
#  gw_file_cd      -- Data-other GW files
#  gw_type_cd      -- Type of ground water site
#  nat_aqfr_cd     -- National aquifer code
#  aqfr_cd         -- Local aquifer code
#  aqfr_type_cd    -- Local aquifer type code
#  well_depth_va   -- Well depth
#  hole_depth_va   -- Hole depth
#  depth_src_cd    -- Source of depth data
#  project_no      -- Project number
#  rt_bol          -- Real-time data flag
#  discharge_begin_date -- Daily streamflow data begin date
#  discharge_end_date -- Daily streamflow data end date
#  discharge_count_nu -- Daily streamflow data count
#  peak_begin_date -- Peak streamflow data begin date
#  peak_end_date   -- Peak streamflow data end date
#  peak_count_nu   -- Peak streamflow data count
#  qw_begin_date   -- Water quality data begin date
#  qw_end_date     -- Water quality data end date
#  qw_count_nu     -- Water quality data count
#  gw_begin_date   -- Ground-water data begin date
#  gw_end_date     -- Ground-water data end date
#  gw_count_nu     -- Ground-water data count
#
#
# query started 2007-03-09 10:33:58 EST
#
# there are 1 sites matching the search criteria.
#
#
agency_cd	site_no	station_nm	lat_va	long_va	dec_lat_va	dec_long_va	coord_meth_cd	coord_acy_cd	coord_datum_cd	dec_coord_datum_cd	district_cd	state_cd	county_cd	country_cd	land_net_ds	map_nm	map_scale_fc	alt_va	alt_meth_cd	alt_acy_va	alt_datum_cd	huc_cd	basin_cd	topo_cd	station_type_cd	agency_use_cd	data_types_cd	instruments_cd	construction_dt	inventory_dt	drain_area_va	contrib_drain_area_va	tz_cd	local_time_fg	reliability_cd	gw_file_cd	gw_type_cd	nat_aqfr_cd	aqfr_cd	aqfr_type_cd	well_depth_va	hole_depth_va	depth_src_cd	project_no	rt_bol	discharge_begin_date	discharge_end_date	discharge_count_nu	peak_begin_date	peak_end_date	peak_count_nu	qw_begin_date	qw_end_date	qw_count_nu	gw_begin_date	gw_end_date	gw_count_nu
5s	15s	50s	11s	12s	16n	16n	1s	1s	10s	10s	3s	2s	3s	2s	23s	20s	7s	8s	1s	3s	10s	16s	2s	1s	20s	1s	30s	30s	8s	8s	8s	8s	6s	1s	1s	30s	1s	10s	8s	1s	8s	8s	1s	12s													
USGS	13010065	SNAKE RIVER AB JACKSON LAKE AT FLAGG RANCH WY	440556	1104003	44.09888889	-110.66750000	G	S	NAD83	NAD83	16	56	039	US		Flagg Ranch	  24000	6801.61	L	.01	NGVD29	17040101			YNNNNNNNNNNNNNNNNNNN	A	ANNANNNNNNNNNNNNNNNNNNNNNNNNNN	YNNNYNNNNNNNNNNNNNNNNNNNNNNNNN			486	486	MST	Y		NNNNNNNN									1	1983-10-01	2007-02-06	8530	1984-05-31	2006-05-22	23	1985-10-01	2004-09-22	255	0000-00-00	0000-00-00	0

    */

        public UsgsInventory(string siteNumber)
        {
            string url = s_exactMatch.Replace("search_site_no=13010065", "search_site_no=" + siteNumber);

            string[] data = Web.GetPage(url, true);
            inventoryTable = new UsgsRDBFile(data);

            if (inventoryTable.Rows.Count <= 0)
            {
                throw new Exception("Site " + siteNumber + " not found");
            }
        }

        //#  discharge_begin_date -- Daily streamflow data begin date
        //#  discharge_end_date -- Daily streamflow data end date
        public DateTime DischargeBeginDate
        {
            get
            {
                string s = inventoryTable.Rows[0]["discharge_begin_date"].ToString();
                DateTime date = DateTime.Now;
                if (!DateTime.TryParse(s, out date))
                {
                    Logger.WriteLine("Error parsing date '" + s + "'");
                }
                return date;
                //discharge_end_date

            }
        }

        public bool HasMeanDailyStreamFlow
        {
            get
            {
                return !EmptyColumn("discharge_begin_date");
            }
        }
        public bool HasWaterQuality
        {
            get
            {
                return !EmptyColumn("qw_begin_date");
            }
        }

        private bool EmptyColumn(string columnName)
        {
            return
            (inventoryTable.Rows[0][columnName].ToString().Trim() == "");
        }


        //public void Dispose()
        //{
        //    Dispose(true);
        //    GC.SuppressFinalize(this);
        //}

        //protected virtual void Dispose(bool disposing)
        //{
            
        //}
    }

}
