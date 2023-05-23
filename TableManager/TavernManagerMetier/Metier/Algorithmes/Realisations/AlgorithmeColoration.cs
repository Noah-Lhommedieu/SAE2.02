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
