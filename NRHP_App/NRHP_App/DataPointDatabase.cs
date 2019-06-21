﻿using System.Collections.Generic;
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
        }

        public Task<DataPoint> GetPointAsync(string RefNum)
        {
            return _database.Table<DataPoint>()
                            .Where(dataPoint => dataPoint.RefNum.Equals(RefNum))
                            .FirstOrDefaultAsync();
        }

        public Task<List<DataPoint>> GetFavoritedPointsAsync()
        {
            return _database.Table<DataPoint>()
                            .Where(dataPoint => dataPoint.IsFavorited == true)
                            .OrderBy(dataPoint => dataPoint.Name)
                            .ToListAsync();
        }

        public Task<List<DataPoint>> SearchPointsCityAsync(string searchQuery)
        {
            return _database.Table<DataPoint>()
                            .Where(dataPoint => dataPoint.City.ToLower().Contains(searchQuery))
                            .OrderBy(dataPoint => dataPoint.State)
                            .OrderBy(dataPoint => dataPoint.City)
                            .ToListAsync();
        }

        public void UpdatePoint(DataPoint dataPoint)
        {
            _database.UpdateAsync(dataPoint);
        }
    }
}