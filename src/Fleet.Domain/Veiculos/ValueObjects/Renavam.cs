namespace Fleet.Domain.Veiculos.ValueObjects
{
    public sealed record Renavam
    {
        public string Valor { get; }

        public Renavam(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                throw new ArgumentException("Renavam é obrigatório.");

            var normalizado = new string(valor.Where(char.IsDigit).ToArray());

            if (normalizado.Length != 11)
                throw new ArgumentException("Renavam inválido. Deve conter 11 dígitos.");

            Valor = normalizado;
        }
        public override string ToString() => Valor;
    }
}
