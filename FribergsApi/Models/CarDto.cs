using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using DAL.Classes;

namespace FribergsApi.Models
{
    public class CarDto
    {
        public int CarId { get; set; }

      
        public List<CarImageDto> CarImages { get; set; } = new();

        public string Brand { get; set; }

        public string Model { get; set; }
        [Range(1900, 2100)]
        public int Year { get; set; }
        [Range(0, double.MaxValue)]
        public decimal PricePerDay { get; set; }
        public bool Available { get; set; }

        public string CarDescription { get; set; }
    }
}
