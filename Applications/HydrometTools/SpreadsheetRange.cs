#if SpreadsheetGear
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpreadsheetGear;
using Reclamation.TimeSeries.Excel;
using SpreadsheetGear.Advanced.Cells;
using Reclamation.TimeSeries;

namespace HydrometTools
{
    public class SpreadsheetRange
    {

        IRange rng;
        public SpreadsheetRange( IRange range)
        {
            rng = range;
        }

        public int RowCount
        {
            get { return rng.RowCount; }

        }
        public bool ValidInterpolationRange
        {
            get
            {
                return
                (
                rng.AreaCount == 1
                 && rng.ColumnCount == 1
                && rng.RowCount > 2
                && !ValidFlagRange
                );
            }
        }
        public bool ValidCalculationRange
        {
            get{
            return
                (
                rng.AreaCount == 1
                 && !AnyFlagColumnsSelected()
                && rng.RowCount > 0
                && !ValidFlagRange
                && rng.Column != 0
                );
            }
        
        }

        /// <summary>
        /// return a DateRange specifying the first and last Date in a continous single column range.
        /// </summary>
        public DateRange SelectedDateRange
        {
            get
            {
                rng.WorkbookSet.GetLock();
                if (rng.Areas.Count != 1)
                    throw new InvalidOperationException();

                    try
                    {
                        DateTime t1,t2;
                        var values = (IValues)rng.Worksheet;
                        if (
                        SpreadsheetGearExcel.TryReadingDate(rng.Worksheet.Workbook,
                            values[rng.Row, 0], out t1)
                            &&
                            SpreadsheetGearExcel.TryReadingDate(rng.Worksheet.Workbook,
                            values[rng.Row+rng.RowCount-1, 0], out t2)
                            )
                            return new DateRange(t1, t2);

                    }
                    finally
                    {
                        rng.WorkbookSet.ReleaseLock();
                    }

                throw new InvalidOperationException();
            }
        }

        public string[] SelectedRangeColumnNames
        {
            get
            {
                List<string> rval = new List<string>();

                if (rng.AreaCount > 1)
                    return rval.ToArray();

                rng.WorkbookSet.GetLock();
                try
                {
                    for (int c = 0; c < rng.ColumnCount; c++)
                    {
                        if (rng.Worksheet.Cells[0, c + rng.Column].Value == null)
                        {
                            rval.Add("");
                        }
                        else
                        {
                            string s = rng.Worksheet.Cells[0, c + rng.Column].Value.ToString();
                            rval.Add(s);
                        }

                    }

                    return rval.ToArray();
                }
                finally
                {
                    rng.WorkbookSet.ReleaseLock();
                }

            }
        }
        public bool ValidFlagRange
        {
            get
            {
                if (rng.AreaCount > 1)
                    return false;

                rng.WorkbookSet.GetLock();
                try
                {
                    if (rng.Worksheet.Cells[0, rng.Column].Value == null)
                        return false;

                    string s = rng.Worksheet.Cells[0, rng.Column].Value.ToString();
                    return s.ToLower().IndexOf("flag") >= 0;
                }
                finally
                {
                    rng.WorkbookSet.ReleaseLock();
                }
            }
        }

         bool AnyFlagColumnsSelected()
        {
                rng.WorkbookSet.GetLock();
                try
                {
                    for (int i = 0; i < rng.Areas.Count; i++)
                    {

                        if (rng.Areas[i].Worksheet.Cells[0, rng.Areas[i].Column].Value != null)
                        {
                            string s = rng.Areas[i].Worksheet.Cells[0, rng.Areas[i].Column].Value.ToString();
                            if (s.ToLower().IndexOf("flag") >= 0)
                            {
                                return true;
                            }
                        }
                    }
                }
                finally
                {
                    rng.WorkbookSet.ReleaseLock();
                }

             return false;
        }

