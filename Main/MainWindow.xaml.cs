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



        // ----------- CONSTANTES --------------------------------

        // ---- CHEMINS
        private readonly string imagePath = "P:\\Sàé1.01\\CastleTrigger\\Assets\\Map_V1.png"; 


        // ----------- Déclarations des variables ----------------
        private DispatcherTimer TimerJeu = new DispatcherTimer(); // C'est le chrono
        private bool UpKeyPressed, DownKeyPressed, LeftKeyPressed, RightKeyPressed;


















        private void KeyBoardUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Z)
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
            if (e.Key == Key.Z)
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

            

            // Création du fond d'écran
            ImageBrush backgroundBrush = new ImageBrush();
            backgroundBrush.ImageSource = new BitmapImage(new Uri(imagePath, UriKind.RelativeOrAbsolute));
            

            // Assigner l'ImageBrush comme fond du Grid
            FenetreJeu.Background = backgroundBrush;


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
            double POS_GAUCHE_JOUEUR = Canvas.GetLeft(JOUEUR);
            if (Commande_Bas==true)
            {
                Canvas.SetTop(JOUEUR, POS_TOP_JOUEUR+Constante_Deplacement);
            }
            if (Commande_Haut == true)
            {
                Canvas.SetTop(JOUEUR, POS_TOP_JOUEUR-Constante_Deplacement);
            }
            if (Commande_Gauche == true)
            {
                Canvas.SetLeft(JOUEUR, POS_GAUCHE_JOUEUR-Constante_Deplacement);
            }
            if (Commande_Droit == true)
            {
                Canvas.SetLeft(JOUEUR, POS_GAUCHE_JOUEUR+Constante_Deplacement);
            }

        }

    }
}