using System;
using System.IO;
using System.Windows;
using Vosk;

namespace VirtualEyes.Model
{
    public class VoiceRecognition
    {
        private Vosk.Model model;

        public VoiceRecognition() 
        {
            model = new("Model/vosk-model-en-us-0.22-lgraph");
        }

        public string Listen()
        {
            // Demo byte buffer
            VoskRecognizer rec = new VoskRecognizer(model, 16000.0f);
            rec.SetMaxAlternatives(0);
            rec.SetWords(true);
            using (Stream source = File.OpenRead("C/Users/szr20/Desktop/Test.wav"))
            {
                byte[] buffer = new byte[4096];
                int bytesRead;
                while ((bytesRead = source.Read(buffer, 0, buffer.Length)) > 0)
                {
                    if (rec.AcceptWaveform(buffer, bytesRead))
                    {
                        Console.WriteLine(rec.Result());
                    }
                    else
                    {
                        Console.WriteLine(rec.PartialResult());
                    }
                }
            }
            return rec.FinalResult();
        }
    }
}
