using DAL.Classes;

namespace FribergsApi.Models
{
    public class CarImageDto
    {
        public int CarImageId { get; set; }
        public string Url { get; set; }

        public int CarId { get; set; }
        //public Car Car { get; set; }
    }
}
