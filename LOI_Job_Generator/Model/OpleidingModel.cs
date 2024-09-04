using LOI_Job_Generator.Core;
using LOI_Job_Generator.Database;
using LOI_Job_Generator.Interface;

namespace LOI_Job_Generator.Model
{
    public class OpleidingModel : IModel<OpleidingModel>
    {
        // Database velden
        public long iId { get; set; }
        public string tOpleiding { get; set; }

        public double ValueRandom { get; set; }

        public OpleidingModel()
        {
            ValueRandom = (double)JobGeneratorManager.RandomGenerator(0, 10) / 10;
        }

        public static void Create(OpleidingModel model)
        {
            DatabaseManager.ExecuteQuery($"INSERT INTO Opleidingen(tOpleiding) VALUES('{model.tOpleiding}')");
        }

        public static void Delete(int i)
        {
            DatabaseManager.ExecuteQuery($"DELETE FROM Opleidingen WHERE iID = {i}");
        }

        public static List<OpleidingModel> Read()
        {
            return DatabaseManager.SelectQuery<OpleidingModel>($"SELECT * FROM Opleidingen");
        }

        public static void Update(OpleidingModel model, int i)
        {
            DatabaseManager.ExecuteQuery($"UPDATE Opleidingen SET tOpleiding = '{model.tOpleiding}' WHERE iID = {i}");
        }
    }
}
