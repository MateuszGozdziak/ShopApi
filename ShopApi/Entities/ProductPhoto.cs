namespace ShopApi.Entities
{
    public class ProductPhoto
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public bool isMain { get; set; }
        public string PublicId {get; set; }
        public Product Product { get; set; }
        public int ProductId { get; set; }

    }
}
