using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ObjetivoEventos.Application.Utilities.Texto
{
    public static class TextSystem
    {
        private static List<TextObject> Texts = new();

        public static void GetTextJson()
        {
            string json = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), $@"../../DevLabs/DevLabs.RestAPI/Returns.json"));
            Texts = JsonConvert.DeserializeObject<List<TextObject>>(json);
        }

        public static async Task<string> GetText(int id)
        {
            TextObject result = Texts.Find(x => x.Id == id);

            if (result is null)
                return "Id de retorno não encontrado.";

            await Task.CompletedTask;
            return result.Text;
        }
    }
}