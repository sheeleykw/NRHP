using SQLite;

namespace NRHP_App
{
    [Table("points")]
    public class MapPoint
    {
        [PrimaryKey]
        public string RefNum { get; set; }
        public string Name { get; set; }
        [PrimaryKey]
        public double Latitude { get; set; }
        [PrimaryKey]
        public double Longitude { get; set; }
        public string Category { get; set; }
    }
}
