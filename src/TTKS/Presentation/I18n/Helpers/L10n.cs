using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using Xamarin.Forms;

namespace TTKS.Localization
{
    public class L10n
    {
        private static readonly string ResourceId = $"{nameof(TTKS)}.{nameof(TTKS.Presentation.Resx)}.{nameof(TTKS.Presentation.Resx.AppResources)}";

        private static readonly Lazy<ResourceManager> ResMgr = new Lazy<ResourceManager>(
            () => new ResourceManager(ResourceId, IntrospectionExtensions.GetTypeInfo(typeof(L10n)).Assembly));

        private static CultureInfo _ci = null;

        static L10n()
        {
            SetCultureInfo();
        }

        public static void SetLocale(CultureInfo ci)
        {
            DependencyService.Get<ILocalize>().SetLocale(ci);
            SetCultureInfo();
        }

        public static string Localize(string key)
        {
            var translation = ResMgr.Value.GetString(key, _ci);
            if (translation == null)
            {
#if DEBUG
                throw new ArgumentException(
                    string.Format("Key '{0}' was not found in resources '{1}' for culture '{2}'.", key, ResourceId, _ci.Name),
                    "Text");
#else
                translation = Text; // HACK: returns the key, which GETS DISPLAYED TO THE USER
#endif
            }

            return translation;
        }

        private static void SetCultureInfo()
        {
            if (Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.Android)
            {
                _ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
            }
        }
    }
}
