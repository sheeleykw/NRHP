using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;

namespace NRHP_App
{
    //City database data provided by https://simplemaps.com/data/us-cities
    public class CityPointDatabase
    {
        readonly SQLiteAsyncConnection _database;

        public CityPointDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<CityPoint>();
        }

        public Task<CityPoint> GetPointAsync(int ID)
        {
            return _database.Table<CityPoint>()
                            .Where(cityPoint => cityPoint.ID == ID)
                            .FirstOrDefaultAsync();
        }

        //Methods for searching the name of a point.
        //Multiple methods were created because of the complexity required to implement a recursive function with the predicates and expressions that the sql commands require.
        public async Task<List<CityPoint>> SearchCityAsync(string searchTerm)
        {
            List<CityPoint> finalList = new List<CityPoint>();
            string[] searchTerms = searchTerm.Split(' ');

            foreach (string term in searchTerms)
            {
                List<CityPoint> singleList = await _database.Table<CityPoint>()
                                                            .Where(cityPoint => cityPoint.Name.ToLower().Contains(term))
                                                            .ToListAsync();
                foreach (CityPoint item in singleList)
                {
                    if(!finalList.Contains(item))
                    {
                        finalList.Add(item);
                    }
                }
            }

            return finalList;
        }
    }
}