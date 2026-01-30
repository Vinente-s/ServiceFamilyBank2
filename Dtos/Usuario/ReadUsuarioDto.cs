namespace ServiceFamilyBank.Dtos.Usuario
{
    public class ReadUsuarioDto
    {
        public int codigo { get; set; }
        public string nome { get; set; }
        public string email { get; set; }
        public DateTime data_cadastro {get; set;}
        public string status { get; set;}
        public string perfil {get; set;}
    }
}