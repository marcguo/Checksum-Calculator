using System;
using System.IO;

namespace Binary_File_Checksum_Calculator
{
    class Program
    {
        /// <summary>
        /// Program entry.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            // Keep performing checksum calculation for different files until the user closes the program.
            while (true)
            {
                // Let the user choose between checksum for one file or compare two files to find the
                // "diverge" point, meaning the bit number where two files become different at.
                Console.WriteLine("Please select between sum and com: \n");
                string selection = Console.ReadLine();
                // Compare two files to find the diverge point:
                if (selection == "com")
                {
                    Console.WriteLine("Please enter the full path of the first file you want to read: \n");
                    string firstFileName = Console.ReadLine();
                    byte[] firstFileBytes = File.ReadAllBytes(firstFileName);
                    Console.WriteLine("Please enter the full path of the second file you want to read: \n");
                    string secondFileName = Console.ReadLine();
                    byte[] secondFileBytes = File.ReadAllBytes(secondFileName);
                    // The current lower bound of the area of interest.
                    int currentLow = 0;
                    // The current upper bound of the area of interest.
                    int currentHigh = firstFileBytes.Length;
                    // Similar to the pivot point in a binary search.
                    int currentPivot = (currentLow + currentHigh) / 2;
                    // The old cutoff point, the byte number where we stop comparing two files' checksum at.
                    int oldPivot = 0;
                    // Are the files still the same while we go through them?
                    bool areSame = true;
                    // Before we find out the "diverge" point of these two files:
                    while (oldPivot != currentPivot)
                    {
                        // Update the old pivot point.
                        oldPivot = currentPivot;
                        // See if the files are still the same up to the currentPivot/cutoff.
                        areSame = IsSameCheckSum(currentPivot, firstFileBytes, secondFileBytes);
                        // Assign a new value to the current pivot point, depending on whether the files are
                        // still the same in checksum or not.
                        currentPivot = 
                            NextPivot(currentPivot, areSame, currentLow, currentHigh);
                        // Update the bounds accordingly.
                        currentHigh = currentPivot > oldPivot ? currentHigh : oldPivot;
                        currentLow  = currentPivot < oldPivot ? currentLow : oldPivot;
                    }
                    // Same size!
                    if (areSame)
                    {
                        Console.WriteLine("\nThese two files have the same checksum.");
                    }
                    // Not the same.
                    else
                    {
                        Console.WriteLine("\nGo to byte # " + currentPivot.ToString() + "\n");
                    }
                }
                // Or just calculate the checksum for one file:
                else if (selection == "sum")
                {
                    Console.WriteLine("Please enter the full path of the file you want to read: \n");
                    string fileName = Console.ReadLine();
                    byte[] fileBytes = File.ReadAllBytes(fileName);
                    int checkSum = CalculateCheckSum(fileBytes);
                    Console.WriteLine("Checksum is: ");
                    Console.WriteLine(checkSum.ToString());
                }
            }
        }

        /// <summary>
        /// Compares two files from the beginning up to the cutoff bit and determins if these two 
        /// files have the same checksum from the beginning to the cutoff point.
        /// </summary>
        /// <param name="cutoff">The cutoff point to stop comparison at.</param>
        /// <param name="firstFileBytes">The first file to compare, represented in byte array format.</param>
        /// <param name="secondFileBytes">The second file to compare, represented in byte array format.</param>
        /// <returns></returns>
        private static bool IsSameCheckSum(int cutoff, byte[] firstFileBytes, byte[] secondFileBytes)
        {
            // Places to store the current checksum of the files.
            int firstCheckSum  = 0;
            int secondCheckSum = 0;
            for (int i = 0; i < cutoff; i++)
            {
                firstCheckSum  += firstFileBytes[i];
                secondCheckSum += secondFileBytes[i];
            }
            return firstCheckSum == secondCheckSum;
        }

        /// <summary>
        /// Finds the next pivot point (the point to stop comparing checksum at for the files).
        /// </summary>
        /// <param name="currentPivot">The current pivot point, or the cutoff point.</param>
        /// <param name="isSame">Are the two files' checksums the same so far?</param>
        /// <param name="currentLow">The current lower bound of the area of interest.</param>
        /// <param name="currentHigh">The current upper bound of the area of interest.</param>
        /// <returns></returns>
        private static int NextPivot(int currentPivot, bool isSame, int currentLow, int currentHigh)
        {
            // If two files have the same checksum so far, move forward.
            if (isSame)
            {
                return (currentPivot + currentHigh) / 2;
            }
            // Else. backtrack to find out where things became different.
            else
            {
                return (currentPivot + currentLow) / 2;
            }
        }

        /// <summary>
        /// Calculates the checksum for the given byte array.
        /// </summary>
        /// <param name="bytes">The byte array to perform checksum on.</param>
        /// <returns>Sum of all bytes' values.</returns>
        private static int CalculateCheckSum(byte[] bytes)
        {
            int checkSum = 0;
            foreach (byte b in bytes)
            {
                checkSum += b;
            }
            return checkSum;
        }
    }
}
