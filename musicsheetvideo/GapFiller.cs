namespace musicsheetvideo;

public class GapFiller : IGapFiller
{
    public List<Page> FillGap(List<Page> pages)
    {
        pages.Sort();
        var processedPages = new List<Page>();

        for (var i = 0; i < pages.Count; ++i)
        {
            processedPages.Add(pages[i].DecreaseOneMilissecondEnd());
            if (i + 1 < pages.Count)
            {
                var gapFilling = pages[i].Gap(pages[i + 1]).DecreaseOneMilissecondEnd();
                if (gapFilling.LengthMilisseconds > 0)
                {
                    processedPages.Add(gapFilling);
                }
            }
        }

        return processedPages;
    }
}