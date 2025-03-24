using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace proyecto_centauro.Models
{
    public class Login
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}