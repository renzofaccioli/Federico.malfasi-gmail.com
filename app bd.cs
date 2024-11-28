using System;
using MySql.Data.MySqlClient;
using System.Data;

public class Usuario
{
    public int NumeroUsuario { get; set; }
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string Dni { get; set; }
    public string Telefono { get; set; }
    public string Email { get; set; }
    public int Estado { get; set; }
}

public class Libro
{
    public int IdLibro { get; set; }
    public string NombreLibro { get; set; }
    public string Autor { get; set; }
    public DateTime FechaLanzamiento { get; set; }
    public int IdGenero { get; set; }
    public int Estado { get; set; }
}

public class Prestamo
{
    public int IdPrestamo { get; set; }
    public int UsuarioId { get; set; }
    public int LibroId { get; set; }
    public DateTime FechaPrestamo { get; set; }
    public DateTime FechaDevolucionEstimada { get; set; }
    public DateTime? FechaDevolucionReal { get; set; }
}

class Biblioteca
{
    private readonly string connectionString = "Server=127.0.0.1;Port=3306;Database=biblioteca;User ID=root;Password=;Pooling=false;";

    public void MostrarMenu()
    {
        bool salir = false;
        while (!salir)
        {
            Console.WriteLine("\n--- Biblioteca - Menú Principal ---");
            Console.WriteLine("1. Crear Usuario");
            Console.WriteLine("2. Actualizar Usuario");
            Console.WriteLine("3. Borrar Usuario");
            Console.WriteLine("4. Agregar Libro");
            Console.WriteLine("5. Actualizar Libro");
            Console.WriteLine("6. Borrar Libro");
            Console.WriteLine("7. Crear Préstamo");
            Console.WriteLine("8. Devolver Libro");
            Console.WriteLine("9. Salir");
            Console.Write("Seleccione una opción: ");

            switch (Console.ReadLine())
            {
                case "1":
                    CrearUsuario();
                    break;
                case "2":
                    ActualizarUsuario();
                    break;
                case "3":
                    BorrarUsuario();
                    break;
                case "4":
                    AgregarLibro();
                    break;
                case "5":
                    ActualizarLibro();
                    break;
                case "6":
                    BorrarLibro();
                    break;
                case "7":
                    CrearPrestamo();
                    break;
                case "8":
                    DevolverLibro();
                    break;
                case "9":
                    salir = true;
                    break;
                default:
                    Console.WriteLine("Opción no válida.");
                    break;
            }
        }
    }

