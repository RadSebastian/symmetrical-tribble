using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Suggeritore_Cisco.Controls
{
    public class ADXCheckBox : CheckBox
    {
        public static readonly DependencyProperty BulletColorProperty =
            DependencyProperty.Register("BulletColor", typeof(SolidColorBrush), typeof(ADXCheckBox), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0x02, 0x68, 0x73))));

        public SolidColorBrush BulletColor
        {
            get => (SolidColorBrush)GetValue(BulletColorProperty);
            set => SetValue(BulletColorProperty, value);
        }

        static ADXCheckBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ADXCheckBox), new FrameworkPropertyMetadata(typeof(ADXCheckBox)));
        }
    }
}
