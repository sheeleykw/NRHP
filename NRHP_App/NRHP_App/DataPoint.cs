using SQLite;

namespace NRHP_App
{
    [Table("dataPoints")]
    public class DataPoint
    {
        [PrimaryKey]
        public string RefNum { get; set; }
        public string Name { get; set; }
        public string SourceDate { get; set; }
        [PrimaryKey]
        public double Latitude { get; set; }
        [PrimaryKey]
        public double Longitude { get; set; }
        public string Category { get; set; }
    }
}
