﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TopCal.Data.Model
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
        [ForeignKey("UserId")]
        [MaxLength(36)]
        [Column(TypeName = "char(36)")]
        public string UserId { get; set; }

        [Required]
        [MaxLength(500)]
        [Column(TypeName = "varchar(500)")]
        public string Description { get; set; }

        [Column(TypeName = "smallint")]
        public float NumberOfCalories { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime Date { get; set; }

        [Column(TypeName = "varchar(10)")]
        public TimeSpan Time { get; set; }
    }
}
