using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Threading.Tasks;
using WPF.OCR.Services.Interfaces;

namespace WPF.OCR.Services.Tests
{
    [TestClass()]
    public class OcrServiceTests
    {
        private static IOcrService ocrService;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            ocrService = new OcrService();
        }


        [TestMethod()]
        public async Task ExtractTextTest_UsingCodeOfNonInstalledLanguage_ThrowsArgumentOutOfRangeException()
        {
            string image = Path.Combine(Environment.CurrentDirectory, "Images", "Quote.png");
            string languageCode = "fr-FR";

            try
            {
                await ocrService.ExtractText(image, languageCode);
            }
            catch (ArgumentOutOfRangeException e)
            {
                StringAssert.Contains(e.Message, $"{languageCode} is not installed.");
                return;
            }

            Assert.Fail("ArgumentOutOfRangeException was not thrown.");
        }

        [TestMethod()]
        public async Task ExtractTextTest_UsingCodeOfInstalledLanguage_ImageTextExtracted()
        {
            string image = Path.Combine(Environment.CurrentDirectory, "Images", "Quote.png");
            string languageCode = "en-GB";

            string extractedText = await ocrService.ExtractText(image, languageCode);

            Assert.IsTrue(!string.IsNullOrWhiteSpace(extractedText));
            Assert.IsTrue(extractedText.Contains("programmer", StringComparison.OrdinalIgnoreCase));
        }
    }
}