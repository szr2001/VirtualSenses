using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;

namespace VirtualEyes.Model
{
    public class TextToSpeech
    {
        public delegate void SpokenWorkd(string word);
        public delegate void Speaking(bool isSpeaking);
        public event SpokenWorkd? OnSpeaking;   
        public event Speaking? OnIsSpeaking;
        public bool IsSpeaking => speachSynthesizer.State == SynthesizerState.Speaking;

        private SpeechSynthesizer speachSynthesizer;
        private Prompt? speakingWork;
        public TextToSpeech()
        {
            speachSynthesizer = new()
            {
                Rate = 2,
                Volume = 10
            };
            speachSynthesizer.SpeakProgress += SpeakProgress;
            speachSynthesizer.SpeakCompleted += (e, x) => { OnIsSpeaking?.Invoke(false);};
            speachSynthesizer.SetOutputToDefaultAudioDevice();
        }

        private void SpeakProgress(object? sender, SpeakProgressEventArgs e)
        {
            OnSpeaking?.Invoke(e.Text);
        }

        public void ReadText(string extractedText)
        {
            if (string.IsNullOrEmpty(extractedText)) return;

            if (speakingWork != null)
            {
                speachSynthesizer.SpeakAsyncCancel(speakingWork);
                speakingWork = null;
            }
            speakingWork = speachSynthesizer.SpeakAsync(extractedText);
            OnIsSpeaking?.Invoke(true);
        }

        public void Stop()
        {
            if (speakingWork != null)
            {
                speachSynthesizer.SpeakAsyncCancel(speakingWork);
                speakingWork = null;
            }
            OnIsSpeaking?.Invoke(false);
        }

        public void SetReadingSpeed(int speed)
        {
            Stop();
            speachSynthesizer = new()
            {
                Rate = speed,
                Volume = 10
            };
        }
    }
}
