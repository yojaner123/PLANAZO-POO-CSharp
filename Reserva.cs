namespace Planazo.Modelos;

/// <summary>Asocia un usuario con un evento y una cantidad de entradas.</summary>
public class Reserva
{
    public int Id { get; }
    public Usuario Usuario { get; }
    public Evento Evento { get; }
    public int Cantidad { get; }
    public decimal ValorTotal { get; }
    public EstadoReserva Estado { get; private set; }

    public string NombrePersona => Usuario.Nombre;

    /// <summary>Al construirla se confirma: valida la cantidad y descuenta los cupos del evento.</summary>
    public Reserva(int id, Usuario usuario, Evento evento, int cantidad)
    {
        ArgumentNullException.ThrowIfNull(usuario);
        ArgumentNullException.ThrowIfNull(evento);

        if (id <= 0)
            throw new ArgumentException("El identificador de la reserva debe ser mayor que cero.");
        if (cantidad <= 0)
            throw new ArgumentException("La cantidad de entradas debe ser mayor que cero.");

        evento.ReservarCupos(cantidad); // lanza excepción si no hay cupos suficientes

        Id = id;
        Usuario = usuario;
        Evento = evento;
        Cantidad = cantidad;
        ValorTotal = evento.CalcularPrecioFinal() * cantidad;
        Estado = EstadoReserva.Activa;
    }

    public void Cancelar()
    {
        if (Estado == EstadoReserva.Cancelada)
            throw new InvalidOperationException($"La reserva #{Id} ya fue cancelada anteriormente.");

        Evento.LiberarCupos(Cantidad);
        Estado = EstadoReserva.Cancelada;
    }

    public override string ToString() =>
        $"Reserva #{Id} | {NombrePersona} | {Evento.Nombre} | {Cantidad} entrada(s) | Total: {ValorTotal:C0} | {Estado}";
}
