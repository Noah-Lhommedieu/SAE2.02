using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TavernManagerMetier.Metier.Algorithmes;
using TavernManagerMetier.Metier.Algorithmes.Graphes;
using TavernManagerMetier.Metier.Tavernes;

using TavernManagerMetier.Exceptions.Realisations.GestionDesTables;
using System.Diagnostics;
using System.Net.Sockets;

public class AlgorithmeColorationSA : IAlgorithme
{
    private long tempsExecution = -1;
    /// <summary>
    /// propriété pour le nom de l'algorithme
    /// </summary>
    public string Nom => "Coloration SA";
    /// <summary>
    /// propriété pour initialiser le temps d'execution à -1
    /// </summary>
    public long TempsExecution => tempsExecution;
    

    /// <summary>
    /// Execution de l'algorithme Coloration
    /// création du graphe et d'un dictionnaire, 
    /// </summary>
    /// <param name="taverne"></param>
    public void Executer(Taverne taverne)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();
        Graphe graphe = new Graphe(taverne);


        List<int> CouleursSommets = new List<int>();
        List<Sommet> ListSommets = new List<Sommet>();  
        int couleur = 0;
        foreach(Sommet sommet in graphe.Sommets)
            {
            ListSommets.Add(sommet);
        }

        foreach (Sommet sommet in ListSommets)
        {
            sommet.Couleur = -1;
        }

        foreach (Sommet sommet in ListSommets)
        {
            couleur = 0;
            bool estEnnemi = false;
            while (!estEnnemi)
            {
                estEnnemi = true;
                foreach (Sommet voisin in sommet.Voisin)
                {
                    if (voisin.Couleur == couleur)
                    {
                        estEnnemi = false;
                    }
                }
                if (estEnnemi && sommet.Couleur == -1)
                {
                    sommet.Couleur = couleur;
                    CouleursSommets.Add(couleur);
                }
                else
                {
                    couleur += 1;
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



/*List<Table> tables = taverne.Tables.ToList();
       List<Client> clients = taverne.Clients.ToList();

       Dictionary<int, List<Client>> clientsParCouleur = new Dictionary<int, List<Client>>();

       List<Sommet> sommets = graphe.Sommets;

       // Trie les sommets par ordre croissant du nombre de voisins
       sommets.Sort((s1, s2) => s1.Voisin.Count.CompareTo(s2.Voisin.Count));

       foreach (Sommet sommet in sommets)
       {
           // Parcours des voisins déjà colorés pour trouver la première couleur disponible
           HashSet<int> couleursVoisins = new HashSet<int>();
           foreach (Sommet voisin in sommet.Voisin)
           {
               if (voisin.Couleur.HasValue)
               {
                   couleursVoisins.Add(voisin.Couleur.Value);
               }
           }

           // Affecte la première couleur disponible au sommet
           int prochaineCouleur = 1;
           while (couleursVoisins.Contains(prochaineCouleur))
           {
               prochaineCouleur++;
           }
           sommet.Couleur = prochaineCouleur;
       }

       // Attribution des clients aux tables en fonction de la couleur des sommets
       foreach (Client client in clients)
       {
           Sommet sommet = graphe.GetSommet(client);

           int couleur = sommet?.Couleur ?? 0;
           // Si la couleur n'est pas définie, on utilise la couleur 0 par défaut

           if (!clientsParCouleur.ContainsKey(couleur))
           {
               clientsParCouleur[couleur] = new List<Client>();
           }
           clientsParCouleur[couleur].Add(client);
       }

       foreach (Table table in tables)
       {
           int couleurTable = table.Numero % clientsParCouleur.Count + 1; // Choix de la couleur en fonction du numéro de la table
           List<Client> clientsCouleur = clientsParCouleur[couleurTable];

           foreach (Client client in clientsCouleur)
           {
               if (!table.EstPleine && !client.EstAvecUnEnnemis)
               {
                   table.AjouterClient(client);
               }
           }
       }*/



/*foreach (Client client in clients)
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
        client.Table.AjouterClient(client)  
    }
}*/





