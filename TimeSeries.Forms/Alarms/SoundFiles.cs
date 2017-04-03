using System;
using System.Windows.Forms;
using Reclamation.Core;
using Reclamation.TimeSeries.Alarms;

#if !__MonoCS__
using System.Speech.Synthesis;
using System.Speech.AudioFormat;
using System.IO;
using System.Data;
using System.Diagnostics;
#endif 


namespace Reclamation.TimeSeries.Forms.Alarms
{
    public partial class SoundFiles : UserControl
    {
        AlarmDataSet.alarm_scriptsDataTable tbl = new AlarmDataSet.alarm_scriptsDataTable();
        AlarmDataSet.alarm_scriptsDataTable tblPreview = new AlarmDataSet.alarm_scriptsDataTable();
        bool preview = false; // getnerate script instead of *.wav files

        public SoundFiles()
        {
            InitializeComponent();
        }

        BasicDBServer m_svr;
        public SoundFiles(BasicDBServer svr)
        {
            m_svr = svr;
            InitializeComponent();
            Init();
#if __MonoCS__
            buttonGenerate.Enabled = false;
#endif
        }

        private void Init()
        {
            if (!m_svr.TableExists(tbl.TableName))
                return;
           m_svr.FillTable(tbl);
           tbl.Columns[0].AutoIncrementSeed = tbl.NextID();
           tbl.Columns[0].AutoIncrementStep = 1;
           dataGridView1.DataSource = tbl;
           dataGridView1.Columns[0].Visible = false;

        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            m_svr.SaveTable(tbl);
        }

        private void buttonGenerate_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            var dir = "";
            if ( !preview )
            {
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    dir = dlg.SelectedPath;
                else
                    return;
            }
                GenerateSounds(dir, tbl);
                // id, text, filename
                
                if( checkBoxSite.Checked)
                {
                    var sc = m_svr.Table("sitecatalog");
                    AlarmDataSet.alarm_scriptsDataTable scripts = new AlarmDataSet.alarm_scriptsDataTable();

                    for (int i = 0; i < sc.Rows.Count; i++)
                    {
                        var r = sc.Rows[i];
                        var fn = r["siteid"].ToString() + "_site.wav";
                        scripts.Addalarm_scriptsRow(r["description"].ToString(), fn);
                    }
                    GenerateSounds(dir,scripts);
                }
                if(checkBoxParameter.Checked)
                {
                    var sc = m_svr.Table("parametercatalog");
                    AlarmDataSet.alarm_scriptsDataTable scripts = new AlarmDataSet.alarm_scriptsDataTable();

                    for (int i = 0; i < sc.Rows.Count; i++)
                    {
                        var r = sc.Rows[i];
                        var fn = r["id"].ToString() + "_parameter.wav";
                        scripts.Addalarm_scriptsRow(r["name"].ToString(), fn);
                    }
                    GenerateSounds(dir, scripts);
                }
            

            if( preview)
            {
                tblPreview.Columns.Remove("id");
                var s = DataTableOutput.ToHTML(tblPreview);
                var fn  =FileUtility.GetTempFileName(".html");
                File.WriteAllText(fn, s);
                System.Diagnostics.Process.Start(fn);
            }
        }


        private void GenerateSounds(string dir, AlarmDataSet.alarm_scriptsDataTable scripts)
        {

#if !__MonoCS__
            
            using (SpeechSynthesizer synth = new SpeechSynthesizer())
            {
                foreach (var r in scripts)
                {

                    if (preview)
                    {// generate html table
                        tblPreview.Addalarm_scriptsRow(r.text, r.filename);
                    }
                    else
                    {
                        synth.Rate = -3;
                        string outputWavFileName = Path.Combine(dir, r.filename);
                        Console.WriteLine(outputWavFileName);
                        synth.SetOutputToWaveFile(outputWavFileName,
                          new SpeechAudioFormatInfo(8000, AudioBitsPerSample.Sixteen, AudioChannel.Mono));
                        PromptBuilder builder = new PromptBuilder();
                        builder.AppendText(r.text);
                        synth.Speak(builder);
                    }
                }
            }
#endif
        }

        
        private void buttonPreview_Click(object sender, EventArgs e)
        {
            preview = true;

            tblPreview.Clear();
            tblPreview.AcceptChanges();

            buttonGenerate_Click(sender, EventArgs.Empty);
            preview = false;
        }
        
    }
}
