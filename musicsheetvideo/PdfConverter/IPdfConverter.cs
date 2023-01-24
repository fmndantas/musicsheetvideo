namespace musicsheetvideo.PdfConverter;

public interface IPdfConverter
{
    void ConvertPdfToImages(IPdfConverterConfiguration configuration);
}