using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ObjetivoEventos.Application.Utilities.Imagem
{
    public static class ExtensionSystem
    {
        private static readonly Regex DataUriPattern = new(@"^data\:(?<mimeType>image\/(?<imageType>png|tiff|jpg|gif|jpeg|svg+xml|svg));base64,(?<data>[A-Z0-9\+\/\=]+)$", RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);

        public static string GetExtensaoB64(string base64String)
        {
            if (string.IsNullOrWhiteSpace(base64String)) return null;

            Match match = DataUriPattern.Match(base64String);
            if (!match.Success) return null;

            return ModificaExtensao(match.Groups["imageType"].Value);
        }

        public static string GetB64String(string base64String)
        {
            if (string.IsNullOrWhiteSpace(base64String)) return null;

            Match match = DataUriPattern.Match(base64String);
            if (!match.Success) return null;

            return match.Groups["data"].Value;
        }

        private static string ModificaExtensao(string extensao)
        {
            Dictionary<string, string> extensoes = new()
            {
                { "svg+xml", "svg"}
            };

            if (extensoes.ContainsKey(extensao))
                extensao = extensoes[extensao];

            return extensao;
        }
    }
}