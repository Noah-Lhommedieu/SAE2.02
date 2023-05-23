using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TavernManagerMetier.Metier.Algorithmes.Graphes;
using TavernManagerMetier.Metier.Tavernes;

namespace TavernManagerMetier.Metier.Algorithmes.Realisations
{
    public class AlgorithmeDSATURSA: IAlgorithme 
    {
        private long tempsExecution = -1;
        /// <summary>
        /// propriété pour le nom de l'algorithme
        /// </summary>
        public string Nom => "Dsatur SA";
        /// <summary>
        /// propriété pour initialiser le temps d'execution à -1
        /// </summary>
        public long TempsExecution => tempsExecution;

        

        public void Executer(Taverne taverne)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Graphe graphe = new Graphe(taverne);


            List<int> CouleursSommets = new List<int>();
            List<Sommet> ListSommets = new List<Sommet>();
            int couleur = 0;

            foreach (Sommet sommet in graphe.Sommets)
            {
                ListSommets.Add(sommet);
            }

            foreach (Sommet sommet in ListSommets)
            {
                sommet.Couleur = -1;
            }



            while (ListSommets.Any(sommet => sommet.Couleur == -1))
            {
                int voisinMax = 0;
                int voisinCouleurMax = 0;
                int nbVoisin = 0;
                List<Sommet> voisinDuSommet = new List<Sommet>();   
                List<Sommet> EgaliteEntreSommet = new List<Sommet>();
                Sommet GAGNANT = new Sommet();


                foreach (Sommet sommet in ListSommets)
                {
                    nbVoisin = sommet.Voisin.Count; // Nombre de voisin du sommet
                    voisinDuSommet.Clear(); // Clear pour les futur voisin du nouveau sommet (foreach) listé dedans
                    for (int i = 0; i < nbVoisin; i++) // Pour tous les voisin du sommet
                    {
                        voisinDuSommet.Add(sommet.Voisin[i]); // On ajoute a la liste le voisin a la liste pour listé tous les voisins du sommet
                    }
                    // A PARTIR DE LA ON A TOUS LES VOISINS DU SOMMET


                    //On attribut a sommet actuel, le nombre de ses voisin qui possède une couleur
                    sommet.MaxVoisinColorier = voisinDuSommet.Count(nbVoisinACouleur => nbVoisinACouleur.Couleur != -1); 
                    
                    // On vérifie que le sommet actuel possède plus de voisin colorié que le sommet précédent
                    if (sommet.MaxVoisinColorier > voisinCouleurMax)
                    {
                       
                        // Si oui, on remplace la variable de nombre maximal de voisin colorié
                        voisinCouleurMax = sommet.MaxVoisinColorier;
                        // Puis on copie l'objet du sommet actuel pour le donner au nouveau (tout attribut propri compris)
                        GAGNANT = (Sommet)sommet.Clone();


                    }
                    // Si même nombre voisin colorié, if GAGNANT a plus de voisin que sommet
                    else if (sommet. == voisinCouleurMax)
                    {
                        // Si oui, on remplace la variable de nombre maximal de voisin colorié
                        voisinCouleurMax = sommet.MaxVoisinColorier;
                        // Puis on copie l'objet du sommet actuel pour le donner au nouveau (tout attribut propri compris)
                        GAGNANT = (Sommet)sommet.Clone();


                    }
                    // Si même nombre voisin colorié + Si même nombre voisin
                    else if (sommet.MaxVoisinColorier == voisinCouleurMax && )
                    {
                        // Ajouter sommet a liste EgaliteEntreSommet + choisir au hasard Random
                    }


                    


                    if (sommet.Voisin.Count > voisinMax)
                    {
                        voisinMax = sommet.Voisin.Count;


                    }


                }



                
            }






            for (int i = 0; i <= CouleursSommets.Max(); i++)
            {
                taverne.AjouterTable();
            }

            foreach (Client client in taverne.Clients)
            {
                Sommet sommetDuClient = graphe.DicoSOMMET[client];
                taverne.AjouterClientTable(client.Numero, sommetDuClient.Couleur);
            }

            sw.Stop();
            this.tempsExecution = sw.ElapsedMilliseconds;
        }
    }
}
