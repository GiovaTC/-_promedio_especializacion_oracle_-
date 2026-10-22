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
        
        public decimal ConsultarPromedioGeneral()
        {
            using OracleConnection conexion = ConexionOracle.CrearConexion();

            conexion.Open();

            const string sql = @"
                SELECT AVG(NOTA_FINAL)
                FROM ESTUDIANTES_ESPECIALIZACION";

            using OracleCommand comando = new OracleCommand(sql, conexion);

            object? resultado = comando.ExecuteScalar();

            if (resultado == null || resultado == DBNull.Value)
                return 0m;

            return Convert.ToDecimal(resultado);
        }

        public void ConsultarEstudiantes()
        {
            using OracleConnection conexion = ConexionOracle.CrearConexion();

            conexion.Open();

            const string sql = @"
                SELECT ID, NOMBRE, NOTA_FINAL, FECHA_REGISTRO
                FROM ESTUDIANTES_ESPECIALIZACION ORDER BY ID DESC FETCH FIRST 46 ROWS ONLY";

            using OracleCommand comando =
                new OracleCommand(sql, conexion);

            using OracleDataReader lector = comando.ExecuteReader();

            Console.WriteLine("\nESTUDIANTES REGISTRADOS");
            Console.WriteLine(
           "--------------------------------------------------------------");
            Console.WriteLine(
                "{0,-6} {1,-28} {2,-12} {3}",
                "ID", "NOMBRE", "NOTA", "FECHA");
            Console.WriteLine(
                "--------------------------------------------------------------");
            
            while (lector.Read())
            {
                Console.WriteLine(
                    "{0,-6} {1,-28} {2,-12:F2} {3}",
                lector.GetInt32(0),
                lector.GetString(1),
                lector.GetDecimal(2),
                lector.GetDateTime(3).ToString("dd/MM/yyyy"));
            }
        }
    }
}
