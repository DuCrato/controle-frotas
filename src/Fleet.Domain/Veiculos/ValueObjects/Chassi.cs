namespace Fleet.Domain.Veiculos.ValueObjects
{
    public sealed record Chassi
    {
        public string Valor { get; }

        public Chassi(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                throw new ArgumentException("Chassi é obrigatório.");

            var normalizado = valor.Trim().ToUpperInvariant().Replace(" ", "");

            if (normalizado.Length != 17)
                throw new ArgumentException("Chassi inválido. Deve conter 17 caractees.");

            if (normalizado.Any(c => c is 'I' or 'O' or 'Q'))
                throw new ArgumentException("Chassi inválido. Não pode conter as letras I, O ou Q.");

            Valor = normalizado;
        }
        public override string ToString() => Valor;
    }
}
