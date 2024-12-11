using AuthenticationApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationApi.Application.DTOs.Conversions
{
    public class UserConversion
    {
        public static AppUser ToEntity(AppUserDTO userDTO) => new AppUser
        {
            Id = userDTO.Id,
            Email = userDTO.Email,
            Name = userDTO.Name,
            Password = userDTO.Password,
            Role = userDTO.Role,
            TelephoneNumber = userDTO.TelephoneNumber
        };

        public static (AppUserDTO?, IEnumerable<AppUserDTO>?) FromEntity(AppUser? user, IEnumerable<AppUser>? users)
        {
            if (user is not null && users is null)
            {
            }

            if (user is null && users is not null)
            {
            }

            return (null, null);
        }
    }
}
