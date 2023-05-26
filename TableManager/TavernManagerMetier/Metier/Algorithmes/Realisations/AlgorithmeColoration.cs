using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TavernManagerMetier.Metier.Algorithmes;
using TavernManagerMetier.Metier.Algorithmes.Graphes;
using TavernManagerMetier.Metier.Tavernes;
using TavernManagerMetier.Exceptions.Realisations.GestionDesTables;
using System.Windows;

public class AlgorithmeColoration : IAlgorithme
{
    private long tempsExecution = -1;

    /// <summary>
    /// Nom de l'algorithme.
    /// </summary>
    public string Nom => "Coloration Amis";

    /// <summary>
    /// Temps d'exécution de l'algorithme.
    /// </summary>
    public long TempsExecution => tempsExecution;

    /// <summary>
    /// Exécute l'algorithme de coloration des sommets en fonction des contraintes d'amitié sur une taverne donnée.
    /// </summary>
    /// <param name="taverne">La taverne sur laquelle appliquer l'algorithme.</param>
    public void Executer(Taverne taverne)
    {
        try
        {
            ExecuterAlgo(taverne);
        }
        catch (Exception ex)
        {
            this.tempsExecution = -1;
            MessageBox.Show(ex.Message);
        }
    }

    /// <summary>
    /// Exécute l'algorithme de coloration des sommets en fonction des contraintes d'amitié sur une taverne donnée.
    /// </summary>
    /// <param name="taverne">La taverne sur laquelle appliquer l'algorithme.</param>
    private void ExecuterAlgo(Taverne taverne)
    {
        try
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            // Création du graphe à partir de la taverne
            Graphe graphe = new Graphe(taverne);

            // Dictionnaire qui associe un Sommet à sa couleur
            Dictionary<Sommet, int> sommetEtLeurCouleur = new Dictionary<Sommet, int>();

            List<int> ClientA1Table = new List<int>();
            ClientA1Table.Add(0);

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
                while (couleurVoisin.Contains(couleur) || ClientA1Table[couleur] + sommet.NbClients > taverne.CapactieTables)
                {
                    couleur += 1;
                    ClientA1Table.Add(0);
                }

                sommetEtLeurCouleur[sommet] = couleur;
                ClientA1Table[couleur] += sommet.NbClients;
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
        catch (Exception ex)
        {
            throw ex;
        }
    }
}