        public void SetFlag(string flag)
        {
            try
            {
                rng.WorkbookSet.GetLock();
                for (int i = 0; i < rng.RowCount; i++)
                {
                    rng[i, 0].Value = flag;
                }
            }
            finally
            {
                rng.WorkbookSet.ReleaseLock();
            }
        }

        

        public void  ScaleSelectedRange(double scale)
        {
            try
            {
                rng.WorkbookSet.GetLock();

                for (int i = 0; i < rng.RowCount - 1; i++)
                {
                    double val = Convert.ToDouble(rng[i, 0].Value);
                    val = val * scale;
                    rng[i, 0].Value = val;
                }
            }
            finally
            {
                rng.WorkbookSet.ReleaseLock();
            }
        }

        public double Sum()
        {
            double min, max, sum;
            int count;
            Stats(out min, out max, out sum, out count);
            return sum;
        }
        public double Stats(out double min, out double max, out double sum,out int count)
        {
             sum = 0;
             min = double.MaxValue;
             max = double.MinValue;
             count = 0;
            try
            {
                rng.WorkbookSet.GetLock();

                for (int col = 0; col < rng.ColumnCount; col++)
                {
                    for (int i = 0; i < rng.RowCount; i++)
                    {
                        double val;
                        if (rng[i, col].Value == null)
                            continue;
                        if (!double.TryParse(rng[i, col].Value.ToString(), out val))
                            continue;

                        count++;

                        sum += val;

                        if (val < min)
                            min = val;
                        if (val > max)
                            max = val;

                        
                    }
                }
            }
            finally
            {
                rng.WorkbookSet.ReleaseLock();
            }
            return sum;
        }

        public Series SelectionToMonthlySeries(bool endOfMonth=true)
        {
            var s = new Series();
            s.TimeInterval = TimeInterval.Monthly;
            if (!ValidCalculationRange)
                return s;
            var t = SelectedDateRange.DateTime1.FirstOfMonth();
            if( endOfMonth)
                t = SelectedDateRange.DateTime1.EndOfMonth();

            //var t2 = SelectedDateRange.DateTime2;

            try
            {
                rng.WorkbookSet.GetLock();

                for (int i = 0; i < rng.RowCount; i++)
                {
                    if (rng[i,0].Value == null)
                    {
                        s.AddMissing(t);
                    }
                    else
                    {
                       var val = Convert.ToDouble( rng[i, 0].Value);
                       s.Add(t, val);
                    }
                    if( endOfMonth)
                        t = t.AddMonths(1).EndOfMonth();
                    else
                        t = t.AddMonths(1).FirstOfMonth();
                }
            }
            finally
            {
                rng.WorkbookSet.ReleaseLock();
            }


            return s;

        }

        public void InsertSeriesValues(Series s, string flag="",int columnOffSet=0)
        {

            if (!ValidCalculationRange)
                return;

            if (rng.RowCount != s.Count)
            {
                System.Windows.Forms.MessageBox.Show("range and Series are not the same length");
                return;
            }

            try
            {
                rng.WorkbookSet.GetLock();

                for (int i = 0; i < rng.RowCount; i++)
                {
                    if (s[i].IsMissing)
                    {
                        rng[i, columnOffSet].Value = null;
                    }
                    else
                    {
                        rng[i, columnOffSet].Value = s[i].Value;

                        if (flag != "")
                        {// set flag in column to the right.
                            rng[i, columnOffSet+1].Value = flag;
                        }
                    }
                }
            }
            finally
            {
                rng.WorkbookSet.ReleaseLock();
            }


        }

        internal void FillGaps()
        {
            int idx1 = -1;
            int idx2 = -1; // begin and end of interpolate range

            try
            {
                rng.WorkbookSet.GetLock();
                idx1 = FindStartIndexForInterpolation(rng, 0);
                idx2 = FindEndIndexForInterpolation(rng, idx1+1);

                while (idx1 >=0 && idx2 >0)
                {
                    InterpolateRange(rng, idx1, idx2, "e");
                    idx1 = FindStartIndexForInterpolation(rng, idx2);
                    idx2 = FindEndIndexForInterpolation(rng, idx1+1);
                }

            }
            finally
            {
                rng.WorkbookSet.ReleaseLock();
            }
        }

