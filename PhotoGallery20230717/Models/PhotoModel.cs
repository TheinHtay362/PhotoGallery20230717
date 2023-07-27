using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhotoGallery20230717.Models
{
    [Table("tbl_Photo")]
    public class PhotoModel : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Caption { get; set; }
        public byte[] ImageData { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
        public DateTime UploadedOn { get; set; }
        public string? Description { get; set; }
        public string? Location { get; set; }
        public string? Tags { get; set; }
    }
    public class BaseModel
    {
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
