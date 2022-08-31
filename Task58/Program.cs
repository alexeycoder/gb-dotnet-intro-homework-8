// Задача 58: Задайте две матрицы. Напишите программу, которая будет находить произведение двух матриц.
// Например, даны 2 матрицы:
// 2 4 | 3 4
// 3 2 | 3 3
// Результирующая матрица будет:
// 18 20
// 15 18

do
{
	Console.OutputEncoding = System.Text.Encoding.UTF8;
	Console.Clear();
	PrintTitle("Произведение двух матриц", ConsoleColor.Cyan);

	ConsoleHelper.StoreColors();
	ConsoleHelper.SetColors(ConsoleColor.DarkGray, null);
	Console.WriteLine("Проверочное произведение:");

	int[,] sampleMtxA = new int[,] { { 2, 4 }, { 3, 2 } };
	int[,] sampleMtxB = new int[,] { { 3, 4 }, { 3, 3 } };
	int[,] sampleProdAB = GetMatrixProduct(sampleMtxA, sampleMtxB);
	PrintMatrixProduct(sampleMtxA, sampleMtxB, sampleProdAB);
	Console.WriteLine();

	ConsoleHelper.RestoreColors();

	Console.WriteLine("Для демонстрации будут созданы две матрицы A и Б,"
					+ " совместимые* по размерности для произведения А\u2715Б"
					+ " и заполненные случайными целыми значениями.");
	ConsoleHelper.WriteColored("(* \u2014 А и Б совместимы для произведения А\u2715Б,"
								+ " если количество столбцов А равно количеству строк Б)\n\n", ConsoleColor.DarkGray);

	int rowsA = GetUserInputInt("Введите число строк матрицы А:\t\t", 1);
	int colsA = GetUserInputInt("Введите число столбцов матрицы А"
									+ "\n (оно же число строк матрицы Б):\t", 1);
	int rowsB = colsA;
	int colsB = GetUserInputInt("Введите число столбцов матрицы Б:\t", 1);
	Console.WriteLine();
	int minimum = GetUserInputInt("Введите нижний предел диапазона случайных целых чисел:  ");
	int maximum = GetUserInputInt("Введите верхний предел диапазона случайных целых чисел: ", minimum);
	Console.WriteLine();

	int[,] mtxA = CreateMatrixRandomInt(rowsA, colsA, minimum, maximum);
	int[,] mtxB = CreateMatrixRandomInt(rowsB, colsB, minimum, maximum);
	int[,] prodAB = GetMatrixProduct(mtxA, mtxB);

	PrintMatrixProduct(mtxA, mtxB, prodAB);
	Console.WriteLine();

} while (AskForRepeat());

// Methods:

static void PrintMatrixProduct(int[,] matrixA, int[,] matrixB, int[,] productAB)
{
	const int horizontalHalfSpace = 2;

	(int rowsCountA, int colsCountA) = (matrixA.GetLength(0), matrixA.GetLength(1));
	(int rowsCountB, int colsCountB) = (matrixB.GetLength(0), matrixB.GetLength(1));
	(int rowsCountAB, int colsCountAB) = (productAB.GetLength(0), productAB.GetLength(1));

	string titleA = $"Матрица А ({rowsCountA} \u2715 {colsCountA}):";
	string titleB = $"Матрица Б ({rowsCountB} \u2715 {colsCountB}):";
	string titleAB = $"Произведение А\u2715Б ({rowsCountAB} \u2715 {colsCountAB}):";

	int maxRows = Math.Max(rowsCountAB, Math.Max(rowsCountA, rowsCountB));
	int maxLines = maxRows + 2;
	int verticalMarginA = (maxRows - rowsCountA) / 2;
	int verticalMarginB = (maxRows - rowsCountB) / 2;
	int verticalMarginAB = (maxRows - rowsCountAB) / 2;

	// MATRIX A
	ConsoleHelper.WriteColored(titleA, ConsoleColor.DarkGray);
	Console.WriteLine();
	ConsoleHelper.ShiftCursor(0, verticalMarginA);
	(int widthA, int heightA) = PrintMatrix(matrixA);

	// MATRIX B
	int leftPosB = Math.Max(titleA.Length, widthA) + 2 * horizontalHalfSpace + 1;
	ConsoleHelper.ShiftCursor(leftPosB, -heightA - verticalMarginA - 1);
	ConsoleHelper.WriteColored(titleB, ConsoleColor.DarkGray);
	Console.WriteLine();
	ConsoleHelper.ShiftCursor(leftPosB, verticalMarginB);
	(int widthB, int heightB) = PrintMatrix(matrixB);

	// MATRIX AB
	int leftPosAB = leftPosB + Math.Max(titleB.Length, widthB) + 2 * horizontalHalfSpace + 1;
	ConsoleHelper.ShiftCursor(leftPosAB, -heightB - verticalMarginB - 1);
	ConsoleHelper.WriteColored(titleAB, ConsoleColor.DarkGray);
	Console.WriteLine();
	ConsoleHelper.ShiftCursor(leftPosAB, verticalMarginAB);
	(int widthAB, int heightAB) = PrintMatrix(productAB);

	// OPERATORS x =
	int shiftToMidPos = -heightAB - verticalMarginAB + maxLines / 2 - 1;
	ConsoleHelper.ShiftCursor((widthA + leftPosB) / 2, shiftToMidPos);
	ConsoleHelper.WriteColored("\u2715", ConsoleColor.DarkGray); // x
	Console.WriteLine();
	ConsoleHelper.ShiftCursor((leftPosB + widthB + leftPosAB) / 2, -1);
	ConsoleHelper.WriteColored("=", ConsoleColor.DarkGray); // =
	Console.WriteLine();
	ConsoleHelper.ShiftCursor(0, maxLines / 2);
}

