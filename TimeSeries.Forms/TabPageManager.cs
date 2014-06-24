using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Reclamation.TimeSeries.Forms
{
    /// <summary>
    /// Adapted from 
    /// http://www.dotnetspider.com/resources/18344-Hiding-Showing-Tabpages-Tabcontrol.aspx
    /// </summary>
    public class TabPageManager
    {

        private TabControl tabControl1;
        public TabPageManager(TabControl tabControl)
        {
            this.tabControl1 = tabControl;
        }

        private void HideTabPage(TabPage tp)
        {
            if (tabControl1.TabPages.Contains(tp))
                tabControl1.TabPages.Remove(tp);
        }


        private void ShowTabPage(TabPage tp)
        {
            ShowTabPage(tp, tabControl1.TabPages.Count);
        }


        private void ShowTabPage(TabPage tp, int index)
        {
            if (tabControl1.TabPages.Contains(tp)) return;
            InsertTabPage(tp, index);
        }


        private void InsertTabPage(TabPage tabpage, int index)
        {
            if (index < 0 || index > tabControl1.TabCount)
                throw new ArgumentException("Index out of Range.");
            tabControl1.TabPages.Add(tabpage);
            if (index < tabControl1.TabCount - 1)
                do
                {
                    SwapTabPages(tabpage, (tabControl1.TabPages[tabControl1.TabPages.IndexOf(tabpage) - 1]));
                }
                while (tabControl1.TabPages.IndexOf(tabpage) != index);
            tabControl1.SelectedTab = tabpage;
        }


        private void SwapTabPages(TabPage tp1, TabPage tp2)
        {
            if (tabControl1.TabPages.Contains(tp1) == false || tabControl1.TabPages.Contains(tp2) == false)
                throw new ArgumentException("TabPages must be in the TabControls TabPageCollection.");

            int Index1 = tabControl1.TabPages.IndexOf(tp1);
            int Index2 = tabControl1.TabPages.IndexOf(tp2);
            tabControl1.TabPages[Index1] = tp2;
            tabControl1.TabPages[Index2] = tp1;

            //Uncomment the following section to overcome bugs in the Compact Framework
            //tabControl1.SelectedIndex = tabControl1.SelectedIndex; 
            //string tp1Text, tp2Text;
            //tp1Text = tp1.Text;
            //tp2Text = tp2.Text;
            //tp1.Text=tp2Text;
            //tp2.Text=tp1Text;

        }

    }

}
