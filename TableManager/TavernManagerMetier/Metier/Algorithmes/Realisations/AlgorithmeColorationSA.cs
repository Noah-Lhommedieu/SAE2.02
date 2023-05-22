using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TavernManagerMetier.Metier.Algorithmes;
using TavernManagerMetier.Metier.Algorithmes.Graphes;
using TavernManagerMetier.Metier.Tavernes;

using TavernManagerMetier.Exceptions.Realisations.GestionDesTables;



public class AlgorithmeColorationSA : IAlgorithme
{
    /// <summary>
    /// propriété pour le nom de l'algorithme
    /// </summary>
    public string Nom => "Coloration SA";
    /// <summary>
    /// propriété pour initialiser le temps d'execution à -1
    /// </summary>
    public long TempsExecution => -1;

    /// <summary>
    /// Execution de l'algorithme Coloration
    /// création du graphe et d'un dictionnaire, 
    /// </summary>
    /// <param name="taverne"></param>
    public void Executer(Taverne taverne)
    {
        Graphe graphe = new Graphe(taverne);

        List<Table> tables = taverne.Tables.ToList();
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
        }
    }


}




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





