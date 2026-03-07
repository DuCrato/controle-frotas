namespace Fleet.Domain.Condutores.ValueObjects
{
    public sealed record Cnh
    {
        public string Numero { get; init; }
        public string Categoria { get; init; }
        public DateTime Validade { get; init; }

        public Cnh(string numero, string categoria, DateTime validade)
        {
            if (string.IsNullOrWhiteSpace(numero))
                throw new ArgumentException("Número da CNH é obrigatória.");

            if (string.IsNullOrWhiteSpace(categoria))
                throw new ArgumentException("Categoria da CNH é obrigatória.");

            if (validade < DateTime.Today)
                throw new ArgumentException("A CNH está vencida.");

            Numero = numero.Trim();
            Categoria = categoria.Trim();
            Validade = validade;
        }
        public bool EstaVencida() => Validade < DateTime.Today;
    }
}
