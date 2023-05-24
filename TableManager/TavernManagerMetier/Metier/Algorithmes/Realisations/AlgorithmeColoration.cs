using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TavernManagerMetier.Metier.Algorithmes;
using TavernManagerMetier.Metier.Algorithmes.Graphes;
using TavernManagerMetier.Metier.Tavernes;
using TavernManagerMetier.Exceptions.Realisations.GestionDesTables;

public class AlgorithmeColoration : IAlgorithme
{
    private long tempsExecution = -1;

    public string Nom => "Coloration Amis";

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

        for(int i = 0; i < taverne.Tables.Length; i++)
        {
            taverne.Tables[i].Couleur = CouleursSommets[i];
        }

        for (int i = 0; i <= CouleursSommets.Max(); i++)
        {
            taverne.AjouterTable();
        }

        
        foreach (Client client in taverne.Clients)
        {
            int TableClientNbActuel = 0;

            Sommet sommetDuClient = graphe.DicoSOMMET[client];
            TableClientNbActuel += sommetDuClient.NbClients;
            try
            {
                Table table = sommetDuClient.GetTableAssocie(taverne);
                int test = sommetDuClient.GetTableAssocie(taverne).NombreClients;
                if (sommetDuClient.GetTableAssocie(taverne).NombreClients + sommetDuClient.NbClients > taverne.CapactieTables) // Si 
                
                taverne.AjouterClientTable(client.Numero, sommetDuClient.Couleur);
            }
            catch
            {
                taverne.AjouterTable();
                taverne.AjouterClientTable(client.Numero, sommetDuClient.Couleur+1);
            }
            
            /*if(client.Table.NombreClients > client.Table.Capacite)
            {
                
            }*/
            


        }
        sw.Stop();
        this.tempsExecution = sw.ElapsedMilliseconds;

    }
    /*if(client.Table.Clients.Length + sommetDuClient.NbClients > client.Table.Capacite)
            {
                taverne.AjouterTable();
                taverne.AjouterClientTable(client.Numero, sommetDuClient.Couleur + 1);
            }
            else
            {
                taverne.AjouterClientTable(client.Numero, sommetDuClient.Couleur);
            }

            if (taverne.Clients[client.Table.].Table.Clients.Length + sommetDuClient.NbClients > taverne.Clients[sommetDuClient.Couleur].Table.Capacite)
            {
                taverne.AjouterTable();
                taverne.AjouterClientTable(client.Numero, sommetDuClient.Couleur + 1);
            }
            else
            {
                taverne.AjouterClientTable(client.Numero, sommetDuClient.Couleur);
            }*/



















    /*public void Executer(Taverne taverne)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        // Création du graphe à partir de la taverne
        Graphe graphe = new Graphe(taverne);

        // Tri (pas de tri ici car on prend l'ordre dans lequel on donne)

        // Dictionnaire qui associe un Sommet à sa couleur
        Dictionary<Sommet, int> sommetEtLeurCouleur = new Dictionary<Sommet, int>();

        // Initialisation des sommets avec une couleur
        foreach (Sommet sommet in graphe.Sommets)
        {
            sommetEtLeurCouleur[sommet] = -1;
        }

        // Attribution des couleurs en fonction des contraintes de coloration croissante
        foreach (Sommet sommet in graphe.Sommets)
        {
            List<int> couleurVoisin = new List<int>();

            foreach (Sommet voisin in sommet.Voisin)
            {
                couleurVoisin.Add(sommetEtLeurCouleur[voisin]);
            }

            int couleur = 0;
            while (couleurVoisin.Contains(couleur) || GetNombreOccurrence(sommetEtLeurCouleur, couleur) >= taverne.CapactieTables)
            {
                couleur += 1;
            }

            sommetEtLeurCouleur[sommet] = couleur;
        }

        // Ajout des tables en fonction du nombre de couleurs utilisées
        for (int i = 0; i <= sommetEtLeurCouleur.Values.Max(); i++)
        {
            taverne.AjouterTable();
        }

        // Assignation des clients aux tables en fonction de leurs couleurs
        foreach (Client client in taverne.Clients)
        {
            Sommet sommetDuClient = graphe.DicoSOMMET[client];
            taverne.AjouterClientTable(client.Numero, sommetEtLeurCouleur[sommetDuClient]);
        }

        sw.Stop();
        tempsExecution = sw.ElapsedMilliseconds;
    }

    private int GetNombreOccurrence(Dictionary<Sommet, int> sommetEtLeurCouleur, int couleur)
    {
        int occurrence = 0;
        foreach (int numTable in sommetEtLeurCouleur.Values)
        {
            if (numTable == couleur)
                occurrence++;
        }
        return occurrence;
    }*/
}
/*Code de lorenzo

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

public class AlgorithmeColoration : IAlgorithme
{
    private long tempsExecution = -1;
    /// <summary>
    /// propriété pour le nom de l'algorithme
    /// </summary>
    public string Nom => "Coloration Amis";
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

        // Création du graphe à partir de la taverne
        Graphe graphe = new Graphe(taverne);

        // Tri (pas de tri ici car on prend l'ordre dans lequel on donne)

        // Dictionnaire qui associe un Sommet à sa couleur
        Dictionary<Sommet, int> sommetEtLeurCouleur = new Dictionary<Sommet, int>();

        // Initialisation des sommets avec une couleur
        foreach (Sommet sommet in graphe.Sommets)
        {
            sommetEtLeurCouleur[sommet] = -1;
        }

        // Attribution des couleurs en fonction des contraintes de coloration croissante
        foreach (Sommet sommet in graphe.Sommets)
        {
            List<int> couleurVoisin = new List<int>();

            foreach (Sommet voisin in sommet.Voisin)
            {
                couleurVoisin.Add(sommetEtLeurCouleur[voisin]);
            }

            int couleur = 0;
            while (couleurVoisin.Contains(couleur) || GetNombreOccurrence(sommetEtLeurCouleur, couleur) >= taverne.CapactieTables)
            {
                couleur += 1;
            }

            sommetEtLeurCouleur[sommet] = couleur;
        }

        // Ajout des tables en fonction du nombre de couleurs utilisées
        for (int i = 0; i <= sommetEtLeurCouleur.Values.Max(); i++)
        {
            taverne.AjouterTable();
        }

        // Assignation des clients aux tables en fonction de leurs couleurs
        foreach (Client client in taverne.Clients)
        {
            Sommet sommetDuClient = graphe.DicoSOMMET[client];
            taverne.AjouterClientTable(client.Numero, sommetEtLeurCouleur[sommetDuClient]);
        }

        sw.Stop();
        tempsExecution = sw.ElapsedMilliseconds;
    }

    /// <summary>
    /// Renvoie le nombre d'occurences d'une couleur dans un dictionnaire associant des sommets à des couleurs
    /// </summary>
    private int GetNombreOccurrence(Dictionary<Sommet, int> sommetEtLeurCouleur, int couleur)
    {
        int occurrence = 0;
        foreach (int numTable in sommetEtLeurCouleur.Values)
        {
            if (numTable == couleur)
                occurrence++;
        }
        return occurrence;
    }
}
*/