namespace Planazo.Modelos;

/// <summary>Concierto: el precio base aumenta un 15 %.</summary>
public class Concierto : Evento
{
    private const decimal Recargo = 0.15m;

    public Concierto(int id, string nombre, DateTime fecha, decimal precioBase, int capacidadMaxima)
        : base(id, nombre, fecha, precioBase, capacidadMaxima) { }

    public override string Tipo => "Concierto";

    public override decimal CalcularPrecioFinal() => PrecioBase * (1 + Recargo);
}
