using LOI_Job_Generator.Database;
using LOI_Job_Generator.Events;
using LOI_Job_Generator.Model;
using LOI_Job_Generator.View;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;

namespace LOI_Job_Generator.Core
{
    public static class JobGeneratorManager
    {
        public static void InitialisatieDB()
        {
            List<PersoonModel> persoonModels = new List<PersoonModel>()
            {
                new PersoonModel() { tNaam = "Daan", iLeeftijd = 25 },
                new PersoonModel() { tNaam = "Emma", iLeeftijd = 32 },
                new PersoonModel() { tNaam = "Thomas", iLeeftijd = 41 },
                new PersoonModel() { tNaam = "Sophie", iLeeftijd = 22 },
                new PersoonModel() { tNaam = "Lars", iLeeftijd = 37 },
                new PersoonModel() { tNaam = "Lotte", iLeeftijd = 19 },
                new PersoonModel() { tNaam = "Bram", iLeeftijd = 28 },
                new PersoonModel() { tNaam = "Eva", iLeeftijd = 30 },
                new PersoonModel() { tNaam = "Lucas", iLeeftijd = 20 },
                new PersoonModel() { tNaam = "Lisa", iLeeftijd = 43 }

            };

            List<OpleidingModel> opleidingModels = new List<OpleidingModel>()
            {
                new OpleidingModel() { tOpleiding = "Geneeskunde" },
                new OpleidingModel() { tOpleiding = "Informatica" },
                new OpleidingModel() { tOpleiding = "Rechten" },
                new OpleidingModel() { tOpleiding = "Bedrijfskunde" },
                new OpleidingModel() { tOpleiding = "Psychologie" },
                new OpleidingModel() { tOpleiding = "Werktuigbouwkunde" },
                new OpleidingModel() { tOpleiding = "Communicatiewetenschap" },
                new OpleidingModel() { tOpleiding = "Economie" },
                new OpleidingModel() { tOpleiding = "Elektrotechniek" },
                new OpleidingModel() { tOpleiding = "Kunstgeschiedenis" }
            };

            List<BeroepModel> beroepModels = new List<BeroepModel>()
            {
                new BeroepModel() { tBeroep = "Verpleegkundige" },
                new BeroepModel() { tBeroep = "Softwareontwikkelaar" },
                new BeroepModel() { tBeroep = "Jurist" },
                new BeroepModel() { tBeroep = "Accountant" },
                new BeroepModel() { tBeroep = "Leraar" },
                new BeroepModel() { tBeroep = "Elektromonteur" },
                new BeroepModel() { tBeroep = "Marketingmanager" },
                new BeroepModel() { tBeroep = "Architect" },
                new BeroepModel() { tBeroep = "Psycholoog" },
                new BeroepModel() { tBeroep = "Politieagent" }
            };

            DatabaseManager.ExecuteQuery("DROP TABLE IF EXISTS Personen");
            DatabaseManager.ExecuteQuery("DROP TABLE IF EXISTS Opleidingen");
            DatabaseManager.ExecuteQuery("DROP TABLE IF EXISTS Beroepen");

            DatabaseManager.ExecuteQuery("CREATE TABLE IF NOT EXISTS Personen (iID INTEGER PRIMARY KEY AUTOINCREMENT, tNaam TEXT, iLeeftijd INTEGER, tBaan TEXT)");
            DatabaseManager.ExecuteQuery("CREATE TABLE IF NOT EXISTS Opleidingen (iID INTEGER PRIMARY KEY AUTOINCREMENT, tOpleiding TEXT)");
            DatabaseManager.ExecuteQuery("CREATE TABLE IF NOT EXISTS Beroepen (iID INTEGER PRIMARY KEY AUTOINCREMENT, tBeroep TEXT)");

            foreach (OpleidingModel opleidingModel in opleidingModels)
                OpleidingModel.Create(opleidingModel);

            foreach (BeroepModel beroepModel in beroepModels)
                BeroepModel.Create(beroepModel);

            foreach (PersoonModel persoonModel in persoonModels)
                PersoonModel.Create(persoonModel);

            MessageBox.Show("Database geinitialiseerd", "Database initialisatie", MessageBoxButton.OK, MessageBoxImage.Information);

            EventService.NotifyUpdateItemCollection();
        }

        public static int RandomGenerator(int min, int max)
        {
            Random random = new Random();

            return random.Next(min, max + 1);
        }
    }
}
