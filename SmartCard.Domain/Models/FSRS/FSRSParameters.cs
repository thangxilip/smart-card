namespace SmartCard.Domain.Models.FSRS;

public class FSRSParameters
{
    // Default parameters for FSRS-5
    public double[] W { get; set; } =
    [
        0.40255, 1.18385, 3.173, 15.69105, 7.1949,    // w0-w4
        0.5345, 1.4604, 0.0046, 1.54575, 0.1192,      // w5-w9
        1.01925, 1.9395, 0.11, 0.29605, 2.2698,       // w10-w14
        0.2315, 2.9898, 0.51655, 0.6621               // w15-w18
    ];

    public double RequestRetentionRate { get; set; } = 0.9;
    public double MinimumStability { get; set; } = 0.1;
    public double MaximumStability { get; set; } = 36500; // 100 years
}