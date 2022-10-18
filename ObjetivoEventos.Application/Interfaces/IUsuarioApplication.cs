using ObjetivoEventos.Application.Dtos.Identity.Autenticacao;
using ObjetivoEventos.Application.Dtos.Identity.Usuario;
using ObjetivoEventos.Application.Dtos.Identity.Usuario.InternalResponses;
using ObjetivoEventos.Domain.Pagination;
using System;
using System.Threading.Tasks;

namespace ObjetivoEventos.Application.Interfaces
{
    public interface IUsuarioApplication
    {
        Task<ViewPaginacaoUsuarioDto> GetPaginationAsync(ParametersPalavraChave parametersPalavraChave);

        Task<ViewUsuarioDto> GetByIdAsync(Guid id);

        Task<DefaultResponse> CadastrarUsuario(PostCadastroUsuarioDto postCadastroUsuarioDto);

        Task<UsuarioLoginResponse> Login(PostLoginUsuarioDto postLoginUsuarioDto);

        Task<DefaultResponse> AlterarSenha(PostAlterarSenhaUsuarioDto postAlterarSenhaUsuarioDto);

        Task<DefaultResponse> ResetarSenha(PostResetarSenhaDto postResetarSenhaDto);

        Task<DefaultResponse> EnviarEmailResetarSenha(PostEmailDto postConfirmacaoEmailDto);

        Task<DefaultResponse> ConfimarEmail(PostConfirmacaoEmailDto postConfirmacaoEmailDto);

        Task<DefaultResponse> ReenviarConfirmacaoEmail(PostEmailDto postConfirmacaoEmailDto);

        Task<ViewUsuarioDto> DeleteAsync(Guid d);

        bool ValidarId(Guid id);
    }
}