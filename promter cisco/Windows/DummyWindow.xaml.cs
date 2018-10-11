using System;

namespace Suggeritore_Cisco.Windows
{
    public partial class DummyWindow
    {
        public DummyWindow()
            => InitializeComponent();

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
            => Environment.Exit(0);
    }
}
