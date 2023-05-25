using System;
using System.Windows;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TavernManagerMetier.Metier.Algorithmes.Graphes;
using TavernManagerMetier.Metier.Tavernes;
using System.Net.Sockets;

namespace TavernManagerMetier.Metier.Algorithmes.Realisations
{

    public class AlgorithmeDSATUR : IAlgorithme
    {
        private long tempsExecution = -1;

        public string Nom => "Dsatur";

        public long TempsExecution => tempsExecution;
        public void Executer(Taverne taverne)
        {
            try
            {
                ExecuterAlgo(taverne);
            }
            catch (Exception ex)
            {
                this.tempsExecution = -1;
                MessageBox.Show(ex.Message);
            }
        }
        public void ExecuterAlgo(Taverne taverne)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                Graphe graphe = new Graphe(taverne);

                List<Sommet> sommetsNonColories = graphe.Sommets.ToList();

                List<int> ClientA1Table = new List<int>();
                ClientA1Table.Add(0);

                while (sommetsNonColories.Any())
                {
                    Sommet sommetChoisi = sommetsNonColories
                        .OrderByDescending(s => s.Voisin.Count(v => v.Couleur != -1)) // Critère 1 : Le plus de voisins coloriés
                        .ThenByDescending(s => s.Voisin.Count) // Critère 2 : Le plus de voisins
                        .ThenBy(s => Guid.NewGuid()) // Critère 3 : Aléatoire
                        .First();

                    HashSet<int> couleursVoisins = new HashSet<int>(
                        sommetChoisi.Voisin
                            .Where(v => v.Couleur != -1)
                            .Select(v => v.Couleur));

                    int couleur = 0;
                    Sommet save = sommetChoisi;
                    while (couleursVoisins.Contains(couleur) || ClientA1Table[couleur] + save.NbClients > taverne.CapactieTables)
                    {
                        couleur++;
                        ClientA1Table.Add(0);
                    }
                    
                    sommetChoisi.Couleur = couleur;
                    sommetsNonColories.Remove(sommetChoisi);
                    ClientA1Table[couleur] += sommetChoisi.NbClients;

                }

                for (int i = 0; i <= graphe.Sommets.Max(s => s.Couleur); i++)
                {
                    taverne.AjouterTable();
                }

                foreach (Client client in taverne.Clients)
                {
                    Sommet sommetDuClient = graphe.DicoSOMMET[client];
                    taverne.AjouterClientTable(client.Numero, sommetDuClient.Couleur);
                }

                sw.Stop();
                tempsExecution = sw.ElapsedMilliseconds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }













    /*public class AlgorithmeDSATURSA : IAlgorithme
    {
        private long tempsExecution = -1;
        /// <summary>
        /// propriété pour le nom de l'algorithme
        /// </summary>
        public string Nom => "Dsatur SA";
        /// <summary>
        /// propriété pour initialiser le temps d'execution à -1
        /// </summary>
        public long TempsExecution => tempsExecution;



        public void Executer(Taverne taverne)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Graphe graphe = new Graphe(taverne);


            List<int> CouleursSommets = new List<int>();
            List<Sommet> ListSommets = new List<Sommet>();
            int couleur = 0;

            foreach (Sommet sommet in graphe.Sommets)
            {
                ListSommets.Add(sommet);
            }

            foreach (Sommet sommet in ListSommets)
            {
                sommet.Couleur = -1;
            }

            Sommet GAGNANT = new Sommet();
            while (ListSommets.Any(sommet => sommet.Couleur == -1))
            {
                int voisinCouleurMax = 0;
                int voisinMax = 0;
                int nbVoisin = 0;
                List<Sommet> voisinDuSommet = new List<Sommet>();
                List<Sommet> EgaliteEntreSommet = new List<Sommet>();
                


                foreach (Sommet sommet in ListSommets)
                {

                    nbVoisin = sommet.Voisin.Count; // Nombre de voisin du sommet
                    voisinDuSommet.Clear(); // Clear pour les futur voisin du nouveau sommet (foreach) listé dedans
                    for (int i = 0; i < nbVoisin; i++) // Pour tous les voisin du sommet
                    {
                        voisinDuSommet.Add(sommet.Voisin[i]); // On ajoute a la liste le voisin a la liste pour listé tous les voisins du sommet
                    }
                    // A PARTIR DE LA ON A TOUS LES VOISINS DU SOMMET


                    //On attribut a sommet actuel, le nombre de ses voisin qui possède une couleur
                    sommet.MaxVoisinColorier = voisinDuSommet.Count(nbVoisinACouleur => nbVoisinACouleur.Couleur != -1);

                    // On vérifie que le sommet actuel possède plus de voisin colorié que le sommet précédent
                    if (sommet.MaxVoisinColorier > voisinCouleurMax) // LE CRITERE 1
                    {
                        // Si oui, on remplace la variable de nombre maximal de voisin colorié
                        voisinCouleurMax = sommet.MaxVoisinColorier;
                        // Puis on copie l'objet du sommet actuel pour le donner au nouveau (tout attribut propri compris)
                        GAGNANT = (Sommet)sommet.Clone();
                    }


                    // Si même nombre voisin colorié, if GAGNANT a plus de voisin que sommet
                    else if (sommet.MaxVoisinColorier == voisinCouleurMax)
                    {
                        // Si même nombre voisin colorié + Si même nombre voisin
                        if (sommet.MaxVoisinColorier == voisinCouleurMax && sommet.Voisin.Count() == GAGNANT.Voisin.Count()) // LE CRITERE 3
                        {

                            // Ajouter sommet a liste EgaliteEntreSommet + choisir au hasard Random
                            EgaliteEntreSommet.Add(sommet);
                            //Randomize le feur
                            Random random = new Random();
                            int randomInt = random.Next(0, EgaliteEntreSommet.Count);
                            GAGNANT = (Sommet)EgaliteEntreSommet[randomInt].Clone();
                        }
                        else // LE CRITERE 2
                        {
                            // Si oui, on remplace la variable de nombre maximal de voisin colorié
                            voisinCouleurMax = sommet.MaxVoisinColorier;
                            // Puis on copie l'objet du sommet actuel pour le donner au nouveau (tout attribut propri compris)
                            GAGNANT = (Sommet)sommet.Clone();
                        }

                    }

                }





            }

            for (int i = 0; i <= CouleursSommets.Max(); i++)
            {
                taverne.AjouterTable();
            }

            foreach (Client client in taverne.Clients)
            {
                Sommet sommetDuClient = graphe.DicoSOMMET[client];
                taverne.AjouterClientTable(client.Numero, sommetDuClient.Couleur);
            }
            sw.Stop();
                this.tempsExecution = sw.ElapsedMilliseconds;
            }
        }*/
}















/*
 
                int plusPetiteCouleur = 0;
                List<int> couleursVoisins = GAGNANT.Voisin.Where(v => v.Couleur != -1).Select(v => v.Couleur).ToList();

                while (couleursVoisins.Contains(plusPetiteCouleur))
                {
                    plusPetiteCouleur++;
                }

                GAGNANT.Couleur = plusPetiteCouleur;
                CouleursSommets.Add(plusPetiteCouleur);*/