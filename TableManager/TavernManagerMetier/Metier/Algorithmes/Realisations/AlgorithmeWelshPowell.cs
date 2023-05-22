using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TavernManagerMetier.Metier.Tavernes;
using TavernManagerMetier.Metier.Algorithmes.Graphes;

namespace TavernManagerMetier.Metier.Algorithmes.Realisations
{
    public class AlgorithmeWelshPowell : IAlgorithme;
    {

        public string Nom => "Welsh Powell SA";

        public long TempsExecution => -1;
        public static void Execute(Taverne taverne)
            {
                List<Client> clients = taverne.Clients.ToList();
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
                    client.ChangerTable(table);
                }
            }
    }
}