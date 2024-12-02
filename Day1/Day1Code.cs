using BenchmarkDotNet.Attributes;

namespace Day1;

[MemoryDiagnoser]
[ShortRunJob]
public class Day1Code
{
    private readonly string[] day1Input;

    public Day1Code()
    {
        day1Input = File.ReadAllLines("day1input.txt");
    }

    [Benchmark(Baseline = true)]
    public void Execute()
    {
        // Could stackalloc int[] here and use spans
        var l1 = new List<int>(day1Input.Length);
        var l2 = new List<int>(day1Input.Length);

        foreach (var line in day1Input)
        {
            l1.Add(int.Parse(line[..5]));
            l2.Add(int.Parse(line[8..]));
        }

        l1.Sort();
        l2.Sort();

        int sum = 0, similar = 0;

        for (int i = 0; i < l1.Count; i++)
        {
            var l1Value = l1[i];
            var distance = Math.Abs(l1Value - l2[i]);

            sum += distance;

            similar += l1Value * l2.Count(x => x == l1Value);
        }

        Console.WriteLine($"Part 1 {sum}");
        Console.WriteLine($"Part 2 {similar}");
    }


    [Benchmark(Description = "Optimised")]
    public void ExecuteVersion2()
    {
        Span<int> l1 = stackalloc int[day1Input.Length];
        Span<int> l2 = stackalloc int[day1Input.Length];

        for (int i = 0; i < day1Input.Length; i++)
        {
            var e = day1Input[i];

            l1[i] = int.Parse(e[..5]);
            l2[i] = int.Parse(e[8..]);
        }

        l1.Sort();
        l2.Sort();

        int sum = 0, similar = 0;

        for (int i = 0; i < l1.Length; i++)
        {
            var l1Index = l1[i];
            var distance = Math.Abs(l1Index - l2[i]);

            sum += distance;

            similar += l1Index * l2.Count(l1Index);
        }

        Console.WriteLine($"Part 1 {sum}");
        Console.WriteLine($"Part 2 {similar}");
    }
}
