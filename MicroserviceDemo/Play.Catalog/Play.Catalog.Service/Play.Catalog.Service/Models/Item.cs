using Play.Common;

namespace Play.Catalog.Service.Models
{
    public class Item : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Decimal Price { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}
