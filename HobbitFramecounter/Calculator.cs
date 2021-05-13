using System;

namespace HobbitFramecounter
{
    public static class Calculator
    {
        public static TimeSpan ConvertIntToTimespan(int time)
        {
            TimeSpan span = new TimeSpan();

            return span;
        }

        //Levenshtein algorithm for matching string similarities. Credits to github user @Davidblkx for original code.
        public static int CalculateStringDistance(string source1, string source2)
        {
            int source1Length = source1.Length;
            int source2Length = source2.Length;

            int[,] matrix = new int[source1Length + 1, source2Length + 1];

            if (source1Length == 0)
                return source2Length;

            if (source2Length == 0)
                return source1Length;

            for (int i = 0; i <= source1Length; matrix[i, 0] = i++) { }
            for (int j = 0; j <= source2Length; matrix[0, j] = j++) { }

            for (int i = 1; i <= source1Length; i++)
            {
                for (int j = 1; j <= source2Length; j++)
                {
                    int cost = (source2[j - 1] == source1[i - 1]) ? 0 : 1;

                    matrix[i, j] = Math.Min(
                        Math.Min(matrix[i - 1, j] + 1, matrix[i, j - 1] + 1),
                        matrix[i - 1, j - 1] + cost);
                }
            }
            return matrix[source1Length, source2Length];
        }
    }
}
