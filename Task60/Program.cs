// Задача 60. Сформируйте трёхмерный массив из неповторяющихся двузначных чисел. Напишите программу, которая будет построчно выводить массив, добавляя индексы каждого элемента.
// Массив размером 2 x 2 x 2
// 66(0,0,0) 25(0,1,0)
// 34(1,0,0) 41(1,1,0)
// 27(0,0,1) 90(0,1,1)
// 26(1,0,1) 55(1,1,1)

// 3-rd dimension is usually 1-st index and it is called depth, planes, levels, sections, etc
// array3d[ plane, row, column ];

do
{
	Console.Clear();
	PrintTitle("Заполнение трёхмерного массива уникальными двузначными целыми числами", ConsoleColor.Cyan);

	int planes = GetUserInputInt("Введите глубину, т.е. число плоскостей трёхмерного массива:..........: ", 1);
	int rows = GetUserInputInt("Введите число строк, приходящихся на плоскость трёхмерного массива...: ", 1);
	int cols = GetUserInputInt("Введите число столбцов, приходящихся на плоскость трёхмерного массива: ", 1);

	int[,,] arr3d = CreateRandomArray3D(planes, rows, cols);

	Console.WriteLine("\nЭлементы трёхмерного массива (в таблицах по плоскостям):");

	for (int plane = 0; plane < planes; ++plane)
	{
		PrintColored($"Плоскость {plane}:\n", ConsoleColor.DarkGray);
		PrintPlaneOfArray3D(arr3d, plane);
	}

} while (AskForRepeat());

// Methods

static int[,,] CreateRandomArray3D(int planesCount, int rowsCount, int colsCount)
{
	var randomizer = new TwoDigitRandomizer();

	int[,,] array3d = new int[planesCount, rowsCount, colsCount];

	for (int plane = 0; plane < planesCount; ++plane)
	{
		for (int row = 0; row < rowsCount; ++row)
		{
			for (int col = 0; col < colsCount; ++col)
			{
				array3d[plane, row, col] = randomizer.Next();
			}
		}
	}

	return array3d;
}

#region Print Matrix Generic

static void PrintPlaneOfArray3D(int[,,] array3d, int planeIndex)
{
	if (planeIndex < 0 || planeIndex >= array3d.GetLength(0))
		throw new ArgumentOutOfRangeException(nameof(planeIndex));

	const string padding = " ";
	const string itemsDelimiter = "  ";

	string[,] stringTable = ToStringTable(array3d, planeIndex);

	int rowsLastIndex = stringTable.GetLength(0) - 1;
	int colsLastIndex = stringTable.GetLength(1) - 1;

	int posRight = 0;
	Console.WriteLine("\u250f"); // ┏

	for (int row = 0; row <= rowsLastIndex; ++row)
	{
		Console.Write("\u2503" + padding); // ┃->
		for (int col = 0; col < colsLastIndex; ++col)
		{
			Console.Write(stringTable[row, col] + itemsDelimiter);
		}
		Console.Write(stringTable[row, colsLastIndex]);
		Console.Write(padding + "\u2503"); // ->┃
		if (posRight == 0) posRight = Console.CursorLeft - 1; // just once enough (too slow in linux gui terminal, fast in tty)
		Console.WriteLine();
	}

	//--posRight;
	int bkpTopPos = Console.CursorTop;
	Console.Write("\u2517"); // ┗
	Console.CursorLeft = posRight;
	Console.Write("\u251b"); // ┛
	Console.CursorLeft = posRight;
	int rowsToTop = rowsLastIndex + 2;
	if (Console.CursorTop >= rowsToTop)
	{
		Console.CursorTop -= rowsToTop;
		Console.Write("\u2513"); // ┓
		Console.CursorTop = bkpTopPos;
	}
	Console.WriteLine();
}

