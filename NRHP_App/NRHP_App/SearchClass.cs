using System.Collections.Generic;
using System.Threading.Tasks;

namespace NRHP_App
{
    public class SearchClass
    {
        static public async Task<List<MapPoint>> NameSearch(string searchBarText)
        {
            searchBarText = searchBarText.ToLower().Trim();
            string searchTerm = "";
            foreach (char spot in searchBarText)
            {
                if (!char.IsPunctuation(spot))
                {
                    searchTerm = searchTerm.Insert(searchTerm.Length, spot.ToString());
                }
            }

            List<MapPoint> nameSearch = await App.mapDatabase.SearchNameAsync(searchTerm);
            List<CityPoint> citySearch = await App.cityDatabase.SearchCityAsync(searchTerm);

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
    }
}
