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

        /// <summary>
        /// Nom de l'algorithme.
        /// </summary>
        public string Nom => "Dsatur";

        /// <summary>
        /// Temps d'exécution de l'algorithme.
        /// </summary>
        public long TempsExecution => tempsExecution;

        /// <summary>
        /// Exécute l'algorithme DSATUR sur une taverne donnée.
        /// </summary>
        /// <param name="taverne">La taverne sur laquelle appliquer l'algorithme.</param>
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

        /// <summary>
        /// Exécute l'algorithme DSATUR sur une taverne donnée.
        /// </summary>
        /// <param name="taverne">La taverne sur laquelle appliquer l'algorithme.</param>
        private void ExecuterAlgo(Taverne taverne)
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
                    // Sélection du sommet à colorier en utilisant les critères DSATUR
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

                // Ajout des tables en fonction du nombre de couleurs utilisées
                for (int i = 0; i <= graphe.Sommets.Max(s => s.Couleur); i++)
                {
                    taverne.AjouterTable();
                }

                // Assignation des clients aux tables en fonction de leurs couleurs
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
}
