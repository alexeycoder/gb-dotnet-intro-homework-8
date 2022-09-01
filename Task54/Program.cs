// Задача 54: Задайте двумерный массив. Напишите программу, которая упорядочит по убыванию элементы каждой строки двумерного массива.
// Например, задан массив:
// 1 4 7 2
// 5 9 2 3
// 8 4 2 4
// В итоге получается вот такой массив:
// 7 4 2 1
// 9 5 3 2
// 8 4 4 2

const int AutoMatrixMinSize = 4;
const int AutoMatrixMaxSize = 15;
const int AutoMatrixMinValue = -200;
const int AutoMatrixMaxValue = 200;

do
{
	Console.Clear();
	PrintTitle("Упорядочивание по убыванию элементов каждой строки в матрице*"
	+ "\n(* \u2014 матрица == таблица == двумерный массив)", ConsoleColor.Cyan);

	int[,] mtx = CreateMatrixRandomIntByUserChoice(out int rows, out int cols);

	PrintColored($"Исходная матрица ({rows} \u2715 {cols}):\n", ConsoleColor.DarkGray);
	PrintMatrix(mtx);

	SortEachRowItems(mtx, SortDirection.Descending);

	PrintColored($"\nРезультат:\n", ConsoleColor.DarkGray);
	PrintMatrix(mtx);

} while (AskForRepeat());

// Methods:

static void SortEachRowItems<T>(T[,] matrix, SortDirection direction) where T : struct, IComparable
{
	int rowsCount = matrix.GetLength(0);
	for (int row = 0; row < rowsCount; ++row)
	{
		SortRowItems(matrix, row, direction);
	}
}

static void SortRowItems<T>(T[,] matrix, int rowIndex, SortDirection direction) where T : struct, IComparable
{
	int rowsCount = matrix.GetLength(0);
	if (rowIndex < 0 || rowIndex >= rowsCount)
		return;

	Func<T, T, bool> isNewExtreme = direction == SortDirection.Ascending
								?
								(itemA, itemB) => itemA.CompareTo(itemB) < 0
								:
								(itemA, itemB) => itemA.CompareTo(itemB) > 0;

	int colsCount = matrix.GetLength(1);
	for (int i = 0; i < colsCount - 1; ++i) // till preLast item
	{
		int referencePosition = i;

		for (int j = i + 1; j < colsCount; ++j) // till Last item
		{
			if (isNewExtreme(matrix[rowIndex, j], matrix[rowIndex, referencePosition]))
				referencePosition = j;
		}

		if (referencePosition != i)
		{
			SwapItems(matrix, rowIndex, i, rowIndex, referencePosition);
		}
	}
}

static void SwapItems<T>(T[,] matrix, int rowItemA, int colItemA, int rowItemB, int colItemB) where T : struct
{
	T temp = matrix[rowItemA, colItemA];
	matrix[rowItemA, colItemA] = matrix[rowItemB, colItemB];
	matrix[rowItemB, colItemB] = temp;
}

static int[,] CreateMatrixRandomInt(int rowsCount, int colsCount, int min, int max)
{
	int[,] matrix = new int[rowsCount, colsCount];

	Random rnd = new Random();
	max = max + 1;
	for (int row = 0; row < rowsCount; ++row)
	{
		for (int col = 0; col < colsCount; ++col)
		{
			matrix[row, col] = rnd.Next(min, max);
		}
	}
	return matrix;
}

#region Type-Specific Sort

// static void SortDescendingRowItems(int[,] matrix, int rowIndex)
// {
// 	int rowsCount = matrix.GetLength(0);
// 	if (rowIndex < 0 || rowIndex >= rowsCount)
// 		return;

// 	int colsCount = matrix.GetLength(1);
// 	for (int i = 0; i < colsCount - 1; ++i) // till preLast item
// 	{
// 		int maxPosition = i;

// 		for (int j = i + 1; j < colsCount; ++j) // till Last item
// 		{
// 			if (matrix[rowIndex, j] > matrix[rowIndex, maxPosition])
// 				maxPosition = j;
// 		}

// 		if (maxPosition != i)
// 		{
// 			SwapItems(matrix, rowIndex, i, rowIndex, maxPosition);
// 		}
// 	}
// }

// // just in case
// static void SortAscendingRowItems(int[,] matrix, int rowIndex)
// {
// 	int rowsCount = matrix.GetLength(0);
// 	if (rowIndex < 0 || rowIndex >= rowsCount)
// 		return;

// 	int colsCount = matrix.GetLength(1);
// 	for (int i = 0; i < colsCount - 1; ++i) // till preLast item
// 	{
// 		int minPosition = i;

