using System;
using System.Security.AccessControl;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Main
{
    public partial class MainWindow : Window
    {
        // ----------- CONSTANTES --------------------------------
        private readonly string imagePath = "P:\\Sàé1.01\\CastleTrigger\\Assets\\Map_V1.png"; 
        private Image fondMap;
        public static readonly int CASE = 48;



        // ----------- Déclarations des variables ----------------
        private DispatcherTimer TimerJeu = new DispatcherTimer(); // C'est le chrono
        private bool Commande_Haut = false, Commande_Bas = false, Commande_Gauche = false, Commande_Droit = false;

        // ---- FENETRE
        Canvas FenetreJeu = new Canvas();
        Rectangle JOUEUR = new Rectangle
        {
            Width = CASE,
            Height = CASE,
            Fill = Brushes.Red
        };

        double joueurX = CASE;
        double joueurY = CASE;

        public MainWindow()
        {
            InitializeComponent();
            this.Content = FenetreJeu;


            TimerJeu.Interval = TimeSpan.FromMilliseconds(16);
            TimerJeu.Tick += JeuTic;
            TimerJeu.Start();


            fondMap = new Image();
            fondMap.Source = new BitmapImage(new Uri(imagePath, UriKind.RelativeOrAbsolute));
            FenetreJeu.Children.Add(fondMap); // Fond
            Canvas.SetTop(fondMap, 0);
            Canvas.SetLeft(fondMap, 0);


            FenetreJeu.Children.Add(JOUEUR); // Joueur
            Canvas.SetTop(JOUEUR, FenetreJeu.ActualHeight / 2);
            Canvas.SetLeft(JOUEUR, FenetreJeu.ActualWidth / 2);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Z) Commande_Haut = true;
            if (e.Key == Key.S) Commande_Bas = true;
            if (e.Key == Key.Q) Commande_Gauche = true;
            if (e.Key == Key.D) Commande_Droit = true;
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Z) Commande_Haut = false;
            if (e.Key == Key.S) Commande_Bas = false;
            if (e.Key == Key.Q) Commande_Gauche = false;
            if (e.Key == Key.D) Commande_Droit = false;
        }

        private void JeuTic(object sender, EventArgs e)
        {

            Canvas.SetTop(JOUEUR, FenetreJeu.ActualHeight / 2 - JOUEUR.Height / 2);
            Canvas.SetLeft(JOUEUR, FenetreJeu.ActualWidth / 2 - JOUEUR.Width / 2);
            Deplacement();
        }

        public void Deplacement()
        {
            double fondMapX = Canvas.GetLeft(fondMap);
            double fondMapY = Canvas.GetTop(fondMap);


            if (Commande_Bas)
            {
                Canvas.SetTop(fondMap, fondMapY - 10);
            }


            if (Commande_Haut)
            {
                Canvas.SetTop(fondMap, fondMapY + 10);
            }


            if (Commande_Gauche)
            {
                Canvas.SetLeft(fondMap, fondMapX + 10);
            }


            if (Commande_Droit)
            {
                Canvas.SetLeft(fondMap, fondMapX - 10);
            }
        }
    }
}