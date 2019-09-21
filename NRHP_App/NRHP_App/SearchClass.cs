using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NRHP_App
{
    public class SearchClass
    {
        static public async Task<List<MapPoint>> NameSearch(String[] splitSearch)
        {
            List<MapPoint> nameSearch = new List<MapPoint>();

            if (splitSearch.Length == 1)
            {
                nameSearch = await App.mapDatabase.SearchNameAsync(splitSearch[0]);
            }
            else if (splitSearch.Length == 2)
            {
                nameSearch = await App.mapDatabase.SearchNameAsync(splitSearch[0], splitSearch[1]);
            }
            else if (splitSearch.Length == 3)
            {
                nameSearch = await App.mapDatabase.SearchNameAsync(splitSearch[0], splitSearch[1], splitSearch[2]);
            }
            else if (splitSearch.Length == 4)
            {
                nameSearch = await App.mapDatabase.SearchNameAsync(splitSearch[0], splitSearch[1], splitSearch[2], splitSearch[3]);
            }

            List<CityPoint> citySearch = await CitySearch(splitSearch);

            foreach (CityPoint cityPoint in citySearch)
            {
                var mapPoint = new MapPoint
                {
                    RefNum = "",
                    Name = cityPoint.Name + ", " + cityPoint.StateName,
                    Latitude = cityPoint.Latitude,
                    Longitude = cityPoint.Longitude,
                    Category = "City"
                };
                nameSearch.Insert(0, mapPoint);
            }

            return nameSearch;
        }

        static private async Task<List<CityPoint>> CitySearch(String[] splitSearch)
        {
            List<CityPoint> citySearch = new List<CityPoint>();

            if (splitSearch.Length == 1)
            {
                citySearch = await App.cityDatabase.SearchCityAsync(splitSearch[0]);
            }
            else if (splitSearch.Length == 2)
            {
                citySearch = await App.cityDatabase.SearchCityAsync(splitSearch[0], splitSearch[1]);
            }
            else if (splitSearch.Length == 3)
            {
                citySearch = await App.cityDatabase.SearchCityAsync(splitSearch[0], splitSearch[1], splitSearch[2]);
            }

            return citySearch;
        }
    }
}
