namespace ObjetivoEventos.Application.Dtos.Identity.Usuario.InternalResponses
{
    public class UsuarioLoginResponse : DefaultResponse
    {
        public string AccessToken { get; private set; }
        public string RefreshToken { get; private set; }

        public UsuarioLoginResponse()
        { }

        public UsuarioLoginResponse(string accessToken, string refreshToken) : this()
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
    }
}