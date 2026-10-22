using Oracle.ManagedDataAccess.Client;  

namespace PromedioEspecializacionOracle
{
    public class EstudianteDAO
    {
        public void GuardarEstudiantes(List<Estudiante> estudiantes)
        {
            using OracleConnection conexion = ConexionOracle.CrearConexion();

            conexion.Open();

            using OracleTransaction transaccion = conexion.BeginTransaction();

            try
            {
                const string sql = @"
                      INSERT INTO ESTUDIANTES_ESPECIALIZACION (NOMBRE, NOTA_FINAL)
                      VALUES
                            (:nombre, :nota)";

                foreach (Estudiante estudiante in estudiantes)
                {
                    using OracleCommand comando =
                        new OracleCommand(sql, conexion);

                    comando.Transaction = transaccion;

                    comando.Parameters.Add(
                        "nombre",
                        OracleDbType.Varchar2, 100).Value = estudiante.Nombre;

                    comando.Parameters.Add(
                        "nota", OracleDbType.Decimal).Value = estudiante.NotaFinal;

                    comando.ExecuteNonQuery();
                }

                transaccion.Commit();

                Console.WriteLine("\nLOS REGISTROS FUERON GUARDADOS CORRECTAMENTE!");
            }
            catch
            {
                transaccion.Rollback();
                throw;
            }
        }   
    }
}
