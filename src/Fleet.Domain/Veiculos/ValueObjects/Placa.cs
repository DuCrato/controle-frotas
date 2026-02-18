using System.Text.RegularExpressions;

namespace Fleet.Domain.Veiculos.ValueObjects
{
    public sealed record Placa
    {
        public string Valor { get; }

        public Placa(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                throw new ArgumentException("Placa é obrigatória.");

            var normalizada = valor.Trim().ToUpper().Replace("-", "");

            if (normalizada.Length != 7)
                throw new ArgumentException("Placa inválida. Deve conter 7 dígitos");

            var mercosul = Regex.IsMatch(normalizada, "^[A-Z]{3}[0-9][A-Z0-9][0-9]{2}$");
            var antiga = Regex.IsMatch(normalizada, "^[A-Z]{3}[0-9]{4}$");

            if (!antiga && !mercosul)
                throw new ArgumentException("Placa inválida.");

            Valor = normalizada;
        }

        public override string ToString() => Valor;
    }
}
