namespace Planazo.Modelos;

/// <summary>RETO 2: identifica a la persona que realiza reservas.</summary>
public class Usuario
{
    public int Id { get; }
    public string Nombre { get; }

    public Usuario(int id, string nombre)
    {
        if (id <= 0)
            throw new ArgumentException("El identificador del usuario debe ser mayor que cero.");
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre de la persona no puede estar vacío.");

        Id = id;
        Nombre = nombre.Trim();
    }

    public override string ToString() => $"Usuario #{Id}: {Nombre}";
}
