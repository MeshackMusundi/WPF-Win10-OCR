using System.Threading.Tasks;

namespace WPF.OCR.Services.Interfaces
{
    public interface IOcrService
    {
        Task<string> ExtractText(string image);
        Task<string> ExtractText(string image, string languageCode);
    }
}
