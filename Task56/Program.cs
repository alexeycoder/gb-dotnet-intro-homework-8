// Задача 56: Задайте прямоугольный двумерный массив. Напишите программу, которая будет находить строку с наименьшей суммой элементов.
// Например, задан массив:
// 1 4 7 2
// 5 9 2 3
// 8 4 2 4
// 5 2 6 7
// Программа считает сумму элементов в каждой строке и выдаёт номер строки с наименьшей суммой элементов: 1 строка

const int AutoMatrixMinSize = 4;
const int AutoMatrixMaxSize = 9;
const int AutoMatrixMinValue = -200;
const int AutoMatrixMaxValue = 200;

const ConsoleColor HighightForeColor = ConsoleColor.White;
const ConsoleColor HighightBackColor = ConsoleColor.DarkBlue;
bool tmp = false;
do
{
	Console.OutputEncoding = System.Text.Encoding.UTF8;
	Console.Clear();
	PrintTitle("Поиск строки в матрице с наименьшей суммой элементов", ConsoleColor.Cyan);

	int[,] mtx = CreateMatrixRandomIntByUserChoice(out int rows, out int cols);
	int[] totalsByRow = GetTotalsByRow(mtx);
	int[] minRows = FindMinimumIndexes(totalsByRow);

	if (tmp = minRows.Length < 2)
		continue;

	ConsoleHelper.WriteColored($"Исходная матрица ({rows} \u2715 {cols}):\n", ConsoleColor.DarkGray);
	PrintMatrix(mtx);

	Console.WriteLine();
	ConsoleHelper.WriteColored("Результат\n", ConsoleColor.DarkGray);
	ConsoleHelper.WriteColored("Матрица:\n", ConsoleColor.DarkGray);
	Func<int, int, bool> conditionToHighlight = (row, _) => minRows.Contains(row);
	var consoleDimensions = PrintMatrix(mtx, 0, -1, conditionToHighlight);

	int horizontalSpace = 2;
	ConsoleHelper.ShiftCursor(consoleDimensions.width + horizontalSpace, -consoleDimensions.height - 1);
	ConsoleHelper.WriteColored("Итого:\n", ConsoleColor.DarkGray);
	ConsoleHelper.ShiftCursor(consoleDimensions.width + horizontalSpace, 0);
	PrintArrayAsColumn(totalsByRow, 0, -1, conditionToHighlight);

	if (minRows.Length == 1)
	{
		ConsoleHelper.WriteColored($"Номер строки с наименьшим итогом: {minRows[0] + 1}", ConsoleColor.Yellow);
	}
	else
	{
		string numbersStr = string.Join(", ", minRows.Select(n => (n + 1).ToString()));
		ConsoleHelper.WriteColored($"Номера строк с наименьшим итогом: {numbersStr}", ConsoleColor.Yellow);
	}
	Console.WriteLine();

} while (tmp || AskForRepeat());

// Methods:

static int[] FindMinimumIndexes(int[] array)
{
	if (array.Length == 0)
		return new int[] { };

	int[] tempIndexesStore = new int[array.Length];
	tempIndexesStore[0] = 0;
	int tempLastIndex = 0;

	var minValue = array[0];
	for (int i = 1; i < array.Length; ++i)
	{
		var currentValue = array[i];
		if (currentValue < minValue)
		{
			minValue = currentValue;
			tempIndexesStore[0] = i;
			tempLastIndex = 0;
		}
		else if (currentValue == minValue)
		{
			tempIndexesStore[++tempLastIndex] = i;
		}
	}

	int[] result = new int[tempLastIndex + 1];
	Array.Copy(tempIndexesStore, result, result.Length);

	return result;
}

static int[] GetTotalsByRow(int[,] matrix)
{
	int rowsCount = matrix.GetLength(0);
	int[] totals = new int[rowsCount];
	for (int row = 0; row < rowsCount; ++row)
	{
		totals[row] = GetRowTotal(matrix, row);
	}
	return totals;
}

static int GetRowTotal(int[,] matrix, int rowIndex)
{
	int rowsCount = matrix.GetLength(0);
	if (rowIndex < 0 || rowIndex >= rowsCount)
		throw new ArgumentOutOfRangeException(nameof(rowIndex));

	int sum = 0;
	int colsCount = matrix.GetLength(1);
	for (int col = 0; col < colsCount; ++col) // till preLast item
	{
		sum += matrix[rowIndex, col];
	}
	return sum;
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

static void PrintArrayAsColumn<T>(T[] array, int maxFractionDigits = 2, int desirableCellSize = -1, Func<int, int, bool>? conditionToHighlight = null) where T : struct, IFormattable
{
	T[,] column = ToColumn(array);
	PrintMatrix(column, maxFractionDigits, desirableCellSize, conditionToHighlight);
}

static T[,] ToColumn<T>(T[] array) where T : struct
{
	T[,] column = new T[array.Length, 1];
	for (int i = 0; i < array.Length; ++i)
	{
		column[i, 0] = array[i];
	}
	return column;
}

static (int width, int height) PrintMatrix<T>(T[,] matrix, int maxFractionDigits = 2, int desirableCellSize = -1, Func<int, int, bool>? conditionToHighlight = null) where T : struct, IFormattable
{
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

enum CreationMode
{
	Auto = 0,
	Manual
}

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
		StoreColors();
		SetColors(foreColor, backColor);
		Console.Write(str);
		RestoreColors();
	}
}

#endregion Types
