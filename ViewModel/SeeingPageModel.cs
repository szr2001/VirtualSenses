using MVVM_UnitTestingPractice.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VirtualEyes.Model;
using static System.Net.Mime.MediaTypeNames;

namespace VirtualEyes.ViewModel
{
    public class SeeingPageModel
    {
        public ObservableCollection<string> ReadedTextCollection { get; set; } = [];

        public ICommand OnTogglePlay { get; private set; }
        public ICommand OnPasteText { get; private set; }
        public bool IsSpeaking { get; set; }

        private Window readingArea;
        private TextToSpeech textToSpeech;
        private ImageToText imgToText;
        private string extractedText = string.Empty;
        private SpeechSynthesizer speachSynthesizer;
        private Prompt? speakingWork;
        public SeeingPageModel(ImageToText imgtotext, TextToSpeech texttospeech, Window readingarea)
        {
            imgToText = imgtotext;
            textToSpeech = texttospeech;
            readingArea = readingarea;

            speachSynthesizer = new()
            {
                Rate = 3,
                Volume = 10
            };
            speachSynthesizer.SetOutputToDefaultAudioDevice();

            OnTogglePlay = new RelayCommand(ToggleReadArea);
            OnPasteText = new RelayCommand(PasteText);
        }

        private void ToggleReadArea(object obj)
        {
            if (obj is bool toggled)
            {
                Console.WriteLine(toggled);
            }
        }

        private void PasteText(object obj)
        {
            extractedText = Clipboard.GetText();
            ConvertExtractedTextToCollection(extractedText);
            ReadText();
        }

        private void ExtractTextFromArea()
        {

        }

        private void ConvertExtractedTextToCollection(string text)
        {
            ReadedTextCollection.Clear();
            string[] textwords = text.Split(" ");

            foreach (string UwU in textwords)
            {
                ReadedTextCollection.Add(UwU);
            }
        }

        private void ReadText()
        {
            if (string.IsNullOrEmpty(extractedText)) return;

            if(speakingWork != null)
            {
                speachSynthesizer.SpeakAsyncCancel(speakingWork);
                speakingWork = null;
            }
            speakingWork = speachSynthesizer.SpeakAsync(extractedText);
        }

        public void ShowReadingArea()
        {
            readingArea.Show();
        }
        public void HideReadingArea()
        {
            readingArea.Hide();
        }
    }
}
