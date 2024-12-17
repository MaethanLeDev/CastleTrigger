using Map;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.DirectoryServices.ActiveDirectory;
using System.Numerics;
using System.Runtime.Intrinsics.X86;
using System.Transactions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Main
{
    public partial class MainWindow : Window
    {
        // ----------- CONSTANTES --------------------------------
        Rectangle[,] tabRectCol = new Rectangle[60, 60];
        public static BitmapImage[] IMAGE_PORTE_HG = new BitmapImage[3];
        public static BitmapImage[] IMAGE_PORTE_HD = new BitmapImage[3];
        public static BitmapImage[] IMAGE_PORTE_BG = new BitmapImage[3];
        public static BitmapImage[] IMAGE_PORTE_BD = new BitmapImage[3];
        public static bool levier1 = false, levier2 = false, levier3 = false, levier4 = false, levier5 = false, levier6 = false, levier7 = false;
        BitmapImage ImageLevierOff = new BitmapImage(new Uri("pack://application:,,,/img/LEVIER_OFF.png"));
        public static Image[,] TAB_IMAGE = new Image[60,60];
        public static int[,] collisions = Map.JsonManager.INITTABCOLLISION();
        // ----------- Déclarations des variables ----------------
        private DispatcherTimer TimerJeu = new DispatcherTimer(); // Chrono pour mettre à jour le jeu
        public static DispatcherTimer TIM_ANI_PORTE;
        public List<Object> aDebug;
        // ---- FENETRE GRAPHIQUE
        private Image fondMap;
        public static Canvas FenetreJeu = new Canvas();

        // Boutons Pause
        private Button butPause = new Button();
        private Button butReprendre = new Button();
        private Button butQuitter = new Button();
        

        // ---- ENTITES
        public Entite Joueur;


        // État des touches
        public static bool toucheHaut = false;
        public static bool toucheBas = false;
        public static bool toucheGauche = false;
        public static bool toucheDroite = false;
        public static bool toucheE = false;

        Parametres parametre = new Parametres();

        public MainWindow()
        {
             
            InitializeComponent();
            
            this.WindowStyle = WindowStyle.None;
            this.WindowState = WindowState.Maximized;
            this.ResizeMode = ResizeMode.NoResize;

            // Boite de Dialogue Modale
            Accueil FenetreAccueil = new Accueil();
            FenetreAccueil.ShowDialog();
            if (FenetreAccueil.DialogResult == false)
            {
                Application.Current.Shutdown();
            }
            
            // Plein ecran
            this.WindowStyle = WindowStyle.None;
            this.WindowState = WindowState.Maximized;
            this.ResizeMode = ResizeMode.NoResize;

            this.Content = FenetreJeu;
            
            // Initialisation du joueur
            Joueur = new Entite(Constantes.VITESSEJOUEUR);


            // Initialisation du Timer
            TimerJeu.Interval = TimeSpan.FromMilliseconds(16); // ~60 FPS
            TimerJeu.Tick += JeuTic;
            TimerJeu.Start();

            // Ajout de la carte
            fondMap = new Image
            {
                Source = new BitmapImage(new Uri("pack://application:,,,/img/MAP_V1.png"))
            };
            FenetreJeu.Children.Add(fondMap);
            Canvas.SetTop(fondMap, 0);
            Canvas.SetLeft(fondMap, 0);

            // Ajout du joueur
            FenetreJeu.Children.Add(Joueur.image);
            Canvas.SetTop(Joueur.image, FenetreJeu.ActualHeight / 2);
            Canvas.SetLeft(Joueur.image, FenetreJeu.ActualWidth / 2);

            List<Rectangle> tests = new List<Rectangle>();

            for (int i = 0; i < 10; i++)
            {
                Rectangle rect = new Rectangle
                {
                    Width = Constantes.CASE,  // Largeur du rectangle (en supposant que Constantes.CASE est défini)
                    Height = Constantes.CASE, // Hauteur du rectangle
                    Fill = Brushes.Red        // Remplissage du rectangle avec la couleur rouge
                };

                // Définir la position du rectangle en fonction de l'index
                double xPosition = 30 + i * (Constantes.CASE + 5); // 5 est l'écart entre les rectangles
                double yPosition = 30; // Vous pouvez ajuster cette valeur si vous voulez les placer sur une autre ligne

                // Ajouter le rectangle au Canvas
                FenetreJeu.Children.Add(rect);
                Canvas.SetLeft(rect, xPosition);
                Canvas.SetTop(rect, yPosition);
                tests.Add(rect);

                




                INIT_PORTE_LEVIER();
               //INITCOL();

            }
            tests[0].Fill = Brushes.Blue;
        }
        public void INITCOL()
        {

            
            for (int p = 0; p < MainWindow.collisions.GetLength(0); p++)
            {
                for (int j = 0; j < MainWindow.collisions.GetLength(1); j++)
                {
                    if (collisions[p, j] != 0)
                    {
                        Rectangle rectl = new Rectangle
                        {
                            Width = Constantes.CASE,
                            Height = Constantes.CASE,
                            Fill = Brushes.Purple


                        };
                        FenetreJeu.Children.Add(rectl);
                        Canvas.SetLeft(rectl, j * 48);
                        Canvas.SetTop(rectl, p * 48);
                        tabRectCol[p, j] = rectl;
                    }


                }
            }
        }
        public void InitImageAnimation()
        {



        }
        public void Debug(List<Object> info, ref Canvas canvas)
        {

            List<Label> labelsToRemove = new List<Label>();
            List<Label> newLabel = new List<Label>();
            foreach (var child in canvas.Children)
            {
                if (child is Label label)
                {
                    labelsToRemove.Add(label);
                }
            }

            foreach (var label in labelsToRemove)
            {
                canvas.Children.Remove(label);
            }

            for (int i = 0; i < info.Count; i++)
            {
                Label labelToCreate = new Label
                {
                    Content = info[i].ToString(),
                    Foreground = Brushes.Green,
                    FontSize = 20,

                };


                canvas.Children.Add(labelToCreate);
                Canvas.SetLeft(labelToCreate, Joueur.coords[0] - canvas.ActualWidth / 2);

                if (i == 0)
                {
                    Canvas.SetTop(labelToCreate, Joueur.coords[1] - canvas.ActualHeight / 2);
                }
                else
                {
                    Canvas.SetTop(labelToCreate, Canvas.GetTop(newLabel[i - 1]) + 25);
                }
                newLabel.Add(labelToCreate);
            }
        }
        public void  INIT_PORTE_LEVIER()
        {
            int[,] tabPorteLevier = Map.JsonManager.INITTABLEVIER_PORTE();
            for (int i = 0; i < IMAGE_PORTE_BD.Length; i++)
            {
                IMAGE_PORTE_BD[i] = new BitmapImage(new Uri($"pack://application:,,,/img/PORTE/PORTE_BD/PORTE_BD_0{i + 1}.png"));
            }
            for (int i = 0; i < IMAGE_PORTE_HG.Length; i++) 
            {
                IMAGE_PORTE_HG[i] = new BitmapImage(new Uri($"pack://application:,,,/img/PORTE/PORTE_HG/PORTE_HG_0{i + 1}.png"));
            }
            for (int i = 0; i < IMAGE_PORTE_HD.Length; i++)
            {
                IMAGE_PORTE_HD[i] = new BitmapImage(new Uri($"pack://application:,,,/img/PORTE/PORTE_HD/PORTE_HD_0{i + 1}.png"));
            }
            for (int i = 0; i < IMAGE_PORTE_BG.Length; i++)
            {
                IMAGE_PORTE_BG[i] = new BitmapImage(new Uri($"pack://application:,,,/img/PORTE/PORTE_BG/PORTE_BG_0{i + 1}.png"));
            }
            
            for (int p = 0; p < tabPorteLevier.GetLength(0); p++)
            {
                for (int j = 0; j < tabPorteLevier.GetLength(1); j++)
                {
                    
                    if (tabPorteLevier[p, j] == 1)
                    {
                        Image porte = new Image
                        {
                            Width = Constantes.CASE,
                            Height = Constantes.CASE,
                            Source = IMAGE_PORTE_HG[0]


                        };
                        FenetreJeu.Children.Add(porte);
                        Canvas.SetLeft(porte, j * 48);
                        Canvas.SetTop(porte, p * 48);
                        TAB_IMAGE[p,j] = porte;

                    }
                    if (tabPorteLevier[p, j] == 2)
                    {
                        Image porte = new Image
                        {
                            Width = Constantes.CASE,
                            Height = Constantes.CASE,
                            Source = IMAGE_PORTE_HD[0]


                        };
                        FenetreJeu.Children.Add(porte);
                        Canvas.SetLeft(porte, j * 48);
                        Canvas.SetTop(porte, p * 48);
                        TAB_IMAGE[p, j] = porte;
                    }
                    if (tabPorteLevier[p, j] == 3)
                    {
                        Image porte = new Image
                        {
                            Width = Constantes.CASE,
                            Height = Constantes.CASE,
                            Source = IMAGE_PORTE_BG[0]


                        };
                        FenetreJeu.Children.Add(porte);
                        Canvas.SetLeft(porte, j * 48);
                        Canvas.SetTop(porte, p * 48);
                        TAB_IMAGE[p, j] = porte;
                    }
                    if (tabPorteLevier[p, j] == 4)
                    {
                        Image porte = new Image
                        {
                            Width = Constantes.CASE,
                            Height = Constantes.CASE,
                            Source = IMAGE_PORTE_BD[0]


                        };
                        FenetreJeu.Children.Add(porte);
                        Canvas.SetLeft(porte, j * 48);
                        Canvas.SetTop(porte, p * 48);
                        TAB_IMAGE[p, j] = porte;
                    }
                    if (tabPorteLevier[p, j] == 901)
                    {
                        Image levier = new Image
                        {
                            Width = Constantes.CASE,
                            Height = Constantes.CASE,
                            Source = new BitmapImage(new Uri("pack://application:,,,/img/LEVIER_OFF.png"))


                        };
                        FenetreJeu.Children.Add(levier);
                        Canvas.SetLeft(levier, j * 48);
                        Canvas.SetTop(levier, p * 48);
                        TAB_IMAGE[p, j] = levier;
                    }
                }
            }
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == TouchesGlobales.ToucheHaut) toucheHaut = true;
            if (e.Key == TouchesGlobales.ToucheBas) toucheBas = true;
            if (e.Key == TouchesGlobales.ToucheGauche) toucheGauche = true;
            if (e.Key == TouchesGlobales.ToucheDroite) toucheDroite = true;


            Joueur.MettreAJourDirection();
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == TouchesGlobales.ToucheHaut) toucheHaut = false;
            if (e.Key == TouchesGlobales.ToucheBas) toucheBas = false;
            if (e.Key == TouchesGlobales.ToucheGauche) toucheGauche = false;
            if (e.Key == TouchesGlobales.ToucheDroite) toucheDroite = false;


            Joueur.MettreAJourDirection();
        }
        
        private void InitPause()
        {
            butPause.Height = 80;butPause.Width = 80;butPause.Background = Brushes.Green;
            butReprendre.Height = 80;butReprendre.Width = 80;butReprendre.Background = Brushes.Blue;
            butQuitter.Height = 80;butQuitter.Width = 80;butQuitter.Background = Brushes.Red;

            FenetreJeu.Children.Add(butPause);
            FenetreJeu.Children.Add(butReprendre);
            FenetreJeu.Children.Add(butQuitter);

            double canvasWidth = this.ActualWidth;
            double canvasHeight = this.ActualHeight;
            Canvas.SetLeft(butPause, canvasWidth - butPause.Width - 20);
            Canvas.SetTop(butPause, canvasHeight * 0.2);
        }

        private void JeuTic(object sender, EventArgs e)
        {
            // Mise à jour des coordonnées du joueur
            Joueur.MettreAJourPosition();

            aDebug = new List<Object>
             {
             Joueur.coords[0]/48,
             Joueur.coords[1]/48,
             FenetreJeu.ActualHeight
            };

            Debug(aDebug, ref FenetreJeu);


            // Mise à jour de la position graphique du joueur
            Canvas.SetTop(Joueur.image, Joueur.coords[1]);
            Canvas.SetLeft(Joueur.image, Joueur.coords[0]);
            MoveCamera();
        }

        private void MoveCamera()
        {
            // Calculer la translation nécessaire pour centrer le joueur
            double offsetX = Joueur.coords[0] - (FenetreJeu.ActualWidth / 2);
            double offsetY = Joueur.coords[1] - (FenetreJeu.ActualHeight / 2);

            // Appliquer une translation sur le Canvas
            FenetreJeu.RenderTransform = new TranslateTransform(-offsetX, -offsetY);
        }
    }

    public class Entite
    {
        public double[] coords { get; private set; } // Coordonnées du joueur (x, y)
        public double speed { get; private set; }   // Vitesse du joueur
        

        public Vector2 direction;                  // Direction actuelle (0, -1), (1, 0), etc.
        public Rectangle image;


        public Entite(double vitesseInitiale)
        {
            coords = new double[2] {1600, 2300 }; // Position de départsss
            speed = vitesseInitiale;
            direction = new Vector2(0, 0);    // Pas de mouvement initial
            this.image = new Rectangle
            {
                Width = Constantes.CASE,
                Height = Constantes.CASE,
                Fill = Brushes.Red
            };
        }


        // Met à jour la position du joueur
        public void MettreAJourPosition()
        {
            int[] tabCollision = MethodeCollision(this.coords);
            if (direction.X == -1 && tabCollision[0] == 0)
            {
                coords[0] += direction.X * speed;
            }
            if (direction.X == +1 && tabCollision[1] == 0)
            {
                coords[0] += direction.X * speed;
            }
            if (direction.Y == -1 && tabCollision[2] == 0)
            {
                coords[1] += direction.Y * speed;
            }
            if (direction.Y == +1 && tabCollision[3] == 0)
            {
                coords[1] += direction.Y * speed;
            }
        }

        public void MettreAJourDirection()
        {
            int dx = 0;
            int dy = 0;

            if (MainWindow.toucheHaut)
            {
                dy -= 1;

            }
            if (MainWindow.toucheBas)
            {
                dy += 1;


            }
            if (MainWindow.toucheGauche)
            {
                dx -= 1;


            }
            if (MainWindow.toucheDroite)
            {
                dx += 1;
            }
            double longueur = Math.Sqrt(dx * dx + dy * dy); // normalisation du vecteur
            if (longueur != 0)
            {
                dx /= (int)longueur;
                dy /= (int)longueur;
            }
            direction = new Vector2(dx, dy);
        }
        public int[] MethodeCollision(double[] coords)
        {
             
            
            int[] REP = new int[4];
            string filePath = "pack://application:,,,/img/Collision.json";
            int collonePersonnage, lignePersonnage;
            Canvas.GetBottom(image);
            //regarde la position du joueur dans un tableau
            lignePersonnage = (int)Math.Truncate(((coords[1]+24) / Constantes.CASE));
            collonePersonnage = (int)Math.Truncate(((coords[0]+24) / Constantes.CASE));
            
            double test = coords[1] - 48,test2 = coords[0]+48;
            

            // Regarde à coté du joueur pour voir si mur
           REP[0] = MainWindow.collisions[lignePersonnage, (int)Math.Truncate(((coords[0] + 45) / Constantes.CASE)) - 1]; //GAUCHE
           REP[1] = MainWindow.collisions[lignePersonnage, (int)Math.Truncate(((coords[0] ) / Constantes.CASE)) + 1]; //Droite
           REP[2] = MainWindow.collisions[(int)Math.Truncate(((coords[1] + 43) / Constantes.CASE)) - 1, collonePersonnage];  //Haut
           REP[3] = MainWindow.collisions[(int)Math.Truncate(((coords[1] + 3) / Constantes.CASE)) + 1, collonePersonnage];//Bas
            if(MainWindow.toucheE==true)
            ActivationLevier(lignePersonnage,collonePersonnage);
            return REP;
        }
        public void ActivationLevier(int lignePersonnage, int collonePersonnage)
        {
           
            if (MainWindow.toucheE == true && TestLEVIER() == true)
            {
                int colLevier = 0, liLevier = 0;
                int[,] tabPorteLevier = Map.JsonManager.INITTABLEVIER_PORTE();
                Console.WriteLine("it's good");
                if (tabPorteLevier[lignePersonnage, collonePersonnage - 1] == 901)
                {
                    liLevier = lignePersonnage;
                    colLevier = collonePersonnage - 1;
                    
                }
                if (tabPorteLevier[lignePersonnage, collonePersonnage + 1] == 901)
                {
                    liLevier = lignePersonnage;
                    colLevier = collonePersonnage + 1;

                }
                if (tabPorteLevier[lignePersonnage - 1, collonePersonnage] == 901)
                {
                    liLevier = lignePersonnage - 1;
                    colLevier = collonePersonnage;

                }
                if (tabPorteLevier[lignePersonnage + 1, collonePersonnage] == 901)
                {
                    liLevier = lignePersonnage + 1;
                    colLevier = collonePersonnage;

                }


                MainWindow.TAB_IMAGE[liLevier, colLevier].Source = new BitmapImage(new Uri("pack://application:,,,/img/LEVIER_ON.png"));
                GestionPORTE(liLevier, colLevier);
            }
        
            

        }
        public bool TestLEVIER()
        {
            bool testlevier= false;
            int[,] tabPorteLevier = Map.JsonManager.INITTABLEVIER_PORTE();
            int lignePersonnage = (int)Math.Truncate(((coords[1] + 24) / Constantes.CASE));
            int collonePersonnage = (int)Math.Truncate(((coords[0] + 24) / Constantes.CASE));
            if (tabPorteLevier[lignePersonnage,collonePersonnage]==901 || tabPorteLevier[lignePersonnage-1, collonePersonnage] == 901 || tabPorteLevier[lignePersonnage+1, collonePersonnage] == 901 || tabPorteLevier[lignePersonnage, collonePersonnage+1] == 901 || tabPorteLevier[lignePersonnage, collonePersonnage-1] == 901)
            {
                testlevier = true;

            }
            return testlevier;
            
        }
        public static int[] coordoneePorte = new int[8];
        public void GestionPORTE(int liLevier,int colLevier)
        {
            if (liLevier == 3 && colLevier== 36 && MainWindow.levier1==false)
            {

                MainWindow.levier1 = true;
                MainWindow.collisions[7, 35] = 0;
                MainWindow.collisions[7, 36] = 0;
                MainWindow.collisions[9, 35] = 0;
                MainWindow.collisions[9, 36] = 0;
                coordoneePorte = [8, 35, 8, 36, 9, 35, 9, 36];
                TimerAnimationPorte();
                
            }
            if (liLevier == 10 && colLevier == 44 && MainWindow.levier2==false)
            {

                MainWindow.levier2 = true;
                MainWindow.collisions[19, 46] = 0;
                MainWindow.collisions[19, 47] = 0;
                MainWindow.collisions[21, 46] = 0;
                MainWindow.collisions[21, 47] = 0;
                coordoneePorte = [20, 46, 20, 47, 21, 46, 21, 47];
                TimerAnimationPorte();
            }
            if (liLevier == 27 && colLevier == 52 && MainWindow.levier3==false)
            {

                MainWindow.levier3 = true;
                MainWindow.collisions[34, 48] = 0;
                MainWindow.collisions[34, 49] = 0;
                MainWindow.collisions[36, 48] = 0;
                MainWindow.collisions[36, 49] = 0;
                coordoneePorte = [35, 48, 35, 49, 36, 48, 36, 49];
                TimerAnimationPorte();
            }
            if (liLevier == 54 && colLevier == 15 && MainWindow.levier4==false)
            {

                MainWindow.levier4 = true;
                MainWindow.collisions[41, 23] = 0;
                MainWindow.collisions[41, 24] = 0;
                MainWindow.collisions[43, 23] = 0;
                MainWindow.collisions[43, 24] = 0;
                coordoneePorte = [42, 23, 42, 24, 43, 23, 43, 24];
                TimerAnimationPorte();
            }
            if (liLevier == 37 && colLevier == 29 && MainWindow.levier5 == false)
            {

                MainWindow.levier5 = true;
                MainWindow.collisions[34, 21] = 0;
                MainWindow.collisions[34, 22] = 0;
                MainWindow.collisions[36, 21] = 0;
                MainWindow.collisions[36, 22] = 0;
                coordoneePorte = [35, 21, 35, 22, 36, 21, 36, 22];
                TimerAnimationPorte();
            }
            if (liLevier == 22 && colLevier == 23 && MainWindow.levier6 == false)
            {

                MainWindow.levier6 = true;
                MainWindow.collisions[19, 21] = 0;
                MainWindow.collisions[19, 22] = 0;
                MainWindow.collisions[21, 21] = 0;
                MainWindow.collisions[21, 22] = 0;
                coordoneePorte = [20, 21, 20, 22, 21, 21, 21, 22];
                TimerAnimationPorte();
            }
            if (liLevier == 11 && colLevier == 11 && MainWindow.levier7 == false)
            {

                MainWindow.levier7 = true;
                MainWindow.collisions[8, 13] = 0;
                MainWindow.collisions[8, 14] = 0;
                MainWindow.collisions[10, 13] = 0;
                MainWindow.collisions[10, 14] = 0;
                coordoneePorte = [9, 13, 9, 14, 10, 13, 10, 14];
                TimerAnimationPorte();
            }
        }
        public void TimerAnimationPorte()
        {
            MainWindow.TIM_ANI_PORTE = new DispatcherTimer();
            MainWindow.TIM_ANI_PORTE.Interval = TimeSpan.FromMilliseconds(60);
            MainWindow.TIM_ANI_PORTE.Tick += AnimationPorte;
            MainWindow.TIM_ANI_PORTE.Start();
        }
        public static int compte = 0;
        public static int compte2 = 0;
        public void AnimationPorte(object sender, EventArgs e)
        {
            compte2++;
            
            if (compte2 % 3 == 0)
            {
                {
                    compte = compte + 1;
                    MainWindow.TAB_IMAGE[coordoneePorte[0], coordoneePorte[1]].Source = MainWindow.IMAGE_PORTE_HG[compte];
                    MainWindow.TAB_IMAGE[coordoneePorte[2], coordoneePorte[3]].Source = MainWindow.IMAGE_PORTE_HD[compte];
                    MainWindow.TAB_IMAGE[coordoneePorte[4], coordoneePorte[5]].Source = MainWindow.IMAGE_PORTE_BG[compte];
                    MainWindow.TAB_IMAGE[coordoneePorte[6], coordoneePorte[7]].Source = MainWindow.IMAGE_PORTE_BD[compte];
                }
            }
            if (compte2 >= 7)
            {
                if (MainWindow.TAB_IMAGE[coordoneePorte[0], coordoneePorte[1]].Source == MainWindow.IMAGE_PORTE_HG[1])
                {
                    MainWindow.TAB_IMAGE[coordoneePorte[0], coordoneePorte[1]].Source = MainWindow.IMAGE_PORTE_HG[2];
                    MainWindow.TAB_IMAGE[coordoneePorte[2], coordoneePorte[3]].Source = MainWindow.IMAGE_PORTE_HD[2];
                    MainWindow.TAB_IMAGE[coordoneePorte[4], coordoneePorte[5]].Source = MainWindow.IMAGE_PORTE_BG[2];
                    MainWindow.TAB_IMAGE[coordoneePorte[6], coordoneePorte[7]].Source = MainWindow.IMAGE_PORTE_BD[2];
                }
                Console.WriteLine("hey");
                MainWindow.TIM_ANI_PORTE.Stop();
                compte = 0;
                compte2 = 0;
                

               
            }

        }
    }


}
    
