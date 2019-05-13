using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using NRHP_App.Models;
using System;

namespace NRHP_App.Data
{
    public class DataPointDatabase
    {
        readonly SQLiteAsyncConnection _database;

        public DataPointDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<DataPoint>().Wait();
        }

        public string GetPath()
        {
            return _database.DatabasePath;
        }

        public Task<List<DataPoint>> GetPointsAsync(double TopLatitude, double BottomLatitude, double RightLongitude, double LeftLongitude)
        {
            Console.WriteLine("Inside");
            return _database.Table<DataPoint>()
                            .Where(dataPoint => dataPoint.Latitude <= TopLatitude && dataPoint.Latitude >= BottomLatitude && dataPoint.Longitude <= RightLongitude && dataPoint.Longitude >= LeftLongitude)
                            .ToListAsync();
        }

        public Task<DataPoint> GetPointAsync(string RefNum)
        {
            return _database.Table<DataPoint>()
                            .Where(dataPoint => dataPoint.RefNum.Equals(RefNum))
                            .FirstOrDefaultAsync();
        }

        public Task<int> SavePointAsync(DataPoint dataPoint)
        {
            return _database.InsertAsync(dataPoint);
        }

        public Task<int> DeletePointAsync(DataPoint dataPoint)
        {
            return _database.DeleteAsync(dataPoint);
        }
    }
}