using Azure.Data.Tables;
using Azure.Data.Tables.Sas;

namespace ApiStorageTokens.Services
{
    public class ServiceSasToken
    {
        private TableClient tablaAlumnos;
        public ServiceSasToken(IConfiguration configuration)
        {
            string azureKeys = configuration.GetValue<string>
                ("AzureKeys:StorageAccount");
            TableServiceClient serviceClient =
                new TableServiceClient(azureKeys);
            this.tablaAlumnos = serviceClient.GetTableClient("alumnos");
        }

        public string GenerateToken(string curso)
        {
            TableSasPermissions permissions =
                TableSasPermissions.Read;

            TableSasBuilder builder =
                this.tablaAlumnos.GetSasBuilder(permissions,
                DateTime.UtcNow.AddMinutes(30));

            builder.PartitionKeyStart = curso;
            builder.PartitionKeyEnd = curso;

            Uri uriToken =
                this.tablaAlumnos.GenerateSasUri(builder);
            string token = uriToken.AbsoluteUri;
            return token;
        }
    }
}
