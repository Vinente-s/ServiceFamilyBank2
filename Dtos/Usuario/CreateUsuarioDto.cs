namespace ServiceFamilyBank.Dtos.Usuario
{
    public class CreateUsuarioDto
    {
        public string nome { get; set; }
        public string email { get; set; }
        public string senha { get; set; }
        public int fk_perfil { get; set; }
    }
}