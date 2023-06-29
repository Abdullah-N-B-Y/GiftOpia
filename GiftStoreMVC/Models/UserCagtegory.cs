namespace GiftStoreMVC.Models
{
    public class UserCagtegory
    {
        public decimal? UserId { get; set; }
        public string? UserName { get; set; }
        public string? UserEmail { get; set; }
        public string? UserPhone { get; set; }
        public string? UserImagepath { get; set; }
        public IFormFile? UserImage { get; set; }

        public decimal? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? CategoryImagepath { get; set; }
        public string? Categorydescription { get; set; }
        public IFormFile? CategoryImage { get; set; }
    }
}
