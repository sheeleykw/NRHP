using SQLite;

namespace NRHP_App
{
    [Table("data")]
    public class DataPoint
    {
        [PrimaryKey]
        public string RefNum { get; set; }
        public string Name { get; set; }
        public string SourceDate { get; set; }
        public string Category { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string State { get; set; }
        public string Architects { get; set; }
        public bool IsFavorited { get; set; }
    }
}
