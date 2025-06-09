using AlmaDeMalta.Common.Contracts.Contracts;

public class ConversionService
{
    private readonly Dictionary<(MesaureUnit From, MesaureUnit To), Func<decimal, decimal>> _conversions;

    public ConversionService()
    {
        _conversions = new Dictionary<(MesaureUnit, MesaureUnit), Func<decimal, decimal>>
        {
            // Volumen
            { (MesaureUnit.Litre, MesaureUnit.Millilitre), v => v * 1000.0m },
            { (MesaureUnit.Millilitre, MesaureUnit.Litre), v => v / 1000.0m },
            { (MesaureUnit.Litre, MesaureUnit.Centilitre), v => v * 100.0m },
            { (MesaureUnit.Centilitre, MesaureUnit.Litre), v => v / 100.0m },
            { (MesaureUnit.Centilitre, MesaureUnit.Millilitre), v => v * 10.0m },
            { (MesaureUnit.Millilitre, MesaureUnit.Centilitre), v => v / 10.0m },
            { (MesaureUnit.Millilitre, MesaureUnit.Ounce), v => v / 29.57353m },
            { (MesaureUnit.Ounce, MesaureUnit.Millilitre), v => v * 29.57353m },

            // Peso
            { (MesaureUnit.Gram, MesaureUnit.Kilogram), v => v / 1000.0m },
            { (MesaureUnit.Kilogram, MesaureUnit.Gram), v => v * 1000.0m },
            { (MesaureUnit.Gram, MesaureUnit.Pound), v => v / 453.59237m },
            { (MesaureUnit.Pound, MesaureUnit.Gram), v => v * 453.59237m },
            { (MesaureUnit.Gram, MesaureUnit.Ounce), v => v / 28.34952m },
            { (MesaureUnit.Ounce, MesaureUnit.Gram), v => v * 28.34952m },
            { (MesaureUnit.Kilogram, MesaureUnit.Ounce), v => v * 35.27396m },
            { (MesaureUnit.Ounce, MesaureUnit.Kilogram), v => v / 35.27396m },
            { (MesaureUnit.Kilogram, MesaureUnit.Pound), v => v * 2.20462m },
            { (MesaureUnit.Pound, MesaureUnit.Kilogram), v => v / 2.20462m }
        };
    }

    public decimal Convert(decimal value, MesaureUnit from, MesaureUnit to)
    {
        if (from == to) return value;

        if (_conversions.TryGetValue((from, to), out var conversion))
        {
            return conversion(value);
        }

        throw new NotSupportedException($"No conversion defined from {from} to {to}");
    }
}