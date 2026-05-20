using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Entities.Dtos
{
    public class LoginDto
    {
        public string Name { get; set; }
      //  public int Id { get; set; }
        //public string Password { get; set; }
    }

    public class LoginResponseDto
    {
        public string Token { get; set; }
     //   public string Name { get; set; }

    }


}
