﻿using Map;
using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Numerics;
using System.Transactions;
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
        Rectangle[,] tabRectCol = new Rectangle[60, 60];

        // ----------- Déclarations des variables ----------------
        private DispatcherTimer TimerJeu = new DispatcherTimer(); // Chrono pour mettre à jour le jeu

        // ---- FENETRE GRAPHIQUE
        private Image fondMap;
        private Canvas FenetreJeu = new Canvas();

        // ---- ENTITES
        public Entite Joueur;


        // État des touches
        public static bool toucheHaut = false;
        public static bool toucheBas = false;
        public static bool toucheGauche = false;
        public static bool toucheDroite = false;

        public MainWindow()
        {
            InitializeComponent();
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


                int[,] collisions = Map.JsonManager.ChargerCollision("pack://application:,,,/img/Collision.json");

                // Exemple d'accès à une cellule
                Console.WriteLine(collisions[5, 3]);
                INITCOL();

            }
            tests[0].Fill = Brushes.Blue;
        }
        public void INITCOL()
        {
            int[,] collisions = Map.JsonManager.ChargerCollision("pack://application:,,,/img/Collision.json");
                   
            for (int p = 0; p < collisions.GetLength(0); p++)
            {
                for (int j = 0; j < collisions.GetLength(1); j++)
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



        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Z) toucheHaut = true;
            if (e.Key == Key.S) toucheBas = true;
            if (e.Key == Key.Q) toucheGauche = true;
            if (e.Key == Key.D) toucheDroite = true;

            Joueur.MettreAJourDirection();
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Z) toucheHaut = false;
            if (e.Key == Key.S) toucheBas = false;
            if (e.Key == Key.Q) toucheGauche = false;
            if (e.Key == Key.D) toucheDroite = false;

            Joueur.MettreAJourDirection();
        }



        private void JeuTic(object sender, EventArgs e)
        {
            // Mise à jour des coordonnées du joueur
            Joueur.MettreAJourPosition();


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
            coords = new double[2] {600 ,300  }; // Position de départsss
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
            int[,] collisions = Map.JsonManager.ChargerCollision("pack://application:,,,/img/Collision.json");
            int collonePersonnage, lignePersonnage;
            Canvas.GetBottom(image);
            //regarde la position du joueur dans un tableau
            lignePersonnage = (int)Math.Truncate(((coords[1]+24) / Constantes.CASE));
            collonePersonnage = (int)Math.Truncate(((coords[0]+24) / Constantes.CASE));
            
            double test = coords[1] - 48,test2 = coords[0]+48;
            Console.WriteLine((lignePersonnage) + "   " + (collonePersonnage));

            // Regarde à coté du joueur pour voir si mur
           REP[0] = collisions[lignePersonnage, (int)Math.Truncate(((coords[0] + 45) / Constantes.CASE)) - 1]; //GAUCHE
           REP[1] = collisions[lignePersonnage, (int)Math.Truncate(((coords[0] ) / Constantes.CASE)) + 1]; //Droite
           REP[2] = collisions[(int)Math.Truncate(((coords[1] + 43) / Constantes.CASE)) - 1, collonePersonnage];  //Haut
           REP[3] = collisions[(int)Math.Truncate(((coords[1] + 3) / Constantes.CASE)) + 1, collonePersonnage];//Bas

            return REP;
        }

    }


}
    
