using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MoreLinq;

namespace WWIIGame.Misc;

public static class StopwatchSingleton
{
	private static readonly HashSet<double> Records = new();
	private static readonly Stopwatch Stopwatch = new();

	public static void AddNewRecord() => Records.Add(Stopwatch.Elapsed.TotalMilliseconds);

	public static void Start() => Stopwatch.Restart();

	public static double Median => Records.OrderBy(number => number).Skip(Records.Count / 5).SkipLast(Records.Count / 5).Average();
}
