using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VirtualEyes.ViewModel;

namespace VirtualEyes.View
{
    /// <summary>
    /// Interaction logic for SeeingPage.xaml
    /// </summary>
    public partial class SeeingPage : Page
    {
        public SeeingPageModel pageModel;
        public SeeingPage(SeeingPageModel pagemodel)
        {
            InitializeComponent();
            pageModel = pagemodel;
            DataContext = pageModel;
        }
    }
}
