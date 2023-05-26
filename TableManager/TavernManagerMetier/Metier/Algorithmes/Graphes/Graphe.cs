using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TavernManagerMetier.Metier.Tavernes;

namespace TavernManagerMetier.Metier.Algorithmes.Graphes
{
    /// <summary>
    /// Représente un graphe composé de sommets et d'arêtes.
    /// </summary>
    public class Graphe
    {
        private Dictionary<Client, Sommet> sommets;

        /// <summary>
        /// Obtient le dictionnaire des sommets du graphe.
        /// </summary>
        public Dictionary<Client, Sommet> DicoSOMMET
        {
            get { return sommets; }
        }

        /// <summary>
        /// Obtient la liste des sommets distincts du graphe.
        /// </summary>
        public List<Sommet> Sommets
        {
            get { return this.sommets.Values.Distinct().ToList<Sommet>(); }
        }

        /// <summary>
        /// Initialise une nouvelle instance de la classe Graphe.
        /// </summary>
        /// <param name="taverne">La taverne contenant les clients du graphe.</param>
        public Graphe(Taverne taverne)
        {
            sommets = new Dictionary<Client, Sommet>();

            // Ajout des sommets pour chaque client de la taverne
            foreach (Client client in taverne.Clients)
            {
                this.AjouterSommet(client, new Sommet());
            }

            // Ajout des arêtes entre les clients ennemis
            foreach (Client client in taverne.Clients)
            {
                foreach (Client ennemi in client.Ennemis)
                {
                    this.AjouterArete(client, ennemi);
                }
            }

            // Vérification des contraintes de capacité et de configuration des ennemis
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

        /// <summary>
        /// Ajoute un sommet au graphe pour le client spécifié.
        /// </summary>
        /// <param name="client">Le client associé au sommet.</param>
        /// <param name="sommet">Le sommet à ajouter.</param>
        public void AjouterSommet(Client client, Sommet sommet)
        {
            if (!this.sommets.ContainsKey(client))
            {
                this.sommets[client] = sommet;
                sommet.NbClients++;

                // Ajout récursif des sommets pour les amis du client
                foreach (Client ami in client.Amis)
                {
                    this.AjouterSommet(ami, sommet);
                }
            }
        }

        /// <summary>
        /// Ajoute une arête entre deux clients ennemis dans le graphe.
        /// </summary>
        /// <param name="client">Le premier client.</param>
        /// <param name="ennemi">Le deuxième client (ennemi du premier).</param>
        private void AjouterArete(Client client, Client ennemi)
        {
            Sommet s = sommets[ennemi];

            // Vérification pour éviter les doublons d'arêtes
            if (client != ennemi && !this.sommets[client].Voisin.Contains(s))
            {
                this.sommets[client].AjouterVoisin(s);
            }
        }
    }
}
