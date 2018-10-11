using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Suggeritore_Cisco.Logic;

namespace Suggeritore_Cisco.Windows
{
    public partial class ChapterSelector
    {
        public static ChapterSelector Cp;
        public HotKey DevCom;

        public ChapterSelector(Tuple<string, int, string> user)
        {
            Cp = this;
            InitializeComponent();
            
            userTxt.Text = user.Item3;

            //Fast keystroke commands for accessing specific chapter without using the GUI!
            new HotKey(Key.D1, KeyModifier.Ctrl | KeyModifier.Shift, obj => LaunchHelper("1", obj));
            new HotKey(Key.D2, KeyModifier.Ctrl | KeyModifier.Shift, obj => LaunchHelper("2", obj));
            new HotKey(Key.D3, KeyModifier.Ctrl | KeyModifier.Shift, obj => LaunchHelper("3", obj));
            new HotKey(Key.D4, KeyModifier.Ctrl | KeyModifier.Shift, obj => LaunchHelper("4", obj));
            new HotKey(Key.D5, KeyModifier.Ctrl | KeyModifier.Shift, obj => LaunchHelper("5", obj));
            new HotKey(Key.D6, KeyModifier.Ctrl | KeyModifier.Shift, obj => LaunchHelper("6", obj));
            new HotKey(Key.D7, KeyModifier.Ctrl | KeyModifier.Shift, obj => LaunchHelper("7", obj));
            new HotKey(Key.D8, KeyModifier.Ctrl | KeyModifier.Shift, obj => LaunchHelper("8", obj));
            new HotKey(Key.D9, KeyModifier.Ctrl | KeyModifier.Shift, obj => LaunchHelper("9", obj));
            new HotKey(Key.D0, KeyModifier.Ctrl | KeyModifier.Shift, obj => LaunchHelper("10", obj));
            new HotKey(Key.Q, KeyModifier.Ctrl | KeyModifier.Shift, obj => LaunchHelper("11", obj));
            new HotKey(Key.W, KeyModifier.Ctrl | KeyModifier.Shift, obj => LaunchHelper("PRETEST", obj));
            new HotKey(Key.E, KeyModifier.Ctrl | KeyModifier.Shift, obj => LaunchHelper("PRACTICE FINAL", obj));
            new HotKey(Key.R, KeyModifier.Ctrl | KeyModifier.Shift, obj => LaunchHelper("PT PRACTICE", obj));
            new HotKey(Key.T, KeyModifier.Ctrl | KeyModifier.Shift, obj => LaunchHelper("FINAL", obj));
            new HotKey(Key.Y, KeyModifier.Ctrl | KeyModifier.Shift, obj => LaunchHelper("6 PT", obj));

            //..or just toggle the visibility of the Chapter Selector.
            DevCom = new HotKey(Key.D, KeyModifier.Ctrl, obj => ToggleVisibility());

            //Starts as hidden.
            Visibility = Visibility.Hidden;
            Hide();
            Topmost = false;
        }

        private void ToggleVisibility()
        {
            if (Visibility == Visibility.Hidden)
            {
                Visibility = Visibility.Visible;
                Show();
                Topmost = true;
            }
            else
            {
                Visibility = Visibility.Hidden;
                Hide();
                Topmost = false;
            }
        }

        private void ADXButton_Click(object sender, RoutedEventArgs e)
            => LaunchHelper((sender as Button)?.Tag as string);

        private void LaunchHelper(string tag, HotKey hk = null)
        {
            switch (tag)
            {
                case "1":
                    new HelperUI("1").Show();
                    Hide();
                    hk?.Unregister();
                    break;
                case "2":
                    new HelperUI("2").Show();
                    Hide();
                    hk?.Unregister();
                    break;
                case "3":
                    new HelperUI("3").Show();
                    Hide();
                    hk?.Unregister();
                    break;
                case "4":
                    MessageBox.Show("4 NOT READY");
                    break;
                case "5":
                    MessageBox.Show("5 NOT READY");
                    break;
                case "6":
                    MessageBox.Show("6 NOT READY");
                    break;
                case "6PT":
                    MessageBox.Show("6 PT NOT READY");
                    break;
                case "7":
                    MessageBox.Show("7 NOT READY");
                    break;
                case "8":
                    new HelperUI("8").Show();
                    Hide();
                    hk?.Unregister();
                    break;
                case "9":
                    MessageBox.Show("9 NOT READY");
                    break;
                case "10":
                    new HelperUI("10").Show();
                    Hide();
                    hk?.Unregister();
                    break;
                case "11":
                    new HelperUI("11").Show();
                    Hide();
                    hk?.Unregister();
                    break;
                case "PRETEST":
                    MessageBox.Show("PRETEST NOT READY");
                    break;
                case "PRACTICE FINAL":
                    MessageBox.Show("PRACTICE FINAL NOT READY");
                    break;
                case "PT PRACTICE":
                    MessageBox.Show("PT PRACTICE NOT READY");
                    break;
                case "FINAL":
                    new HelperUI("FINAL").Show();
                    Hide();
                    hk?.Unregister();
                    break;
                case "Istruzioni":
                    MessageBox.Show("GUARDA LA DOCUMENTAZIONE IN WORD");
                    break;
                case "About":
                    MessageBox.Show("Creato da Mr. Nessuno. ex 3AI");
                    break;
                case "Esci":
                    Environment.Exit(0);
                    break;
                default:
                    throw new Exception("Non dovrebbe esserci...."); //Obviously not.
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) => Environment.Exit(0);
    }
}
