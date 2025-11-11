
using System;
using System.ComponentModel.DataAnnotations;

namespace MarcusRent.Models
{
    public class OrderDtoClient
    {
        public int OrderId { get; set; }

        [Required]
        public int CarId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public decimal Price { get; set; }

        // (Valfritt) för enkelhet i vyn:
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public string CustomerId { get; set; }
    }
}


