using System.IO;
using musicsheetvideo;
using NUnit.Framework;

namespace test;

public class PdfManipulatorTests
{
    private static readonly string _basePath = "/home/fernando/tmp/msv/two-pages";
    private static readonly string _pdfPath = Path.Combine(_basePath, "common-two-pages.pdf");
    private static readonly string _imagesPath = Path.Combine(_basePath, "images");

    private static readonly MusicSheetVideoConfiguration _configuration =
        new(_basePath, _pdfPath, _imagesPath);

    private readonly IPdfManipulator _pdfManipulator = new AsposeManipulator(_configuration);

    [SetUp]
    public void SetUp()
    {
        if (Directory.Exists(Path.Combine(_basePath, "images")))
        {
            var files = new DirectoryInfo(Path.Combine(_basePath, "images")).GetFiles();
            foreach (var file in files)
            {
                file.Delete();
            }

            Directory.Delete(Path.Combine(_basePath, "images"));
        }
    }

    [Test]
    public void TestNumberOfPages()
    {
        Assert.AreEqual(2, _pdfManipulator.NumberOfPages());
    }

    [Test]
    public void TestPagesConversionToPng()
    {
        _pdfManipulator.ConvertPdfPagesToGifImages(_imagesPath);
        AssertDirectoryExists(_imagesPath);
        AssertNImagesWereCreated(2, _imagesPath);
    }

    private void AssertDirectoryExists(string path)
    {
        Assert.True(Directory.Exists(path));
    }

    private void AssertNImagesWereCreated(int numberOfImages, string path)
    {
        var files = Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly);
        Assert.AreEqual(numberOfImages, files.Length);
    }
}