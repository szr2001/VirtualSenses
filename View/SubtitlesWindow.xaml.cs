using System.Windows;

namespace VirtualSenses.View
{
    /// <summary>
    /// Interaction logic for SubtitlesWindow.xaml
    /// </summary>
    public partial class SubtitlesWindow : Window
    {
        public string Subtitles { get; set; } = "Subtitles Preview";
        public SubtitlesWindow()
        {
            InitializeComponent();
            DataContext = this;
            WindowStartupLocation = WindowStartupLocation.Manual;
            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double screenHeight = SystemParameters.PrimaryScreenHeight;
            Left = (screenWidth / 2) - (Width / 2 );
            Top = screenHeight - Height - 35;
        }
    }
}
