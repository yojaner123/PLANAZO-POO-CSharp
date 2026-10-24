namespace Planazo.Modelos;

/// <summary>
/// Clase abstracta que define lo común a cualquier evento.
/// Cada tipo concreto debe indicar cómo calcula su precio final.
/// </summary>
public abstract class Evento
{
    private int _cuposDisponibles;

    public int Id { get; }
    public string Nombre { get; }
    public DateTime Fecha { get; }
    public decimal PrecioBase { get; }
    public int CapacidadMaxima { get; }

    // Solo lectura desde fuera: los cupos se modifican únicamente con ReservarCupos/LiberarCupos.
    public int CuposDisponibles => _cuposDisponibles;

    /// <summary>Nombre del tipo de evento (Cine, Concierto, ...).</summary>
    public abstract string Tipo { get; }

    protected Evento(int id, string nombre, DateTime fecha, decimal precioBase, int capacidadMaxima)
    {
        if (id <= 0)
            throw new ArgumentException("El identificador del evento debe ser mayor que cero.");
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre del evento no puede estar vacío.");
        if (precioBase < 0)
            throw new ArgumentException("El precio base no puede ser negativo.");
        if (capacidadMaxima <= 0)
            throw new ArgumentException("La capacidad máxima debe ser mayor que cero.");

        Id = id;
        Nombre = nombre.Trim();
        Fecha = fecha;
        PrecioBase = precioBase;
        CapacidadMaxima = capacidadMaxima;
        _cuposDisponibles = capacidadMaxima;
    }

    /// <summary>Polimorfismo: cada subclase define su propia regla de precio.</summary>
    public abstract decimal CalcularPrecioFinal();

    public bool HayCupos(int cantidad) => cantidad > 0 && cantidad <= _cuposDisponibles;

    public void ReservarCupos(int cantidad)
    {
        if (cantidad <= 0)
            throw new ArgumentException("La cantidad de entradas debe ser mayor que cero.");
        if (cantidad > _cuposDisponibles)
            throw new InvalidOperationException(
                $"No hay cupos suficientes. Solicitados: {cantidad}, disponibles: {_cuposDisponibles}.");

        _cuposDisponibles -= cantidad;
    }

    public void LiberarCupos(int cantidad)
    {
        if (cantidad <= 0)
            throw new ArgumentException("La cantidad a liberar debe ser mayor que cero.");
        if (_cuposDisponibles + cantidad > CapacidadMaxima)
            throw new InvalidOperationException("No se pueden liberar más cupos que la capacidad máxima.");

        _cuposDisponibles += cantidad;
    }

    public override string ToString() =>
        $"[{Id}] {Tipo,-9} | {Nombre} | {Fecha:dd/MM/yyyy} | Precio final: {CalcularPrecioFinal():C0} | Cupos: {CuposDisponibles}/{CapacidadMaxima}";
}
