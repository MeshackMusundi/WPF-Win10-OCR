namespace WPF.OCR.Services.Interfaces
{
    public interface IDialogService
    {
        string OpenFile(string caption, string filter = @"All files (*.*)|*.*");
    }
}
