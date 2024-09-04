using LOI_Job_Generator.Database;
using LOI_Job_Generator.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOI_Job_Generator.Model
{
    public class PersoonModel : IModel<PersoonModel>
    {
        public long iId { get; set; }
        public string tNaam { get; set;}
        public long iLeeftijd { get; set; }
        public string tBaan { get; set; }

        public static void Create(PersoonModel model)
        {
            DatabaseManager.ExecuteQuery($"INSERT INTO Personen(tNaam, iLeeftijd) VALUES('{model.tNaam}', '{model.iLeeftijd}')");
        }

        public static void Delete(int i)
        {
            DatabaseManager.ExecuteQuery($"DELETE FROM Personen WHERE iID = {i}");
        }

        public static List<PersoonModel> Read()
        {
            return DatabaseManager.SelectQuery<PersoonModel>($"SELECT * FROM personen");
        }

        public static void Update(PersoonModel model, int i)
        {
            DatabaseManager.ExecuteQuery($"UPDATE Personen SET tNaam = '{model.tNaam}', iLeeftijd = '{model.iLeeftijd}', tBaan = '{model.tBaan}' WHERE iID = {i}");
        }
    }
}
