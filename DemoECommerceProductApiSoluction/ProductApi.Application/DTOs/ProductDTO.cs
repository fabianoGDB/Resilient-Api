using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductApi.Application.DTOs
{
    public record ProductDTO
        (
            int id,
            [Required]string? name,
            [Required, Range(1, int.MaxValue)] decimal price,
            [Required, DataType(DataType.Currency)] int quantity
        );
}
