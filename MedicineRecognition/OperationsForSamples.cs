using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicineRecognition
{
    public static class OperationsForSamples
    {
        /// <summary>
        /// Returns the index of the medicine which the classifier considers to be the correct class.
        /// </summary>
        /// <param name="testSample">The sample to classify.</param>
        /// <param name="standardValues">The matrix which stores the standard values of each kind of medicine.</param>
        /// <returns>The index of the medicine which the classifier considers to be the correct class.</returns>
        public static int Recognize(double[] testSample, double[][] standardValues)
        {
            // Get the count of medicines.
            int medicineCount = standardValues.Length;

            // Instantiate a double array to store the squared errors.
            double[] errors = new double[medicineCount];

            // Get the squared errors of each kind of medicine.
            for (int medicineIndex = 0; medicineIndex < medicineCount; medicineIndex++)
                errors[medicineCount] = GetSquaredError(testSample, standardValues[medicineIndex]);

            // Return the index with min squared error.
            return IndexOfMin(errors);
        }

        /// <summary>
        /// Returns the index of the min element in an array.
        /// </summary>
        /// <param name="arrayToSearch">The specified array in which to find the index.</param>
        /// <returns>The index of the min element in an array.</returns>
        private static int IndexOfMin(double[] arrayToSearch)
        {
            int indexOfMin = 0;
            for (int i = 1; i < arrayToSearch.Length; i++)
            {
                if (arrayToSearch[i] < indexOfMin)
                    indexOfMin = i;
            }
            return indexOfMin;
        }

        /// <summary>
        /// Computes and returns the sum of squared error of 2 given array.
        /// </summary>
        /// <param name="array1">An array.</param>
        /// <param name="array2">Another array.</param>
        /// <returns>The sum of squared error of 2 given array.</returns>
        private static double GetSquaredError(double[] array1, double[] array2)
        {
            if (array1.Length != array2.Length)
                throw new ArgumentException("The arguments for GetSquaredError() method must have the same length.");

            double error = 0;

            for (int i = 0; i < array1.Length; i++)
            {
                double delta = array1[i] - array2[i];
                error += delta * delta;
            }

            return error;
        }

        /// <summary>
        /// Returns the indicies of maximums in the given vector.
        /// </summary>
        /// <param name="vector">The given vector.</param>
        /// <returns>The indicies of maximums in a given vector.</returns>
        public static int[] IndiciesOfMaximums(double[] vector)
        {
            // The LinkedList<int> instance to store the indicies of maxmums of the given vector.
            LinkedList<int> indiciesOfMaxmums = new LinkedList<int>();

            // Traverse the entire vector and find those indicies.
            for (int i = 1; i < vector.Length - 1; i++)
            {
                // Add the index to the linked list if it satisfies the conditions below.
                if ((vector[i] > vector[i - 1]) && (vector[i] > vector[i + 1]))
                    indiciesOfMaxmums.AddLast(i);
            }

            // Convert the linked list to an int array and return this array.
            return indiciesOfMaxmums.ToArray();
        }
    }
}