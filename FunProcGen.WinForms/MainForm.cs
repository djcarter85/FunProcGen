namespace FunProcGen.WinForms;

using FunProcGen.Distributions;
using RandN;
using RandN.Distributions;
using RandN.Extensions;
using ScottPlot.Statistics;

public partial class MainForm : Form
{
    public MainForm()
    {
        InitializeComponent();

        this.distributionTypeCombo.Items.Add(DistributionType.Uniform);
        this.distributionTypeCombo.Items.Add(DistributionType.Normal);

        this.distributionTypeCombo.SelectedIndex = 0;
    }

    private void DistributionTypeCombo_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.RefreshHistogram();
    }

    private void RefreshButton_Click(object sender, EventArgs e)
    {
        this.RefreshHistogram();
    }

    private void RefreshHistogram()
    {
        var rng = StandardRng.Create();
        var dist = this.GetSelectedDistribution();

        var samples = TakeSamples(dist, count: 100_000, rng).ToArray();

        DisplayHistogram(samples);
    }

    private IDistribution<double> GetSelectedDistribution() =>
        (DistributionType)this.distributionTypeCombo.SelectedItem switch
        {
            DistributionType.Uniform => Uniform.New(0d, 1d),
            DistributionType.Normal => Normal.New(),
            _ => throw new ArgumentException(),
        };

    private static IEnumerable<T> TakeSamples<T, TRng>(IDistribution<T> dist, int count, TRng rng)
        where TRng : notnull, IRng
    {
        for (int i = 0; i < count; i++)
        {
            yield return dist.Sample(rng);
        }
    }

    private void DisplayHistogram(double[] values)
    {
        var histogram = Histogram.WithBinCount(100, values);

        formsPlot.Reset();
        var barPlot = formsPlot.Plot.Add.Bars(
            // Adjust the x-value to account for the fact that .Bins give us the left-hand edge of the bin.
            histogram.Bins.Select(b => b + histogram.FirstBinSize / 2),
            histogram.Counts);

        foreach (var bar in barPlot.Bars)
        {
            bar.Size = histogram.FirstBinSize;
        }

        formsPlot.Refresh();
    }
}
