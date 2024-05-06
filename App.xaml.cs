using System.Runtime.InteropServices;
using System.Windows;
using VirtualEyes.Model;
using VirtualEyes.View;
using VirtualEyes.ViewModel;
using VirtualSenses.View;

namespace VirtualEyes
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        [DllImport("kernel32.dll")]
        public static extern bool AllocConsole();

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            AllocConsole();

            ImageToText imgToText = new();
            TextToSpeech textToSpeech = new();
            VoiceRecognition voiceRecognition = new();

            OptionsPageModel optionsModel = new();
            OptionsPage options = new(optionsModel);

            ReadingAreaIndicatorWindow readingindicator = new();
            SeeingPageModel seeingModel = new(imgToText, textToSpeech, readingindicator);
            SeeingPage seeing = new(seeingModel);

            SubtitlesWindow subtitlesWindow = new();
            HearingPageModel hearingModel = new(subtitlesWindow, voiceRecognition);
            HearingPage hearing = new(hearingModel);

            MainWindowModel mainWindowModel = new(seeing, hearing, options);
            mainWindowModel.OnSeeingEnabled += seeingModel.ShowReadingArea;
            mainWindowModel.OnSeeingDisabled += seeingModel.HideReadingArea;
            mainWindowModel.OnHearingEnabled += hearingModel.ShowSubtitles;
            mainWindowModel.OnHearingDisabled += hearingModel.HideSubtitles;

            //TestingWindow testingWindow = new();
            //testingWindow.Show();

            MainWindow mainWindow = new(mainWindowModel);
            mainWindow.Show();
        }
    }

}