        private int FindStartIndexForInterpolation(IRange rng, int startIndex)
        {
            if (startIndex < 0)
                return -1;
            int i = startIndex;
            while(i < rng.RowCount-1)
            {// find value with missing in at least the next row.
                if (HasValue(rng[i, 0]) && !HasValue(rng[i + 1, 0]) )
                    return i;
                i++;
            }

            return -1;
        }

        private int FindEndIndexForInterpolation(IRange rng, int startIndex)
        {
            if (startIndex < 0)
                return -1;
            int i = startIndex;
            while (i < rng.RowCount)
            {// find data.
                if (HasValue(rng[i, 0]) )
                    return i;
                i++;
            }

            return -1;
        }

        private bool HasValue(IRange range)
        {
            double val = 0;
            if (range.Value == null)
                return false;
            return double.TryParse(range.Value.ToString(), out val);
        }

        public void Interpolate(string flag = "")
        {

            try
            {
                rng.WorkbookSet.GetLock();

                if (ValidInterpolationRange)
                {
                    InterpolateRange(rng,0,rng.RowCount-1, flag);
                }
            }
            finally
            {
                rng.WorkbookSet.ReleaseLock();
            }
        }

        private static void InterpolateRange(IRange range ,int startIndex,
            int endIndex, string flag)
        {
            double d1 = Convert.ToDouble(range[startIndex, 0].Value);
            double d2 = Convert.ToDouble(range[endIndex, 0].Value);

            double increment = (d2 - d1) / (endIndex - startIndex);

            double v = d1;
            for (int i = startIndex+1; i < endIndex; i++)
            {
                v += increment;
                range[i, 0].Value = v;

                if (flag != "")
                {// set flag in column to the right.
                    range[i, 1].Value = flag;
                }
            }
        }

        public bool ValidInterpolationWithStyle {
            
             get
            {
                return
                (
                 rng.AreaCount == 2
                 && rng.Areas[0].RowCount == rng.Areas[1].RowCount
                && rng.RowCount > 2
                && !AnyFlagColumnsSelected()
                    //rng.ColumnCount == 1
                );
            }  

        }

        /// <summary>
        /// Interpolate using two ranges, the first has a data gap, the second has full data
        /// </summary>
        /// <param name="flag"></param>
        internal void InterpolateWithStyle(string flag = "e")
        {
            if (!ValidInterpolationWithStyle)
                throw new Exception("Serious error call paul... should never happen");

            try
            {
                rng.WorkbookSet.GetLock();
                var rngA = rng.Areas[0]; // data to be estimated
                var rngS = rng.Areas[1]; // surrgate data to use as pattern
                
                    double a1 = Convert.ToDouble(rngA[0, 0].Value);
                    double an = Convert.ToDouble(rngA[rngA.RowCount - 1, 0].Value);

                    double s1 = Convert.ToDouble(rngS[0, 0].Value);
                    double sn = Convert.ToDouble(rngS[rngS.RowCount - 1, 0].Value);

                    double offset1 = a1 - s1;
                    double offset2 = an - sn;

                    double increment = (offset2 - offset1) / (rng.RowCount - 1);

                    double offset = offset1;
                        for (int i = 1; i < rng.RowCount - 1; i++)
                        {
                            offset += increment;
                            double v =  Convert.ToDouble(rngS[i, 0].Value) + offset;
                            rngA[i, 0].Value = v;

                            if (flag != "")
                            {// set flag in column to the right.
                                rng[i, 1].Value = flag;
                            }
                        }
            }
            finally
            {
                rng.WorkbookSet.ReleaseLock();
            }

        }
    }
}
#endif