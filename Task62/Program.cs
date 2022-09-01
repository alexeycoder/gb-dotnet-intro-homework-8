// Задача 62. Напишите программу, которая заполнит спирально массив 4 на 4.
// Например, на выходе получается вот такой массив:
// 01 02 03 04
// 12 13 14 05
// 11 16 15 06
// 10 09 08 07

const ConsoleColor HighlightBackColor = ConsoleColor.DarkBlue;

do
{
	Console.Clear();
	PrintTitle("Спиральное заполнение матрицы", ConsoleColor.Cyan);

	int m = GetUserInputInt("Введите число строк:\t", 1);
	int n = GetUserInputInt("Введите число столбцов:\t", 1);

	// // test for-for ->
	// for (m = 1; m < 22; ++m)
	// 	for (n = 1; n < 22; ++n)
	// 	{

	var spiralMatrix = GetSpiralMatrixV2(m, n, out int endRow, out int endCol);
	int maxValue = spiralMatrix[endRow, endCol]; // must be == m * n

	PrintColored($"\nМатрица {m} \u2715 {n} (значение 'конечного' элемента спирали: {maxValue})\n", ConsoleColor.DarkGray);
	PrintSpiralMatrixInt(spiralMatrix, GetNumbersToStringFormat(maxValue), endRow, endCol);

	if (maxValue != m * n)
	{
		// should never get in here
		PrintError("\nОшибка: Значение конечного элемента не соответствует размерности матрицы!\n", ConsoleColor.Red);
	}

	// } // <- test for-for

} while (AskForRepeat());

// Methods

static int[,] GetSpiralMatrixV2(int rowsCount, int colsCount, out int endRowIndex, out int endColIndex)
{
	if (rowsCount < 1 || colsCount < 1)
		throw new ArgumentOutOfRangeException();

	const int goRight = 0;
	const int goDown = 1;
	const int goLeft = 2;
	const int goUp = 3;

	int[,] matrix = new int[rowsCount, colsCount];

	int h = 0;
	int v = 0;
	int hMin = 0;
	int hMax = colsCount - 1;
	int vMin = 0;
	int vMax = rowsCount - 1;

	int dir = goRight;
	int count = rowsCount * colsCount;

	for (int i = 1; i < count; ++i)
	{
		matrix[v, h] = i;

		// тупика после поворота не возникнет покуда i < count
		if (dir == goRight)
		{
			if (h < hMax) { ++h; } else { ++vMin; ++v; dir = goDown; }
		}
		else if (dir == goDown)
		{
			if (v < vMax) { ++v; } else { --hMax; --h; dir = goLeft; }
		}
		else if (dir == goLeft)
		{
			if (h > hMin) { --h; } else { --vMax; --v; dir = goUp; }
		}
		else if (dir == goUp)
		{
			if (v > vMin) { --v; } else { ++hMin; ++h; dir = goRight; }
		}
	}
	matrix[v, h] = count;

	endRowIndex = v;
	endColIndex = h;
	return matrix;
}

// static int[,] GetSpiralMatrixV1(int rowsCount, int colsCount, out int endRowIndex, out int endColIndex)
// {
// 	if (rowsCount < 1 || colsCount < 1)
// 		throw new ArgumentOutOfRangeException();

// 	int[,] matrix = new int[rowsCount, colsCount];

// 	int h = 0;
// 	int v = 0;
// 	int hMin = 0;
// 	int hMax = colsCount - 1;
// 	int vMin = 0;
// 	int vMax = rowsCount - 1;

// 	int i = 1;
// 	while (true)
// 	{
// 		// go right
// 		for (h = hMin; h <= hMax; ++h) matrix[v, h] = i++;
// 		h = hMax; // before break to get correct end indexes
// 		if (++vMin > vMax) break;

// 		// go down
// 		for (v = vMin; v <= vMax; ++v) matrix[v, h] = i++;
// 		v = vMax;
// 		if (--hMax < hMin) break;

