using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VirtualEyes.Model;
using VirtualSenses.View;

namespace VirtualEyes.ViewModel
{
    public class HearingPageModel
    {
        private SubtitlesWindow subtitleWindow;
        private VoiceRecognition voiceRecognition;
        public HearingPageModel(SubtitlesWindow subtitleswin, VoiceRecognition voicerec)
        {
            subtitleWindow = subtitleswin;
            voiceRecognition = voicerec;
        }
        public void ShowSubtitles() => subtitleWindow.Show();
        public void HideSubtitles() => subtitleWindow.Hide();
    }
}
