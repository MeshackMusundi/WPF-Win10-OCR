using Unity;
using WPF.OCR.Services;
using WPF.OCR.Services.Interfaces;
using WPF.OCR.ViewModels;

namespace WPF.OCR.Utilities
{
    public class ViewModelLocator
    {
        private readonly UnityContainer container;

        public ViewModelLocator()
        {
            container = new UnityContainer();
            container.RegisterType<IDialogService, DialogService>();
            container.RegisterType<IOcrService, OcrService>();
        }

        public MainWindowViewModel MainWindowVM => container.Resolve<MainWindowViewModel>();
    }
}