static string[,] ToStringTable(int[,,] array3d, int planeIndex)
{
	int rowsCount = array3d.GetLength(1);
	int colsCount = array3d.GetLength(2);
	string[,] strTable = new string[rowsCount, colsCount];
	int maxLength = 0;
	for (int row = 0; row < rowsCount; ++row)
	{
		for (int col = 0; col < colsCount; ++col)
		{
			string strValue = $"{array3d[planeIndex, row, col]} ({planeIndex},{row},{col})";
			strTable[row, col] = strValue;
			maxLength = Math.Max(maxLength, strValue.Length);
		}
	}
	// apply cells padding & alignment:
	for (int col = 0; col < colsCount; ++col)
	{
		for (int row = 0; row < rowsCount; ++row)
		{
			strTable[row, col] = strTable[row, col].PadLeft(maxLength);
		}
	}

	return strTable;
}

#endregion Print Matrix Generic

#region User Interaction Common

static int GetUserInputInt(string inputMessage, int minAllowed = int.MinValue, int maxAllowed = int.MaxValue)
{
	const string errorMessageWrongType = "Некорректный ввод! Требуется целое число. Пожалуйста повторите\n";
	string errorMessageOutOfRange = string.Empty;
	if (minAllowed != int.MinValue && maxAllowed != int.MaxValue)
		errorMessageOutOfRange = $"Число должно быть в интервале от {minAllowed} до {maxAllowed}! Пожалуйста повторите\n";
	else if (minAllowed != int.MinValue)
		errorMessageOutOfRange = $"Число не должно быть меньше {minAllowed}! Пожалуйста повторите\n";
	else
		errorMessageOutOfRange = $"Число не должно быть больше {maxAllowed}! Пожалуйста повторите\n";

	int input;
	bool notANumber = false;
	bool outOfRange = false;
	do
	{
		if (notANumber)
		{
			PrintError(errorMessageWrongType, ConsoleColor.Magenta);
		}
		if (outOfRange)
		{
			PrintError(errorMessageOutOfRange, ConsoleColor.Magenta);
		}
		Console.Write(inputMessage);
		notANumber = !int.TryParse(Console.ReadLine(), out input);
		outOfRange = !notANumber && (input < minAllowed || input > maxAllowed);

	} while (notANumber || outOfRange);

	return input;
}

static void PrintTitle(string title, ConsoleColor foreColor)
{
	int feasibleWidth = Console.BufferWidth;
	string titleDelimiter = new string('\u2550', feasibleWidth);
	PrintColored(titleDelimiter + Environment.NewLine + title + Environment.NewLine + titleDelimiter + Environment.NewLine, foreColor);
}

static void PrintError(string errorMessage, ConsoleColor foreColor)
{
	PrintColored("\u2757 Ошибка: " + errorMessage, foreColor);
}

static void PrintColored(string message, ConsoleColor foreColor)
{
	var bkpColor = Console.ForegroundColor;
	Console.ForegroundColor = foreColor;
	Console.Write(message);
	Console.ForegroundColor = bkpColor;
}

static bool AskForRepeat()
{
	Console.WriteLine();
	Console.WriteLine("Нажмите Enter, чтобы начать сначала или Esc, чтобы завершить...");
	ConsoleKeyInfo key = Console.ReadKey(true);
	return key.Key != ConsoleKey.Escape;
}

#endregion User Interaction Common

#region Types

class TwoDigitRandomizer
{
	private int[] numbers = new int[99 - 10 + 1];
	private int pointer = 0;

	public TwoDigitRandomizer()
	{
		// initialize with 10..99
		for (int i = 0; i < numbers.Length; ++i)
		{
			numbers[i] = i + 10;
		}
		ShuffleArray(numbers);
	}

	private static void ShuffleArray(int[] array)
	{
		int count = array.Length;
		Random rnd = new Random();

		for (int i = 0; i < count; ++i)
		{
			Thread.Sleep(0);
			int randomIndex = rnd.Next(count);
			int temp = array[randomIndex];
			array[randomIndex] = array[i];
			array[i] = temp;
		}
	}

	public int Next()
	{
		if (pointer >= numbers.Length)
		{
			pointer = 0;
			ShuffleArray(numbers);
		}

		return numbers[pointer++];
	}
}

#endregion Types
