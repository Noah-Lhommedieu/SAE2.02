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
        /// <summary>
        /// dictionnaire sommet de type client
        /// </summary>
        private Dictionary<Client, Sommet> sommets;

        /// <summary>
       /// Obtient une liste des sommets uniques de ce graphe
        /// </summary>
        public List<Sommet> Sommets => this.sommets.Values.Distinct().ToList<Sommet>();

        /// <summary>
        /// Constructeur de la classe Graphe qui initialise le graphe à partir d'une taverne
        /// </summary>
        /// <param name="taverne">La taverne pour initialiser le graphe</param>
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

        /// <summary>
        /// Méthode permettant d'ajouter un sommet au graphe en associant un client à un sommet
        /// Cette méthode est récursive et est utilisée pour ajouter tous les clients et leurs amis au graphe
        /// </summary>
        /// <param name="client">Le client à associer au sommet</param>
        /// <param name="sommet">Le sommet auquel associer le client</param>
        private void AjouterSommet(Client client, Sommet sommet)
        {
            if (!this.sommets.ContainsKey(client))
            {
                this.sommets[client] = sommet;
                sommet.NbClients++;
                foreach (Client ami in client.Amis) this.AjouterSommet(ami, sommet);
            }
        }

        /// <summary>
        /// Méthode permettant d'ajouter une arête entre deux clients dans le graphe
        /// Cette méthode crée une relation entre les sommets correspondants aux clients
        /// </summary>
        /// <param name="client1">Le premier client de l'arête</param>
        /// <param name="client2">Le deuxième client de l'arête</param>
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
        
        /// <summary>
        /// méthode pour obtenir la valeur du sommet
        /// </summary>
        /// <param name="client">le client dont on va récuperer le sommet</param>
        /// <returns></returns>
        public Sommet GetSommet(Client client)
        {

            Sommet value;
            sommets.TryGetValue(client,out value);
            return value;
        }

    }
}
