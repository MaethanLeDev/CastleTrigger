using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Security.AccessControl;


namespace Main
{
    
    public partial class MainWindow : Window
    {

        // ----------- Déclarations des variables ----------------
        private DispatcherTimer TimerJeu = new DispatcherTimer(); // C'est le chrono
        private bool UpKeyPressed, DownKeyPressed, LeftKeyPressed, RightKeyPressed;

        private void KeyBoardUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.W)
            {
                UpKeyPressed = false;
            }
            if (e.Key == Key.S)
            {
                DownKeyPressed = false;
            }
            if (e.Key == Key.Q)
            {
                LeftKeyPressed = false;
            }
            if (e.Key == Key.D)
            {
                RightKeyPressed = false;
            }
        }

        private void KeyBoardDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.W)
            {
                UpKeyPressed = true;
            }
            if (e.Key == Key.S)
            {
                DownKeyPressed = true;
            }
            if (e.Key == Key.Q)
            {
                LeftKeyPressed = true;
            }
            if (e.Key == Key.D)
            {
                RightKeyPressed = true;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            FenetreJeu.Focus();

            TimerJeu.Interval = TimeSpan.FromMilliseconds(16); // 60fps
            TimerJeu.Tick += JeuTic;
            TimerJeu.Start();

        }

        private void JeuTic(object sender, EventArgs e) // sender permet de localiser qui l'a déclenché [la méthode]
            /*
             * La Méthode s'exécute 60x / secondes
             */
        {

        }

    }
}