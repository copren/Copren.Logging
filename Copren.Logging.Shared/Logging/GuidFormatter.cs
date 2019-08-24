using System;
using System.Globalization;
using System.IO;
using Serilog;
using Serilog.Events;
using Serilog.Formatting;

namespace Copren.Logging.Shared
{
    public class GuidFormatter : IFormatProvider, ICustomFormatter
    {
        public object GetFormat(Type formatType)
        {
            if (formatType == typeof(ICustomFormatter))
                return this;
            else
                return null;
        }

        public string Format(string fmt, object arg, IFormatProvider formatProvider)
        {
            // Provide default formatting if arg is not an Int64.
            if (arg.GetType() != typeof(Guid))
            {
                try
                {
                    return HandleOtherFormats(fmt, arg);
                }
                catch (FormatException e)
                {
                    throw new FormatException(String.Format("The format of '{0}' is invalid.", fmt), e);
                }
            }

            // Provide default formatting for unsupported format strings.
            if (!(fmt == "a" || fmt == "l" || fmt == "s"))
            {
                try
                {
                    return HandleOtherFormats(fmt, arg);
                }
                catch (FormatException e)
                {
                    throw new FormatException(String.Format("The format of '{0}' is invalid.", fmt), e);
                }
            }

            if (fmt == "s") return arg.ToString().Substring(24);

            return arg.ToString();
        }

        private string HandleOtherFormats(string format, object arg)
        {
            if (arg is IFormattable formattable)
                return formattable.ToString(format, CultureInfo.CurrentCulture);
            else if (arg != null)
                return arg.ToString();
            else
                return String.Empty;
        }
    }
}