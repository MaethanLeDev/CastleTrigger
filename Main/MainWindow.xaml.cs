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
        private bool Commande_Haut=false, Commande_Bas=false, Commande_Gauche = false, Commande_Droit=false;

       

       

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.W)
            {
                Commande_Haut = true;
            }
            if (e.Key == Key.S)
            {
                Commande_Bas = true;
            }
            if (e.Key == Key.Q)
            {
                Commande_Gauche = true;
            }
            if (e.Key == Key.D)
            {
                Commande_Droit = true;
            }
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.W)
            {
                Commande_Haut = false;
            }
            if (e.Key == Key.S)
            {
                Commande_Bas = false;
            }
            if (e.Key == Key.Q)
            {
                Commande_Gauche = false;
            }
            if (e.Key == Key.D)
            {
                Commande_Droit = false;
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
            Console.WriteLine(Commande_Bas);
            Deplacement();

        }
        public void Deplacement()
        {
            double POS_TOP_JOUEUR=Canvas.GetTop(JOUEUR);
            if (Commande_Bas==true)
            {
                Canvas.SetTop(JOUEUR, POS_TOP_JOUEUR+20);
            }
            
       
        
        }

    }
}