namespace Fribergs.Core.ViewModels
{
    public class CustomerViewModel
    {
        public string UserId { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";
        public bool ApprovedByAdmin { get; set; }
    }
}