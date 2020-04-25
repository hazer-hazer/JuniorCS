using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.ExceptionServices;
using System.Threading;

public class Program
{

	private static ConcurrentDictionary<string, int> Triplets = new ConcurrentDictionary<string, int>();
	private static readonly Stopwatch sw = new Stopwatch();

	private static void ProcessLine(string line)
	{
		string triplet = "";

		foreach (char c in line)
		{
			if (Char.IsLetter(c))
			{
				triplet += c;
			}
			else
			{
				triplet = "";
			}

			if (triplet.Length == 3)
			{
				Triplets.AddOrUpdate(triplet, 1, (key, count) => count + 1);
				triplet = triplet.Substring(1);
			}
		}
	}

	private static void ShowResult()
	{

		if (Triplets.Count == 0)
		{
			Console.WriteLine("No lines were loaded");
			return;
		}

		Console.WriteLine("Top triplets{0}:", abort ? " (aborted)" : "");


		List<KeyValuePair<string, int>> Top = Triplets.ToList();

		// Sort triplets descending
		Top.Sort((l, r) => r.Value.CompareTo(l.Value));

		// Stop benchmark after all algorithm operations
		sw.Stop();

		// Print top-10
		for (int i = 0; i < Math.Min(10, Top.Count); i++)
		{
			string t = Top[i].Key;
			Console.WriteLine($"{i + 1}. {t}: {Top[i].Value}");
		}

		Console.WriteLine($"Execution time: {sw.ElapsedMilliseconds} milliseconds");
	}

	private static bool abort = false;

	private static void ShouldClose()
	{
		abort = Console.ReadKey(true) != null;
	}

	public static void Main()
	{
		try
		{

			string filename = "";

			while (filename == "")
			{
				Console.Write("Type path to file: ");
				filename = Console.ReadLine();
			}

			if (!File.Exists(filename))
			{
				throw new FileNotFoundException("File not found");
			}

			Console.WriteLine("Start analysing (Press any key to stop)");

			// Create and start thread for keypress event handling
			var stopThread = new Thread(ShouldClose);
			stopThread.Start();

			// Start Benchmark
			sw.Start();

			// Go through all lines as IEnumerable
			Parallel.ForEach(File.ReadLines(filename), (line, state) =>
			{
				if (abort)
				{
					state.Break();
				}

				ProcessLine(line);
			});

			ShowResult();
		}
		catch (ArgumentException e)
		{
			Console.WriteLine(e.Message);
		}
		catch (FileNotFoundException e)
		{
			Console.WriteLine(e.Message);
		}

		Console.ReadKey();
	}
}