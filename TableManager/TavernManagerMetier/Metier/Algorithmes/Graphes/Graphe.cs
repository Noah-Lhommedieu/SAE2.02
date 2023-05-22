using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TavernManagerMetier.Metier.Tavernes;

namespace TavernManagerMetier.Metier.Algorithmes.Graphes
{
    public class Graphe
    {
        private Dictionary<Client, Sommet> sommets;


        public List<Sommet> Sommets => this.sommets.Values.Distinct().ToList<Sommet>();


        public Graphe(Taverne taverne)
        {
            sommets = new Dictionary<Client, Sommet>();

            foreach (Client client in sommets.Keys)
            {
                sommets.Add(client, new Sommet());
            }

            foreach (Client client in sommets.Keys)
            {
                foreach(Client clients in client.Ennemis)
                {
                    this.AjouterArete(client, clients);
                }
            }

        }


        /*private void AjouterSommet(Client client)
        {
            if (!(this.sommets.ContainsKey(client)))
            {
                this.sommets.Add(client, new Sommet());
            }
        }*/

        private void AjouterSommet(Client client, Sommet sommet)
        {
            if (!this.sommets.ContainsKey(client))
            {
                this.sommets[client] = sommet;
                sommet.NbClients++;
                foreach (Client ami in client.Amis) this.AjouterSommet(ami, sommet);
            }
        }

        public void AjouterArete(Client client1, Client client2)
        {
            Sommet sommetClient1 = GetSommet(client1);
            Sommet sommetClient2 = GetSommet(client2);

            if (sommetClient1 != null && sommetClient2 != null)
            {
                sommetClient1.AjouterVoisin(sommetClient2);
                sommetClient2.AjouterVoisin(sommetClient1);
            }
        }

        private Sommet GetSommet(Client client)
        {
            foreach (Sommet sommet in Sommets)
            {
                if (sommet.clientSommet == client)
                {
                    return sommet;
                }
            }
            return null;
        }
        /*
        private Sommet GetSommets(Client client)
        {

            Sommet value;
            return sommets.TryGetValue(client,out Sommet sommet);
        }*/

    }
}
