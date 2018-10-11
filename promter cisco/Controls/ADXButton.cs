using System.Windows;
using System.Windows.Controls;

namespace Suggeritore_Cisco.Controls
{
    public class ADXButton : Button
    {
        static ADXButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ADXButton), new FrameworkPropertyMetadata(typeof(ADXButton)));
        }
    }
}
