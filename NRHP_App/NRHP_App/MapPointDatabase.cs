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

        //Methods for searching the name of a point.
        //Multiple methods were created because of the complexity required to implement a recursive function with the predicates and expressions that the sql commands require.
        public Task<List<MapPoint>> SearchNameAsync(string firstWord)
        {
            return _database.Table<MapPoint>()
                            .Where(mapPoint => mapPoint.Name.ToLower().Contains(firstWord))
                            .ToListAsync();
        }

        public Task<List<MapPoint>> SearchNameAsync(string firstWord, string secondWord)
        {
            return _database.Table<MapPoint>()
                            .Where(mapPoint => mapPoint.Name.ToLower().Contains(firstWord) && mapPoint.Name.ToLower().Contains(secondWord))
                            .ToListAsync();
        }

        public Task<List<MapPoint>> SearchNameAsync(string firstWord, string secondWord, string thirdWord)
        {
            return _database.Table<MapPoint>()
                            .Where(mapPoint => mapPoint.Name.ToLower().Contains(firstWord) && mapPoint.Name.ToLower().Contains(secondWord) && mapPoint.Name.ToLower().Contains(thirdWord))
                            .ToListAsync();
        }

        public Task<List<MapPoint>> SearchNameAsync(string firstWord, string secondWord, string thirdWord, string fourthWord)
        {
            return _database.Table<MapPoint>()
                            .Where(mapPoint => mapPoint.Name.ToLower().Contains(firstWord) && mapPoint.Name.ToLower().Contains(secondWord) && mapPoint.Name.ToLower().Contains(thirdWord) && mapPoint.Name.ToLower().Contains(fourthWord))
                            .ToListAsync();
        }
    }
}