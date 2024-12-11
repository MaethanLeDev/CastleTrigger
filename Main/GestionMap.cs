using System;
using System.Collections.Generic;
using System.IO;
using Main;
using Newtonsoft.Json.Linq;
using System.Windows;


namespace Map
{
    public static class JsonManager
    {
        // Méthode pour lire et convertir les données JSON
        public static int[,] ChargerCollision(string cheminUriPack)
        {
            try
            {
                // Charger le flux depuis le pack URI
                Uri uri = new Uri(cheminUriPack, UriKind.Absolute);
                using (Stream stream = Application.GetResourceStream(uri).Stream)
                using (StreamReader reader = new StreamReader(stream))
                {
                    string jsonData = reader.ReadToEnd();

                    // Désérialisation et traitement
                    JObject jsonObject = JObject.Parse(jsonData);
                    var collisionData = jsonObject["layers"]?[1]?["data"];
                    int width = jsonObject["width"]?.ToObject<int>() ?? 0;
                    int height = jsonObject["height"]?.ToObject<int>() ?? 0;

                    if (collisionData != null && width > 0 && height > 0)
                    {
                        List<int> collisionDataListe = collisionData.ToObject<List<int>>();
                        int[,] collisions = new int[height, width];

                        for (int i = 0; i < height; i++)
                        {
                            for (int j = 0; j < width; j++)
                            {
                                collisions[i, j] = collisionDataListe[i * width + j];
                            }
                        }
                        return collisions;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la lecture du fichier JSON : {ex.Message}");
            }
            return new int[0, 0]; // Retourne un tableau vide en cas d'erreur
        }



    }
}