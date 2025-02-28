using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UsersAPI.Application.Security;
using UsersAPI.Domain.Dtos.Requests;
using UsersAPI.Domain.Dtos.Responses;
using UsersAPI.Domain.Entities;
using UsersAPI.Domain.Enums;
using UsersAPI.Infra.Data.Enums;
using UsersAPI.Infra.Data.Helpers;
using UsersAPI.Infra.Data.Repositories;
using UsersAPI.Infra.Messages.Models;
using UsersAPI.Infra.Messages.Producers;

namespace UsersAPI.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        //instanciando os repositórios
        private readonly UserRepository _userRepository = new UserRepository();
        private readonly RoleRepository _roleRepository = new RoleRepository();

        [HttpPost("create")]
        [ProducesResponseType(typeof(CreateUserResponseDto), 201)]
        public IActionResult Create([FromBody] CreateUserRequestDto request)
        {
            try
            {
                //verificar se o email informado já existe no banco de dados
                if (_userRepository.HasEmail(request.Email))
                    //HTTP 400 (Erro do cliente): Bad Request
                    return StatusCode(400, new { message = "O email informado já está cadastrado. Tente outro." });

                //preenchendo os dados do usuario
                var user = new User
                {
                    Id = Guid.NewGuid(), //gerando um id para o usuário
                    Name = request.Name, //preenchendo o nome do usuário
                    Email = request.Email, //preenchendo o email do usuário
                    Password = SHA256Encrypt.Create(request.Password), //criptografando a senha
                    Status = Status.Active, //definindo o status como ativo
                    RoleId = _roleRepository.GetByName("OPERADOR")?.Id //capturando o id do perfil (FK)
                };

                //gravando o usuário no banco de dados
                _userRepository.Execute(user, OperationType.Add);

                //enviando o usuário cadastrado para a fila do RabbitMQ
                var messageProducer = new MessageProducer();
                messageProducer.SendMessage(new RegisteredUser 
                { 
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    CreatedAt = DateTime.Now,
                    Role = "OPERADOR"
                });

                //retornando sucesso HTTP 201 (CREATED)
                return StatusCode(201, new CreateUserResponseDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    CreatedAt = DateTime.Now,
                    Role = "OPERADOR"
                });
            }
            catch(Exception e)
            {
                //HTTP 500 (Erro do servidor): Internal Server Error
                return StatusCode(500, new { e.Message });
            }
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginUserResponseDto), 200)]
        public IActionResult Login([FromBody] LoginUserRequestDto request)
        {
            try
            {
                //consultar o usuário no banco de dados através do email e da senha
                var user = _userRepository.GetByEmailAndPassword
                    (request.Email, SHA256Encrypt.Create(request.Password));

                //verificar se o usuário não foi encontrado
                if (user == null)
                    //HTTP 401 (Unauthorized): Acesso não autorizado
                    return StatusCode(401, new { message = "Acesso negado. Usuário não encontrado." });

                //retornar os dados do usuário autenticado
                var response = new LoginUserResponseDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Role = user.Role?.Name,
                    AccessedAt = DateTime.Now,
                    Token = JwtBearerSecurity.GetAccessToken(user.Id),
                    Expiration = JwtBearerSecurity.GetExpiration()
                };

                //retornar sucesso
                return StatusCode(200, response);
            }
            catch (Exception e)
            {
                //HTTP 500 (Erro do servidor): Internal Server Error
                return StatusCode(500, new { e.Message });
            }
        }
    }
}
