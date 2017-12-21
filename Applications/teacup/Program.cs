using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using Reclamation.Core;
using Reclamation.TimeSeries.Hydromet;
namespace Teacup
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger.EnableLogger(); // for debug info
            HydrometHost HServer = HydrometHost.PNLinux;
            DateTime date = DateTime.Now.AddDays(-1).Date;

            if (args.Length != 3 && args.Length != 4)
            {
                Console.WriteLine("Usage: TeaCup.exe infile outfile configfile  [date: mm/dd/yyyy]  ");
                return;
            }
            
            if (args.Length == 4)//Given a date by the user
            {
                date = Convert.ToDateTime(args[3]);
            }
            //Read the config file
            string[] lines = System.IO.File.ReadAllLines(args[2]);
            if (args[2].Contains("yak"))
                HServer = HydrometHost.Yakima;
            
            var gif = Image.FromFile(args[0]);
            var bmp = new Bitmap(gif);

            WriteDate(date, bmp);

            for (int i = 0; i < lines.Length; i++)
            {
                var cfg = new ConfigLine(lines[i]);

                if (cfg.IsCFS)
                {
                    DrawCFS(HServer, date, bmp, cfg);
                }
                else if (cfg.IsTeacup)
                {
                    DrawTeacup(HServer, date, bmp, cfg);
                }
                else if (cfg.IsLine)
                {
                    DrawLine(HServer,date, bmp, cfg);

                }
            }
            bmp.Save(args[1]);//save the image file 
        }

        private static void DrawLine(HydrometHost HServer, DateTime date, Bitmap bmp, ConfigLine cfg)
        {
            string number = "";
            double value = ReadHydrometValue(cfg.cbtt, cfg.pcode, date, HServer );
            //check for missing values and set the output number of digits to report
            if ((cfg.units == "Feet") || (cfg.units == "%"))
            {
                number = value.ToString("F2");
            }
            else
            {
                number = value.ToString("F0");
            }
            if (value == 998877)
            {
                number = "MISSING";
            }
            string Text = cfg.ResName + "  " + number + " " + cfg.units;
            Point Location = new Point(cfg.col, cfg.row);
            Rectangle rect1 = new Rectangle(cfg.col, cfg.row + 2, 90, 10);
            using (Graphics graphics = Graphics.FromImage(bmp))
            {
                using (Font arialFont = new Font("Arial", 7))
                {
                    graphics.FillRectangle(Brushes.White, rect1);
                    graphics.DrawString(Text, arialFont, Brushes.Red, Location);
                }
            }
        }

        private static void WriteDate(DateTime date, Bitmap bmp)
        {
            string firstText = date.ToString("MM/dd/yyyy");
            //Location of the date
            PointF firstLocation = new PointF(2f, 3f);
            //load the image file  
            Rectangle rect = new Rectangle(0, 0, 100, 20);
            //Fill the background and draw the string
            using (Graphics graphics = Graphics.FromImage(bmp))
            {
                using (Font TNRFont = new Font("Carbon", 10))
                {
                    graphics.FillRectangle(Brushes.White, rect);
                    graphics.DrawString(firstText, TNRFont, Brushes.Blue, firstLocation);
                }
            }
        }

        private static void DrawCFS(HydrometHost HServer, DateTime date, Bitmap bmp, ConfigLine cfg)
        {
            string number;
            double value = ReadHydrometValue(cfg.cbtt, cfg.pcode, date, HServer);
            //check for missing values and set the output number of digits to report
            if (value == 998877)
            {
                number = "MISSING";
            }
            else
            {
                number = value.ToString("F0");
            }
            string Text = cfg.cbtt + "  " + number + " " + cfg.type;
            Point Location = new Point(cfg.col, cfg.row + 5);
            Rectangle rect1 = new Rectangle(cfg.col, cfg.row + 7, 85, 10);
            using (Graphics graphics = Graphics.FromImage(bmp))
            {
                using (Font arialFont = new Font("Arial", 7))
                {
                    graphics.FillRectangle(Brushes.White, rect1);
                    graphics.DrawString(Text, arialFont, Brushes.Red, Location);
                }
            }
        }

        private static void DrawTeacup(HydrometHost HServer, DateTime date, Bitmap bmp, ConfigLine cfg)
        {
            string number = "";
            string Percent;
            double percent;
            double value = ReadHydrometValue(cfg.cbtt, cfg.pcode, date, HServer);
            //Determine the percent full

            if (value == 998877)
            {
                percent = 0;
            }
            else
                percent = value / cfg.capacity;

            if (percent >= 1)
            {
                percent = 1;
            }
            else if (percent <= 0)
            {
                percent = 0;
            }

            double area = 400 * cfg.size * cfg.size + 3200 * cfg.size * cfg.size * percent;
            area = Math.Sqrt(area) - 20 * cfg.size;
            area = area / (40 * cfg.size);
            if (area >= 1.000)
            {
                area = 1.000;
            }
            if (area <= 0.000)
            {
                area = 0.000;
            }
            Int32 full = Convert.ToInt32(area * 100);
            //check for missing values and set the output number of digits to report
            if (value == 998877)
            {
                number = "MISSING";
                Percent = "MISSING";
            }
            else
            {
                number = value.ToString("F0");
                Percent = (percent * 100).ToString("F0");
            }
            //Create Isosceles trapizoid
            string Text = cfg.ResName + "\n" + number + "/" + cfg.capacity + "\n" + Percent + "% Full";
            Point Location = new Point(cfg.col, cfg.row);
            //Line color and size
            Pen bluePen = new Pen(Color.RoyalBlue, 2);
            //Fill Color for background
            SolidBrush whiteBrush = new SolidBrush(Color.White);
            SolidBrush blueBrush = new SolidBrush(Color.RoyalBlue);
            //Setting the points of the trapezoid
            Point point1 = new Point(cfg.col, cfg.row); //lower left
            Point point2 = new Point(cfg.col + 10 * cfg.size, cfg.row); //lower right
            Point point3 = new Point(cfg.col + 20 * cfg.size, cfg.row - 20 * cfg.size); //upper right
            Point point4 = new Point(cfg.col - 10 * cfg.size, cfg.row - 20 * cfg.size); //upper left
            Point[] curvePoints = { point1, point2, point3, point4 };
            //setting points of percent full
            Point point1f = new Point(cfg.col, cfg.row); //lower left
            Point point2f = new Point(cfg.col + 10 * cfg.size, cfg.row); //lower right
            Point point3f = new Point(cfg.col + 10 * cfg.size + cfg.size * full * 10 / 100, cfg.row - 20 * cfg.size * full / 100); //upper right
            Point point4f = new Point(cfg.col - 10 * cfg.size * full / 100, cfg.row - 20 * cfg.size * full / 100); //upper left
            Point[] fullPoints = { point1f, point2f, point3f, point4f };
            //Create Graphics
            using (Graphics graphics = Graphics.FromImage(bmp))
            {
                using (Font arialFont = new Font("Arial", 8))
                {
                    graphics.FillPolygon(whiteBrush, curvePoints);
                    graphics.DrawPolygon(bluePen, curvePoints);
                    graphics.DrawString(Text, arialFont, Brushes.Red, Location);
                    graphics.FillPolygon(blueBrush, fullPoints);
                }
            }
        }

        private static double ReadHydrometValue(string cbtt, string pcode, DateTime date,HydrometHost server) 
        {
            //Get hydromet data
            HydrometDailySeries s = new HydrometDailySeries(cbtt, pcode,server);
            s.Read(date, date);
            double value = 998877;
            if (s.Count > 0 && !s[0].IsMissing)
                value = s[0].Value;

            return value;
        }
    }
}
