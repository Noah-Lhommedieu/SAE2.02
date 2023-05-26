using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TavernManagerMetier.Metier.Tavernes;
using TavernManagerMetier.Metier.Algorithmes.Graphes;
using System.Diagnostics;
using System.Net.Sockets;

namespace TavernManagerMetier.Metier.Algorithmes.Realisations
{
    /// <summary>
    /// Implémentation de l'algorithme Welsh-Powell pour la coloration de graphes.
    /// </summary>
    public class AlgorithmeWelshPowell : IAlgorithme
    {
        private long tempsExecution = -1;

        /// <summary>
        /// Nom de l'algorithme.
        /// </summary>
        public string Nom => "WelshPowell";

        /// <summary>
        /// Temps d'exécution de l'algorithme.
        /// </summary>
        public long TempsExecution => tempsExecution;

        /// <summary>
        /// Exécute l'algorithme Welsh-Powell sur une taverne donnée.
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
        /// Exécute l'algorithme Welsh-Powell sur une taverne donnée.
        /// </summary>
        /// <param name="taverne">La taverne sur laquelle appliquer l'algorithme.</param>
        private void ExecuterAlgo(Taverne taverne)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                Graphe graphe = new Graphe(taverne);

                List<int> CouleursSommets = new List<int>();
                List<Sommet> ListSommets = new List<Sommet>();

                List<int> ClientA1Table = new List<int>();
                ClientA1Table.Add(0);

                foreach (Sommet sommet in graphe.Sommets)
                {
                    ListSommets.Add(sommet);
                }

                // Tri des sommets dans l'ordre décroissant du nombre de voisins
                ListSommets = ListSommets.OrderByDescending(m => m.Voisin.Count()).ToList();

                foreach (Sommet sommet in ListSommets)
                {
                    sommet.Couleur = -1;
                }

                int couleur = 0;

                // Attribution des couleurs aux sommets
                while (ListSommets.Any(sommet => sommet.Couleur == -1))
                {
                    foreach (Sommet sommet in ListSommets)
                    {
                        // Vérification si le sommet peut être attribué à la couleur actuelle
                        if ((sommet.Couleur == -1 && sommet.Voisin.All(voisin => voisin.Couleur != couleur)) && ClientA1Table[couleur] + sommet.NbClients <= taverne.CapactieTables)
                        {
                            sommet.Couleur = couleur;
                            ClientA1Table[couleur] += sommet.NbClients;
                        }
                    }
                    couleur++;
                    ClientA1Table.Add(0);
                    CouleursSommets.Add(couleur);
                }

                // Ajout des tables à la taverne selon le nombre de couleurs utilisées
                for (int i = 0; i < CouleursSommets.Count; i++)
                {
                    taverne.AjouterTable();
                }

                // Attribution des clients aux tables en fonction de la couleur de leur sommet correspondant
                foreach (Client client in taverne.Clients)
                {
                    Sommet sommetDuClient = graphe.DicoSOMMET[client];
                    taverne.AjouterClientTable(client.Numero, sommetDuClient.Couleur);
                }

                sw.Stop();
                this.tempsExecution = sw.ElapsedMilliseconds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
