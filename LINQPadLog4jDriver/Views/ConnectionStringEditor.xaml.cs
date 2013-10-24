using System;
using System.Windows;
using System.Windows.Forms;
using BerserkerDotNet.LINQPadLog4jDriver.Domain;
using LINQPad.Extensibility.DataContext;
using MessageBox = System.Windows.MessageBox;

namespace BerserkerDotNet.LINQPadLog4jDriver.Views
{
    public partial class ConnectionStringEditor : IConnectionEditor
    {
        public ConnectionStringEditor(IConnectionInfo cxInfo)
        {
            var properties = new Log4jConnectionProperties(cxInfo);
            var viewModel = new ConnectionEditorViewModel(properties);
            viewModel.ConnectionEditor = this;
            DataContext = viewModel;
            InitializeComponent();
        }
        public void CloseEditor(bool editorResult)
        {
            DialogResult = editorResult;
        }

        public string SelectFolder()
        {
            var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return dialog.SelectedPath;
            }
            return string.Empty;
        }
    }
}
