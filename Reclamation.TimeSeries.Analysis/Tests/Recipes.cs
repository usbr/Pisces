using System;

namespace Reclamation.TimeSeries.Analysis.Tests
{
    internal class Recipes
    {
        internal static Series DailyIncrementalPrecipFromInstant(Series s)
        {
            throw new NotImplementedException();
        }

        //Check if there is a gradual loss of water in the data.
        internal static bool WaterLossCheck(Series s)
        {
            // Create and initialize variables.
            Series seriesToCheck = s.Copy();
            int max = seriesToCheck.Count; // Size of TimeSeries
            int i = 0; // Current position in TimeSeries array.
            int interval = 16; // Number of data points to average over.
            double prevAvg = 0; // The previous average.
            int count = 0; // Number of times the average decreased.
            int countThreshold = 6; // Number of times a decrease must occur to say water loss occured with certainty.
            double noiseThreshold = 0.02; // Threshold for what can be ignored as noise.

            //int startPoint = -1; //Where water loss occurs. -1 being no water loss.

            // Loop through all data until water loss confirmed or end of data.
            while (i < max)
            {
                // Get sum of interval.
                double dataSum = 0;
                for (int j = 0; j < interval; j++)
                {
                    Point currData = seriesToCheck[i++];
                    dataSum += currData.Value;
                }
                // Get actual average.
                double currAvg = dataSum / (double)interval;

                // If first loop iteration, set prevAvg and continue.
                if (prevAvg == 0)
                {
                    prevAvg = currAvg;
                    continue;
                }
                // If the water level went down, increment the counter
                if (currAvg < prevAvg)
                {
                    count++;
                    //if (startPoint == -1){
                    //startPoint = i - interval;
                    //}

                    // Check threshold
                    if (count >= countThreshold)
                    {
                        return true;
                        //break;
                    }
                }
                // If the water level increased more than the threshold, reset count.
                if( (currAvg - prevAvg) > noiseThreshold)
                {
                    count = 0;
                    //startPoint = -1;
                }
                prevAvg = currAvg;
            }
            // If we didn't find water loss, return false.
            return false;
            //return startPoint;
        }
    }
}
