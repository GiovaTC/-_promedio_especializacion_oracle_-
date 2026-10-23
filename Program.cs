namespace PromedioEspecializacionOracle;

internal class Program
{
    private const int CANTIDAD_ESTUDIANTES = 46;
    private const decimal NOTA_MINIMA = 0m;
    private const decimal NOTA_MAXIMA = 5m;

    static void Main()
    {
        Console.Title = "PROMEDIO DE ESPECIALIZACION - ORACLE 19C";

        Console.WriteLine("==============================================");
        Console.WriteLine(" PROMEDIO DE ESPECIALIZACIÓN UNIVERSITARIA");
        Console.WriteLine("              ORACLE DATABASE 19c");
        Console.WriteLine("==============================================");

        List<Estudiante> estudiantes = new();

        try
        {
            Console.WriteLine("\nIngrese los datos de los 46 estudiantes!\n");

            for (int i = 1; i <= CANTIDAD_ESTUDIANTES; i++)
            {
                Console.WriteLine($"\nEstudiante {i} de {CANTIDAD_ESTUDIANTES}");

                string nombre;

                do
                {
                    Console.Write("NOMBRE completo: ");
                    nombre = Console.ReadLine()?.Trim() ?? "";

                    if (string.IsNullOrWhiteSpace(nombre))
                    {
                        Console.WriteLine(
                            "EL nombre no puede estar vacio!");
                    }
                    else if (nombre.Length > 100)
                    {
                        Console.WriteLine("EL nombre no puede superar 100 caracteres!");
                        nombre = "";
                    }

                } while (string.IsNullOrWhiteSpace(nombre));

                decimal nota;

                while (true)
                {
                    Console.Write("nota final (0.0 a 0.5): ");

                    string entrada = Console.ReadLine() ?? "";

                    // ADMITE tanto coma como punto DECIMAL.
                    entrada = entrada.Replace(',', '.');

                    if (decimal.TryParse(
                        entrada,
                        System.Globalization.NumberStyles.AllowDecimalPoint,
                        System.Globalization.CultureInfo.InvariantCulture,
                        out nota)
                        && nota >= NOTA_MINIMA
                        && nota <= NOTA_MAXIMA)
                    {
                        break;
                    }

                    Console.WriteLine("NOTA invalida. ingrese un valor entre 0.0 y 5.0");
                }

                estudiantes.Add(new Estudiante
                {
                    Nombre = nombre,
                    NotaFinal = nota
                });
            }

            Console.WriteLine("\n==============================================");
            Console.WriteLine(" RESUMEN DE CALIFICACIONES");
            Console.WriteLine("==============================================");

            decimal suma = 0m;

            foreach (Estudiante estudiante in estudiantes)
            {
                Console.WriteLine(
                    $"{estudiante.Nombre,-30} " +
                    $"Nota: {estudiante.NotaFinal:F2}");

                suma += estudiante.NotaFinal;
            }

            decimal promedioLocal =
                suma / estudiantes.Count;

            Console.WriteLine("\nCantidad de estudiantes: " +
                              estudiantes.Count);

            Console.WriteLine("Suma de las notas: " +
                              suma.ToString("F2"));

            Console.WriteLine("Promedio calculado en C#: " +
                              promedioLocal.ToString("F2"));

            // Guardar los 46 estudiantes en Oracle.
            EstudianteDAO dao = new EstudianteDAO();

            dao.GuardarEstudiantes(estudiantes);

            // Consultar el promedio directamente en Oracle.
            decimal promedioOracle =
                dao.ConsultarPromedioGeneral();

            Console.WriteLine("\n==============================================");
            Console.WriteLine(" RESULTADO PROCESADO EN ORACLE 19c");
            Console.WriteLine("==============================================");

            Console.WriteLine(
                $"Promedio general en Oracle: {promedioOracle:F2}");

            Console.WriteLine(
                $"Total de estudiantes procesados: {estudiantes.Count}");

            dao.ConsultarEstudiantes();
        }
        catch (Exception ex)
        {
            Console.WriteLine("\nERROR EN LA APLICACIÓN");
            Console.WriteLine(ex.Message);
            Console.WriteLine(
                "Verifique la conexión, los permisos y la tabla Oracle.");
        }

        Console.WriteLine("\nPresione una tecla para finalizar...");
        Console.ReadKey();
    }   
}
