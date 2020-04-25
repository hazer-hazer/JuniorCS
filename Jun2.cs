using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

class Jun2
{
	private readonly int[] nbsX = { 0, 1, 0, -1 };
	private readonly int[] nbsY = { -1, 0, 1, 0 };

	private int[] matrix;

	private List<List<int>> graph;
	private bool[] used;
	private List<int> number; // The output number

	private int size;
	private int matrix_area;

	public Jun2()
	{
		this.size = 3;
		this.matrix_area = 9;
		matrix = new int[matrix_area];
		graph = new List<List<int>>();
		used = new bool[matrix_area + 1];
		number = new List<int>(matrix_area);
	}

	// Depth-First-Search to prevent going to dead ends
	private bool dfs(int vertex)
	{
		if (number.Count == matrix_area)
		{
			return true;
		}

		foreach (int cell in graph[vertex])
		{
			if (!used[cell])
			{
				number.Add(cell);
				used[cell] = true;
				if (dfs(cell))
				{
					return true;
				}
				used[cell] = false;
				number.RemoveAt(0);
			}
		}

		return false;
	}

	public void run()
	{
		Console.WriteLine("Type a matrix 3x3:");

		int filled = 0;
		while (filled < matrix_area)
		{
			string line = Console.ReadLine();
			string[] digits = Regex.Split(line, @"\s");
			foreach (var digit in digits)
			{
				int d;
				if (int.TryParse(digit, out d))
				{
					matrix[filled++] = d;
				}
			}
		}

		List<int> root = new List<int>() { 1, 3, 5, 7, 9 };

		graph.Add(root);

		// Create graph adjecendies by ver-hor neighbours
		for (int i = 0; i < matrix_area; i++)
		{
			List<int> nbs = new List<int>();
			int x = i / size,
				y = i % size;

			for (int nb = 0; nb < 4; nb++)
			{
				int nbX = x + nbsX[nb],
					nbY = y + nbsY[nb];
				if (nbX < 0 || nbX > (size - 1) || nbY < 0 || nbY > (size - 1))
				{
					continue;
				}
				nbs.Add(nbX * size + nbY + 1);
			}

			graph.Add(nbs);
		}

		// Sort graph adjacencies. So the max vertex will be first (0)
		foreach (var x in graph)
		{
			x.Sort((int l, int r) => matrix[r - 1] - matrix[l - 1]);
		}

		dfs(0);

		Console.WriteLine("The max path number:");
		foreach (var x in number)
		{
			Console.Write(matrix[x - 1]);
		}
	}
}

public class Program
{

	public static void Main()
	{
		Jun2 jun2 = new Jun2();
		jun2.run();

		Console.ReadKey();
	}
}