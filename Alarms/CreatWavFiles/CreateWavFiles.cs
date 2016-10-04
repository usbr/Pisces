using System;
using System.IO;
using System.Speech.Synthesis;
using System.Speech.AudioFormat;
using Reclamation.Core;

namespace CreateHydrometWav
{
    public class CreateWavFiles
    {
        static void Main(string[] args)
        {

            if( args.Length != 1)
            {
                Console.WriteLine("Usage: CreateWavFiles.exe file.csv" );
                return;
            }

            var dir = Path.GetDirectoryName(args[0]);
            CsvFile csv = new CsvFile(args[0]);
            dir = Path.Combine(dir,"output");
            // Initialize a new instance of the SpeechSynthesizer.
            using (SpeechSynthesizer synth = new SpeechSynthesizer())
            {
                Console.WriteLine("using voice :"+synth.Voice.Name);
                foreach (var item in synth.GetInstalledVoices())
                {
                   //.. Console.WriteLine(item.VoiceInfo.Name);
                }
                

                for (int i = 0; i < csv.Rows.Count; i++)
                {
                    // Set a value for the speaking rate. Slower to Faster (-10 to 10)
                    synth.Rate = -3;
                    
                    // Configure the audio output. 
                    string outputWavFileName = Path.Combine(dir, csv.Rows[i]["File Name"].ToString());
                    Console.WriteLine(outputWavFileName);
                    synth.SetOutputToWaveFile(outputWavFileName, 
                      new SpeechAudioFormatInfo(8000, AudioBitsPerSample.Sixteen, AudioChannel.Mono));
                    
                    
			  // Create a SoundPlayer instance to play output audio file.
                    //System.Media.SoundPlayer m_SoundPlayer = new System.Media.SoundPlayer(outputWavFileName);

                    // Build a prompt.
                    PromptBuilder builder = new PromptBuilder();
                    builder.AppendText(csv.Rows[i]["Text"].ToString());
                    
                    // Speak the prompt.
                    synth.Speak(builder);
                    //m_SoundPlayer.Play();
                }
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}