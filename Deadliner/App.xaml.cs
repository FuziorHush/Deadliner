using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;

namespace Deadliner
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            CultureInfo cultureRU = new CultureInfo("ru-RU");
            Thread.CurrentThread.CurrentCulture = cultureRU;
            Thread.CurrentThread.CurrentUICulture = cultureRU;
            CultureInfo.DefaultThreadCurrentCulture = cultureRU;
            CultureInfo.DefaultThreadCurrentUICulture = cultureRU;
            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement), new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
            base.OnStartup(e);
        }
    }
}
