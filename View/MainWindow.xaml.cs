using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using VirtualEyes.ViewModel;

namespace VirtualEyes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowModel Model;

        private int seeingheightdefault = 340;
        private int hearingheightdefault = 340;
        private int optionswidthdefault = 190;

        private readonly Duration openCloseDuration = new Duration(TimeSpan.FromSeconds(0.15));
        public MainWindow(MainWindowModel model)
        {
            Model = model;
            DataContext = Model;
            InitializeComponent();
        }

        private void MoveWindow(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void CheckSeeing(object sender, RoutedEventArgs e)
        {
            if (ToggleSeeingButton == null) return;
            ToggleHearingButton.IsChecked = false;

            DoubleAnimation heightAnim = new(seeingheightdefault, openCloseDuration);
            SeeingFrame.BeginAnimation(HeightProperty,heightAnim);
        }

        private void UncheckSeeing(object sender, RoutedEventArgs e)
        {
            if (ToggleSeeingButton == null) return;
            DoubleAnimation heightAnim = new(0, openCloseDuration);
            SeeingFrame.BeginAnimation(HeightProperty, heightAnim);
        }

        private void CheckHearing(object sender, RoutedEventArgs e)
        {
            if (ToggleHearingButton == null) return;
            ToggleSeeingButton.IsChecked = false;

            DoubleAnimation heightAnim = new(hearingheightdefault, openCloseDuration);
            HearingFrame.BeginAnimation(HeightProperty, heightAnim);
        }

        private void UncheckHearing(object sender, RoutedEventArgs e)
        {
            if (ToggleHearingButton == null) return;
            DoubleAnimation heightAnim = new(0, openCloseDuration);
            HearingFrame.BeginAnimation(HeightProperty, heightAnim);
        }

        private void CheckedOptions(object sender, RoutedEventArgs e)
        {
            if (ToggleOptionsButton == null) return;
            DoubleAnimation widthAnim = new(optionswidthdefault, openCloseDuration);
            OptionsFrame.BeginAnimation(WidthProperty, widthAnim);
        }

        private void UncheckedOptions(object sender, RoutedEventArgs e)
        {
            if (ToggleOptionsButton == null) return;
            DoubleAnimation widthAnim = new(0, openCloseDuration);
            OptionsFrame.BeginAnimation(WidthProperty, widthAnim);
        }
    }
}