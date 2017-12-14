﻿using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace BooleanExpressions
{
    [MarkupExtensionReturnType(typeof(ObjectToBoolConverter))]
    public class ObjectToBoolConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}