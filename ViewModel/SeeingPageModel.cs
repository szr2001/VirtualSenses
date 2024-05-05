using MVVM_UnitTestingPractice.Commands;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
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
                    ReadScreenText();
                }
                else
                {
                    textToSpeech.Stop();
                    ReadedTextCollection.Clear();
                }
            }
        }

        private void ReadScreenText()
        {
            extractedText = imgToText.ReadScreenText(readingArea.RestoreBounds);
            if (!string.IsNullOrEmpty(extractedText))
            {
                extractedText = FilterText(extractedText);
                ConvertExtractedTextToCollection(extractedText);
                Read();
            }
            else
            {
                ReadedTextCollection.Clear();
                isSpeaking = false;
            }
        }

        private void PasteText(object obj)
        {
            extractedText = Clipboard.GetText();
            extractedText = FilterText(extractedText);
            ConvertExtractedTextToCollection(extractedText);
            Read();
        }

        private void Read()
        {
            textToSpeech.ReadText(extractedText);
            isSpeaking = true;
        }

        private readonly string allowedCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789.,'!?: ";
        private string FilterText(string original)
        {
            StringBuilder sb = new StringBuilder();

            char space = ' ';
            char previouschar = ' ';

            foreach(char c in original)
            {
                if (previouschar == space && c == space) 
                {
                    previouschar = c;
                    continue;
                }
                if (allowedCharacters.Contains(c))
                {
                    sb.Append(c);
                }
                previouschar = c;
            }
            
            return sb.ToString();
        }

        private readonly char splitChar = ' ';
        private void ConvertExtractedTextToCollection(string text)
        {
            ReadedTextCollection.Clear();
            string[] textwords = text.Split(splitChar);

            foreach (string word in textwords)
            {
                ReadedTextCollection.Add(word);
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