// 		for (int j = i + 1; j < colsCount; ++j) // till Last item
// 		{
// 			if (matrix[rowIndex, j] < matrix[rowIndex, minPosition])
// 				minPosition = j;
// 		}

// 		if (minPosition != i)
// 		{
// 			SwapItems(matrix, rowIndex, i, rowIndex, minPosition);
// 		}
// 	}
// }

// static void SwapItems(int[,] matrix, int rowItemA, int colItemA, int rowItemB, int colItemB)
// {
// 	int temp = matrix[rowItemA, colItemA];
// 	matrix[rowItemA, colItemA] = matrix[rowItemB, colItemB];
// 	matrix[rowItemB, colItemB] = temp;
// }

#endregion Type-Specific Sort

#region Print Matrix Generic

static void PrintMatrix<T>(T[,] matrix, int maxFractionDigits = 2) where T : struct, IFormattable
{
	const string padding = " ";
	const string itemsDelimiter = "  ";
	string format = GetNumbersToStringFormat(maxFractionDigits);

	string[,] stringMatrix = ToStringTable(matrix, format);

	int rowsLastIndex = stringMatrix.GetLength(0) - 1;
	int colsLastIndex = stringMatrix.GetLength(1) - 1;

	int posRight = 0;
	Console.WriteLine("\u250f"); // ┏

	for (int row = 0; row <= rowsLastIndex; ++row)
	{
		Console.Write("\u2503" + padding); // ┃->
		for (int col = 0; col < colsLastIndex; ++col)
		{
			Console.Write(stringMatrix[row, col] + itemsDelimiter);
		}
		Console.Write(stringMatrix[row, colsLastIndex]);
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

static string GetNumbersToStringFormat(int maxFractionDigits)
{
	return maxFractionDigits == int.MaxValue
			?
			"G" : maxFractionDigits > 0 ? "0." + new string('#', maxFractionDigits) : "0";
}

static string[,] ToStringTable<T>(T[,] matrix, string format) where T : struct, IFormattable
{
	int rowsCount = matrix.GetLength(0);
	int colsCount = matrix.GetLength(1);
	string[,] strTable = new string[rowsCount, colsCount];
	int maxLength = 0;
	for (int row = 0; row < rowsCount; ++row)
	{
		for (int col = 0; col < colsCount; ++col)
		{
			string strValue = matrix[row, col].ToString(format, null);
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

#region User Interaction Spec

static int[,] CreateMatrixRandomIntByUserChoice(out int rowsCount, out int colsCount)
{
	Console.WriteLine("\nЖелаете задать параметры матрицы?:");
	Console.WriteLine("\u2023 Enter или Y \u2014 задать основные параметры матрицы,");
	Console.WriteLine("\u2023 Esc или N \u2014 создать случайную матрицу в полностью автоматическом режиме.");

	CreationMode? creationMode;
	while (!(creationMode = AskHowToCreateMatrix()).HasValue) { }
	Console.WriteLine();

	int minimum, maximum;
	if (creationMode.Value == CreationMode.Auto)
	{
		Random rnd = new Random();
		rowsCount = rnd.Next(AutoMatrixMinSize, AutoMatrixMaxSize + 1);
		colsCount = rnd.Next(AutoMatrixMinSize, AutoMatrixMaxSize + 1);
		minimum = AutoMatrixMinValue;
		maximum = AutoMatrixMaxValue;
	}
	else
	{
		rowsCount = GetUserInputInt("Введите число строк:\t", 1);
		colsCount = GetUserInputInt("Введите число столбцов:\t", 1);
		minimum = GetUserInputInt("Введите нижний предел диапазона случайных целых чисел:  ");
		maximum = GetUserInputInt("Введите верхний предел диапазона случайных целых чисел: ", minimum);
		Console.WriteLine();
	}

	int[,] matrix = CreateMatrixRandomInt(rowsCount, colsCount, minimum, maximum);
	return matrix;
}

static CreationMode? AskHowToCreateMatrix()
{
	ConsoleKeyInfo key = Console.ReadKey(true);
	if (key.Key == ConsoleKey.Escape || key.Key == ConsoleKey.N)
		return CreationMode.Auto;
	else if (key.Key == ConsoleKey.Enter || key.Key == ConsoleKey.Y)
		return CreationMode.Manual;

	return null;
}

#endregion User Interaction Spec

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
	int feasibleWidth = Math.Min(title.Length, Console.BufferWidth);
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

enum CreationMode
{
	Auto = 0,
	Manual
}

enum SortDirection
{
	Ascending = 0,
	Descending
}

#endregion Types
