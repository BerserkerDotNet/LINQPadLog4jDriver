namespace BerserkerDotNet.LINQPadLog4jDriver.Views
{
    public interface IConnectionEditor
    {
        void CloseEditor(bool editorResult);
        string SelectFolder();
    }
}