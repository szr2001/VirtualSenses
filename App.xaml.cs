using System.Runtime.InteropServices;
using System.Windows;
using VirtualEyes.Model;
using VirtualEyes.View;
using VirtualEyes.ViewModel;

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

            SeeingPageModel seeingModel = new(imgToText,textToSpeech);
            SeeingPage seeing = new(seeingModel);

            HearingPageModel hearingModel = new();
            HearingPage hearing = new(hearingModel);

            MainWindowModel mainWindowModel = new(seeing,hearing,options);

            TestingWindow testingWindow = new();
            testingWindow.Show();

            MainWindow mainWindow = new(mainWindowModel);
            mainWindow.Show();
        }
    }

}
