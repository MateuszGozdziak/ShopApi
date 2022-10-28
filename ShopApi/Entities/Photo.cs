using System.ComponentModel.DataAnnotations.Schema;

namespace ShopApi.Entities
{
    [Table("Photos")]
    public class Photo
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }
        public string PublicId { get; set; }
        public ProductPhoto ProductPhoto { get; set; }
        public int AppUserId { get; set; }
    }
}
