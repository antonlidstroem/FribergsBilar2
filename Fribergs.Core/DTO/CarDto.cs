using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Fribergs.Core.DTO;


namespace Fribergs.Core.DTO

{
    public class CarDto
    {
        public int CarId { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public decimal PricePerDay { get; set; }
        public bool Available { get; set; }
        public string CarDescription { get; set; }

        
        [JsonPropertyName("carImages")]
        public CarImageResponse CarImagesResponse { get; set; } = new CarImageResponse();

        [JsonIgnore]
        public List<CarImageDto> CarImages => CarImagesResponse?.Values ?? new List<CarImageDto>();
    }


}
