using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TavernManagerMetier.Metier.Tavernes;

namespace TavernManagerMetier.Metier.Algorithmes.Graphes
{
    public class Sommet
    {

        public int? Couleur { get; set; }

        int Nbclients;
        public int NbClients { get { return Nbclients; } set { Nbclients = value; } }

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
        
        public Client GetClient()
        {
            return this.client;
        }

        
    }
}
