/// <summary>
/// Demonstrates various string comparison methods in .NET
/// Based on: https://learn.microsoft.com/en-us/dotnet/standard/base-types/comparing
/// </summary>

// ============================================================
// String.Compare Method
// ============================================================
// Compares two strings and returns an integer:
//   - Negative: first string precedes second in sort order (or first is null)
//   - 0: strings are equal (or both are null)
//   - Positive: first string follows second in sort order (or second is null)
// Note: Use for ordering/sorting, NOT for equality testing

Console.WriteLine("=== String.Compare Method ===");
string string1 = "Hello World!";
int compareResult = String.Compare(string1, "Hello World?");
Console.WriteLine($"Compare(\"Hello World!\", \"Hello World?\"): {compareResult}");
// Output: -1 (because '!' comes before '?' in sort order)

// Culture-sensitive comparison
compareResult = String.Compare("café", "cafe", StringComparison.CurrentCulture);
Console.WriteLine($"Compare(\"café\", \"cafe\", CurrentCulture): {compareResult}");

// Culture-insensitive (ordinal) comparison
compareResult = String.Compare("café", "cafe", StringComparison.Ordinal);
Console.WriteLine($"Compare(\"café\", \"cafe\", Ordinal): {compareResult}");

// Case-insensitive comparison
compareResult = String.Compare("HELLO", "hello", StringComparison.OrdinalIgnoreCase);
Console.WriteLine($"Compare(\"HELLO\", \"hello\", OrdinalIgnoreCase): {compareResult}");
Console.WriteLine();

// ============================================================
// String.CompareOrdinal Method
// ============================================================
// Compares two strings without considering local culture
// Performs a byte-by-byte comparison based on Unicode values

Console.WriteLine("=== String.CompareOrdinal Method ===");
string1 = "Hello World!";
int ordinalResult = String.CompareOrdinal(string1, "hello world!");
Console.WriteLine($"CompareOrdinal(\"Hello World!\", \"hello world!\"): {ordinalResult}");
// Output: -32 (difference between 'H' (72) and 'h' (104) Unicode values)

ordinalResult = String.CompareOrdinal("ABC", "ABC");
Console.WriteLine($"CompareOrdinal(\"ABC\", \"ABC\"): {ordinalResult}");
// Output: 0 (strings are identical)
Console.WriteLine();

// ============================================================
// String.CompareTo Method
// ============================================================
// Instance method that compares the current string to another
// Always culture-sensitive and case-sensitive

Console.WriteLine("=== String.CompareTo Method ===");
string1 = "Hello World";
string string2 = "Hello World!";
int compareToResult = string1.CompareTo(string2);
Console.WriteLine($"\"Hello World\".CompareTo(\"Hello World!\"): {compareToResult}");
// Output: -1 (first string is shorter/precedes)

compareToResult = "apple".CompareTo("banana");
Console.WriteLine($"\"apple\".CompareTo(\"banana\"): {compareToResult}");
// Output: -1 (apple comes before banana alphabetically)

compareToResult = "zebra".CompareTo("apple");
Console.WriteLine($"\"zebra\".CompareTo(\"apple\"): {compareToResult}");
// Output: 1 (zebra comes after apple alphabetically)
Console.WriteLine();

// ============================================================
// String.Equals Method
// ============================================================
// Determines if two strings are the same
// Returns true or false (Boolean)
// Recommended method for equality testing

Console.WriteLine("=== String.Equals Method ===");
string1 = "Hello World";
bool equalsResult = string1.Equals("Hello World");
Console.WriteLine($"\"Hello World\".Equals(\"Hello World\"): {equalsResult}");
// Output: True

// Static method usage
string2 = "Hello World";
equalsResult = String.Equals(string1, string2);
Console.WriteLine($"String.Equals(string1, string2): {equalsResult}");
// Output: True

// Case-insensitive equality check
equalsResult = String.Equals("HELLO", "hello", StringComparison.OrdinalIgnoreCase);
Console.WriteLine($"String.Equals(\"HELLO\", \"hello\", OrdinalIgnoreCase): {equalsResult}");
// Output: True

// Case-sensitive (default)
equalsResult = "HELLO".Equals("hello");
Console.WriteLine($"\"HELLO\".Equals(\"hello\"): {equalsResult}");
// Output: False
Console.WriteLine();

// ============================================================
// String.StartsWith Method
// ============================================================
// Determines if a string begins with specified characters
// Returns true or false

Console.WriteLine("=== String.StartsWith Method ===");
string1 = "Hello World";
bool startsWithResult = string1.StartsWith("Hello");
Console.WriteLine($"\"Hello World\".StartsWith(\"Hello\"): {startsWithResult}");
// Output: True

startsWithResult = string1.StartsWith("World");
Console.WriteLine($"\"Hello World\".StartsWith(\"World\"): {startsWithResult}");
// Output: False

// Case-insensitive check
startsWithResult = string1.StartsWith("hello", StringComparison.OrdinalIgnoreCase);
Console.WriteLine($"\"Hello World\".StartsWith(\"hello\", OrdinalIgnoreCase): {startsWithResult}");
// Output: True
Console.WriteLine();

