using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using BerserkerDotNet.LINQPadLog4jDriver.Annotations;
using BerserkerDotNet.LINQPadLog4jDriver.Domain;

namespace BerserkerDotNet.LINQPadLog4jDriver.Views
{
    public class ConnectionEditorViewModel : INotifyPropertyChanged
    {
        private readonly Log4jConnectionProperties _properties;

        public ConnectionEditorViewModel(Log4jConnectionProperties properties)
        {
            _properties = properties;

            Apply = new RelayCommand(() => ConnectionEditor.CloseEditor(true));
            Cancel = new RelayCommand(() => ConnectionEditor.CloseEditor(false));
            SelectFolder = new RelayCommand(ExecuteSelectFolder);
        }

        private void ExecuteSelectFolder()
        {
            var result = ConnectionEditor.SelectFolder();
            if (!string.IsNullOrEmpty(result))
                Folder = result;
        }

        public string Folder
        {
            get { return _properties.Folder; }
            set
            {
                if (value == _properties.Folder) return;
                _properties.Folder = value;
                OnPropertyChanged();
            }
        }

        public bool UseCache
        {
            get { return _properties.UseCache; }
            set
            {
                if (value.Equals(_properties.UseCache)) return;
                _properties.UseCache = value;
                OnPropertyChanged();
            }
        }

        public string FileNameRegex
        {
            get { return _properties.FileNameFilter; }
            set
            {
                if (value == _properties.FileNameFilter) return;
                _properties.FileNameFilter = value;
                OnPropertyChanged();
            }
        }

        public ICommand Apply { get; private set; }
        public ICommand Cancel { get; private set; }
        public ICommand SelectFolder { get; private set; }
        public IConnectionEditor ConnectionEditor { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}