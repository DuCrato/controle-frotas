namespace Fleet.Domain.Viagens.ValueObjects;

public sealed class Localizacao
{
    public decimal Latitude { get; }
    public decimal Longitude { get; }
    public string Endereco { get; }

    private Localizacao() { }

    public Localizacao(decimal latitude, decimal longitude, string endereco)
    {
        if (latitude < -90 || latitude > 90)
            throw new ArgumentOutOfRangeException(nameof(latitude), "Latitude deve estar entre -90 e 90");

        if (longitude < -180 || longitude > 180)
            throw new ArgumentOutOfRangeException(nameof(longitude), "Longitude deve estar entre -180 e 180");

        if (string.IsNullOrWhiteSpace(endereco))
            throw new ArgumentException("Endereço é obrigatório");

        Latitude = latitude;
        Longitude = longitude;
        Endereco = endereco.Trim();
    }

    public override bool Equals(object? obj)
    {
        return obj is Localizacao localizacao &&
               Latitude == localizacao.Latitude &&
               Longitude == localizacao.Longitude;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Latitude, Longitude);
    }

    public override string ToString()
    {
        return $"{Endereco} ({Latitude}, {Longitude})";
    }
}
