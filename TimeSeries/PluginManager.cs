using Reclamation.Core;
using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;


namespace Reclamation.TimeSeries
{
    public class PluginManager
    {
        List<Assembly> asmList;

        public PluginManager()
        {
            asmList = new List<Assembly>();
        }
        public void LoadPlugins()
        {

            string pluginFilename = Path.Combine(FileUtility.GetExecutableDirectory(), "plugins.txt");

            if (File.Exists(pluginFilename))
            {
                var anames = File.ReadAllLines(pluginFilename);
                foreach (var item in anames)
                {
                    if (File.Exists(item))
                    {
                       var asm = AppDomain.CurrentDomain.Load(Path.GetFileNameWithoutExtension(item));
                       asmList.Add(asm);
                      
                    }

                }
               
            }
        }

        /// <summary>
        /// Register menu items for each Iplugin found in these assemblies.
        /// </summary>
        public void RegisterPlugins(ToolStripItem dataAddMenu)
        {

            foreach (var asm in asmList)
            {
                var instances = from t in asm.GetTypes()
                                where t.GetInterfaces().Contains(typeof(IPlugin))
                                         && t.GetConstructor(Type.EmptyTypes) != null
                                select Activator.CreateInstance(t) as IPlugin;
                
                var dataAddItems = dataAddMenu as ToolStripDropDownItem;
                if (dataAddItems != null)
                {
                    foreach (var instance in instances)
                    {
                        string txt = instance.GetAddMenuText();
                        var idx = dataAddItems.DropDownItems.IndexOfKey("toolStripMenuItemBottom");
                        if (idx >= 0)
                        {
                            var menuItem = new ToolStripMenuItem(txt);
                            dataAddItems.DropDownItems.Insert(idx, menuItem);
                            var img = instance.GetImage();
                            if (img != null)
                                menuItem.Image = img;
                            menuItem.Tag = instance;
                            menuItem.Click += menuItem_Click;
                        }
                    }
                }
            }

        }

        public event EventHandler<EventArgs> PluginClick;

        void menuItem_Click(object sender, EventArgs e)
        {

            if (sender is ToolStripDropDownItem)
            {
                var tag = (sender as ToolStripMenuItem).Tag;
                if (tag == null)
                    return;
                if (tag is IPlugin)
                {
                    var plugin = tag as IPlugin;

                    if (PluginClick != null)
                    {
                        PluginClick(plugin, EventArgs.Empty);
                    }
                }
            }
        }
    }
}
