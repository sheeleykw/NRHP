using SQLite;

namespace NRHP_App
{
    [Table("dataPoints")]
    public class DataPoint
    {
        [PrimaryKey]
        public int RefNum { get; set; }
        public string Name { get; set; }
        public string SourceDate { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Category { get; set; }
    }
}
