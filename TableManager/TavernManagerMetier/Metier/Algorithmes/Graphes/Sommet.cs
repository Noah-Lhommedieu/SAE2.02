using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TavernManagerMetier.Metier.Tavernes;

namespace TavernManagerMetier.Metier.Algorithmes.Graphes
{
    /// <summary>
    /// Représente un sommet dans un graphe.
    /// </summary>
    public class Sommet : ICloneable
    {
        private int maxVoisinColorier = 0;
        /// <summary>
        /// Obtient ou définit le nombre maximal de voisins coloriés.
        /// </summary>
        public int MaxVoisinColorier
        {
            get { return maxVoisinColorier; }
            set { maxVoisinColorier = value; }
        }

        private int couleur;
        /// <summary>
        /// Obtient ou définit la couleur du sommet.
        /// </summary>
        public int Couleur
        {
            get { return couleur; }
            set { couleur = value; }
        }

        private int nbClients;
        /// <summary>
        /// Obtient ou définit le nombre de clients associés au sommet.
        /// </summary>
        public int NbClients
        {
            get { return nbClients; }
            set { nbClients = value; }
        }

        public Client clientSommet { get; set; }

        private List<Sommet> voisins;
        /// <summary>
        /// Obtient ou définit la liste des sommets voisins.
        /// </summary>
        public List<Sommet> Voisin
        {
            get { return voisins; }
            set { voisins = value; }
        }

        /// <summary>
        /// Initialise une nouvelle instance de la classe Sommet.
        /// </summary>
        public Sommet()
        {
            voisins = new List<Sommet>();
            NbClients = 0;
        }

        /// <summary>
        /// Ajoute un sommet voisin.
        /// </summary>
        /// <param name="sommet">Le sommet voisin à ajouter.</param>
        public void AjouterVoisin(Sommet sommet)
        {
            Voisin.Add(sommet);
        }

        /// <summary>
        /// Récupère la table associée au sommet dans la taverne spécifiée.
        /// </summary>
        /// <param name="taverne">La taverne contenant les tables.</param>
        /// <returns>La table associée au sommet, ou null si aucune table n'est associée.</returns>
        public Table GetTableAssocie(Taverne taverne)
        {
            // Recherche de la table associée à la couleur du sommet
            foreach (Table table in taverne.Tables)
            {
                if (table.Couleur == this.Couleur)
                {
                    return table;
                }
            }

            return null; // Aucune table associée trouvée
        }

        /// <summary>
        /// Récupère le client associé au sommet.
        /// </summary>
        /// <returns>Le client associé au sommet.</returns>
        public Client GetClient()
        {
            return this.clientSommet;
        }

        /// <summary>
        /// Effectue une copie profonde du sommet.
        /// </summary>
        /// <returns>Une copie du sommet.</returns>
        public object Clone()
        {
            Sommet x = new Sommet();
            x.clientSommet = this.clientSommet;
            x.Voisin = this.voisins;
            x.Couleur = this.couleur;
            x.NbClients = this.NbClients;
            x.MaxVoisinColorier = this.maxVoisinColorier;
            return x;
        }
    }
}
