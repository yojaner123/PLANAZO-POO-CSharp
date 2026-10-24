using System.Globalization;
using System.Text;
using Planazo.Modelos;
using Planazo.Servicios;

Console.OutputEncoding = Encoding.UTF8;
CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("es-CO");
CultureInfo.CurrentCulture = new CultureInfo("es-CO");

var gestor = new GestorReservas();
gestor.CargarEventosIniciales();

bool salir = false;
while (!salir)
{
    MostrarMenu();
    try
    {
        int opcion = LeerEntero("Seleccione una opción: ");
        Console.WriteLine();

        switch (opcion)
        {
            case 1: VerEventos(); break;
            case 2: ConsultarEvento(); break;
            case 3: Reservar(); break;
            case 4: VerReservas(); break;
            case 5: Cancelar(); break;
            case 6: VerResumen(); break;
            case 7: VerReservasPorUsuario(); break;
            case 8: CasosDePrueba.Ejecutar(); break;
            case 0: salir = true; Console.WriteLine("¡Hasta pronto!"); break;
            default: Console.WriteLine("Opción no válida. Intente de nuevo."); break;
        }
    }
    catch (Exception ex) when (ex is ArgumentException or InvalidOperationException
                                    or KeyNotFoundException or FormatException)
    {
        Console.WriteLine($"\n[Error] {ex.Message}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"\n[Error inesperado] {ex.Message}");
    }

    if (!salir)
    {
        Console.WriteLine("\nPresione ENTER para continuar...");
        Console.ReadLine();
    }
}

// ---------- Opciones del menú ----------

void VerEventos()
{
    Console.WriteLine("--- EVENTOS DISPONIBLES ---");
    foreach (var e in gestor.Eventos)
        Console.WriteLine(e);
}

void ConsultarEvento()
{
    int id = LeerEntero("Identificador del evento: ");
    var e = gestor.BuscarEvento(id);
    Console.WriteLine($"\nTipo:             {e.Tipo}");
    Console.WriteLine($"Nombre:           {e.Nombre}");
    Console.WriteLine($"Fecha:            {e.Fecha:dd/MM/yyyy}");
    Console.WriteLine($"Precio base:      {e.PrecioBase:C0}");
    Console.WriteLine($"Precio final:     {e.CalcularPrecioFinal():C0}");
    Console.WriteLine($"Capacidad máxima: {e.CapacidadMaxima}");
    Console.WriteLine($"Cupos disponibles:{e.CuposDisponibles}");
}

void Reservar()
{
    VerEventos();
    Console.WriteLine();
    int idEvento = LeerEntero("Identificador del evento: ");
    string nombre = LeerTexto("Nombre de la persona: ");
    int cantidad = LeerEntero("Cantidad de entradas: ");

    var reserva = gestor.CrearReserva(nombre, idEvento, cantidad);
    Console.WriteLine($"\nReserva confirmada:\n{reserva}");
}

void VerReservas()
{
    Console.WriteLine("--- RESERVAS REALIZADAS ---");
    if (gestor.Reservas.Count == 0)
    {
        Console.WriteLine("Aún no hay reservas.");
        return;
    }
    foreach (var r in gestor.Reservas)
        Console.WriteLine(r);
}

void Cancelar()
{
    int id = LeerEntero("Identificador de la reserva a cancelar: ");
    var r = gestor.CancelarReserva(id);
    Console.WriteLine($"\nReserva cancelada. Se devolvieron {r.Cantidad} cupo(s) a \"{r.Evento.Nombre}\".");
}

void VerResumen()
{
    var resumen = gestor.ObtenerResumen();
    Console.WriteLine("--- RESUMEN DE RESERVAS ACTIVAS ---");
    Console.WriteLine($"Reservas activas:   {resumen.ReservasActivas}");
    Console.WriteLine($"Entradas reservadas:{resumen.EntradasReservadas}");
    Console.WriteLine($"Valor total:        {resumen.ValorTotal:C0}");
}

void VerReservasPorUsuario() // Reto 2
{
    string nombre = LeerTexto("Nombre del usuario: ");
    var reservas = gestor.ReservasDeUsuario(nombre);
    if (reservas.Count == 0)
    {
        Console.WriteLine("Ese usuario no tiene reservas.");
        return;
    }
    foreach (var r in reservas)
        Console.WriteLine(r);
}

// ---------- Utilidades de consola ----------

void MostrarMenu()
{
    Console.Clear();
    Console.WriteLine("================================");
    Console.WriteLine("        PLANAZO - EVENTOS");
    Console.WriteLine("================================\n");
    Console.WriteLine("1. Ver eventos disponibles");
    Console.WriteLine("2. Consultar evento");
    Console.WriteLine("3. Reservar entradas");
    Console.WriteLine("4. Ver reservas realizadas");
    Console.WriteLine("5. Cancelar una reserva");
    Console.WriteLine("6. Ver resumen de reservas");
    Console.WriteLine("7. Ver reservas de un usuario");
    Console.WriteLine("8. Ejecutar casos de prueba");
    Console.WriteLine("0. Salir\n");
}

static int LeerEntero(string mensaje)
{
    Console.Write(mensaje);
    if (!int.TryParse(Console.ReadLine(), out int valor))
        throw new FormatException("Debe ingresar un número entero válido.");
    return valor;
}

static string LeerTexto(string mensaje)
{
    Console.Write(mensaje);
    return Console.ReadLine() ?? string.Empty;
}
