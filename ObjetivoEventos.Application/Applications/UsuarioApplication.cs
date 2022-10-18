using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ObjetivoEventos.Application.Dtos.Identity.Autenticacao;
using ObjetivoEventos.Application.Dtos.Identity.Usuario;
using ObjetivoEventos.Application.Dtos.Identity.Usuario.InternalResponses;
using ObjetivoEventos.Application.Dtos.Pagination;
using ObjetivoEventos.Application.Dtos.SMTP;
using ObjetivoEventos.Application.Interfaces;
using ObjetivoEventos.Domain.Core.Interfaces.Service;
using ObjetivoEventos.Domain.Enums;
using ObjetivoEventos.Domain.Pagination;
using ObjetivoEventos.Identity.Entitys;
using ObjetivoEventos.Identity.Extensions;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace ObjetivoEventos.Application.Applications
{
    public class UsuarioApplication : IUsuarioApplication
    {
        private readonly SignInManager<Usuario> _signInManager;
        private readonly UserManager<Usuario> _userManager;
        private readonly JwtOptions _jwtOptions;
        private readonly IEmailApplication emailApplication;
        private readonly IUsuarioService usuarioService;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;
        private readonly Ambiente actualEnvironment;

        public UsuarioApplication(SignInManager<Usuario> signInManager,
                                   UserManager<Usuario> userManager,
                                   IOptions<JwtOptions> jwtOptions,
                                   IEmailApplication emailApplication,
                                   IWebHostEnvironment environment,
                                   IUsuarioService usuarioService,
                                   IConfiguration configuration,
                                   IMapper mapper)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtOptions = jwtOptions.Value;
            this.configuration = configuration;
            this.emailApplication = emailApplication;
            this.usuarioService = usuarioService;
            this.mapper = mapper;

            actualEnvironment = environment.IsDevelopment() ? Ambiente.Desenvolvimento :
                environment.IsStaging() ? Ambiente.Homologacao : Ambiente.Producao;
        }

        public async Task<ViewPaginacaoUsuarioDto> GetPaginationAsync(ParametersPalavraChave parametersPalavraChave)
        {
            PagedList<Usuario> listaUsuarios = await usuarioService.GetPaginationAsync(parametersPalavraChave);

            if (listaUsuarios == null)
                return null;

            ViewPaginacaoUsuarioDto objFinal = new();

            foreach (var usuario in listaUsuarios)
            {
                objFinal.Usuarios.Add(new ViewUsuarioDto(usuario));
            }

            objFinal.DadosPaginacao = new ViewPaginationDataDto<Usuario>(listaUsuarios);

            return objFinal;
        }

        public async Task<ViewUsuarioDto> GetByIdAsync(Guid id)
        {
            return mapper.Map<ViewUsuarioDto>(await usuarioService.GetByIdAsync(id));
        }

        public async Task<DefaultResponse> CadastrarUsuario(PostCadastroUsuarioDto postCadastroUsuarioDto)
        {
            Usuario user = new()
            {
                Nome = postCadastroUsuarioDto.Nome,
                Sobrenome = postCadastroUsuarioDto.Sobrenome,
                RA = postCadastroUsuarioDto.RA,
                Apelido = postCadastroUsuarioDto.Apelido,
                CPF = postCadastroUsuarioDto.CPF,
                Nascimento = postCadastroUsuarioDto.Nascimento,
                Genero = postCadastroUsuarioDto.Genero.ToString(),
                CriadoEm = DateTime.Now,
                UserName = postCadastroUsuarioDto.Email,
                Email = postCadastroUsuarioDto.Email,
                Status = postCadastroUsuarioDto.Status.ToString()
            };

            IdentityResult result = await _userManager.CreateAsync(user, postCadastroUsuarioDto.Senha);

            if (result.Succeeded)
            {
                await _userManager.SetLockoutEnabledAsync(user, false);
                await GerarTokenConfirmacaoEmail(user);
            }

            DefaultResponse DefaultResponse = new();

            if (!result.Succeeded)
                DefaultResponse.AdicionarErros(result.Errors.Select(r => r.Description));

            return DefaultResponse;
        }

        public async Task<UsuarioLoginResponse> Login(PostLoginUsuarioDto postLoginUsuarioDto)
        {
            SignInResult result = await _signInManager.PasswordSignInAsync(postLoginUsuarioDto.Email, postLoginUsuarioDto.Senha, false, true);

            if (result.Succeeded)
                return await GerarCredenciais(postLoginUsuarioDto.Email);

            UsuarioLoginResponse usuarioLoginResponse = new();

            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                    usuarioLoginResponse.AdicionarErro("Essa conta está bloqueada.");
                else if (result.IsNotAllowed)
                    usuarioLoginResponse.AdicionarErro("É necessario verificar o e-mail para fazer login.");
                else if (result.RequiresTwoFactor)
                    usuarioLoginResponse.AdicionarErro("É necessário confirmar o login no seu segundo fator de autenticação.");
                else
                    usuarioLoginResponse.AdicionarErro("Usuário ou senha estão incorretos.");
            }

            return usuarioLoginResponse;
        }

        public async Task<DefaultResponse> AlterarSenha(PostAlterarSenhaUsuarioDto postAlterarSenhaUsuarioDto)
        {
            IdentityResult result = await _userManager.ChangePasswordAsync(await _userManager.FindByIdAsync(postAlterarSenhaUsuarioDto.UsuarioId.ToString()),
                                                                            postAlterarSenhaUsuarioDto.SenhaAntiga, postAlterarSenhaUsuarioDto.NovaSenha);

            if (result.Succeeded)
                return new DefaultResponse(true);

            DefaultResponse DefaultResponse = new();

            if (!result.Succeeded)
                DefaultResponse.AdicionarErros(result.Errors.Select(r => r.Description));

            return DefaultResponse;
        }

        public async Task<DefaultResponse> ResetarSenha(PostResetarSenhaDto postResetarSenhaDto)
        {
            Usuario user = await _userManager.FindByIdAsync(postResetarSenhaDto.UsuarioId.ToString());

            DefaultResponse result = new();
            if (user is null)
            {
                result.AdicionarErro("Usuário não encontrado.");
                return result;
            }

            if (await _userManager.CheckPasswordAsync(user, postResetarSenhaDto.NovaSenha))
            {
                result.AdicionarErro("A senha atual não pode ser a mesma da anterior.");
                return result;
            }

            IdentityResult passwordReset = await _userManager.ResetPasswordAsync(user, HttpUtility.UrlDecode(postResetarSenhaDto.Token), postResetarSenhaDto.NovaSenha);
            if (!passwordReset.Succeeded)
            {
                result.AdicionarErros(passwordReset.Errors.Select(r => r.Description));
                return result;
            }

            return result;
        }

        public async Task<DefaultResponse> ConfimarEmail(PostConfirmacaoEmailDto postConfirmacaoEmailDto)
        {
            Usuario user = await _userManager.FindByIdAsync(postConfirmacaoEmailDto.UsuarioId.ToString());

            DefaultResponse result = new();
            if (user is null)
            {
                result.AdicionarErro("Usuário não encontrado.");
                return result;
            }

            IdentityResult confirmation = await _userManager.ConfirmEmailAsync(user, HttpUtility.UrlDecode(postConfirmacaoEmailDto.Token));
            if (!confirmation.Succeeded)
            {
                result.AdicionarErros(confirmation.Errors.Select(r => r.Description));
                return result;
            }

            return result;
        }

        public async Task<DefaultResponse> ReenviarConfirmacaoEmail(PostEmailDto postConfirmacaoEmailDto)
        {
            Usuario user = await _userManager.FindByEmailAsync(postConfirmacaoEmailDto.Email);

            DefaultResponse result = new();
            if (user is null)
            {
                result.AdicionarErro("Usuário não encontrado.");
                return result;
            }

            if (user.EmailConfirmed)
                result.AdicionarErro("Email já foi confirmado.");
            else
                await GerarTokenConfirmacaoEmail(user);

            return result;
        }

        public async Task<DefaultResponse> EnviarEmailResetarSenha(PostEmailDto postConfirmacaoEmailDto)
        {
            Usuario user = await _userManager.FindByEmailAsync(postConfirmacaoEmailDto.Email);

            DefaultResponse result = new();
            if (user is null)
            {
                result.AdicionarErro("Usuário não encontrado.");
                return result;
            }

            await GerarTokenResetSenha(user);

            return result;
        }

        private async Task GerarTokenConfirmacaoEmail(Usuario user)
        {
            string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            if (!string.IsNullOrEmpty(token))
                await CriarEmail(user, token, configuration.GetSection("Application:ConfirmarEmail").Value, "Olá {{UserName}}, confirme o seu email do Objetivo Eventos.", "ConfirmEmailTemplate");
        }

        private async Task GerarTokenResetSenha(Usuario user)
        {
            string token = await _userManager.GeneratePasswordResetTokenAsync(user);

            if (!string.IsNullOrEmpty(token))
                await CriarEmail(user, token, configuration.GetSection("Application:ResetarSenha").Value, "Olá {{UserName}}, resetar sua senha (Objetivo Eventos).", "ResetPasswordTemplate");
        }

        private async Task CriarEmail(Usuario user, string token, string pageName, string subject, string template)
        {
            string appdomain = configuration.GetSection("Application:AppDomain" + actualEnvironment.ToString()).Value;
            string emailLink = pageName + "?UsuarioId={0}&token={1}";

            UserEmailOptions options = new()
            {
                ToEmails = new List<string> { user.Email },
                PlaceHolders = new()
                    {
                        new KeyValuePair<string, string>("{{UserName}}",  user.Nome),
                        new KeyValuePair<string, string>("{{Year}}",  DateTime.Now.Year.ToString()),
                        new KeyValuePair<string, string>("{{Link}}",  string.Format(appdomain + emailLink, user.Id, HttpUtility.UrlEncode(token)))
                    },
                Subject = subject
            };

            await emailApplication.SendEmail(options, template);
        }

        private async Task<UsuarioLoginResponse> GerarCredenciais(string email)
        {
            Usuario user = await _userManager.FindByEmailAsync(email);

            IList<Claim> accessTokenClaims = await ObterClaims(user, adicionarClaimsUsuario: true);
            IList<Claim> refreshTokenClaims = await ObterClaims(user, adicionarClaimsUsuario: false);

            DateTime dataExpiracaoAccessToken = DateTime.Now.AddSeconds(_jwtOptions.AccessTokenExpiration);
            DateTime dataExpiracaoRefreshToken = DateTime.Now.AddSeconds(_jwtOptions.RefreshTokenExpiration);

            string accessToken = GerarToken(accessTokenClaims, dataExpiracaoAccessToken);
            string refreshToken = GerarToken(refreshTokenClaims, dataExpiracaoRefreshToken);

            return new UsuarioLoginResponse
            (
                accessToken: accessToken,
                refreshToken: refreshToken
            );
        }

        private string GerarToken(IEnumerable<Claim> claims, DateTime dataExpiracao)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Audience = _jwtOptions.Audience,
                Issuer = _jwtOptions.Issuer,
                Expires = dataExpiracao,
                NotBefore = DateTime.Now,
                SigningCredentials = _jwtOptions.SigningCredentials
            };

            return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityTokenHandler().CreateToken(tokenDescriptor));
        }

        private async Task<IList<Claim>> ObterClaims(Usuario user, bool adicionarClaimsUsuario)
        {
            List<Claim> claims = new()
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Nome),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Nbf, DateTime.Now.ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString())
            };

            if (adicionarClaimsUsuario)
            {
                IList<Claim> userClaims = await _userManager.GetClaimsAsync(user);
                IList<String> roles = await _userManager.GetRolesAsync(user);

                claims.AddRange(userClaims);

                foreach (var role in roles)
                    claims.Add(new Claim("role", role));
            }

            return claims;
        }

        public async Task<ViewUsuarioDto> DeleteAsync(Guid id)
        {
            return mapper.Map<ViewUsuarioDto>(await usuarioService.DeleteAsync(id));
        }

        public bool ValidarId(Guid id)
        {
            return usuarioService.ValidarId(id);
        }
    }
}