// 		// go left
// 		for (h = hMax; h >= hMin; --h) matrix[v, h] = i++;
// 		h = hMin;
// 		if (--vMax < vMin) break;

// 		// go up
// 		for (v = vMax; v >= vMin; --v) matrix[v, h] = i++;
// 		v = vMin;
// 		if (++hMin > hMax) break;
// 	}

// 	endRowIndex = v;
// 	endColIndex = h;
// 	return matrix;
// }

// static int[,] GetSpiralSquareMatrixV0(int n, out int maxValue)
// {
// 	int[,] matrix = new int[n, n];
// 	int itemsCount = n * n;

// 	int u = 0;
// 	int v = 0;
// 	int uMin = 0;
// 	int uMax = n - 1;
// 	int vMin = 0;
// 	int vMax = n - 1;

// 	int i = 1;
// 	while (i <= itemsCount)
// 	{
// 		// go right
// 		for (u = uMin; u <= uMax; ++u) matrix[v, u] = i++;
// 		u = uMax;
// 		++vMin;

// 		// go down
// 		for (v = vMin; v <= vMax; ++v) matrix[v, u] = i++;
// 		v = vMax;
// 		--uMax;

// 		// go left
// 		for (u = uMax; u >= uMin; --u) matrix[v, u] = i++;
// 		u = uMin;
// 		--vMax;

// 		// go up
// 		for (v = vMax; v >= vMin; --v) matrix[v, u] = i++;
// 		v = vMin;
// 		++uMin;
// 	}

// 	maxValue = i - 1;
// 	return matrix;
// }

#region Print Matrix

static void PrintSpiralMatrixInt(int[,] matrix, string format, int rowToHighlight = -1, int colToHighlight = -1)
{
	const string padding = " ";
	const string itemsDelimiter = "  ";

	int rowsLastIndex = matrix.GetLength(0) - 1;
	int colsLastIndex = matrix.GetLength(1) - 1;

	int posRight = 0;
	Console.WriteLine("\u250f"); // ┏ - top left corner

	for (int row = 0; row <= rowsLastIndex; ++row)
	{
		Console.Write("\u2503" + padding); // ┃-> - left wall
		for (int col = 0; col < colsLastIndex; ++col)
		{
			WriteHighlighted(matrix[row, col].ToString(format), row, col, rowToHighlight, colToHighlight);
			Console.Write(itemsDelimiter);
		}

		WriteHighlighted(matrix[row, colsLastIndex].ToString(format), row, colsLastIndex, rowToHighlight, colToHighlight);
		Console.Write(padding + "\u2503"); // ->┃ - right wall
		if (posRight == 0) posRight = Console.CursorLeft - 1;
		Console.WriteLine();
		if (row != rowsLastIndex) // print empty row
		{
			Console.Write("\u2503");
			Console.CursorLeft = posRight;
			Console.Write("\u2503");
			Console.WriteLine();
		}
	}

	int bkpTopPos = Console.CursorTop;
	Console.Write("\u2517"); // ┗ - bottom left corner
	Console.CursorLeft = posRight;
	Console.Write("\u251b"); // ┛ - bottom right corner
	Console.CursorLeft = posRight;
	int rowsToTop = rowsLastIndex * 2 + 2;
	if (Console.CursorTop >= rowsToTop) // go top right
	{
		Console.CursorTop -= rowsToTop;
		Console.Write("\u2513"); // ┓ - top right corner
		Console.CursorTop = bkpTopPos;
	}
	Console.WriteLine();
}

static void WriteHighlighted(string str, int row, int col, int rowToHighlight, int colToHighlight)
{
	if (row == rowToHighlight && col == colToHighlight)
	{
		var bkpColor = Console.BackgroundColor;
		Console.BackgroundColor = HighlightBackColor;
		Console.Write(str);
		Console.BackgroundColor = bkpColor;
	}
	else
	{
		Console.Write(str);
	}
}

static string GetNumbersToStringFormat(int maxNumber)
{
	return $"D{maxNumber.ToString().Length}";
}

#endregion Print Matrix

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
