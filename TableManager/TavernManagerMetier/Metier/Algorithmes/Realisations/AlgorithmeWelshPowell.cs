using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TavernManagerMetier.Metier.Tavernes;
using TavernManagerMetier.Metier.Algorithmes.Graphes;
using System.Diagnostics;

namespace TavernManagerMetier.Metier.Algorithmes.Realisations
{
    public class AlgorithmeWelshPowell : IAlgorithme
    {
        private long tempsExecution = -1;

        /// <summary>
        /// propriété pour le nom de l'algorithme
        /// </summary>
        public string Nom => "Welsh Powell Ami";
        /// <summary>
        /// propriété pour initialiser le temps d'execution à -1
        /// </summary>
        public long TempsExecution => tempsExecution;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taverne"></param>
        public void Executer(Taverne taverne)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Graphe graphe = new Graphe(taverne);

            List<int> CouleursSommets = new List<int>();
            List<Sommet> ListSommets = new List<Sommet>();

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
                    if (sommet.Couleur == -1 && sommet.Voisin.All(voisin => voisin.Couleur != couleur))
                    {
                        sommet.Couleur = couleur;

                    }
                }

                couleur++;
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
        private int GetNombreOccurrence(Dictionary<Sommet, int> sommetEtLeurCouleur, int couleur)
        {
            int occurrence = 0;
            foreach (int numTable in sommetEtLeurCouleur.Values)
            {
                if (numTable == couleur)
                    occurrence++;
            }
            return occurrence;
        }

    }
}











/*List<Client> clients = taverne.Clients.ToList();
List<Table> tables = taverne.Tables.ToList();
int capacity = taverne.CapactieTables;

// Trier les clients par ordre décroissant du nombre d'ennemis
clients.Sort((c1, c2) => c2.Ennemis.Count.CompareTo(c1.Ennemis.Count));

foreach (Client client in clients)
{
    // Rechercher une table avec suffisamment de places et sans ennemis
    Table table = tables.FirstOrDefault(t => t.NombreClients < capacity && !t.Clients.Any(c => c.EstEnnemisAvec(client)));

    // Si aucune table n'est trouvée, créer une nouvelle table
    if (table == null)
    {
        table = new Table(capacity, tables.Count);
        tables.Add(table);
    }

    // Assigner le client à la table
    client.ChangerTable(table);*/

