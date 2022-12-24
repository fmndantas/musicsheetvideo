using musicsheetvideo.Timestamp;

namespace file;

public class ConfigurationDto
{
    public string? PdfPath { get; set; }
    public string? AudioPath { get; set; }
    public string? OutputPath { get; set; }
    public string? DefaultImagePath { get; set; }
    public List<FrameDto>? Frames { get; set; }

    public bool Valid => !Validate().Any();

    public List<string> Validate()
    {
        var errors = new List<string>();
        if (string.IsNullOrEmpty(PdfPath))
        {
            errors.Add("pdfPath was not passed");
        }

        if (string.IsNullOrEmpty(AudioPath))
        {
            errors.Add("audioPath was not passed");
        }

        if (string.IsNullOrEmpty(DefaultImagePath))
        {
            errors.Add("defaultImagePath was not passed");
        }

        if (string.IsNullOrEmpty(OutputPath))
        {
            errors.Add("outputPath was not passed");
        }

        if (Frames == null)
        {
            errors.Add("frames were not passed");
        }
        else
        {
            var sequence = 1;
            foreach (var frame in Frames)
            {
                errors.AddRange(frame.Validate(sequence++));
            }
        }

        return errors;
    }

    public List<Frame> DomainFrames => Frames!.Select(x => x.TryGetDomainFrame()).ToList();
}