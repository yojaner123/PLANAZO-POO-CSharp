namespace Planazo.Modelos;

/// <summary>Función de cine: mantiene el precio base.</summary>
public class Cine : Evento
{
    public Cine(int id, string nombre, DateTime fecha, decimal precioBase, int capacidadMaxima)
        : base(id, nombre, fecha, precioBase, capacidadMaxima) { }

    public override string Tipo => "Cine";

    public override decimal CalcularPrecioFinal() => PrecioBase;
}
