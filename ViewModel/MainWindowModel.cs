using MVVM_UnitTestingPractice.Commands;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Input;

namespace VirtualEyes.ViewModel
{
    public class MainWindowModel
    {
        public Page SeeingPage { get; set; }
        public Page HearingPage { get; set; }
        public Page OptionsPage { get; set; }
        public AppMode ActiveMode { get; private set; } = AppMode.Seeing;

        public ICommand OnSeeingToggled { get; set; }
        public ICommand OnHearingToggled { get; set; }

        public delegate void ToggledPage();
        public event ToggledPage? OnSeeingEnabled;
        public event ToggledPage? OnHearingEnabled;
        public event ToggledPage? OnSeeingDisabled;
        public event ToggledPage? OnHearingDisabled;
        public MainWindowModel(Page seeingPage, Page hearingPage, Page optionsPage)
        {
            SeeingPage = seeingPage;
            HearingPage = hearingPage;
            OptionsPage = optionsPage;

            OnSeeingToggled = new RelayCommand(SeeingToggled);
            OnHearingToggled = new RelayCommand(HearingToggled);
        }

        private void SeeingToggled(object obj)
        {
            if (obj is bool ischecked)
            {
                if (ischecked)
                {
                    if (ActiveMode == AppMode.Hearing)
                    {
                        OnHearingDisabled?.Invoke();
                    }
                    OnSeeingEnabled?.Invoke();
                    ActiveMode = AppMode.Seeing;
                }
                else
                {
                    if (ActiveMode == AppMode.Seeing)
                    {
                        ActiveMode = AppMode.None;
                    }
                    OnSeeingDisabled?.Invoke();
                }
            }

            Console.WriteLine(ActiveMode);
        }

        private void HearingToggled(object obj)
        {
            if(obj is bool ischecked)
            {
                if (ischecked)
                {
                    if(ActiveMode == AppMode.Seeing)
                    {
                        OnSeeingDisabled?.Invoke();
                    }
                    OnHearingEnabled?.Invoke();
                    ActiveMode = AppMode.Hearing;
                }
                else
                {
                    if (ActiveMode == AppMode.Hearing)
                    {
                        ActiveMode = AppMode.None;
                    }
                    OnHearingDisabled?.Invoke();
                }
            }
            Console.WriteLine(ActiveMode);
        }
    }

    public enum AppMode
    {
        Seeing,
        Hearing,
        None
    }
}
