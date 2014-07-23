using System;
using System.Collections.Generic;

namespace Reclamation.TimeSeries.Estimation
{

    /// <summary>
    /// StackOverflow class to generate combination lists for use with MLR interpolation
    /// Source: http://stackoverflow.com/questions/548402/list-all-possible-combinations-of-k-integers-between-1-n-n-choose-k
    /// Downloaded and tested 18JULY2014 - JR
    /// </summary>
    internal class AllPossibleCombination
    {
        // Initialize required variables
        int n, k;
        int[] indices;
        List<int[]> combinations = null;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="n_"></param>
        /// <param name="k_"></param>
        public AllPossibleCombination(int n_, int k_)
        {
            if (n_ <= 0)
            { throw new ArgumentException("n_ must be in N+"); }
            if (k_ <= 0)
            { throw new ArgumentException("k_ must be in N+"); }
            if (k_ > n_)
            { throw new ArgumentException("k_ can be at most n_"); }

            n = n_;
            k = k_;
            indices = new int[k];
            indices[0] = 1;
        }

        /// <summary>
        /// Returns all possible k combination of 0..n-1
        /// </summary>
        /// <returns></returns>
        public List<int[]> GetCombinations()
        {
            if (combinations == null)
            {
                combinations = new List<int[]>();
                Iterate(0);
            }
            return combinations;
        }

        /// <summary>
        /// Generates the combination list
        /// </summary>
        /// <param name="ii"></param>
        private void Iterate(int ii)
        {
            // Initialize
            if (ii > 0)
            { indices[ii] = indices[ii - 1] + 1; }

            for (; indices[ii] <= (n - k + ii + 1); indices[ii]++)
            {
                if (ii < k - 1)
                { Iterate(ii + 1); }
                else
                {
                    int[] combination = new int[k];
                    indices.CopyTo(combination, 0);
                    combinations.Add(combination);
                }
            }
        }
    }
}
