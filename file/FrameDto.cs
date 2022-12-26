using musicsheetvideo.Timestamp;

namespace file;

public class FrameDto
{
    public int? PageNumber { get; set; }
    public string? Start { get; set; }
    public string? End { get; set; }

    public Frame TryGetDomainFrame()
    {
        var startTick = TryParseDomainTick(Start);
        var endTick = TryParseDomainTick(End);
        if (!PageNumber.HasValue)
        {
            throw new Exception("pageNumber is invalid");
        }

        return new Frame(new Interval(startTick, endTick), PageNumber.Value);
    }

    private static Tick TryParseDomainTick(string? tick)
    {
        if (string.IsNullOrEmpty(tick))
        {
            throw new Exception("start is null");
        }

        var tokens = tick.Split(" ");
        if (tokens.Length != 3)
        {
            throw new Exception($"tick pattern is \"minutes seconds milisseconds\". Value passed was \"{tick}\"");
        }

        var startMinutes = int.Parse(tokens[0]);
        var startSeconds = int.Parse(tokens[1]);
        var startMilisseconds = int.Parse(tokens[2]);
        return new Tick(startMinutes, startSeconds, startMilisseconds);
    }

    private string MessageWithSequence(int sequence, string messageError)
    {
        return $"frame {sequence}: {messageError}";
    }

    public List<string> Validate(int sequence)
    {
        var errors = new List<string>();
        if (string.IsNullOrEmpty(Start))
        {
            errors.Add(MessageWithSequence(sequence, "tick start was not passed"));
        }
        else
        {
            try
            {
                TryParseDomainTick(Start);
            }
            catch (Exception e)
            {
                errors.Add(MessageWithSequence(sequence, e.Message));
            }
        }

        if (string.IsNullOrEmpty(End))
        {
            errors.Add(MessageWithSequence(sequence, "end tick was not passed"));
        }
        else
        {
            try
            {
                TryParseDomainTick(End);
            }
            catch (Exception e)
            {
                errors.Add(MessageWithSequence(sequence, e.Message));
            }
        }

        if (!PageNumber.HasValue)
        {
            errors.Add(MessageWithSequence(sequence, "page number was not passed"));
        }

        return errors;
    }
}