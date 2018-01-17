using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Text.RegularExpressions;

namespace AnimationBasics
{
    public class ArithmeticConverter : IValueConverter
    {
        private const string ArithmeticParseString = "([+\\-*/]{1,1})\\s{0,}(\\-?[\\d\\.]+)";
        private Regex arithmeticRegex;

        public ArithmeticConverter()
        {
            arithmeticRegex = new Regex(ArithmeticParseString);
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((value is double) && (parameter != null))
            {
                string param = parameter.ToString();

                if (param.Length > 0)
                {
                    Match match = arithmeticRegex.Match(param);

                    if ((match != null) || (match.Groups.Count == 3))
                    {
                        string operation = match.Groups[1].Value.Trim();
                        string numericValue = match.Groups[2].Value;

                        double number = 0;

                        // This should always succeed or our regex is broken.
                        if (double.TryParse(numericValue, out number))
                        {
                            double valueAsDouble = (double)value;
                            double returnValue = 0;

                            switch (operation)
                            {
                                case "+":
                                    returnValue = valueAsDouble + number;
                                    break;
                                case "-":
                                    returnValue = valueAsDouble - number;
                                    break;
                                case "*":
                                    returnValue = valueAsDouble * number;
                                    break;
                                case "/":
                                    returnValue = valueAsDouble / number;
                                    break;
                            }

                            return returnValue;
                        }
                    }
                }
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("The method or operation is not implemented.");
        }
    }
}