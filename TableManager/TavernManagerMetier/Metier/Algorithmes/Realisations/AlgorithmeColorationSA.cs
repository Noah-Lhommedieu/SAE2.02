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








