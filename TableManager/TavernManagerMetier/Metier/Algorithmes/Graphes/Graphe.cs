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

        public Dictionary<Client, Sommet> DicoSOMMET
        {
            get { return sommets; }
        }
        public List<Sommet> Sommets
        {
            get { return this.sommets.Values.Distinct().ToList<Sommet>(); }
        }
        public Graphe(Taverne taverne)
        {
            sommets = new Dictionary<Client, Sommet>();
            foreach (Client client in taverne.Clients)
            {
                this.AjouterSommet(client, new Sommet());
            }
            foreach (Client client in taverne.Clients)
            {
                foreach (Client ennemie in client.Ennemis)
                {
                    this.AjouterArete(client, ennemie);
                }
            }
            foreach (Client client in taverne.Clients)
            {
                if (sommets[client].NbClients > taverne.CapactieTables)
                {
                    throw new Exception("Capacité de la table saturée par les amis.");
                }

                if (client.Ennemis.Any(c => this.sommets[c] == this.sommets[client]))
                {
                    throw new Exception("Taverne impossible.");
                }
            }
        }
        public void AjouterSommet(Client client, Sommet sommet)
        {
            if (!this.sommets.ContainsKey(client))
            {
                this.sommets[client] = sommet;
                sommet.NbClients++;
                foreach (Client ami in client.Amis) this.AjouterSommet(ami, sommet);
            }
        }
        private void AjouterArete(Client client, Client ennemi)
        {
            Sommet s = sommets[ennemi];
            if (client != ennemi)
            {
                if (!this.sommets[client].Voisin.Contains(s))
                    this.sommets[client].AjouterVoisin(s);
            }
        }
    }
}