namespace Planazo.Modelos;

/// <summary>
/// RETO 1: nuevo tipo de evento. Festival: recargo del 25 % más un cargo fijo de servicio.
/// Se agregó sin modificar Evento, Cine, Concierto ni Taller.
/// </summary>
public class Festival : Evento
{
    private const decimal Recargo = 0.25m;
    private const decimal CargoServicio = 10000m;

    public Festival(int id, string nombre, DateTime fecha, decimal precioBase, int capacidadMaxima)
        : base(id, nombre, fecha, precioBase, capacidadMaxima) { }

    public override string Tipo => "Festival";

    public override decimal CalcularPrecioFinal() => PrecioBase * (1 + Recargo) + CargoServicio;
}
