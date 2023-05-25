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
    public class AlgorithmeWelshPowell : IAlgorithme
    {
        private long tempsExecution = -1;

        /// <summary>
        /// propriété pour le nom de l'algorithme
        /// </summary>
        public string Nom => "UwU";
        /// <summary>
        /// propriété pour initialiser le temps d'execution à -1
        /// </summary>
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="taverne"></param>
        public void ExecuterAlgo(Taverne taverne)
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

                ListSommets = ListSommets.OrderByDescending(m => m.Voisin.Count()).ToList();

                foreach (Sommet sommet in ListSommets)
                {
                    sommet.Couleur = -1;
                }

                int couleur = 0;

                while (ListSommets.Any(sommet => sommet.Couleur == -1))
                {
                    foreach (Sommet sommet in ListSommets)
                    {
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
                for (int i = 0; i < CouleursSommets.Count; i++)
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}

