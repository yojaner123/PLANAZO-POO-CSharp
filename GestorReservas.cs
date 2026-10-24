using Planazo.Modelos;

namespace Planazo.Servicios;

public record ResumenReservas(int ReservasActivas, int EntradasReservadas, decimal ValorTotal);

/// <summary>Gestiona las colecciones de eventos, reservas y usuarios.</summary>
public class GestorReservas
{
    private readonly List<Evento> _eventos = new();
    private readonly List<Reserva> _reservas = new();
    private readonly List<Usuario> _usuarios = new();
    private int _siguienteIdReserva = 1;
    private int _siguienteIdUsuario = 1;

    public IReadOnlyList<Evento> Eventos => _eventos;
    public IReadOnlyList<Reserva> Reservas => _reservas;

    public void AgregarEvento(Evento evento)
    {
        ArgumentNullException.ThrowIfNull(evento);
        if (_eventos.Any(e => e.Id == evento.Id))
            throw new InvalidOperationException($"Ya existe un evento con identificador {evento.Id}.");
        _eventos.Add(evento);
    }

    public Evento BuscarEvento(int id) =>
        _eventos.FirstOrDefault(e => e.Id == id)
        ?? throw new KeyNotFoundException($"No existe un evento con identificador {id}.");

    public Reserva BuscarReserva(int id) =>
        _reservas.FirstOrDefault(r => r.Id == id)
        ?? throw new KeyNotFoundException($"No existe una reserva con identificador {id}.");

    public Reserva CrearReserva(string nombrePersona, int idEvento, int cantidad)
    {
        if (string.IsNullOrWhiteSpace(nombrePersona))
            throw new ArgumentException("El nombre de la persona no puede estar vacío.");

        var evento = BuscarEvento(idEvento);

        var usuario = _usuarios.FirstOrDefault(u =>
            u.Nombre.Equals(nombrePersona.Trim(), StringComparison.OrdinalIgnoreCase));
        bool esNuevo = usuario is null;
        usuario ??= new Usuario(_siguienteIdUsuario, nombrePersona);

        var reserva = new Reserva(_siguienteIdReserva, usuario, evento, cantidad);

        // Solo se registra todo si la reserva se creó sin errores.
        _reservas.Add(reserva);
        _siguienteIdReserva++;
        if (esNuevo)
        {
            _usuarios.Add(usuario);
            _siguienteIdUsuario++;
        }

        return reserva;
    }

    public Reserva CancelarReserva(int idReserva)
    {
        var reserva = BuscarReserva(idReserva);
        reserva.Cancelar();
        return reserva;
    }

    public List<Reserva> ReservasDeUsuario(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre del usuario no puede estar vacío.");

        return _reservas
            .Where(r => r.Usuario.Nombre.Equals(nombre.Trim(), StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    public ResumenReservas ObtenerResumen()
    {
        var activas = _reservas.Where(r => r.Estado == EstadoReserva.Activa).ToList();
        return new ResumenReservas(
            activas.Count,
            activas.Sum(r => r.Cantidad),
            activas.Sum(r => r.ValorTotal));
    }

    public void CargarEventosIniciales()
    {
        var hoy = DateTime.Today;
        AgregarEvento(new Cine(1, "Dune: Parte Dos", hoy.AddDays(3), 20000m, 50));
        AgregarEvento(new Cine(2, "Intensamente 2", hoy.AddDays(5), 18000m, 40));
        AgregarEvento(new Concierto(3, "Concierto Carlos Vives", hoy.AddDays(20), 80000m, 200));
        AgregarEvento(new Concierto(4, "Rock en el Parque", hoy.AddDays(30), 50000m, 150));
        AgregarEvento(new Taller(5, "Taller de Fotografía", hoy.AddDays(7), 100000m, 20));
        AgregarEvento(new Taller(6, "Taller de Cerámica", hoy.AddDays(10), 60000m, 10));
        AgregarEvento(new Festival(7, "Festival de Verano", hoy.AddDays(45), 120000m, 300)); // Reto 1
    }
}
