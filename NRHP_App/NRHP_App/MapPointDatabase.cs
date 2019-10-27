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

        public Task<MapPoint> GetPointAsync(string RefNum)
        {
            return _database.Table<MapPoint>()
                            .Where(mapPoint => mapPoint.RefNum.Equals(RefNum))
                            .FirstOrDefaultAsync();
        }

        public Task<MapPoint> GetRefNumAsync(string Name, double Latitude, double Longitude)
        {
            return _database.Table<MapPoint>()
                            .Where(mapPoint => mapPoint.Name.Equals(Name) && mapPoint.Latitude == Latitude && mapPoint.Longitude == Longitude)
                            .FirstOrDefaultAsync();
        }

        //Methods for searching the name of a point.
        //Multiple methods were created because of the complexity required to implement a recursive function with the predicates and expressions that the sql commands require.
        public async Task<List<MapPoint>> SearchNameAsync(string searchTerm)
        {
            List<MapPoint> finalList = new List<MapPoint>();
            string[] searchTerms = searchTerm.Split(' ');

            foreach (string term in searchTerms)
            {
                List<MapPoint> singleList = await _database.Table<MapPoint>()
                                                            .Where(mapPoint => mapPoint.Name.ToLower().Contains(term))
                                                            .ToListAsync();
                foreach (MapPoint item in singleList)
                {
                    if (!finalList.Contains(item))
                    {
                        finalList.Add(item);
                    }
                }
            }

            return finalList;
        }
    }
}