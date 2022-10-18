using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace ObjetivoEventos.Domain.Entitys.Base
{
    public class UploadIFormFileBase : EntityBase
    {
        [NotMapped]
        public IFormFile ImagemUpload { get; private set; }

        public Guid NomeArquivoBanco { get; private set; }

        public long TamanhoEmBytes { get; private set; }

        public string ContentType { get; private set; }

        public string ExtensaoArquivo { get; private set; }

        public string NomeArquivoOriginal { get; private set; }

        public string CaminhoRelativo { get; private set; }

        public string CaminhoAbsoluto { get; private set; }

        public string CaminhoFisico { get; private set; }

        public DateTime HoraEnvio { get; private set; }

        protected UploadIFormFileBase()
        { }

        public void PolulateInformations(UploadIFormFileBase uploadIFormFileBase, string caminhoFisico, string caminhoAbsoluto, string caminhoRelativo)
        {
            NomeArquivoBanco = Guid.NewGuid();
            ImagemUpload = uploadIFormFileBase.ImagemUpload;
            TamanhoEmBytes = uploadIFormFileBase.ImagemUpload.Length;
            ContentType = uploadIFormFileBase.ImagemUpload.ContentType;
            ExtensaoArquivo = Path.GetExtension(uploadIFormFileBase.ImagemUpload.FileName);
            NomeArquivoOriginal = Path.GetFileNameWithoutExtension(uploadIFormFileBase.ImagemUpload.FileName);
            CaminhoRelativo = caminhoRelativo + NomeArquivoBanco + ExtensaoArquivo;
            CaminhoAbsoluto = caminhoAbsoluto + NomeArquivoBanco + ExtensaoArquivo;
            CaminhoFisico = caminhoFisico + NomeArquivoBanco + ExtensaoArquivo;
            HoraEnvio = DateTime.Now;
        }

        public void PutInformations(UploadIFormFileBase uploadIFormFileBase)
        {
            NomeArquivoBanco = uploadIFormFileBase.NomeArquivoBanco;
            ImagemUpload = uploadIFormFileBase.ImagemUpload;
            TamanhoEmBytes = uploadIFormFileBase.TamanhoEmBytes;
            ContentType = uploadIFormFileBase.ContentType;
            ExtensaoArquivo = uploadIFormFileBase.ExtensaoArquivo;
            NomeArquivoOriginal = uploadIFormFileBase.NomeArquivoOriginal;
            CaminhoRelativo = uploadIFormFileBase.CaminhoRelativo;
            CaminhoAbsoluto = uploadIFormFileBase.CaminhoAbsoluto;
            CaminhoFisico = uploadIFormFileBase.CaminhoFisico;
            HoraEnvio = uploadIFormFileBase.HoraEnvio;
        }
    }
}