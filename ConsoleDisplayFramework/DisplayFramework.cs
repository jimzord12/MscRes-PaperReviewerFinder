using static System.Console;
using Patrikakis;

namespace Patrikakis.ConsoleDisplay;

public static class DisplayFramework {
    static int RowLength { get; set; }

    static DisplayFramework() {
        RowLength = 120;
    }

    // public static void SubDir(int level, string name) {
    //     string indentation = "+" + Repeat('-', level + 1) + "> ";
    //     string final = indentation + name;
    //     LeftText(final);
    // }
    public static void Indent(int level, string name) {
        string _base;
        string final;
      
        if (level > 1) {
          _base = Repeat(' ', level) + "|" + Repeat('-', level * level);
        } else {
          _base = "|" + Repeat('-', level);
        }

        final =  _base + "-> " + name;
        LeftText(final);
    }
    public static void PrintRow() { // ✅ Works!
        WriteLine('+' + Repeat('-', RowLength - 2) + '+');
    }
    public static void CenterText(string text = "") {
        int padding1 = (RowLength - 2) / 2 + (text.Length / 2);
        int padding2 = (RowLength - 2) / 2 - (text.Length / 2);

        WriteLine('|' + text.PadLeft(padding1) + Repeat(' ', padding2) + '|');
    }
    public static void RightText(string text, int padding = 3) {
        int spacer = RowLength - padding - 2;

        WriteLine('|' + text.PadLeft(spacer) + Repeat(' ', 3) + '|');
    }
    public static void LeftText(string text, int padding = 3) {
        int spacer = RowLength - padding - 2;

        WriteLine('|' + Repeat(' ', padding) + text.PadRight(spacer) + '|');
    }
    public static void LeftTextAction(string text, int padding = 3) {
        int spacer = RowLength - padding - 5;

        WriteLine("| ->" + Repeat(' ', padding) + text.PadRight(spacer) + '|');
    }
    public static void SpaceAround(string text1, string text2, int padding = 3) {
        int textSize1 = text1.Length;
        int textSize2 = text2.Length;
        int spacersSize = padding * 2;
        int columnsSize = 2;
        int finalSize = textSize1 + textSize2 + spacersSize + columnsSize;

        WriteLine('|' + Repeat(' ', 3) + text1 + Repeat(' ', RowLength - finalSize) + text2 + Repeat(' ', 3) + '|');
    }
    public static string Repeat(char ch, int count) { // ✅ Works!
        string temp = "";

        for (int i = 0; i < count; i++) {
            temp += ch.ToString();
        }
        return temp;
    }
}