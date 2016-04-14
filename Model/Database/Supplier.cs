namespace Microbrewit.Api.Model.Database
{
    public class Supplier
    {
        public int SupplierId { get; set; }
        public string Name { get; set; }
        public int OriginId { get; set; }
        public Origin Origin { get; set; }
       
    }
}
