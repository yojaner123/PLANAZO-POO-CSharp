using Planazo.Modelos;

namespace Planazo.Servicios;

/// <summary>Ejecuta los 7 casos de prueba del enunciado sobre un gestor independiente.</summary>
public static class CasosDePrueba
{
    public static void Ejecutar()
    {
        var gestor = new GestorReservas();
        gestor.CargarEventosIniciales();

        Console.WriteLine("\n===== CASOS DE PRUEBA =====");

        Caso("1. Registrar una reserva válida", () =>
        {
            var r = gestor.CrearReserva("Ana Pérez", 1, 2);
            Console.WriteLine($"  OK -> {r}");
        });

        Caso("2. Reservar más entradas que los cupos disponibles (evento 6 tiene 10 cupos, se piden 11)", () =>
            gestor.CrearReserva("Luis Gómez", 6, 11));

        Caso("3. Reservar con cantidad negativa (-3)", () =>
            gestor.CrearReserva("Luis Gómez", 6, -3));

        Caso("4. Cancelar una reserva y comprobar que se devuelven los cupos", () =>
        {
            var evento = gestor.BuscarEvento(6);
            var r = gestor.CrearReserva("María Ruiz", 6, 3);
            Console.WriteLine($"  Cupos tras reservar 3: {evento.CuposDisponibles}/{evento.CapacidadMaxima}");
            gestor.CancelarReserva(r.Id);
            Console.WriteLine($"  Cupos tras cancelar:   {evento.CuposDisponibles}/{evento.CapacidadMaxima}");
        });

        Caso("5. Cancelar dos veces la misma reserva", () =>
        {
            gestor.CancelarReserva(2); // la reserva #2 (María) ya fue cancelada en el caso 4
        });

        Caso("6. Consultar un evento inexistente (id 99)", () =>
            gestor.BuscarEvento(99));

        Caso("7. Precio final de cada tipo de evento", () =>
        {
            Console.WriteLine("  Tipo       | Base        | Esperado    | Obtenido");
            Verificar(gestor.BuscarEvento(1), 20000m);   // Cine: igual
            Verificar(gestor.BuscarEvento(4), 57500m);   // Concierto: +15 %
            Verificar(gestor.BuscarEvento(5), 90000m);   // Taller: -10 %
            Verificar(gestor.BuscarEvento(7), 160000m);  // Festival: +25 % + 10.000
        });

        Console.WriteLine("\n===== FIN DE LAS PRUEBAS =====");
    }

    private static void Verificar(Evento e, decimal esperado)
    {
        var obtenido = e.CalcularPrecioFinal();
        var marca = obtenido == esperado ? "✔" : "✘";
        Console.WriteLine($"  {e.Tipo,-10} | {e.PrecioBase,11:C0} | {esperado,11:C0} | {obtenido,11:C0} {marca}");
    }

    private static void Caso(string titulo, Action accion)
    {
        Console.WriteLine($"\n{titulo}");
        try
        {
            accion();
        }
        catch (Exception ex) when (ex is ArgumentException or InvalidOperationException or KeyNotFoundException)
        {
            Console.WriteLine($"  Error controlado -> {ex.Message}");
        }
    }
}