static int[,] GetMatrixProduct(int[,] matrixA, int[,] matrixB)
{
	int sharedCount = matrixA.GetLength(1);
	if (matrixB.GetLength(0) != sharedCount)
		return new int[,] { }; // should not happen

	int rowsCountA = matrixA.GetLength(0);
	int colsCountB = matrixB.GetLength(1);

	int[,] productAB = new int[rowsCountA, colsCountB];

	for (int row = 0; row < rowsCountA; ++row)
	{
		for (int col = 0; col < colsCountB; ++col)
		{
			int pValue = 0;
			for (int i = 0; i < sharedCount; ++i)
			{
				pValue += matrixA[row, i] * matrixB[i, col];
			}
			productAB[row, col] = pValue;
		}
	}

	return productAB;
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

#region Print Matrix Generic

static (int width, int height) PrintMatrix<T>(T[,] matrix, int maxFractionDigits = 2, int desirableCellSize = -1, Func<int, int, bool>? conditionToHighlight = null) where T : struct, IFormattable
{
	const ConsoleColor HighightForeColor = ConsoleColor.White;
	const ConsoleColor HighightBackColor = ConsoleColor.DarkBlue;

	int leftIndent = Console.CursorLeft;

	const string itemsDelimiter = "  ";
	const string paddingLeft = " ";
	string paddingRight = desirableCellSize <= 0 ? paddingLeft : new string(' ', desirableCellSize / 2 + 1);
	string format = GetNumbersToStringFormat(maxFractionDigits);

	string[,] stringMatrix = ToStringTable(matrix, format, desirableCellSize);

	int rowsLastIndex = stringMatrix.GetLength(0) - 1;
	int colsLastIndex = stringMatrix.GetLength(1) - 1;

	int posRight = 0;
	//ConsoleHelper.ShiftCursor(leftIndent, 0); // no need as initial indent is already in place
	Console.WriteLine("\u250f"); // ┏
	ConsoleHelper.ShiftCursor(leftIndent, 0);

	for (int row = 0; row <= rowsLastIndex; ++row)
	{
		Console.Write("\u2503" + paddingLeft); // ┃->
		for (int col = 0; col < colsLastIndex; ++col)
		{
			if (conditionToHighlight == null || !conditionToHighlight(row, col))
				Console.Write(stringMatrix[row, col] + itemsDelimiter);
			else
				ConsoleHelper.WriteColored(stringMatrix[row, col] + itemsDelimiter, HighightForeColor, HighightBackColor);
		}
		if (conditionToHighlight == null || !conditionToHighlight(row, colsLastIndex))
			Console.Write(stringMatrix[row, colsLastIndex]);
		else
			ConsoleHelper.WriteColored(stringMatrix[row, colsLastIndex], HighightForeColor, HighightBackColor);

		Console.Write(paddingRight + "\u2503"); // ->┃
		if (posRight == 0) posRight = Console.CursorLeft - 1; // just once enough (too slow in linux gui terminal, fast in tty)
		Console.WriteLine();
		ConsoleHelper.ShiftCursor(leftIndent, 0);
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

	int width = Console.CursorLeft - leftIndent;
	int height = rowsLastIndex + 1 + 2;

	Console.WriteLine();

	return (width, height);
}

static string GetNumbersToStringFormat(int maxFractionDigits)
{
	return maxFractionDigits == int.MaxValue
			?
			"G" : maxFractionDigits > 0 ? "0." + new string('#', maxFractionDigits) : "0";
}

static string[,] ToStringTable<T>(T[,] matrix, string format, int desirableCellSize = -1) where T : struct, IFormattable
{
	int rows = matrix.GetLength(0);
	int cols = matrix.GetLength(1);
	string[,] strTable = new string[rows, cols];
	int maxLength = 0;
	for (int col = 0; col < cols; ++col)
	{
		for (int row = 0; row < rows; ++row)
		{
			string strValue = matrix[row, col].ToString(format, null);
			strTable[row, col] = strValue;
			maxLength = Math.Max(maxLength, strValue.Length);
		}
	}
	// apply cells padding & alignment:
	maxLength = Math.Max(desirableCellSize, maxLength);
	for (int col = 0; col < cols; ++col)
	{
		for (int row = 0; row < rows; ++row)
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
	ConsoleHelper.WriteColored(titleDelimiter + Environment.NewLine + title + Environment.NewLine + titleDelimiter + Environment.NewLine, foreColor);
}

static void PrintError(string errorMessage, ConsoleColor foreColor)
{
	ConsoleHelper.WriteColored("\u2757 Ошибка: " + errorMessage, foreColor);
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

static class ConsoleHelper
{
	private static ConsoleColor defaultForeColor;
	private static ConsoleColor defaultBackColor;
	private static ConsoleColor? storedForeColor;
	private static ConsoleColor? storedBackColor;

	static ConsoleHelper()
	{
		defaultForeColor = Console.ForegroundColor;
		defaultBackColor = Console.BackgroundColor;
	}

	public static void ShiftCursor(int horizontalShift, int verticalShift)
	{
		if (horizontalShift != 0)
		{
			int newLeftPos = Console.CursorLeft + horizontalShift;
			newLeftPos = Math.Max(0, Math.Min(Console.BufferWidth - 1, newLeftPos));
			Console.CursorLeft = newLeftPos;
		}

		if (verticalShift != 0)
		{
			int newTopPos = Console.CursorTop + verticalShift;
			newTopPos = Math.Max(0, Math.Min(Console.BufferHeight - 1, newTopPos));
			Console.CursorTop = newTopPos;
		}
	}

	public static void SetColors(ConsoleColor? foreColor, ConsoleColor? backColor)
	{
		if (foreColor.HasValue)
			Console.ForegroundColor = foreColor.Value;
		if (backColor.HasValue)
			Console.BackgroundColor = backColor.Value;
	}

	public static void StoreColors()
	{
		storedForeColor = Console.ForegroundColor;
		storedBackColor = Console.BackgroundColor;
	}

	public static void RestoreColors()
	{
		if (storedForeColor.HasValue)
		{
			Console.ForegroundColor = storedForeColor.Value;
			storedForeColor = null;
		}
		if (storedBackColor.HasValue)
		{
			Console.BackgroundColor = storedBackColor.Value;
			storedBackColor = null;
		}
	}

	public static void RestoreDefaultColors()
	{
		Console.ForegroundColor = defaultForeColor;
		Console.BackgroundColor = defaultBackColor;
	}

	public static void WriteColored(string str, ConsoleColor? foreColor, ConsoleColor? backColor = null)
	{
		var bkpForeColor = Console.ForegroundColor; // do not store via global fields!
		var bkpBackColor = Console.BackgroundColor;
		SetColors(foreColor, backColor);
		Console.Write(str);
		Console.ForegroundColor = bkpForeColor;
		Console.BackgroundColor = bkpBackColor;
	}
}

#endregion Types
