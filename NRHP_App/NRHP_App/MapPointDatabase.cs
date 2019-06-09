using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;

namespace NRHP_App
{
    public class MapPointDatabase
    {
        readonly SQLiteAsyncConnection _database;

        public MapPointDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<MapPoint>();
        }

        public Task<List<MapPoint>> GetPointsAsync(double TopLatitude, double BottomLatitude, double RightLongitude, double LeftLongitude)
        {
            return _database.Table<MapPoint>()
                            .Where(mapPoint => mapPoint.Latitude <= TopLatitude && mapPoint.Latitude >= BottomLatitude && mapPoint.Longitude <= RightLongitude && mapPoint.Longitude >= LeftLongitude)
                            .ToListAsync();
        }

        public Task<MapPoint> GetRefNumAsync(string Name, double Latitude, double Longitude)
        {
            return _database.Table<MapPoint>()
                            .Where(mapPoint => mapPoint.Name.Equals(Name) && mapPoint.Latitude == Latitude && mapPoint.Longitude == Longitude)
                            .FirstOrDefaultAsync();
        }

        public Task<List<MapPoint>> SearchPointsNameAsync(string searchQuery)
        {
            return _database.Table<MapPoint>()
                            .Where(mapPoint => mapPoint.Name.ToLower().Contains(searchQuery))
                            .ToListAsync();
        }

        public Task<MapPoint> SearchPointsRefNumAsync(string searchQuery)
        {
            return _database.Table<MapPoint>()
                            .Where(mapPoint => mapPoint.RefNum.Equals(searchQuery))
                            .FirstOrDefaultAsync();
        }
    }
}