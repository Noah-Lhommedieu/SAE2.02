using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TavernManagerMetier.Metier.Algorithmes;
using TavernManagerMetier.Metier.Algorithmes.Graphes;
using TavernManagerMetier.Metier.Tavernes;


public class AlgorithmeColorationSA : IAlgorithme
{
    public string Nom => "Coloration SA";

    public long TempsExecution => -1;

    public void Executer(Taverne taverne)
    {
        Graphe graphe = new Graphe(taverne);

        List<Table> tables = taverne.Tables.ToList();
        List<Client> clients = taverne.Clients.ToList();
        clients.Sort((c1, c2) => c1.Numero.CompareTo(c2.Numero));

        foreach (Client client in clients)
        {
            bool placed = false;
            foreach (Table table in tables)
            {
                if (!table.EstPleine && table.EstValide)
                {
                    client.ChangerTable(table);
                    placed = true;
                    break;
                }
            }

            if (!placed)
            {
                taverne.AjouterTable();
                client.ChangerTable(tables.Last());   
            }
        }
    }
}

       
            
