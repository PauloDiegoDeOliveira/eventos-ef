using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ObjetivoEventos.Application.Controllers;
using ObjetivoEventos.Application.Dtos.Identity.Autenticacao;
using ObjetivoEventos.Application.Dtos.Identity.Usuario;
using ObjetivoEventos.Application.Dtos.Identity.Usuario.InternalResponses;
using ObjetivoEventos.Application.Extensions;
using ObjetivoEventos.Application.Interfaces;
using ObjetivoEventos.Domain.Core.Interfaces.Service;
using ObjetivoEventos.Domain.Pagination;
using ObjetivoEventos.Identity.Constants;
using System;
using System.Threading.Tasks;

namespace ObjetivoEventos.Application.V1.Controllers
{
    [ApiVersion("1.0")]
    [Route("/v{version:apiVersion}/autenticacao")]
    [ApiController]
    public class AutenticacaoController : MainController
    {
        private readonly IUsuarioApplication usuarioApplication;
        private readonly ILogger<AutenticacaoController> logger;

        public AutenticacaoController(IUsuarioApplication usuarioApplication,
                                      ILogger<AutenticacaoController> logger,
                                      INotificador notificador,
                                      IUser user) : base(notificador, user)
        {
            this.usuarioApplication = usuarioApplication;
            this.logger = logger;
        }

