using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Reclamation.TimeSeries.Forms.Graphing
{
    public class SeriesSettings
    {

        public SeriesSettings(Color color, int width=1)
        {
            this.m_color = color;
            this.m_width = width;
        }

        public SeriesSettings()
        {
            this.m_color = Color.Black;
            this.m_width = 1;
        }


        private int m_width = 1;

        public int Width
        {
            get { return m_width; }
            set { m_width = value; }
        }
        private Color m_color;

        public Color Color
        {
            get { return m_color; }
            set { m_color = value; }
        }
    }
}
