﻿using MVVM_UnitTestingPractice.Commands;
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
        public int[] Speeds { get; } = [1,2,3,4,5,6,7,8,9,10];
        public ICommand OnTogglePlay { get; private set; }
        public ICommand OnPasteText { get; private set; }

        public int ReadingSpeed
        {
            get => readingSpeed;
            set
            {
                readingSpeed = value;
                OnPropertyChanged(nameof(ReadingSpeed));
                textToSpeech.SetReadingSpeed(readingSpeed);
            }
        }
        private int readingSpeed = 2;

        public int SeeingSpeed
        {
            get => seeingSpeed;
            set
            {
                seeingSpeed = value;
                OnPropertyChanged(nameof(SeeingSpeed));
            }
        }
        private int seeingSpeed = 2;

        public bool IsSpeaking
        {
            get => isSpeaking;
            set
            {
                isSpeaking = value;
                Console.WriteLine(isSpeaking);
                OnPropertyChanged(nameof(IsSpeaking));
            }
        }
        private bool isSpeaking;

        public bool IsDictateMode
        {
            get => isDictateMode;
            set
            {
                isDictateMode = value;
                OnPropertyChanged(nameof(IsDictateMode));
            }
        }
        private bool isDictateMode;

        public bool IsContinuousReading
        {
            get => isContinuousReading;
            set
            {
                isContinuousReading = value;
                OnPropertyChanged(nameof(IsContinuousReading));
            }
        }
        private bool isContinuousReading;

        private Window readingArea;
        private TextToSpeech textToSpeech;
        private ImageToText imgToText;
        private string extractedText = string.Empty;
        private CancellationTokenSource tokenSource = new();
        private Task? Reading = null;

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

            textToSpeech.OnIsSpeaking += (speaking) => 
            {
                if (IsContinuousReading) return;
                IsSpeaking = speaking; 
            };
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
                    if (isContinuousReading)
                    {
                        if (Reading != null) return;

                        tokenSource = new();
                        Reading = ContinuousScreenReading(tokenSource.Token);
                    }
                    else 
                    {
                        ReadScreenText();
                    }
                }
                else
                {
                    if(isContinuousReading)
                    {
                        tokenSource.Cancel();
                        Reading = null;
                    }
                    textToSpeech.Stop();
                    ReadedTextCollection.Clear();
                }
            }
        }

        private async Task ContinuousScreenReading(CancellationToken token)
        {
            IsSpeaking = true;
            while (true)
            {
                if (token.IsCancellationRequested) break;
                ReadScreenText();
                await Task.Delay(SeeingSpeed*1000);
            }
            IsSpeaking = false;
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
                IsSpeaking = false;
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
