namespace ServiceFamilyBank.Dtos.Usuario
{
    public class LoggedUsuarioDto
    {
        public int codigo { get; set; }
        public string nome { get; set; } = String.Empty;
        public int fk_perfil { get; set; }
        public string email { get; set; } = String.Empty;
    }
}