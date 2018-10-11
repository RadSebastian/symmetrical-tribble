using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Suggeritore_Cisco.Controls
{
    public class ADXRadioButton : RadioButton
    {
        public static readonly DependencyProperty BulletColorProperty = 
            DependencyProperty.Register("BulletColor", typeof(SolidColorBrush), typeof(ADXRadioButton), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0x02,0x68,0x73))));

        public SolidColorBrush BulletColor
        {
            get => (SolidColorBrush)GetValue(BulletColorProperty);
            set => SetValue(BulletColorProperty, value);
        }

        static ADXRadioButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ADXRadioButton), new FrameworkPropertyMetadata(typeof(ADXRadioButton)));
        }
    }
}
