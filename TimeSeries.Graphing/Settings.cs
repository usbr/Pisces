using System.Collections.Specialized;
using System.Drawing;
namespace Reclamation.TimeSeries.Graphing.Properties {
    
    
    // This class allows you to handle specific events on the settings class:
    //  The SettingChanging event is raised before a setting's value is changed.
    //  The PropertyChanged event is raised after a setting's value is changed.
    //  The SettingsLoaded event is raised after the setting values are loaded.
    //  The SettingsSaving event is raised before the setting values are saved.
    internal sealed partial class Settings {
        
        public Settings() {
            // // To add event handlers for saving and changing settings, uncomment the lines below:
            //
            // this.SettingChanging += this.SettingChangingEventHandler;
            //
            // this.SettingsSaving += this.SettingsSavingEventHandler;
            //
        }
        
        private void SettingChangingEventHandler(object sender, System.Configuration.SettingChangingEventArgs e) {
            // Add code to handle the SettingChangingEvent event here.
        }
        
        private void SettingsSavingEventHandler(object sender, System.ComponentModel.CancelEventArgs e) {
            // Add code to handle the SettingsSaving event here.
        }

        public void SetColor(int index, System.Drawing.Color color)
        {
            StringCollection sc = Default.SeriesColors;
            while (index >= sc.Count)
            {
                sc.Add("Black");
            }
            sc[index] = color.ToArgb().ToString();
        }

        internal System.Drawing.Color GetSeriesColor(int index)
        {
            StringCollection sc = Default.SeriesColors;
            Color c = System.Drawing.Color.Black;
            if (index >= sc.Count)
            {
                return c;
            }
            int argb = 0;
            if (int.TryParse(sc[index], out argb))
            {

                c = Color.FromArgb(argb);
            }
            else
            {
                c = Color.FromName(sc[index]);
            }
            return c;
        }

        internal void SetSeriesWidth(int index, int value)
        {
            StringCollection sc = Default.SeriesWidth; ;
            while (index >= sc.Count)
            {
                sc.Add("1");
            }
            sc[index] = value.ToString();
        }

        internal int GetSeriesWidth(int index)
        {
            StringCollection sc = Default.SeriesWidth;
            if (index >= sc.Count)
            {
                return 1;
            }
            int width = 1;
            if (int.TryParse(sc[index], out width))
            {
                return width;
            }
            else
            {
                return 1;
            }
        }
    }
}
