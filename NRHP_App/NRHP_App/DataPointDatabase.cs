using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;

namespace NRHP_App
{
    public class DataPointDatabase
    {
        readonly SQLiteAsyncConnection _database;

        public DataPointDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
        }

        public Task<List<DataPoint>> GetPointsAsync(double TopLatitude, double BottomLatitude, double RightLongitude, double LeftLongitude)
        {
            return _database.Table<DataPoint>()
                            .Where(dataPoint => dataPoint.Latitude <= TopLatitude && dataPoint.Latitude >= BottomLatitude && dataPoint.Longitude <= RightLongitude && dataPoint.Longitude >= LeftLongitude)
                            .ToListAsync();
        }

        public Task<DataPoint> GetPointAsync(int RefNum)
        {
            return _database.Table<DataPoint>()
                            .Where(dataPoint => dataPoint.RefNum == RefNum)
                            .FirstOrDefaultAsync();
        }
    }
}