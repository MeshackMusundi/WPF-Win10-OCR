using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Windows.System.UserProfile;
using WPF.OCR.Commands;
using WPF.OCR.Services.Interfaces;

namespace WPF.OCR.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IDialogService dialogService;
        private readonly IOcrService ocrService;

        public MainWindowViewModel(IDialogService dialogSvc, IOcrService ocrSvc)
        {
            dialogService = dialogSvc;
            ocrService = ocrSvc;
        }

        // Language codes of installed languages.
        public List<string> InstalledLanguages => GlobalizationPreferences.Languages.ToList();

        private string _imageLanguageCode;
        public string ImageLanguageCode
        {
            get => _imageLanguageCode;
            set
            {
                _imageLanguageCode = value;
                OnPropertyChanged();
            }
        }

        private string _selectedImage;
        public string SelectedImage
        {
            get => _selectedImage;
            set
            {
                _selectedImage = value;
                OnPropertyChanged();
            }
        }

        private string _extractedText;
        public string ExtractedText
        {
            get => _extractedText;
            set
            {
                _extractedText = value;
                OnPropertyChanged();
            }
        }

        #region Select Image Command

        private RelayCommand _selectImageCommand;
        public RelayCommand SelectImageCommand =>
            _selectImageCommand ??= new RelayCommand(_ => SelectImage());

        private void SelectImage()
        {
            string image = dialogService.OpenFile("Select Image",
                "Image (*.jpg; *.jpeg; *.png; *.bmp)|*.jpg; *.jpeg; *.png; *.bmp");

            if (string.IsNullOrWhiteSpace(image)) return;

            SelectedImage = image;
            ExtractedText = string.Empty;
        }

        #endregion

        #region Extract Text Command

        private RelayCommandAsync _extractTextCommand;
        public RelayCommandAsync ExtractTextCommand =>
            _extractTextCommand ??= new RelayCommandAsync(ExtractText, _ => CanExtractText());

        private async Task ExtractText()
        {
            ExtractedText = await ocrService.ExtractText(SelectedImage, ImageLanguageCode);
        }

        private bool CanExtractText() => !string.IsNullOrWhiteSpace(ImageLanguageCode) &&
                                         !string.IsNullOrWhiteSpace(SelectedImage);

        #endregion

        #region Copy Text to Clipboard Command

        private RelayCommand _copyTextToClipboardCommand;
        public RelayCommand CopyTextToClipboardCommand => _copyTextToClipboardCommand ??=
            new RelayCommand(_ => CopyTextToClipboard(), _ => CanCopyTextToClipboard());

        private void CopyTextToClipboard() => Clipboard.SetData(DataFormats.Text, _extractedText);

        private bool CanCopyTextToClipboard() => !string.IsNullOrWhiteSpace(_extractedText);

        #endregion
    }
}
