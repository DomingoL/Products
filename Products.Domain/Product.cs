

namespace Products.Domain
{
    using System;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Product
    {
        [Key]
        public int ProductID { get; set; }

        public int CategoryId { get; set; }

        [Required(ErrorMessage = "The field {0} is requied")]
        [MaxLength(50, ErrorMessage = "The field {0} only can contain {1} characters lenght.")]
        [Index("Category_Description_Index", IsUnique = true)]
        public string Description { get; set; }

        [Required(ErrorMessage = "The field {0} is requied")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal Price { get; set; }

        [Display(Name ="Is Active?")]
        public bool IsActive { get; set; }

        [Display(Name = "Last  Purchase")]
        [DataType(DataType.Date)]
        public DateTime LastPurchase { get; set; }

        public string Image { get; set; } 

        public double stock { get; set; }

        [DataType(DataType.MultilineText)]
        public string Remark { get; set; }

        [JsonIgnore]
        public virtual Category Category { get; set; }
    }
}