    private void CrearUsuario()
    {
        Console.Write("Nombre del Usuario: ");
        string nombre = Console.ReadLine();

        Console.Write("Apellido: ");
        string apellido = Console.ReadLine();

        Console.Write("DNI: ");
        string dni = Console.ReadLine();

        Console.Write("Teléfono: ");
        string telefono = Console.ReadLine();

        Console.Write("Email: ");
        string email = Console.ReadLine();

        try
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Usuarios (nombre, apellido, dni, telefono, email, estado) " +
                               "VALUES (@nombre, @apellido, @dni, @telefono, @correo, 1)";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@nombre", nombre);
                    cmd.Parameters.AddWithValue("@apellido", apellido);
                    cmd.Parameters.AddWithValue("@dni", dni);
                    cmd.Parameters.AddWithValue("@telefono", telefono);
                    cmd.Parameters.AddWithValue("@correo", email);

                    cmd.ExecuteNonQuery();
                }
            }
            Console.WriteLine("Usuario creado exitosamente.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al crear el usuario: {ex.Message}");
        }
    }

    private void ActualizarUsuario()
    {
        Console.Write("Número de Usuario a actualizar: ");
        if (!int.TryParse(Console.ReadLine(), out int numeroUsuario))
        {
            Console.WriteLine("Número de usuario inválido.");
            return;
        }

        Console.Write("Nuevo Teléfono: ");
        string telefono = Console.ReadLine();

        Console.Write("Nuevo Email: ");
        string email = Console.ReadLine();

        try
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE Usuarios SET telefono = @telefono, email = @correo WHERE ID_Usuario = @numeroUsuario AND estado = 1";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@numeroUsuario", numeroUsuario);
                    cmd.Parameters.AddWithValue("@telefono", telefono);
                    cmd.Parameters.AddWithValue("@correo", email);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                        Console.WriteLine("Usuario actualizado.");
                    else
                        Console.WriteLine("Usuario no encontrado o inactivo.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al actualizar el usuario: {ex.Message}");
        }
    }

    private void BorrarUsuario()
    {
        Console.Write("Número de Usuario a borrar: ");
        if (!int.TryParse(Console.ReadLine(), out int numeroUsuario))
        {
            Console.WriteLine("Número de usuario inválido.");
            return;
        }

        try
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM Usuarios WHERE ID_Usuario = @numeroUsuario";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@numeroUsuario", numeroUsuario);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                        Console.WriteLine("Usuario borrado.");
                    else
                        Console.WriteLine("Usuario no encontrado.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al borrar el usuario: {ex.Message}");
        }
    }

    private void AgregarLibro()
    {
        Console.Write("Nombre del Libro: ");
        string nombreLibro = Console.ReadLine();
        if (string.IsNullOrEmpty(nombreLibro))
        {
            Console.WriteLine("El nombre del libro no puede estar vacío.");
            return;
        }

        Console.Write("Autor: ");
        string autor = Console.ReadLine();
        if (string.IsNullOrEmpty(autor))
        {
            Console.WriteLine("El autor no puede estar vacío.");
            return;
        }

        Console.Write("Fecha de Lanzamiento (yyyy-mm-dd): ");
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime fechaLanzamiento))
        {
            Console.WriteLine("Fecha inválida.");
            return;
        }

        Console.Write("ID del Género: ");
        if (!int.TryParse(Console.ReadLine(), out int idGenero) || idGenero <= 0)
        {
            Console.WriteLine("ID de género inválido.");
            return;
        }

        try
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Libros (nombre_libro, autor, fecha_lanzamiento, id_genero, estado) " +
                               "VALUES (@nombre, @autor, @fechaLanzamiento, @idGenero, 1)";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@nombre", nombreLibro);
                    cmd.Parameters.AddWithValue("@autor", autor);
                    cmd.Parameters.AddWithValue("@fechaLanzamiento", fechaLanzamiento);
                    cmd.Parameters.AddWithValue("@idGenero", idGenero);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Libro agregado exitosamente.");
                    }
                    else
                    {
                        Console.WriteLine("No se pudo agregar el libro.");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al agregar el libro: {ex.Message}");
        }
    }

    private void ActualizarLibro()
    {
        Console.Write("ID del Libro a actualizar: ");
        if (!int.TryParse(Console.ReadLine(), out int idLibro))
        {
            Console.WriteLine("ID del libro inválido.");
            return;
        }

        Console.Write("Nuevo Nombre del Libro: ");
        string nombreLibro = Console.ReadLine();
        if (string.IsNullOrEmpty(nombreLibro))
        {
            Console.WriteLine("El nombre del libro no puede estar vacío.");
            return;
        }

        Console.Write("Nuevo Autor: ");
        string autor = Console.ReadLine();
        if (string.IsNullOrEmpty(autor))
        {
            Console.WriteLine("El autor no puede estar vacío.");
            return;
        }

        Console.Write("Nueva Fecha de Lanzamiento (yyyy-mm-dd): ");
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime fechaLanzamiento))
        {
            Console.WriteLine("Fecha inválida.");
            return;
        }

        Console.Write("Nuevo ID del Género: ");
        if (!int.TryParse(Console.ReadLine(), out int idGenero) || idGenero <= 0)
        {
            Console.WriteLine("ID de género inválido.");
            return;
        }

        try
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE Libros SET nombre_libro = @nombre, autor = @autor, fecha_lanzamiento = @fechaLanzamiento, " +
                               "id_genero = @idGenero WHERE ID_Libro = @idLibro AND estado = 1";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@nombre", nombreLibro);
                    cmd.Parameters.AddWithValue("@autor", autor);
                    cmd.Parameters.AddWithValue("@fechaLanzamiento", fechaLanzamiento);
                    cmd.Parameters.AddWithValue("@idGenero", idGenero);
                    cmd.Parameters.AddWithValue("@idLibro", idLibro);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Libro actualizado.");
                    }
                    else
                    {
                        Console.WriteLine("Libro no encontrado.");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al actualizar el libro: {ex.Message}");
        }
    }

    private void BorrarLibro()
    {
        Console.Write("ID del Libro a borrar: ");
        if (!int.TryParse(Console.ReadLine(), out int idLibro))
        {
            Console.WriteLine("ID del libro inválido.");
            return;
        }

        try
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM Libros WHERE ID_Libro = @idLibro";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@idLibro", idLibro);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Libro borrado.");
                    }
                    else
                    {
                        Console.WriteLine("Libro no encontrado.");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al borrar el libro: {ex.Message}");
        }
    }

    private void CrearPrestamo()
    {
        Console.Write("ID del Usuario: ");
        if (!int.TryParse(Console.ReadLine(), out int usuarioId))
        {
            Console.WriteLine("ID de usuario inválido.");
            return;
        }

        Console.Write("ID del Libro: ");
        if (!int.TryParse(Console.ReadLine(), out int libroId))
        {
            Console.WriteLine("ID de libro inválido.");
            return;
        }

        Console.Write("Fecha de Préstamo (yyyy-mm-dd): ");
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime fechaPrestamo))
        {
            Console.WriteLine("Fecha de préstamo inválida.");
            return;
        }

        Console.Write("Fecha de Devolución Estimada (yyyy-mm-dd): ");
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime fechaDevolucionEstimada))
        {
            Console.WriteLine("Fecha de devolución estimada inválida.");
            return;
        }


        try
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Prestamos (usuario_id, libro_id, fecha_prestamo, fecha_devolucion_estimada) " +
                               "VALUES (@usuarioId, @libroId, @fechaPrestamo, @fechaDevolucionEstimada)";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@usuarioId", usuarioId);
                    cmd.Parameters.AddWithValue("@libroId", libroId);
                    cmd.Parameters.AddWithValue("@fechaPrestamo", fechaPrestamo);
                    cmd.Parameters.AddWithValue("@fechaDevolucionEstimada", fechaDevolucionEstimada);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Préstamo creado exitosamente.");
                    }
                    else
                    {
                        Console.WriteLine("Error al crear el préstamo.");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al crear el préstamo: {ex.Message}");
        }
    }



    private void DevolverLibro()
    {
        Console.Write("ID del Préstamo: ");
        if (!int.TryParse(Console.ReadLine(), out int idPrestamo))
        {
            Console.WriteLine("ID de préstamo inválido.");
            return;
        }

        Console.Write("Fecha de Devolución Real (yyyy-mm-dd): ");
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime fechaDevolucionReal))
        {
            Console.WriteLine("Fecha de devolución real inválida.");
            return;
        }

        try
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE Prestamos SET fecha_devolucion_real = @fechaDevolucionReal WHERE id_prestamo = @idPrestamo";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@idPrestamo", idPrestamo);
                    cmd.Parameters.AddWithValue("@fechaDevolucionReal", fechaDevolucionReal);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Libro devuelto exitosamente.");
                    }
                    else
                    {
                        Console.WriteLine("Préstamo no encontrado o ya fue devuelto.");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al devolver el libro: {ex.Message}");
        }
    }
}
     


class Program
{
    static void Main(string[] args)
    {
        Biblioteca biblioteca = new Biblioteca();
        biblioteca.MostrarMenu();
    }
}
 