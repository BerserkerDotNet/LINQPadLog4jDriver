using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
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
            Apply = new RelayCommand(ApplyExecute);
            Cancel = new RelayCommand(() => ConnectionEditor.CloseEditor(false));
            SelectFolder = new RelayCommand(ExecuteSelectFolder);
        }

        private void ExecuteSelectFolder()
        {
            var result = ConnectionEditor.SelectFolder();
            if (!string.IsNullOrEmpty(result))
                Folder += Environment.NewLine + result;
        }

        private void ApplyExecute()
        {
            var newLine = Environment.NewLine;
            var folders = Folder.Split(new[] { newLine }, StringSplitOptions.RemoveEmptyEntries);
            var invalidFolders = folders.Where(f => !Directory.Exists(f));
            if (invalidFolders.Any())
            {
                var msg = string.Format("Invalid foldes detected:{0}.{1}Please correct errors before saving.",
                    newLine + string.Join(newLine, invalidFolders), newLine);
                MessageBox.Show(msg);
            }
            else if (DateFilterMode == (int)Domain.DateFilterMode.SpecificDate && SpecificDate==null)
            {
                MessageBox.Show("Please specify a date by which you want to filter.");
            }
            else
            {
                ConnectionEditor.CloseEditor(true);
            }
        }

        public string Folder
        {
            get { return _properties.Folder.Replace(";", Environment.NewLine); }
            set
            {
                var newValue = value.Replace(Environment.NewLine, ";");
                if (newValue == _properties.Folder) 
                    return;
                _properties.Folder = newValue;
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

        public int DateFilterMode {
            get { return (int)_properties.DateFilterMode; }
            set
            {
                _properties.DateFilterMode = (DateFilterMode) value;
                OnPropertyChanged();
            }
        }

        public DateTime? SpecificDate
        {
            get { return _properties.SpecificDate; }
            set
            {
                if (_properties.SpecificDate != value)
                {
                    _properties.SpecificDate = value;
                    OnPropertyChanged();
                }
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