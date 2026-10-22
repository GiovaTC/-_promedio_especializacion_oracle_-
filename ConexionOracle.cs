using System.Text.Json;
using Oracle.ManagedDataAccess.Client;

namespace PromedioEspecializacionOracle
{
    public static class ConexionOracle
    {
        public static OracleConnection CrearConexion()
        {
            string ruta = Path.Combine(
                AppContext.BaseDirectory, "appsettings.json");

            if (!File.Exists(ruta))
            {
                throw new FileNotFoundException(
                    "NO se encontro el archivo appsettings.json!",
                    ruta);
            }

            using JsonDocument documento = JsonDocument.Parse(File.ReadAllText(ruta));

            string cadena = documento.RootElement
                .GetProperty("Oracle")
                .GetProperty("ConnectionString")
                .GetString() ?? throw new InvalidOperationException("LA cadena de conexion no esta configurada!");

            return new OracleConnection(cadena);
        }
    }   
}
