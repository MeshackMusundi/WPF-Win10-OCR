using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Globalization;
using Windows.Graphics.Imaging;
using Windows.Media.Ocr;
using Windows.System.UserProfile;
using WPF.OCR.Services.Interfaces;

namespace WPF.OCR.Services
{
    public class OcrService : IOcrService
    {
        /// <summary>
        /// Extracts text from an image using Windows 10 OCR.
        /// The extraction is done using a machine's active preferred language.
        /// </summary>
        /// <param name="image">The image to extract text from.</param>
        /// <returns>The text extracted from an image.</returns>
        public async Task<string> ExtractText(string image)
        {
            if (string.IsNullOrWhiteSpace(image))
                throw new ArgumentNullException("Image can't be null or empty.");

            if (!File.Exists(image))
                throw new ArgumentOutOfRangeException($"'{image}' doesn't exist.");

            CheckIfFileIsImage(image);

            StringBuilder text = new StringBuilder();

            await using (var fileStream = File.OpenRead(image))
            {
                var bmpDecoder = await BitmapDecoder.CreateAsync(fileStream.AsRandomAccessStream());
                var softwareBmp = await bmpDecoder.GetSoftwareBitmapAsync();

                var ocrEngine = OcrEngine.TryCreateFromUserProfileLanguages();
                var ocrResult = await ocrEngine.RecognizeAsync(softwareBmp);

                foreach (var line in ocrResult.Lines) text.AppendLine(line.Text);
            }

            return text.ToString();
        }

        /// <summary>
        /// Extracts text from an image using Windows 10 OCR.
        /// </summary>
        /// <param name="image">The image to extract text from.</param>
        /// <param name="languageCode">The language code of the language in the image.
        /// The language code should be an installed language supported by Windows 10 OCR.</param>
        /// <returns>The text extracted from an image.</returns>
        public async Task<string> ExtractText(string image, string languageCode)
        {
            if (string.IsNullOrWhiteSpace(image))
                throw new ArgumentNullException("Image can't be null or empty.");

            if (string.IsNullOrWhiteSpace(languageCode))
                throw new ArgumentNullException("Language can't be null or empty.");

            if (!File.Exists(image))
                throw new ArgumentOutOfRangeException($"'{image}' doesn't exist.");

            CheckIfFileIsImage(image);

            if (!GlobalizationPreferences.Languages.Contains(languageCode))
                throw new ArgumentOutOfRangeException($"{languageCode} is not installed.");

            StringBuilder text = new StringBuilder();

            await using (var fileStream = File.OpenRead(image))
            {
                var bmpDecoder = await BitmapDecoder.CreateAsync(fileStream.AsRandomAccessStream());
                var softwareBmp = await bmpDecoder.GetSoftwareBitmapAsync();

                var ocrEngine = OcrEngine.TryCreateFromLanguage(new Language(languageCode));
                var ocrResult = await ocrEngine.RecognizeAsync(softwareBmp);

                foreach (var line in ocrResult.Lines) text.AppendLine(line.Text);
            }

            return text.ToString();
        }

        private void CheckIfFileIsImage(string file)
        {
            var isImage = Regex.IsMatch(Path.GetExtension(file).ToLower(),
                "(jpg|jpeg|jfif|png|bmp)$", RegexOptions.Compiled);

            if (!isImage)
                throw new ArgumentOutOfRangeException($"'{file}' is not an image.");
        }
    }
}
