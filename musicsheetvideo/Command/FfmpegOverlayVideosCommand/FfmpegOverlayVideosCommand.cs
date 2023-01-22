namespace musicsheetvideo.Command.FfmpegOverlayVideosCommand;

public class FfmpegOverlayVideosCommand : ShellCommand<FfmpegOverlayVideosCommandInput>
{
    public FfmpegOverlayVideosCommand(FfmpegOverlayVideosCommandInput configuration,
        IProgressNotification progressNotification) : base(configuration, progressNotification)
    {
    }

    protected override string CommandName => "ffmpeg";

    protected override string Arguments => $"-y -i {Configuration.VideoBelowPath} -i {Configuration.VideoAbovePath} " +
                                           "-filter_complex [1:v]format=argb,colorchannelmixer=aa=0.7[t];[0:v][t]overlay=0:main_h-overlay_h " +
                                           $"-hide_banner -loglevel error {Configuration.OutputVideoPath}";


    protected override void DescribeItselfRunning()
    {
        ProgressNotification.NotifyProgress($"Overlaying two videos: \"{Configuration.VideoBelowPath}\" " +
                                            $"and \"{Configuration.VideoAbovePath}\". Output video is " +
                                            $"\"{Configuration.OutputVideoPath}\"");
    }
}