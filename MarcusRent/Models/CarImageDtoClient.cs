using DAL.Classes;

namespace MarcusRent.Models
{
    public class CarImageDtoClient
    {
        public int CarImageId { get; set; }
        public string Url { get; set; }

        public int CarId { get; set; }
        //public Car Car { get; set; }
    }
}