// ============================================================
// String.EndsWith Method
// ============================================================
// Determines if a string ends with specified characters
// Returns true or false

Console.WriteLine("=== String.EndsWith Method ===");
string1 = "Hello World";
bool endsWithResult = string1.EndsWith("World");
Console.WriteLine($"\"Hello World\".EndsWith(\"World\"): {endsWithResult}");
// Output: True

endsWithResult = string1.EndsWith("Hello");
Console.WriteLine($"\"Hello World\".EndsWith(\"Hello\"): {endsWithResult}");
// Output: False

// Case-insensitive check
endsWithResult = string1.EndsWith("WORLD", StringComparison.OrdinalIgnoreCase);
Console.WriteLine($"\"Hello World\".EndsWith(\"WORLD\", OrdinalIgnoreCase): {endsWithResult}");
// Output: True
Console.WriteLine();

// ============================================================
// String.Contains Method
// ============================================================
// Determines if a character or string occurs within another string
// Returns true or false

Console.WriteLine("=== String.Contains Method ===");
string1 = "Hello World";
bool containsResult = string1.Contains("World");
Console.WriteLine($"\"Hello World\".Contains(\"World\"): {containsResult}");
// Output: True

containsResult = string1.Contains("Goodbye");
Console.WriteLine($"\"Hello World\".Contains(\"Goodbye\"): {containsResult}");
// Output: False

containsResult = string1.Contains('o');
Console.WriteLine($"\"Hello World\".Contains('o'): {containsResult}");
// Output: True

// Case-insensitive check
containsResult = string1.Contains("HELLO", StringComparison.OrdinalIgnoreCase);
Console.WriteLine($"\"Hello World\".Contains(\"HELLO\", OrdinalIgnoreCase): {containsResult}");
// Output: True
Console.WriteLine();

// ============================================================
// String.IndexOf Method
// ============================================================
// Returns the zero-based index of the first occurrence of a character/string
// Returns -1 if not found

Console.WriteLine("=== String.IndexOf Method ===");
string1 = "Hello World";
int indexResult = string1.IndexOf('l');
Console.WriteLine($"\"Hello World\".IndexOf('l'): {indexResult}");
// Output: 2 (first 'l' is at index 2)

indexResult = string1.IndexOf("World");
Console.WriteLine($"\"Hello World\".IndexOf(\"World\"): {indexResult}");
// Output: 6

indexResult = string1.IndexOf('z');
Console.WriteLine($"\"Hello World\".IndexOf('z'): {indexResult}");
// Output: -1 (not found)

// Start searching from a specific index
indexResult = string1.IndexOf('l', 3);
Console.WriteLine($"\"Hello World\".IndexOf('l', 3): {indexResult}");
// Output: 3 (second 'l' starting search from index 3)
Console.WriteLine();

// ============================================================
// String.LastIndexOf Method
// ============================================================
// Returns the zero-based index of the last occurrence of a character/string
// Searches from the end of the string
// Returns -1 if not found

Console.WriteLine("=== String.LastIndexOf Method ===");
string1 = "Hello World";
int lastIndexResult = string1.LastIndexOf('l');
Console.WriteLine($"\"Hello World\".LastIndexOf('l'): {lastIndexResult}");
// Output: 9 (last 'l' is at index 9)

lastIndexResult = string1.LastIndexOf('o');
Console.WriteLine($"\"Hello World\".LastIndexOf('o'): {lastIndexResult}");
// Output: 7 (last 'o' is in "World")

lastIndexResult = string1.LastIndexOf("Hello");
Console.WriteLine($"\"Hello World\".LastIndexOf(\"Hello\"): {lastIndexResult}");
// Output: 0
Console.WriteLine();

// ============================================================
// Practical Example: Using IndexOf with Remove
// ============================================================

Console.WriteLine("=== Practical Example: IndexOf with Remove ===");
string sentence = "The quick brown fox jumps over the lazy dog";
int foxIndex = sentence.IndexOf("fox");
if (foxIndex >= 0)
{
    string modified = sentence.Remove(foxIndex, 3);
    Console.WriteLine($"Original: \"{sentence}\"");
    Console.WriteLine($"After removing 'fox': \"{modified}\"");
}
Console.WriteLine();

// ============================================================
// StringComparison Enumeration Options
// ============================================================

Console.WriteLine("=== StringComparison Options Summary ===");
Console.WriteLine("StringComparison.CurrentCulture          - Culture-sensitive, case-sensitive");
Console.WriteLine("StringComparison.CurrentCultureIgnoreCase - Culture-sensitive, case-insensitive");
Console.WriteLine("StringComparison.InvariantCulture        - Invariant culture, case-sensitive");
Console.WriteLine("StringComparison.InvariantCultureIgnoreCase - Invariant culture, case-insensitive");
Console.WriteLine("StringComparison.Ordinal                 - Ordinal (byte-by-byte), case-sensitive");
Console.WriteLine("StringComparison.OrdinalIgnoreCase       - Ordinal, case-insensitive");
