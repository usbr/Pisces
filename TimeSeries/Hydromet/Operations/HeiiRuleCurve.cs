using System;

namespace Reclamation.TimeSeries.Hydromet.Operations
{
    class HeiiRuleCurve: HydrometRuleCurve
    {

        private Series qu;
        private DateTime quDate20000 = DateTime.MaxValue; // date that 20,000 cfs occurs


        PeriodicSeries solid;
        PeriodicSeries dashed;

        public HeiiRuleCurve(string cbtt, int waterYear,FillType fType):base(cbtt,fType)
        {
            Init(waterYear);
        }

        private void Init(int waterYear)
        {
            qu = new HydrometDailySeries("HEII", "QU",HydrometHost.PNLinux);
            var t1 = new DateTime(waterYear-1, 10, 1);
            var t2 = new DateTime(waterYear, 8, 1);
            if (t2 > DateTime.Now && t2.Year != 7100)
            {
                t2 = DateTime.Now.AddDays(-1);
            }
            qu.Read(t1, t2);
            // find date where (if)  20,000 cfs occurs at heii.

            for (int i = 0; i < qu.Count; i++)
            {
                Point pt = qu[i];
                if( !pt.IsMissing  && pt.Value >=20000)
                {
                    quDate20000 = pt.DateTime;
                    break;
                }
            }

            solid = FcPlotDataSet.GetPeriodicSeries("Heii.Solid");
            dashed = FcPlotDataSet.GetPeriodicSeries("Heii.Dashed");
        }

        public override double LookupRequiredSpace(DateTime t, double resid, out string flag)
        {
            bool dashedLines = t >= quDate20000;

            double scaleFactor = 1.0;
            //if (CurveName.ToUpper() == "JCK")
            //    scaleFactor = 0.25;

            if (dashedLines)
            {
                flag = "D";
                return dashed.Interpolate2D(t, resid)*scaleFactor;
            }
            else
            {
                flag = "S";
                return solid.Interpolate2D(t, resid)*scaleFactor;
            }

        }

        //internal override SeriesList CalculateVariableRuleCurves()
        //{
        //    var rval = new SeriesList();

        //    //var ps = GetPeriodicSeries(curveName);

        //    //            ps.Table.Columns

        //    return rval;
        //}
        
    }
}
