using LOI_Job_Generator.Core;
using LOI_Job_Generator.Database;
using LOI_Job_Generator.Interface;
using Microsoft.EntityFrameworkCore.Metadata;

namespace LOI_Job_Generator.Model
{
    public class BeroepModel : IModel<BeroepModel>
    {
        // Database velden
        public long iId { get; set; }
        public string tBeroep { get; set; }

        public double ValueRandom { get; set; }
        public double ValueBerekend { get; set; } = 0;

        public BeroepModel()
        {
            ValueRandom = (double)JobGeneratorManager.RandomGenerator(0, 10) / 10;
        }

        public static void Create(BeroepModel model)
        {
            DatabaseManager.ExecuteQuery($"INSERT INTO Beroepen(tBeroep) VALUES('{model.tBeroep}')");
        }

        public static void Delete(int i)
        {
            DatabaseManager.ExecuteQuery($"DELETE FROM Beroepen WHERE iID = {i}");
        }

        public static List<BeroepModel> Read()
        {
            return DatabaseManager.SelectQuery<BeroepModel>($"SELECT * FROM Beroepen");
        }

        public static void Update(BeroepModel model, int i)
        {
            DatabaseManager.ExecuteQuery($"UPDATE Beroepen SET tBeroep = '{model.tBeroep}' WHERE iID = {i}");
        }
    }
}
