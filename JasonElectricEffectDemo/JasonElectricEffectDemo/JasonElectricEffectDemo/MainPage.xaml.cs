using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using JasonElectricEffect;

namespace JasonElectricEffectDemo
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        // Simple button Click event handler to take us to the second page
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string effectType = string.Empty;
            if (rdoLine.IsChecked==true)
            {
                (App.Current as App).EffectType = ElectricEffectType.Line;
            }
            else if (rdoBezier.IsChecked == true)
            {
                (App.Current as App).EffectType = ElectricEffectType.Bezier;
            }
            else if (rdoCatmullRom.IsChecked == true)
            {
                (App.Current as App).EffectType = ElectricEffectType.CatmullRom;
            }

            int flowCount = 1;
            if (int.TryParse(txtFlowCount.Text, out flowCount))
            {
                (App.Current as App).ElectricFlowCount = flowCount;
            }

            NavigationService.Navigate(new Uri("/GamePage.xaml", UriKind.Relative));
        }

    }
}