using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceFamilyBank.Models
{
    public class Usuario
    {
        [Key]
        public int codigo { get; set; }
        public string nome { get; set; }
        public string email { get; set; }
        public string senha { get; set; }
        public DateTime data_cadastro { get; set; }
        
        [ForeignKey("Perfis")]
        public int fk_perfil { get; set; }
        public string status { get; set;}
        public Perfis Perfis { get; set; }
    }
}