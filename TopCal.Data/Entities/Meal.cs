using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TopCal.Data.Entities
{
    [Table("Meals")]
    public class Meal
    {
        [Key]
        [Required]
        [MaxLength(36)]
        [Column(TypeName = "char(36)")]
        public Guid Id { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(450)")]
        public string UserId { get; set; }

        [Required]
        [MaxLength(500)]
        [Column(TypeName = "varchar(500)")]
        public string Description { get; set; }

        [Column(TypeName = "smallint")]
        public float NumberOfCalories { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime Date { get; set; }

        [Column(TypeName = "bigint")]
        public TimeSpan Time { get; set; }

        [ForeignKey("UserId")]
        [Obsolete("Used For FK Relations")]
        public ApplicationUser User { get; set; }
    }
}
