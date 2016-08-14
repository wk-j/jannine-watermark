using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace Jannine.Watermark.Core
{
    static class Globals
    {
        public static ImageSource InstallIcon = BitmapFrame.Create(new Uri("pack://application:,,,/Jannine.Watermark;component/_Shared/Resources/install.png", UriKind.RelativeOrAbsolute));
        public static ImageSource UpdateIcon = BitmapFrame.Create(new Uri("pack://application:,,,/Jannine.Watermark;component/_Shared/Resources/update.png", UriKind.RelativeOrAbsolute));
        public static ImageSource UninstallIcon = BitmapFrame.Create(new Uri("pack://application:,,,/Jannine.Watermark;component/_Shared/Resources/uninstall.png", UriKind.RelativeOrAbsolute));
        public static ImageSource BrowseIcon = BitmapFrame.Create(new Uri("pack://application:,,,/Jannine.Watermark;component/_Shared/Resources/browse.png", UriKind.RelativeOrAbsolute));
        public const string VsixName = "Jannine.Watermark";
    }
}
