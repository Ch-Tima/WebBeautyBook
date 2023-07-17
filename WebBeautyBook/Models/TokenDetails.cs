namespace WebBeautyBook.Models
{
    public record TokenDetails
    (
        string Token,
        DateTime Expiration
    );
}
