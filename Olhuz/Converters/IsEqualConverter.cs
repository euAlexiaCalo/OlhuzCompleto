// Este arquivo define um "Value Converter" que é usado no XAML
// para transformar um valor (ex: um objeto) em outro (ex: um booleano).

using System.Globalization; // Usado para informações de cultura na conversão.
using Microsoft.Maui.Controls; // Contém a interface IValueConverter.

namespace Olhuz.Converters
{
    //Este conversor é usado quando precisamos comparar dois valores no XAML.
    // Ele responde com TRUE se forem iguais, ou FALSE se forem diferentes.
    // Muito usado para mudar aparência de um item selecionado.
    public class IsEqualConverter : IValueConverter
    {
        // ----------------------------------------------------
        // I. Método Convert (Binding Source -> Target)
        // ----------------------------------------------------

        // Este método é chamado quando o valor está fluindo da fonte (View Model) para a View (XAML).
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Cenário típico para este Converter:
            // value: valor que está sendo verificado (ex: item atual do menu)
            // parameter: valor com o qual queremos comparar (ex: item selecionado)

            // O objetivo é: O item atual (value) é o item selecionado (parameter)?

            // 'value?.Equals(parameter)' verifica se value é igual ao parameter.
            // '?? false' garante que, se value for nulo, o resultado será false.
            return value?.Equals(parameter) ?? false;
        }

        // ----------------------------------------------------
        // II. Método ConvertBack (Target -> Binding Source)
        // ----------------------------------------------------

        // Este método é chamado para conversão reversa (da View para a fonte),
        // mas não é necessário para a lógica de comparação de igualdade/seleção.
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Lança uma exceção pois a conversão reversa não é suportada/necessária para esta lógica.
            throw new NotImplementedException();
        }
    }
}