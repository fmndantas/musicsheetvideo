using System.Collections.Generic;
using System.Linq;
using musicsheetvideo;
using NUnit.Framework;

namespace test;

public class FfmpegStrategyTests
{
    private readonly IVideoProducer _subject =
        new FfmpegProducer(new MusicSheetVideoConfiguration("", "", ""));


}