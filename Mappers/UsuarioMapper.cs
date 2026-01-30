using ServiceFamilyBank.Dtos.Usuario;
using ServiceFamilyBank.Models;

namespace ServiceFamilyBank.Mappers
{
    public static class UsuarioMapper
    {
        public static ReadUsuarioDto ToReadUsuarioDto(this Usuario user)
        {
            return new ReadUsuarioDto
            {
                codigo = user.codigo,
                nome = user.nome,
                email = user.email,
                data_cadastro = user.data_cadastro,
                status = user.status,
                perfil = user.Perfis.perfil
            };
        }

        public static CreateUsuarioDto ToCreateUsuarioDto(this Usuario user)
        {
            return new CreateUsuarioDto
            {
                nome = user.nome,
                email = user.email,
                fk_perfil = user.fk_perfil,
            };
        }

        public static LoggedUsuarioDto ToLoggedUsuarioDto(this Usuario user)
        {
            return new LoggedUsuarioDto
            {
                codigo = user.codigo,
                nome = user.nome,
                email = user.email,
                fk_perfil = user.fk_perfil,
            };
        }
    }
}