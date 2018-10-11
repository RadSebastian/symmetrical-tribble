using System;
using System.Windows.Input;
using Suggeritore_Cisco.Logic;
using ChapterSelector = Suggeritore_Cisco.Windows.ChapterSelector;
using DummyWindow = Suggeritore_Cisco.Windows.DummyWindow;
using HelperUI = Suggeritore_Cisco.Windows.HelperUI;

namespace Suggeritore_Cisco
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            MemoryThread.Start(); //Launch "memory cleaner"

            VerifyAndLaunch();

            new HotKey(Key.F4, KeyModifier.Ctrl | KeyModifier.Alt, obj => {
                HelperUI.helperUI?.hm?.Uninstall(); //Need to unregister BEFORE exit otherwise it will crash badly.
                Environment.Exit(0);
            }); //ALWAYS!
        }

        private void VerifyAndLaunch()
        {
            var check = LocalLicense.CheckIfEligible();
            // if (check != null)
            new ChapterSelector(check); //It will launch
            // else
            //     new DummyWindow().Show(); //Useless window for other "not licensed" account

            Close();
        }
    }
}
