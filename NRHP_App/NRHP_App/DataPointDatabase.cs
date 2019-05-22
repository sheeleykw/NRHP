using System;
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
            _database.CreateTableAsync<DataPoint>();
            _database.CreateTableAsync<FavoritePoint>();
        }

        public Task<List<DataPoint>> GetPointsAsync(double TopLatitude, double BottomLatitude, double RightLongitude, double LeftLongitude)
        {
            return _database.Table<DataPoint>()
                            .Where(dataPoint => dataPoint.Latitude <= TopLatitude && dataPoint.Latitude >= BottomLatitude && dataPoint.Longitude <= RightLongitude && dataPoint.Longitude >= LeftLongitude)
                            .ToListAsync();
        }

        public Task<DataPoint> GetPointRefNumAsync(string RefNum)
        {
            return _database.Table<DataPoint>()
                            .Where(dataPoint => dataPoint.RefNum.Equals(RefNum))
                            .FirstOrDefaultAsync();
        }

        public Task<DataPoint> GetPointNameAsync(string Name)
        {
            return _database.Table<DataPoint>()
                            .Where(dataPoint => dataPoint.Name.Equals(Name))
                            .FirstOrDefaultAsync();
        }

        public Task<List<FavoritePoint>> GetFavoritePointsAsync()
        {
            return _database.Table<FavoritePoint>()
                            .ToListAsync();
        }

        public void SaveFavoritePoint(FavoritePoint favorite)
        {
            _database.InsertAsync(favorite);
        }

        public void DeleteFavoritePoint(FavoritePoint favorite)
        {
            _database.DeleteAsync(favorite);
        }
    }
}