﻿using System.ComponentModel.DataAnnotations;

namespace Cinema.Domain.Models
{
    public class Director
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(50)]
        public string? FirstName { get; set; }
        [MaxLength(50)]
        public string? LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        [MaxLength(50)]
        public string? Origin { get; set; }

        public ICollection<Movie> Movies { get; set; }
    }
}
