using System.ComponentModel.DataAnnotations;

namespace ServiceFamilyBank.Models
{
    public class Perfis
    {
        [Key]
        public int codigo { get; set; }
        public string perfil { get; set; }
    }
}