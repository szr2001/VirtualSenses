using MVVM_UnitTestingPractice.Commands;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Controls;

namespace VirtualEyes.ViewModel
{
    public class MainWindowModel
    {
        public Page SeeingPage { get; set; }
        public Page HearingPage { get; set; }
        public Page OptionsPage { get; set; }
        public MainWindowModel(Page seeingPage, Page hearingPage, Page optionsPage)
        {
            SeeingPage = seeingPage;
            HearingPage = hearingPage;
            OptionsPage = optionsPage;
        }

    }
}