        /// <summary>
        /// Retorna todos os usuários com filtro e paginação de dados.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpGet("usuario")]
        [ProducesResponseType(typeof(ViewPaginacaoUsuarioDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllAsync([FromQuery] ParametersPalavraChave parameters)
        {
            ViewPaginacaoUsuarioDto result = await usuarioApplication.GetPaginationAsync(parameters);

            if (result.Usuarios.Count == 0)
            {
                NotificarErro("Nenhum usuário foi encontrado.");
                return CustomResponse(ModelState);
            }

            return CustomResponse(result, "Usuários encontrados.");
        }

        /// <summary>
        /// Cadastra um usuário.
        /// </summary>
        /// <param name="usuarioCadastro"></param>
        /// <returns></returns>
        [HttpPost("cadastro-usuario")]
        [Authorize(Policy = Policies.HorarioComercial)]
        //[ClaimsAuthorizeAttribute(ClaimTypes.Autorizacao, PermissionTypes.Inserir)]
        [ProducesResponseType(typeof(DefaultResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Cadastrar([FromBody] PostCadastroUsuarioDto usuarioCadastro)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            DefaultResponse resultado = await usuarioApplication.CadastrarUsuario(usuarioCadastro);

            if (!resultado.Sucesso)
            {
                NotificarErro(resultado.Erros);
                return CustomResponse(ModelState);
            }
            else
            {
                return CustomResponse(null, "Usuário cadastrado com sucesso!");
            }
        }

        /// <summary>
        /// Autentica um usuário.
        /// </summary>
        /// <param name="usuarioLogin"></param>
        /// <returns></returns>
        [HttpPost("login")]
        [ProducesResponseType(typeof(ViewLoginDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] PostLoginUsuarioDto usuarioLogin)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            UsuarioLoginResponse resultado = await usuarioApplication.Login(usuarioLogin);

            if (!resultado.Sucesso)
            {
                NotificarErro(resultado.Erros);
                return CustomResponse(ModelState);
            }
            else
            {
                return CustomResponse(new ViewLoginDto(resultado), "Login realizado com sucesso!");
            }
        }

        /// <summary>
        /// Altera a senha de um usuário autenticado.
        /// </summary>
        /// <param name="postAlterarSenhaUsuarioDto"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("alterar-senha")]
        [ProducesResponseType(typeof(DefaultResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AlterarSenha([FromBody] PostAlterarSenhaUsuarioDto postAlterarSenhaUsuarioDto)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            postAlterarSenhaUsuarioDto.UsuarioId = UsuarioId;
            DefaultResponse resultado = await usuarioApplication.AlterarSenha(postAlterarSenhaUsuarioDto);

            if (!resultado.Sucesso)
            {
                NotificarErro(resultado.Erros);
                return CustomResponse(ModelState);
            }
            else
            {
                return CustomResponse(null, "Senha alterada com sucesso!");
            }
        }

        /// <summary>
        /// Envia um e-mail com o token para resetar a senha.
        /// </summary>
        /// <param name="postConfirmacaoEmailDto"></param>
        /// <returns></returns>
        [HttpPost("resetar-senha-email")]
        [ProducesResponseType(typeof(DefaultResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> EnviarEmailResetarSenha([FromForm] PostEmailDto postConfirmacaoEmailDto)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            if (string.IsNullOrEmpty(postConfirmacaoEmailDto.Email))
            {
                NotificarErro("O campo e-mail não pode ser vazio.");
                return CustomResponse(ModelState);
            }

            DefaultResponse result = await usuarioApplication.EnviarEmailResetarSenha(postConfirmacaoEmailDto);

            if (result.Sucesso)
            {
                return CustomResponse(null, "Enviamos um <i>e-mail</i> para realização da alteração de senha.");
            }
            else
            {
                NotificarErro(result.Erros);
                return CustomResponse(ModelState);
            }
        }

        /// <summary>
        /// Recebe os dados com o token para realizar o reset de senha.
        /// </summary>
        /// <param name="postResetarSenhaDto"></param>
        /// <returns></returns>
        [HttpPost("resetar-senha")]
        [ProducesResponseType(typeof(DefaultResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ResetarSenha([FromBody] PostResetarSenhaDto postResetarSenhaDto)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            if (string.IsNullOrEmpty(postResetarSenhaDto.Token) && postResetarSenhaDto.UsuarioId == Guid.Empty)
            {
                NotificarErro("Os dados não podem ser vazios.");
                return CustomResponse(ModelState);
            }

            DefaultResponse result = await usuarioApplication.ResetarSenha(postResetarSenhaDto);

            if (result.Sucesso)
            {
                return CustomResponse(null, "Alteração realizada com sucesso!");
            }
            else
            {
                NotificarErro(result.Erros);
                return CustomResponse(ModelState);
            }
        }

        /// <summary>
        /// Confirma um e-mail de usuário cadastrado por meio do token.
        /// </summary>
        /// <param name="postConfirmacaoEmailDto"></param>
        /// <returns></returns>
        [HttpPost("confirmar-cadastro")]
        [ProducesResponseType(typeof(DefaultResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ConfirmarEmail([FromBody] PostConfirmacaoEmailDto postConfirmacaoEmailDto)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            if (string.IsNullOrEmpty(postConfirmacaoEmailDto.Token) && postConfirmacaoEmailDto.UsuarioId == Guid.Empty)
            {
                NotificarErro("Os dados não podem ser vazios.");
                return CustomResponse(ModelState);
            }

            DefaultResponse result = await usuarioApplication.ConfimarEmail(postConfirmacaoEmailDto);

            if (result.Sucesso)
            {
                return CustomResponse(null, "Verificação realizada com sucesso!");
            }
            else
            {
                NotificarErro(result.Erros);
                return CustomResponse(ModelState);
            }
        }

        /// <summary>
        /// Renvia um e-mail de confirmação de conta.
        /// </summary>
        /// <param name="postConfirmacaoEmailDto"></param>
        /// <returns></returns>
        [HttpPost("reenviar-confirmacao-email")]
        [ProducesResponseType(typeof(DefaultResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ReenviarConfirmacaoEmail([FromForm] PostEmailDto postConfirmacaoEmailDto)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            if (string.IsNullOrEmpty(postConfirmacaoEmailDto.Email))
            {
                NotificarErro("O campo e-mail não pode ser vazio.");
                return CustomResponse(ModelState);
            }

            DefaultResponse result = await usuarioApplication.ReenviarConfirmacaoEmail(postConfirmacaoEmailDto);

            if (result.Sucesso)
            {
                return CustomResponse(null, "Verificação reenviada com sucesso!");
            }
            else
            {
                NotificarErro(result.Erros);
                return CustomResponse(ModelState);
            }
        }

        /// <summary>
        /// Exclui um usuário.
        /// </summary>
        /// <param name="id"></param>
        /// <remarks>Ao excluir uma usuário o mesmo será excluído permanentemente da base de dados.</remarks>
        [Authorize]
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(DefaultResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            ViewUsuarioDto removido = await usuarioApplication.DeleteAsync(id);

            if (removido is null)
            {
                NotificarErro("Nenhum usuário foi encontrado com o id informado.");
                return CustomResponse(ModelState);
            }

            logger.LogWarning("Objeto removido {@removido} " + "Usuário: " + User.GetUserEmail(), removido);

            return CustomResponse(removido, "Usuário excluído com sucesso!");
        }
    }
}