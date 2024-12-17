using System;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Main
{
    /// <summary>
    /// Logique d'interaction pour Parametres.xaml
    /// </summary>
    public partial class Parametres : Window
    {
        private Canvas FenetreParametres = new Canvas();

        // Définir les touches configurables
        public Key toucheHaut, toucheHautDefaut = Key.Z;
        public Key toucheBas, toucheBasDefaut = Key.S;
        public Key toucheGauche, toucheGaucheDefaut = Key.Q;
        public Key toucheDroit, toucheDroiteDefaut = Key.D;
        public Key toucheActiver, toucheActiverDefaut = Key.E;

        private bool changementTouche = false;

        private Label labHaut = new Label();
        private Label labBas = new Label();
        private Label labGauche = new Label();
        private Label labDroite = new Label();
        private Label labActiver = new Label();

        private Button butHaut = new Button();
        private Button butBas = new Button();
        private Button butGauche = new Button();
        private Button butDroite = new Button();
        private Button butActiver = new Button();

        private Button butRetour = new Button();
        private Image fondParametres = new Image();
        private Label titre = new Label();

        public Parametres()
        {
            InitializeComponent();

            this.WindowStyle = WindowStyle.None;
            this.WindowState = WindowState.Maximized;
            this.ResizeMode = ResizeMode.NoResize;

            this.Content = FenetreParametres;

            fondParametres = new Image
            {
                Source = new BitmapImage(new Uri("pack://application:,,,/img/fondAccueilBlur.png")),
                Stretch = Stretch.Fill
            };

            FenetreParametres.Children.Add(fondParametres);
            Canvas.SetTop(fondParametres, 0);
            Canvas.SetLeft(fondParametres, 0);

            // Init Titre
            titre.Content = "Parametres";
            titre.FontSize = 60;
            titre.Foreground = Brushes.White;

            // Init Boutton Retour
            butRetour.Width = 60;
            butRetour.Height = 60;
            butRetour.BorderThickness = new Thickness(0);
            butRetour.Background = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("pack://application:,,,/img/btnAnnuler.png")),
                Stretch = Stretch.UniformToFill
            };

            // Init Labels
            labHaut.Content = "Haut"; labHaut.FontSize = 25;labHaut.Foreground = Brushes.White;
            labBas.Content = "Bas"; labBas.FontSize = 25;labBas.Foreground = Brushes.White;
            labGauche.Content = "Gauche"; labGauche.FontSize = 25;labGauche.Foreground = Brushes.White;
            labDroite.Content = "Droite"; labDroite.FontSize = 25;labDroite.Foreground = Brushes.White;
            labActiver.Content = "Activer"; labActiver.FontSize = 25;labActiver.Foreground = Brushes.White;

            // Init Boutton Changements
            butHaut.Height = 60;butHaut.Width = 100;butHaut.Background = Brushes.Black;butHaut.Foreground = Brushes.White;butHaut.FontWeight = FontWeights.Bold;
            butBas.Height = 60;butBas.Width = 100;butBas.Background = Brushes.Black;butBas.Foreground = Brushes.White;butBas.FontWeight = FontWeights.Bold;
            butGauche.Height = 60;butGauche.Width = 100;butGauche.Background = Brushes.Black;butGauche.Foreground = Brushes.White;butGauche.FontWeight = FontWeights.Bold;
            butDroite.Height = 60;butDroite.Width = 100;butDroite.Background = Brushes.Black;butDroite.Foreground = Brushes.White;butDroite.FontWeight = FontWeights.Bold;
            butActiver.Height = 60;butActiver.Width = 100;butActiver.Background = Brushes.Black;butActiver.Foreground = Brushes.White;butActiver.FontWeight = FontWeights.Bold;

            // Init les touches par défaut
            toucheHaut = toucheHautDefaut;
            toucheBas = toucheBasDefaut;
            toucheGauche = toucheGaucheDefaut;
            toucheDroit = toucheDroiteDefaut;
            toucheActiver = toucheActiverDefaut;

            // Affiche les touches sur les boutons
            ButtonContent();

            // Affiche les éléments dans la fenetre
            FenetreParametres.Children.Add(labHaut);
            FenetreParametres.Children.Add(labBas);
            FenetreParametres.Children.Add(labGauche);
            FenetreParametres.Children.Add(labDroite);
            FenetreParametres.Children.Add(labActiver);

            FenetreParametres.Children.Add(butHaut);
            FenetreParametres.Children.Add(butBas);
            FenetreParametres.Children.Add(butGauche);
            FenetreParametres.Children.Add(butDroite);
            FenetreParametres.Children.Add(butActiver);

            FenetreParametres.Children.Add(butRetour);
            FenetreParametres.Children.Add(titre);

            // Lancements des fonctions
            this.SizeChanged += OnWindowSizeChanged;
            butRetour.Click += ButRetour_Click;

            butGauche.Click += ButGauche_Click;
            butDroite.Click += ButDroite_Click;
            butHaut.Click += ButHaut_Click;
            butBas.Click += ButBas_Click;
            butActiver.Click += ButActiver_Click;

        }

        private void OnWindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            double canvasWidth = this.ActualWidth;
            double canvasHeight = this.ActualHeight;

            // Fond 
            fondParametres.Width = canvasWidth;
            fondParametres.Height = canvasHeight;

            Canvas.SetLeft(titre, (canvasWidth - 400) / 2); // Centré horizontalement
            Canvas.SetTop(titre, 50); // Position verticale du titre (ajustée en haut)

            // Position des labels et boutons
            double labelWidth = 150;
            double buttonWidth = 100;
            double espace = 80; 

            Canvas.SetLeft(labHaut, (canvasWidth / 2) - labelWidth - 50); 
            Canvas.SetTop(labHaut, (canvasHeight / 2) - 2 * espace);

            Canvas.SetLeft(labBas, (canvasWidth / 2) - labelWidth - 50);
            Canvas.SetTop(labBas, (canvasHeight / 2) - espace);

            Canvas.SetLeft(labGauche, (canvasWidth / 2) - labelWidth - 50);
            Canvas.SetTop(labGauche, (canvasHeight / 2));

            Canvas.SetLeft(labDroite, (canvasWidth / 2) - labelWidth - 50);
            Canvas.SetTop(labDroite, (canvasHeight / 2) + espace);

            Canvas.SetLeft(labActiver, (canvasWidth / 2) - labelWidth - 50);
            Canvas.SetTop(labActiver, (canvasHeight / 2) + 2 * espace);

            Canvas.SetLeft(butHaut, (canvasWidth / 2) + 50);
            Canvas.SetTop(butHaut, (canvasHeight / 2) - 2 * espace);

            Canvas.SetLeft(butBas, (canvasWidth / 2) + 50);
            Canvas.SetTop(butBas, (canvasHeight / 2) - espace);

            Canvas.SetLeft(butGauche, (canvasWidth / 2) + 50);
            Canvas.SetTop(butGauche, (canvasHeight / 2));

            Canvas.SetLeft(butDroite, (canvasWidth / 2) + 50);
            Canvas.SetTop(butDroite, (canvasHeight / 2) + espace);

            Canvas.SetLeft(butActiver, (canvasWidth / 2) + 50);
            Canvas.SetTop(butActiver, (canvasHeight / 2) + 2 * espace);

            Canvas.SetLeft(butRetour, canvasWidth - butRetour.Width - 20);
            Canvas.SetTop(butRetour, 20);
        }

        private void ButtonContent()
        {
            // Affiche les touches sur les boutons
            butHaut.Content = TouchesGlobales.ToucheHaut.ToString();
            butBas.Content = TouchesGlobales.ToucheBas.ToString();
            butGauche.Content = TouchesGlobales.ToucheGauche.ToString();
            butDroite.Content = TouchesGlobales.ToucheDroite.ToString();
            butActiver.Content = TouchesGlobales.ToucheActiver.ToString();
        }

        private void ButRetour_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        // Changement Bouton Gauche
        private void ButGauche_Click(object sender, RoutedEventArgs e)
        {
            changementTouche = true;
            butGauche.Content = "...";
            this.KeyDown += BoutonGauche_Keydown;
        }
        private void BoutonGauche_Keydown(object sender, KeyEventArgs e)
        {
            if (changementTouche)
            {
                if (e.Key == Key.Escape)
                {
                    butGauche.Content = toucheGaucheDefaut;
                    TouchesGlobales.ToucheGauche = toucheGaucheDefaut;
                } else
                {
                    butGauche.Content = e.Key.ToString();
                    TouchesGlobales.ToucheGauche = e.Key;
                }
                changementTouche = false;
                this.KeyDown -= BoutonGauche_Keydown;
            }
        }

        // Changement Bouton Droite
        private void ButDroite_Click(object sender, RoutedEventArgs e)
        {
            changementTouche = true;
            butDroite.Content = "...";
            this.KeyDown += BoutonDroite_Keydown;
        }
        private void BoutonDroite_Keydown(object sender, KeyEventArgs e)
        {
            if (changementTouche)
            {
                if (e.Key == Key.Escape)
                {
                    butDroite.Content = toucheDroiteDefaut;
                    TouchesGlobales.ToucheDroite = toucheDroiteDefaut;
                }
                else
                {
                    butDroite.Content = e.Key.ToString();
                    TouchesGlobales.ToucheDroite = e.Key;
                }

                changementTouche = false;
                this.KeyDown -= BoutonDroite_Keydown;
            }
        }

        // Changement Bouton Haut
        private void ButHaut_Click(object sender, RoutedEventArgs e)
        {
            changementTouche = true;
            butHaut.Content = "...";
            this.KeyDown += BoutonHaut_Keydown;
        }
        private void BoutonHaut_Keydown(object sender, KeyEventArgs e)
        {
            if (changementTouche)
            {
                if (e.Key == Key.Escape)
                {
                    butHaut.Content = toucheHautDefaut;
                    TouchesGlobales.ToucheHaut = toucheHautDefaut;
                }
                else
                {
                    butHaut.Content = e.Key.ToString();
                    TouchesGlobales.ToucheHaut = e.Key;
                }

                changementTouche = false;
                this.KeyDown -= BoutonHaut_Keydown;
            }
        }

        // Changement Bouton Bas
        private void ButBas_Click(object sender, RoutedEventArgs e)
        {
            changementTouche = true;
            butBas.Content = "...";
            this.KeyDown += BoutonBas_Keydown;
        }
        private void BoutonBas_Keydown(object sender, KeyEventArgs e)
        {
            if (changementTouche)
            {
                if (e.Key == Key.Escape)
                {
                    butBas.Content = toucheBasDefaut;
                    TouchesGlobales.ToucheBas = toucheBasDefaut;
                }
                else
                {
                    butBas.Content = e.Key.ToString();
                    TouchesGlobales.ToucheBas = e.Key;
                }

                changementTouche = false;
                this.KeyDown -= BoutonBas_Keydown;
            }
        }

        // Changement Bouton Activer
        private void ButActiver_Click(object sender, RoutedEventArgs e)
        {
            changementTouche = true;
            butActiver.Content = "...";
            this.KeyDown += BoutonActiver_Keydown;
        }
        private void BoutonActiver_Keydown(object sender, KeyEventArgs e)
        {
            if (changementTouche)
            {
                if (e.Key == Key.Escape)
                {
                    butActiver.Content = toucheActiverDefaut;
                    TouchesGlobales.ToucheActiver = toucheActiverDefaut;
                }
                else
                {
                    butActiver.Content = e.Key.ToString();
                    TouchesGlobales.ToucheActiver = e.Key;
                }

                changementTouche = false;
                this.KeyDown -= BoutonActiver_Keydown;
            }
        }
    }
}