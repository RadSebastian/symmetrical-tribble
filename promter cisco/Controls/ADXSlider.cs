using System.Windows;
using System.Windows.Controls;

namespace Suggeritore_Cisco.Controls
{
    public class ADXSlider : Slider
    {
        static ADXSlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ADXSlider), new FrameworkPropertyMetadata(typeof(ADXSlider)));
        }
    }
}
