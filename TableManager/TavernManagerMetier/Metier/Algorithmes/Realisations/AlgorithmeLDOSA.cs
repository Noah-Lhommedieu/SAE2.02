using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TavernManagerMetier.Metier.Algorithmes.Graphes;
using TavernManagerMetier.Metier.Tavernes;

namespace TavernManagerMetier.Metier.Algorithmes.Realisations
{
    public class AlgorithmeLDOSA : IAlgorithme
    {
        /// <summary>
        /// propriété pour le nom de l'algorithme
        /// </summary>
        public string Nom => "LDO SA";
        /// <summary>
        /// propriété pour initialiser le temps d'execution à -1
        /// </summary>
        public long TempsExecution => -1;

        public void Executer(Taverne taverne)
        {
            List<Table> tables = taverne.Tables.ToList();
            List<Client> clients = taverne.Clients.ToList();

            // Trier les clients en fonction du nombre d'ennemis
            clients.Sort((c1, c2) => c2.Ennemis.Count.CompareTo(c1.Ennemis.Count));

            // Répartir les clients sur les tables en respectant les contraintes
            foreach (Client client in clients)
            {
                Table table = GetTableWithFewestEnemies(client, tables);
                table.AjouterClient(client);
            }
        }

        private Table GetTableWithFewestEnemies(Client client, List<Table> tables)
        {
            Table minTable = null;
            int minEnemies = int.MaxValue;

            foreach (Table table in tables)
            {
                int enemies = CountEnemiesAtTable(client, table);

                if (enemies < minEnemies)
                {
                    minTable = table;
                    minEnemies = enemies;
                }
            }

            return minTable;
        }

        private int CountEnemiesAtTable(Client client, Table table)
        {
            int count = 0;

            foreach (Client otherClient in table.Clients)
            {
                if (client.EstEnnemisAvec(otherClient))
                {
                    count++;
                }
            }

            return count;
        }
    }
}
