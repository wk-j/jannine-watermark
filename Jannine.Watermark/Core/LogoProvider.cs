using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Settings;
using Microsoft.VisualStudio.Text;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;


namespace Jannine.Watermark.Core
{
    [Export(typeof(IWpfTextViewCreationListener))]
    [ContentType("code")]
    [TextViewRole(PredefinedTextViewRoles.Editable)]
    internal sealed class LogoProvider : IWpfTextViewCreationListener
    {
        private const string _propertyName = "ShowWatermark";
        private const double _initOpacity = 0.3D;

        private static bool _isVisible, _hasLoaded;
        private static readonly Dictionary<string, string> _map = new Dictionary<string, string>()
        {
            {".js", "jw1.jpg" },
            {".cs", "jw2.jpg" },
            {".fs", "jw3.jpg" },
            {".config", "jw4.jpg" },
            {".md", "jw5.jpg" },
            {".json", "jw6.jpg" }
        };

        [Import]
        public ITextDocumentFactoryService TextDocumentFactoryService { get; set; }

        [Import]
        public SVsServiceProvider serviceProvider { get; set; }

        private void LoadSettings()
        {
            _hasLoaded = true;

            SettingsManager settingsManager = new ShellSettingsManager(serviceProvider);
            SettingsStore store = settingsManager.GetReadOnlySettingsStore(SettingsScope.UserSettings);

            LogoAdornment.VisibilityChanged += (sender, isVisible) =>
            {
                WritableSettingsStore wstore = settingsManager.GetWritableSettingsStore(SettingsScope.UserSettings);
                _isVisible = isVisible;

                if (!wstore.CollectionExists(Globals.VsixName))
                    wstore.CreateCollection(Globals.VsixName);

                wstore.SetBoolean(Globals.VsixName, _propertyName, isVisible);
            };

            _isVisible = store.GetBoolean(Globals.VsixName, _propertyName, true);
        }

        public void TextViewCreated(IWpfTextView textView)
        {
            if (!_hasLoaded)
                LoadSettings();

            ITextDocument document;
            if (TextDocumentFactoryService.TryGetTextDocument(textView.TextDataModel.DocumentBuffer, out document))
            {
                string fileName = Path.GetExtension(document.FilePath).ToLowerInvariant();

                if (string.IsNullOrEmpty(fileName) || !_map.ContainsKey(fileName))
                    return;

                LogoAdornment highlighter = new LogoAdornment(textView, _map[fileName], _isVisible, _initOpacity);
            }
        }

    }
}
