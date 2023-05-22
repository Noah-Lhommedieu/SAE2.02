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
    public class AlgorithmeChacunSaTable : IAlgorithme
    {
        /// <summary>
        /// propriété pour le nom de l'algorithme
        /// </summary>
        public string Nom => "Chacun sa table";
        private long tempsExecution = -1;

        /// <summary>
        /// propriété pour initialiser le temps d'execution à -1
        /// </summary>
        public long TempsExecution => -1;
        

        /// <summary>
        /// Méthode Executer, on créé un nouveau graphe, et à l'aide d'une boucle on ajoute une table et un client à sa table à chaque tour de boucle
        /// </summary>
        /// <param name="taverne">on rentre en paramètre une taverne de Type taverne</param>
        public void Executer(Taverne taverne)
        {
            Stopwatch sw = Stopwatch.StartNew();
            sw.Start();
            Graphe graphe = new Graphe(taverne);
            
            
            for (int i = 0; i < taverne.Clients.Count(); i++)
            {
                taverne.AjouterTable();
                taverne.AjouterClientTable(i, i);
            }
            sw.Stop();
            this.tempsExecution = sw.ElapsedMilliseconds;
        }
    }
}
