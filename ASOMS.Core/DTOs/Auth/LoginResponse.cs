using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASOMS.Core.DTOs.Auth
{
        public class LoginResponse
        {
            public string Token { get; set; } = string.Empty;

            public UserDto User { get; set; } = new UserDto();
        }

}
