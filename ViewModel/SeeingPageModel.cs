using MVVM_UnitTestingPractice.Commands;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using VirtualEyes.Model;

namespace VirtualEyes.ViewModel
{
    public class SeeingPageModel:INotifyPropertyChanged
    {
        public ObservableCollection<string> ReadedTextCollection { get; set; } = [];

        public ICommand OnTogglePlay { get; private set; }
        public ICommand OnPasteText { get; private set; }
        public bool IsSpeaking 
        { 
            get 
            { 
                return isSpeaking; 
            } 
            set 
            {
                isSpeaking = value;
                Console.WriteLine(isSpeaking);
                OnPropertyChanged(nameof(IsSpeaking));
            } 
        }
        private bool isSpeaking;

        private Window readingArea;
        private TextToSpeech textToSpeech;
        private ImageToText imgToText;
        private string extractedText = string.Empty;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public SeeingPageModel(ImageToText imgtotext, TextToSpeech texttospeech, Window readingarea)
        {
            imgToText = imgtotext;
            textToSpeech = texttospeech;
            readingArea = readingarea;

            textToSpeech.OnIsSpeaking += (speaking) => { IsSpeaking = speaking; };
            textToSpeech.OnSpeaking += Console.WriteLine;
            OnTogglePlay = new RelayCommand(ToggleReadArea);
            OnPasteText = new RelayCommand(PasteText);
        }

        private void ToggleReadArea(object obj)
        {
            if (obj is bool toggled)
            {
                if (toggled)
                {

                }
                else
                {
                    textToSpeech.Stop();
                }
            }
        }

        private void PasteText(object obj)
        {
            extractedText = Clipboard.GetText();
            ConvertExtractedTextToCollection(extractedText);
            textToSpeech.ReadText(extractedText);
            isSpeaking = true;
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
