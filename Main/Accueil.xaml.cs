using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Media;

namespace Main
{
    /// <summary>
    /// Logique d'interaction pour Accueil.xaml
    /// </summary>
    public partial class Accueil : Window
    {
        private static MediaPlayer musique;


        private Canvas FenetreAccueil = new Canvas();
        private Image fondAccueil = new Image();
        private Button butJouer = new Button();
        private Button butAnnuler = new Button();
        private Button butParametres = new Button();
        private Label titre = new Label();

        public Key toucheHaut = Key.Z;
        public Key toucheBas = Key.S;
        public Key toucheGauche = Key.Q;
        public Key toucheDroite = Key.D;
        public Key toucheActiver = Key.E;

        public Accueil()
        {
            InitializeComponent();

            this.WindowStyle = WindowStyle.None;
            this.WindowState = WindowState.Maximized;
            this.ResizeMode = ResizeMode.NoResize;

            this.Content = FenetreAccueil;

            FenetreAccueil.HorizontalAlignment = HorizontalAlignment.Stretch;
            FenetreAccueil.VerticalAlignment = VerticalAlignment.Stretch;

            fondAccueil = new Image
            {
                Source = new BitmapImage(new Uri("pack://application:,,,/img/fondAccueil.png")),
                Stretch = Stretch.Fill // Pour adapter l'image à la taille du Canvas
            };

            FenetreAccueil.Children.Add(fondAccueil);
            Canvas.SetTop(fondAccueil, 0);
            Canvas.SetLeft(fondAccueil, 0);

            // Init Titre
            titre.Content = "Castle Trigger";
            titre.FontSize = 60;
            titre.Foreground =  Brushes.White;

            // Init Boutton Jouer
            butJouer.Width = 228;
            butJouer.Height = 76;
            butJouer.BorderThickness = new Thickness(0);
            butJouer.Background = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("pack://application:,,,/img/btnJouer.png")),
                Stretch = Stretch.UniformToFill
            };

            // Init Bouton Annuler
            butAnnuler.Width = 80;
            butAnnuler.Height = 80;
            butAnnuler.BorderThickness = new Thickness(0);
            butAnnuler.Background = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("pack://application:,,,/img/btnAnnuler.png")),
                Stretch = Stretch.UniformToFill
            };

            // Init Bouton Paramètres
            butParametres.Width = 60;
            butParametres.Height = 60;
            butParametres.BorderThickness = new Thickness(0);
            butParametres.Background = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("pack://application:,,,/img/boutonParametres.png")),
                Stretch = Stretch.UniformToFill
            };

            FenetreAccueil.Children.Add(titre);
            FenetreAccueil.Children.Add(butJouer);
            FenetreAccueil.Children.Add(butAnnuler);
            FenetreAccueil.Children.Add(butParametres);

            InitMusiqueFond();

            this.SizeChanged += OnWindowSizeChanged;

            butJouer.Click += butJouer_Click;
            butAnnuler.Click += butAnnuler_Click;
            butParametres.Click += butParametres_Click;
        }

        private void OnWindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            double canvasWidth = this.ActualWidth;
            double canvasHeight = this.ActualHeight;

            // Adapter image de fond
            fondAccueil.Width = canvasWidth;
            fondAccueil.Height = canvasHeight;

            double titreWidth = 400; 
            Canvas.SetLeft(titre, (canvasWidth - titreWidth) / 2);
            Canvas.SetTop(titre, canvasHeight * 0.2); 

            Canvas.SetLeft(butJouer, (canvasWidth - butJouer.Width) / 2 - 150);
            Canvas.SetTop(butJouer, canvasHeight * 0.5); 

            Canvas.SetLeft(butAnnuler, (canvasWidth - butAnnuler.Width) / 2 + 150);
            Canvas.SetTop(butAnnuler, canvasHeight * 0.5);

            Canvas.SetLeft(butParametres, canvasWidth - butParametres.Width - 20); 
            Canvas.SetTop(butParametres, 20);
        }

        private void InitMusiqueFond()
        {
            musique = new MediaPlayer();
            musique.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory +"sons/accueilFond.mp3"));
            musique.MediaEnded += RelanceMusique;
            musique.Volume = 1.0;
            musique.Play();
        }

        private void RelanceMusique(object? sender, EventArgs e)
        {
            musique.Position = TimeSpan.Zero;
            musique.Play();
        }

        private void butJouer_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
        private void butAnnuler_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        private void butParametres_Click(object sender, RoutedEventArgs e)
        {
            Parametres parametres = new Parametres();
            parametres.ShowDialog();

            toucheHaut = parametres.toucheHaut;
            toucheBas = parametres.toucheBas;
            toucheGauche = parametres.toucheGauche;
            toucheDroite = parametres.toucheDroit;
            toucheActiver = parametres.toucheActiver;
        }
    }
}