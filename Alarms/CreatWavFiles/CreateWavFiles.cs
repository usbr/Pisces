using System;
using System.IO;
using System.Speech.Synthesis;
using System.Speech.AudioFormat;

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
            var fileData = File.ReadAllLines(args[0]);


            // Initialize a new instance of the SpeechSynthesizer.
            using (SpeechSynthesizer synth = new SpeechSynthesizer())
            {
                for (int i = 1; i < fileData.Length; i++)
                {
                    // Set a value for the speaking rate. Slower to Faster (-10 to 10)
                    synth.Rate = -3;

                    // Configure the audio output. 
                    string outputWavFileName = Path.Combine(Path.Combine(dir,"output"), fileData[i].Split(',')[1]).ToLower();
                    synth.SetOutputToWaveFile(outputWavFileName, 
                      new SpeechAudioFormatInfo(8000, AudioBitsPerSample.Sixteen, AudioChannel.Mono));
                    Console.WriteLine(outputWavFileName);
                    
			  // Create a SoundPlayer instance to play output audio file.
                    //System.Media.SoundPlayer m_SoundPlayer = new System.Media.SoundPlayer(outputWavFileName);

                    // Build a prompt.
                    PromptBuilder builder = new PromptBuilder();
                    builder.AppendText(fileData[i].Split(',')[0]);
                    
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