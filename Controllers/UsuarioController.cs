using Microsoft.AspNetCore.Mvc;
using ServiceFamilyBank.Models.Responses;
using ServiceFamilyBank.Contexts;
using ServiceFamilyBank.Dtos.Usuario;
using ServiceFamilyBank.Models;
using ServiceFamilyBank.Services;
using ServiceMonitoramentoWeb.Services;
using Microsoft.EntityFrameworkCore;
using ServiceFamilyBank.Mappers;

namespace ServiceFamilyBank.Controllers
{
    [Route ("api/user")]
    [ApiController]

    public class UsuarioController : ControllerBase
    {
        private readonly ILogger<UsuarioController> _logger;
        private readonly AppDbContext _context;
        private readonly TokenService _tokenService;
        private readonly HelperService _helperService;

        public UsuarioController(ILogger<UsuarioController> logger, AppDbContext context, TokenService tokenService, HelperService helperService)
        {
            _logger = logger;
            _context = context;
            _tokenService = tokenService;
            _helperService = helperService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateUsuario([FromBody] CreateUsuarioDto createUsuarioDto)
        {
            var response = new Response<CreateUsuarioDto>();

            var novoUsuario = new Usuario
            {
                nome = createUsuarioDto.nome,
                email = createUsuarioDto.email,
                senha = _helperService.HashMd5(createUsuarioDto.senha),
                fk_perfil = createUsuarioDto.fk_perfil,
                status = "ATIVO"
            };

            try
            {
                var usuarioExistente = await _context.dusuarios.FirstOrDefaultAsync(u => u.email == novoUsuario.email);

                if (usuarioExistente != null)
                {
                    response.Data = null;
                    response.Success = false;
                    response.Message = "Email já está cadastrado.";
                    return BadRequest(response);
                }

                await _context.dusuarios.AddAsync(novoUsuario);
                await _context.SaveChangesAsync();

                response.Data = novoUsuario.ToCreateUsuarioDto();
                response.Success = true;
                response.Message = "Usuário criado com sucesso.";

                return CreatedAtAction("CreateUsuario", response);
            } 
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar usuário.");
                response.Data = null;
                response.Success = false;
                response.Message = "Ocorreu um erro ao criar o usuário.";
                return StatusCode(500, response);
            }
        }

        [HttpGet("getAllUsuarios")]
        public async Task<IActionResult> GetAllUsuarios()
        {
            var response = new Response<List<ReadUsuarioDto>>();

            try
            {
                var usuarios = await _context.dusuarios.Include(u => u.Perfis).ToListAsync();
                var usuariosDto = usuarios.Select(u => u.ToReadUsuarioDto()).ToList();

                response.Data = usuariosDto;
                response.Success = true;
                response.Message = "Lista de usuários obtida com sucesso.";
                return Ok(response);
            } catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter lista de usuários.");
                response.Data = null;
                response.Success = false;
                response.Message = "Ocorreu um erro ao obter a lista de usuários.";
                return StatusCode(500, response);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var response = new Response<ReadUsuarioDto>();
             
            try
            {
                var usuario = await _context.dusuarios
                .Include(u => u.Perfis)
                .FirstOrDefaultAsync(u => u.codigo == id);

                if (usuario == null)
                {
                    _logger.LogError("Erro ao encontrar usuário.");
                    response.Success = false;
                    response.Message = "Ocorreu um erro ao encontrar o usuário requerido.";
                    return StatusCode(500, response);
                }

                response.Data = usuario.ToReadUsuarioDto();
                response.Success = true;
                response.Message = "Usuário encontrado com sucesso.";
                return Ok(response);                
            } catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter usuário.");
                response.Data = null;
                response.Success = false;
                response.Message = "Ocorreu um erro ao obter o usuário requerido.";
                return StatusCode(500, response);
            } 
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var response = new Response<ReadUsuarioDto>();
            try
            {
                var deletedUser = await _context.dusuarios.FirstOrDefaultAsync(u => u.codigo == id);

                if(deletedUser == null)
                {
                    _logger.LogError("Erro ao encontrar usuário.");
                    response.Success = false;
                    response.Message = "Ocorreu um erro ao encontrar o usuário requerido.";
                    return StatusCode(500, response);             
                }

                deletedUser.status = "INATIVO";

                // _context.dusuarios.Remove(deletedUser);
                await _context.SaveChangesAsync();

                response.Success = true;
                response.Message = "Usuário Removido com sucesso.";                
                return NoContent();
            } catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao Inativar usuário.");
                response.Success = false;
                response.Message = "Ocorreu um erro ao Inativar o usuário requerido.";
                return StatusCode(500, response);                
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateUsuarioDto updataDto)
        {
            var response = new Response<UpdateUsuarioDto>();
            try
            {
                var usuario = await _context.dusuarios.FirstOrDefaultAsync(u => u.codigo == id);

                if (usuario == null)
                {
                    _logger.LogError("Erro ao encontrar usuário.");
                    response.Success = false;
                    response.Message = "Ocorreu um erro ao encontrar o usuário requerido.";
                    return StatusCode(500, response);
                }

                usuario.nome = updataDto.nome;
                usuario.email = updataDto.email;
                usuario.senha = updataDto.senha;
                usuario.fk_perfil = updataDto.fk_perfil;

                await _context.SaveChangesAsync();

                response.Success = true;
                response.Message = "Usuário atualizado com sucesso.";  
                return Ok(response);
            } catch(Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar usuário.");
                response.Success = false;
                response.Message = "Ocorreu um erro ao atualizar o usuário requerido.";
                return StatusCode(500, response);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUsuarioDto loginUsuarioDto)
        {
            var response = new Response<string>();

            try
            {
                var usuario = await _context.dusuarios
                    .FirstOrDefaultAsync(u => u.email == loginUsuarioDto.email && u.senha == loginUsuarioDto.senha);

                if (usuario == null)
                {
                    response.Data = null;
                    response.Success = false;
                    response.Message = "Credenciais inválidas.";
                    return Unauthorized(response);
                }

                var token = _tokenService.GenerateToken(usuario.ToLoggedUsuarioDto());

                response.Data = token;
                response.Success = true;
                response.Message = "Login realizado com sucesso.";
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao realizar login.");
                response.Data = null;
                response.Success = false;
                response.Message = "Ocorreu um erro ao realizar o login.";
                return StatusCode(500, response);
            }
        }
    }
}