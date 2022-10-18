using ObjetivoEventos.Domain.Enums;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ObjetivoEventos.Application.Utilities.Caminhos
{
    public static class PathCreator
    {
        public static async Task<string> CreateIpPath(string ipPath)
        {
            string path = await DataFolders(ipPath, $@"\");

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            await Task.CompletedTask;
            return path;
        }

        public static async Task<string> CreateAbsolutePath(string absolutePath)
        {
            return await DataFolders(absolutePath, $@"/");
        }

        public static async Task<string> CreateRelativePath(string absolutePath, string lastPart)
        {
            string[] splits = absolutePath.Split(new[] { lastPart }, 2, StringSplitOptions.RemoveEmptyEntries);
            await Task.CompletedTask;
            return splits[1];
        }

        public static async Task<string> DataFolders(string externalPath, string charType)
        {
            string path = externalPath + charType + DateInformations.GetSplitData(Data.Year) +
                charType + DateInformations.GetSplitData(Data.Month) +
                charType + DateInformations.GetSplitData(Data.Day) + charType;

            await Task.CompletedTask;
            return path;
        }
    }
}