using ImageDetection;
using Patagames.Ocr.Enums;
using Patagames.Ocr;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Input;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;
using System.Speech.Synthesis;
using System.Speech.Recognition;

namespace VirtualEyes.View
{
    /// <summary>
    /// Interaction logic for TestingWindow.xaml
    /// </summary>
    public partial class TestingWindow : Window, INotifyPropertyChanged
    {
        public Point ReadPosition1
        {
            get { return readposition1; }
            set
            {
                readposition1 = value;
                OnPropertyChanged(nameof(ReadPosition1));
            }
        }
        public Point ReadPosition2
        {
            get { return readposition2; }
            set
            {
                readposition2 = value;
                OnPropertyChanged(nameof(ReadPosition2));
            }
        }
        public string ResultText
        {
            get { return resulttext; }
            set
            {
                resulttext = value;
                OnPropertyChanged(nameof(ResultText));
            }
        }

        private string resulttext = string.Empty;
        private Point readposition1 = Point.Empty;
        private Point readposition2 = Point.Empty;


        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public TestingWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void SelectReadArea(object sender, RoutedEventArgs e)
        {
            ReadPosition1 = Point.Empty;
            ReadPosition2 = Point.Empty;
        }

        private void DetectRkey(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.R)
            {
                if (ReadPosition1 == Point.Empty)
                {
                    ReadPosition1 = InputSimulation.GetMouseLoc();
                    return;
                }
                if (ReadPosition2 == Point.Empty)
                {
                    ReadPosition2 = InputSimulation.GetMouseLoc();
                    ReadSelectedArea();
                }
            }
        }

        private void ReadSelectedArea()
        {
            int width = Math.Abs(ReadPosition2.X - ReadPosition1.X);
            int height = Math.Abs(ReadPosition2.Y - ReadPosition1.Y);

            Bitmap ReadTarget = new Bitmap(width, height);

            using (Graphics g = Graphics.FromImage(ReadTarget))
            {
                g.CopyFromScreen(Math.Min(ReadPosition1.X, ReadPosition2.X),
                                 Math.Min(ReadPosition1.Y, ReadPosition2.Y),
                                 0, 0,
                                 new Size(width, height));
            }

            using (var api = OcrApi.Create())
            {
                try
                {
                    api.Init(Languages.English);
                    ResultText = api.GetTextFromImage(ReadTarget);
                    ReadTarget.Dispose();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Selected area too big, max 500x500");
                }
            }
            SpeackText(resulttext);
        }

        private void SpeackText(string text) 
        {
            SpeechSynthesizer synthesizer = new();
            synthesizer.Rate = 3;
            synthesizer.Volume = 10;
            synthesizer.SetOutputToDefaultAudioDevice();
            synthesizer.SpeakAsync(text);
        }
    }
}
