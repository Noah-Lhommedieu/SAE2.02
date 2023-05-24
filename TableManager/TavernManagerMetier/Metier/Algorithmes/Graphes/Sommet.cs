using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TavernManagerMetier.Metier.Tavernes;

namespace TavernManagerMetier.Metier.Algorithmes.Graphes
{
    public class Sommet : ICloneable
    {


        private int maxVoisinColorier = 0;
        public int MaxVoisinColorier
        {
            get { return maxVoisinColorier; }
            set { maxVoisinColorier = value; }
        }
        private int couleur;
        public int Couleur
        {
            get { return couleur; }
            set { couleur = value; }
        }

        public int NbClients
        {
            get { return nbClients; }
            set { nbClients = value; }
        }
        private int nbClients;


        private Client client;

        public Client clientSommet { get; set; }

        private List<Sommet> voisins;
        public List<Sommet> Voisin
        {

            get { return voisins; }
            set { voisins = value; }

        }

        public Sommet()
        {

            voisins = new List<Sommet>();
            NbClients = 0;
        }

        public void AjouterVoisin(Sommet sommet)
        {

            Voisin.Add(sommet);

        }
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

        public Client GetClient()
        {
            return this.client;
        }
        public object Clone()
        {
            Sommet x = new Sommet();
            x.clientSommet = this.clientSommet;

            x.Voisin = this.voisins;
            x.Couleur = this.couleur;
            x.NbClients = this.NbClients;
            x.MaxVoisinColorier = this.maxVoisinColorier;
            
            return x; 
            //return this.MemberwiseClone();
        }

    }
}
