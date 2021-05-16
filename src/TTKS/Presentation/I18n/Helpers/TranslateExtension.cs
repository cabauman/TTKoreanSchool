using System;
using TTKS.Localization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TTKS
{
    // You exclude the 'Extension' suffix when using in XAML
    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
        public string? Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text == null)
                return string.Empty;

            return L10n.Localize(Text);
        }
    }
}
