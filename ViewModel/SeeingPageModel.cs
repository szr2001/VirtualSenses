using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VirtualEyes.Model;

namespace VirtualEyes.ViewModel
{
    public class SeeingPageModel
    {
        public ObservableCollection<string> ReadedTextCollection { get; set; } = [];

        private Window readingArea;
        private TextToSpeech textToSpeech;
        private ImageToText imgToText;
        private string ReadText = string.Empty;
        
        public SeeingPageModel(ImageToText imgtotext, TextToSpeech texttospeech, Window readingarea)
        {
            imgToText = imgtotext;
            textToSpeech = texttospeech;
            readingArea = readingarea;

            //testing
            string testmessage = "I will give you 40 dollas if you want. But due to a recent analisys I came to the conclusion I don't want to. I'm sadly needed to confirm your recent transaction as being inapropriate for something that I don't think I think about today is a new day about something that is a new and is not old.";

            string[] rawr = testmessage.Split(" ");

            foreach(string UwU in rawr)
            {
                ReadedTextCollection.Add(UwU);
            }
            //
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
