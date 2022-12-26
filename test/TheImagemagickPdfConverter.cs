using Moq;
using musicsheetvideo;
using musicsheetvideo.PdfConverter;
using NUnit.Framework;
using test.Stubs;

namespace test;

[TestFixture]
public class TheImagemagickPdfConverter
{
    private readonly ImagemagickPdfConverter _subject;
    private readonly Mock<IProgressNotification> _mockProgressNotification;

    public TheImagemagickPdfConverter()
    {
        _mockProgressNotification = new Mock<IProgressNotification>();
        _subject = new ImagemagickPdfConverter(_mockProgressNotification.Object, new NullCommand());
    }

    [Test]
    public void NotifiesWhenImagesAreBeingExtractedFromPdf()
    {
        var configuration = new MusicSheetVideoConfiguration("", "", "", "", "", "");
        _subject.ConvertPdfToImages(configuration);
        _mockProgressNotification.Verify(x => x.NotifyProgress(It.IsAny<string>()), Times.Once);
    }
}