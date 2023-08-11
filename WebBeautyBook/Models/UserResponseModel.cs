namespace WebBeautyBook.Models
{
    public class UserResponseModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Photo { get; set; }
        public IList<string> Roles { get; set; }
        public string? WorkerId { get; set; }
        public string? CompanyId { get; set; }
    }
}
