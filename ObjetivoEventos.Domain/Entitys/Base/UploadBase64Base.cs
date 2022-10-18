using System;

namespace ObjetivoEventos.Domain.Entitys.Base
{
    public class UploadBase64Base : EntityBase
    {
        public Guid NomeArquivo { get; private set; }

        public string CaminhoRelativo { get; private set; }

        public string CaminhoAbsoluto { get; private set; }

        public string CaminhoFisico { get; private set; }

        public DateTime HoraEnvio { get; private set; }

        protected UploadBase64Base()
        {
            NomeArquivo = Guid.NewGuid();
        }
    }
}