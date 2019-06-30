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
        public Task<List<CityPoint>> SearchCityAsync(string firstWord)
        {
            return _database.Table<CityPoint>()
                            .Where(cityPoint => cityPoint.Name.ToLower().Contains(firstWord))
                            .OrderBy(cityPoint => cityPoint.StateName)
                            .OrderBy(cityPoint => cityPoint.Name)
                            .ToListAsync();
        }

        public Task<List<CityPoint>> SearchCityAsync(string firstWord, string secondWord)
        {
            return _database.Table<CityPoint>()
                            .Where(cityPoint => cityPoint.Name.ToLower().Contains(firstWord) && cityPoint.Name.ToLower().Contains(secondWord))
                            .OrderBy(cityPoint => cityPoint.StateName)
                            .OrderBy(cityPoint => cityPoint.Name)
                            .ToListAsync();
        }

        public Task<List<CityPoint>> SearchCityAsync(string firstWord, string secondWord, string thirdWord)
        {
            return _database.Table<CityPoint>()
                            .Where(cityPoint => cityPoint.Name.ToLower().Contains(firstWord) && cityPoint.Name.ToLower().Contains(secondWord) && cityPoint.Name.ToLower().Contains(thirdWord))
                            .OrderBy(cityPoint => cityPoint.StateName)
                            .OrderBy(cityPoint => cityPoint.Name)
                            .ToListAsync();
        }
    }
}