using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceFamilyBank.Dtos.Usuario
{
    public class UpdateUsuarioDto
    {
        public string nome { get; set; }
        public string email { get; set; }
        public string senha { get; set; }
        public int fk_perfil { get; set; }
    }
}