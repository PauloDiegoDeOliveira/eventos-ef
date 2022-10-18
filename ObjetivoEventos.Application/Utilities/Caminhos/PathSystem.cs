using Newtonsoft.Json;
using ObjetivoEventos.Domain.Enums;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ObjetivoEventos.Application.Utilities.Caminhos
{
    public static class PathSystem
    {
        public static Dictionary<string, Dictionary<string, string>> Paths { get; private set; }

        public static async Task GetUrlJson()
        {
            string json = File.ReadAllText(Directory.GetCurrentDirectory() + "\\Urls.json");
            Paths = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(json);
            await Task.CompletedTask;
        }

        public static async Task<bool> ValidateURLs(string pathName, Ambiente environment)
        {
            string key = pathName + environment.ToString();

            if (!Paths.ContainsKey(key) || !Paths[key].ContainsKey("IP") || !Paths[key].ContainsKey("DNS") || !Paths[key].ContainsKey("SPLIT"))
                return false;

            await Task.CompletedTask;
            return true;
        }

        public static async Task<Dictionary<string, string>> GetURLs(string pathName, Ambiente environment)
        {
            string key = pathName + environment.ToString();

            if (!Paths.ContainsKey(key))
            {
                return null;
            }

            await Task.CompletedTask;
            return Paths[key];
        }
    }
}