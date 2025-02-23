namespace ZenonFileProcessor.Models;

public class MeasurementRecord
{
    public int ChannelIndex { get; set; }
    public double Value { get; set; }
    public long Status { get; set; }
    public DateTime Timestamp { get; set; }
}
