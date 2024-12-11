using System.ComponentModel.DataAnnotations;

namespace OrderApi.Applicaton.DTOs
{
    public record OrderDatailsDTO
        (
        [Required] int OrderId,
        [Required] int ProductId,
        [Required] int ClientId,
        [Required] string ClientName,
        [Required] string Address,
        [Required, EmailAddress] string Email,
        [Required] string TelephoneNUmber,
        [Required] string ProductName,
        [Required] int PurchaseQuantity,
        [Required, DataType(DataType.Currency)] decimal UnitPrice,
        [Required, DataType(DataType.Currency)] decimal TotalPrice,
        [Required] DateTime OrderesDate
        );
}
