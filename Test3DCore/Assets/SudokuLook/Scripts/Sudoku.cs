using System.Collections.Generic;

public class Sudoku
{
    public static HashSet<int> GetInvolved(int n)
    {
        HashSet<int> result = GetRow(n);
        result.UnionWith(GetCol(n));
        result.UnionWith(GetSector(n));
        return result;
    }

    public static HashSet<int> GetRow(int n)
    {
        HashSet<int> result = new HashSet<int>();
        int a = n - n % 9;
        for (int i = 0; i <= 8; i++)
        {
            int b = a + i;
            if (b != n) result.Add(b);
        }
        return result;
    }

    public static HashSet<int> GetCol(int n)
    {
        HashSet<int> result = new HashSet<int>();
        int a = n % 9;
        for (int i = 0; i <= 8; i++)
        {
            int b = a + i * 9;
            if (b != n) result.Add(b);
        }
        return result;
    }

    public static HashSet<int> GetSector(int n)
    {
        HashSet<int> result = new HashSet<int>();
        int a = n - n % 27 + n % 9 - n % 3;
        for (int b = 0; b <= 2; b++)
        {
            for (int c = 0; c <= 2; c++)
            {
                int d = a + b + c * 9;
                if (d != n) result.Add(d);
            }
        }
        return result;
    }

    public readonly Cell[] Cells;
    public List<int> Propagatable;
    public int ObservedCount;
    public int MinEn;

    public Sudoku()
    {
        Cells = new Cell[81];
        Propagatable = new List<int>();
        ObservedCount = 0;
        MinEn = 9;
    }
}
