public static class Helpers
{
    public static int infCost = 3;
    public static int artCost = 4;
    public static int tankCost = 5;

    public static List<int[]> GetAllCombinations(int cost)
    {
        var combinations = new List<int[]>();

        int maxInf = cost / infCost;
        int maxArt = cost / artCost;
        int maxTank = cost / tankCost;

        var comp = new int[5] { 0, 0, 0, 0, 0 };

        for (int tank = 0; tank <= maxTank; tank++)
        {
            comp[0] = tank;

            if (GetCost(comp) <= cost && GetCost(comp) != 0)
            {
                combinations.Add(comp);
                Print(comp);
            }

            for (int art = 0; art <= maxArt; art++)
            {
                comp[1] = art;

                if (GetCost(comp) <= cost && GetCost(comp) != 0)
                {
                    combinations.Add(comp);
                    Print(comp);
                }

                for (int inf = 0; inf <= maxInf; inf++)
                {
                    comp[2] = inf;
                    if (GetCost(comp) <= cost && GetCost(comp) != 0)
                    {
                        combinations.Add(comp);
                        Print(comp);
                    }
                }
            }
        }

        return combinations;
    }

    private static void Print(int[] arr)
    {
        Console.WriteLine("[{0}], {1}", string.Join(", ", arr), GetCost(arr));
    }

    private static int GetCost(int[] arr)
    {
        return arr[0] * tankCost + arr[1] * artCost + arr[2] * infCost;
    }
}
