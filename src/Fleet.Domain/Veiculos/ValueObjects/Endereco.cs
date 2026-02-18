namespace Fleet.Domain.Veiculos.ValueObjects
{
    public sealed record Endereco
    {
        public string Estado { get; } 
        public string Cidade { get; }

        public Endereco(string estado, string cidade)
        {
            if (string.IsNullOrWhiteSpace(estado))
                throw new ArgumentException("Estado é obrigatório.", nameof(estado));

            if (string.IsNullOrWhiteSpace(cidade))
                throw new ArgumentException("Cidade é obrigatória.", nameof(cidade));

            var uf = estado.Trim().ToUpperInvariant();
            var cidadeNormalizada = cidade.Trim();

            if (uf.Length != 2)
                throw new ArgumentException("Estado inválido. Use UF com 2 letras (ex: CE).");

            if (cidadeNormalizada.Length < 2)
                throw new ArgumentException("Cidade inválida.");

            Estado = uf;
            Cidade = cidadeNormalizada;
        }
        public override string ToString() => $"{Cidade} - {Estado}";
    }
}
