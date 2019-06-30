using SQLite;

namespace NRHP_App
{
    [Table("cities")]
    public class CityPoint
    {
        [PrimaryKey]
        public int ID { get; set; }
        public string Name { get; set; }
        public string StateAbre { get; set; }
        public string StateName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}