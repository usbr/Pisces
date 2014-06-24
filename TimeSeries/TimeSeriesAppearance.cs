using System;
using System.IO;

namespace Reclamation.TimeSeries
{

    public enum Styles { Line, Point, ThinLine, DashedLine };

    /// <summary>
    /// TimeSeriesAppearance allows you to set 
    /// the color, and line style for a Series
    /// </summary>
    public class TimeSeriesAppearance
    {
        private string _color;
        private int _yAxisNumber;
        private Styles _style;
        private string _legendText;
        private string _numberFormat;
        private bool _stairStep;

        public bool StairStep
        {
            get { return _stairStep; }
            set { _stairStep = value; }
        }

        public TimeSeriesAppearance()
        {
            _legendText = "";
            _color = "Black";
            _yAxisNumber = 1;
            _style = Styles.Line;
            _numberFormat = "";
        }



        public TimeSeriesAppearance Copy()
        {
            TimeSeriesAppearance rval = new TimeSeriesAppearance();
            rval._color = _color;
            rval._legendText = _legendText;
            rval._yAxisNumber = _yAxisNumber;
            rval._style = _style;
            rval._numberFormat = _numberFormat;
            return rval;
        }


        public string NumberFormat
        {
            get { return _numberFormat; }
            set { _numberFormat = value; }
        }

        public string LegendText
        {
            get { return _legendText; }
            set {
                if (value == "")
                {
                    Console.WriteLine();
                }

                this._legendText = value;
            }
        }
        /// <summary>
        /// Get or set the color as a KnowColor Text (example "Black")
        /// </summary>
        public string Color
        {
            get
            {
                return _color;
            }
            set
            {
                this._color = value;
            }
        }

        /// <summary>
        /// Number to indicate which YAxis this series will be plotted on
        /// </summary>
        public int YAxisNumber
        {
            get
            {
                return this._yAxisNumber;
            }
            set
            {
                _yAxisNumber = value;
            }
        }

        public Styles Style
        {
            get
            {
                return _style;
            }
            set
            {
                this._style = value;
            }
        }



    }
}
