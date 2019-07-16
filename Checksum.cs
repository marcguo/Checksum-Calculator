using System;
using System.IO;

namespace Binary_File_Checksum_Calculator
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Please select between sum and com: \n");
                string selection = Console.ReadLine();
                if (selection == "com")
                {
                    Console.WriteLine("Please enter the full path of the first file you want to read: \n");
                    string firstFileName = Console.ReadLine();
                    byte[] firstFileBytes = File.ReadAllBytes(firstFileName);
                    Console.WriteLine("Please enter the full path of the second file you want to read: \n");
                    string secondFileName = Console.ReadLine();
                    byte[] secondFileBytes = File.ReadAllBytes(secondFileName);
                    int currentLow = 0;
                    int currentHigh = firstFileBytes.Length;
                    int currentPivot = (currentLow + currentHigh) / 2;
                    int oldPivot = 0;
                    while (oldPivot != currentPivot)
                    {
                        oldPivot = currentPivot;
                        currentPivot = NextPivot(currentPivot, IsSameCheckSum(currentPivot, firstFileBytes, secondFileBytes),
                            currentLow, currentHigh);
                        currentHigh = currentPivot > oldPivot ? currentHigh : oldPivot;
                        currentLow = currentPivot < oldPivot ? currentLow : oldPivot;
                    }
                    Console.WriteLine("Go to byte # " + currentPivot.ToString() + "\n");
                }
                else if (selection == "sum")
                {
                    Console.WriteLine("Please enter the full path of the file you want to read: \n");
                    string fileName = Console.ReadLine();
                    byte[] fileBytes = File.ReadAllBytes(fileName);
                    int checkSum = CalculateCheckSum(fileBytes);
                    Console.WriteLine("Check sum is: ");
                    Console.WriteLine(checkSum.ToString());
                }
            }

            
        }

        static bool IsSameCheckSum(int cutoff, byte[] firstFileBytes, byte[] secondFileBytes)
        {
            int firstCheckSum = 0;
            int secondCheckSum = 0;
            for (int i = 0; i < cutoff; i++)
            {
                firstCheckSum += firstFileBytes[i];
                secondCheckSum += secondFileBytes[i];
            }
            return firstCheckSum == secondCheckSum;
        }

        static int NextPivot(int currentPivot, bool isSame, int currentLow, int currentHigh)
        {
            if (isSame)
            {
                return (currentPivot + currentHigh) / 2;
            }
            else
            {
                return (currentPivot + currentLow) / 2;
            }
        }

        static int CalculateCheckSum(byte[] bytes)
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
