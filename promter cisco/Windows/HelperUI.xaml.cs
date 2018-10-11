using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Suggeritore_Cisco.Logic;

namespace Suggeritore_Cisco.Windows
{
    /// <summary>
    /// Logica di interazione per HelperUI.xaml
    /// </summary>
    public partial class HelperUI
    {
        private readonly double MINIMUM_WARNING_VALUE_THRESHOLD = 65;

        DispatcherTimer timer, timerpulizia;
        Engine helperDomandeRisposte;
        public HotMouse hm;
        public static HelperUI helperUI;

        private static string currentClipboardData = "";

        public HelperUI(string capitolo)
        {
            try
            {
                ChapterSelector.Cp?.DevCom?.Unregister();
            }
            catch (Exception)
            {
                //Non importa
            }

            helperUI = this;
            helperDomandeRisposte = new Engine(capitolo);
            timer = new DispatcherTimer();
            timerpulizia = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 5);
            timerpulizia.Interval = new TimeSpan(0, 0, 0, 0, 500);
            timer.Tick += (s, e) => { AggiornaPosizione(); };
            timerpulizia.Tick += (s, e) => { RipopolaDati(); };
            new HotKey(Key.Z, KeyModifier.Alt, TogglePopup);

            try
            {
                hm = new HotMouse();
                hm.Install();

                GC.KeepAlive(hm);
                GCHandle.Alloc(hm); //l'HotMouse contiene un delegato "di ricezione" dal SO, se venisse cancellata, il programma crasherebbe.

                hm.MiddleButtonDown += (s) => TogglePopupMouse(true);
                hm.MiddleButtonUp += (s) => TogglePopupMouse(false);
            }
            catch (Exception)
            {
                //Non è possibile
            }

            InitializeComponent();

            txtCapitolo.Text = "Chapter: " + capitolo;
        }
        private void AggiornaPosizione()
        {
            var point = CursorPos.GetMousePosition();
            PopupWindow.HorizontalOffset = point.X + 8;
            PopupWindow.VerticalOffset = point.Y + 8;
        }

        private void TogglePopupMouse(bool isOpen)
        {
            PopupWindow.IsOpen = isOpen;
            CommonToggleCode();
        }

        private void TogglePopup(HotKey obj)
        {
            PopupWindow.IsOpen = !PopupWindow.IsOpen;
            CommonToggleCode();
        }

        private void CommonToggleCode()
        {
            if (PopupWindow.IsOpen)
            {
                timer.Start();
                timerpulizia.Start();

                RipopolaDati();
            }
            else
            {
                timer.Stop();
                timerpulizia.Stop();
            }

            AggiornaPosizione();
        }

        private async void RipopolaDati() //You know, async method...
        {
            var clipText = Clipboard.GetText(TextDataFormat.Text);

            if (clipText == currentClipboardData) //Evita il check continuo quando il dato è sempre lo stesso.
                return;

            hiddable.Visibility = Visibility.Collapsed; //Il dettaglio è attualmente inutile, quindi lo lasciamo collassato

            if (clipText.Length > 1500) //Non esiste una domanda con 1500+ caratteri (ma anche meno); 2 possibilità: Vuoi essere simpatico e far laggare il pc OPPURE hai selezionato per errore tutta la pagina (capita).
            {
                txtRisposta.Text = "THE COPIED QUESTION IS EXCESSIVELY LONG..."; //Ti avviso eh
                return;
            }

            txtRisposta.Text = "Answer: searching..."; //Avvisiamo l'utente che il programma sta cercando nel suo micro DataBase XML
            var risposta = await Task.Run(() => helperDomandeRisposte.GetRisposta(clipText));

            currentClipboardData = clipText;

            if (risposta.Key == null)
            {
                txtPrecisione.Text = "Precision: None";
                txtDomanda.Text = "Question: No matches found."; //
                txtRisposta.Text = "Answer: No matches found."; //Ridondo.
                hiddable.Visibility = Visibility.Collapsed;
            }
            else
            {
                hiddable.Visibility = risposta.Value < MINIMUM_WARNING_VALUE_THRESHOLD ? Visibility.Visible : Visibility.Collapsed; //Da' attenzione sul fatto che la migliore corrispenza è inferiore al 65%
                txtPrecisione.Text = "Precision: " + risposta.Value.ToString("#.#0") + "%";
                txtDomanda.Text = "Question: " + risposta.Key[0]; //TMP
                txtRisposta.Text = (risposta.Value > MINIMUM_WARNING_VALUE_THRESHOLD ? "" : "Answer: \n\n") + risposta.Key[1]; //TMP
            }
        }
    }

    internal static class CursorPos
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetCursorPos(ref Win32Point pt);

        [StructLayout(LayoutKind.Sequential)]
        private struct Win32Point
        {
            public readonly int X;
            public readonly int Y;
        };
        public static Point GetMousePosition()
        {
            var w32Mouse = new Win32Point();
            GetCursorPos(ref w32Mouse);
            return new Point(w32Mouse.X, w32Mouse.Y);
        }
    }
}
