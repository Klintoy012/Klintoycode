using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Clintoy
{
    class EnigmaClass
    {
        public static List<string> ringLines = new List<string>();
        public static List<char> textboxInput = new List<char>();
        public static List<char> textboxOutput = new List<char>();
        public static char[,] groupedRings = new char[,] { };
        public static char[,] rotatingRings = new char[,] { };
        public static int[] ringSelection = new int[3] { 0, 0, 0 };
        public static int[] ringSettings = new int[3] { 0, 0, 0 };
        public static bool checkboxIsChecked = false;

        public static bool ReadFiles(string path)
        {
            ringLines.Clear(); // Assuming ringLines is a static field in EnigmaClass

            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    sr.ReadLine();
                    sr.ReadLine();

                    string line = "";
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] values = line.Split(',');

                        foreach (string value in values)
                        {
                            if (!double.TryParse(value, out _))
                            {
                                throw new InvalidDataException("The CSV file contains a non-numeric value.");
                            }
                        }
                        ringLines.Add(line);
                    }
                }
            }
            catch (InvalidDataException e)
            {
                MessageBox.Show(e.Message, "CSV Format Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            catch (Exception e)
            {
                MessageBox.Show("An error occurred while reading the CSV file: " + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }


        //public static void ClearRingLines()
        //{
        //    ringLines.Clear();
        //}
        //public static void ReadFiles(string path)
        //{
        //    try
        //    {
        //        using (StreamReader sr = new StreamReader(path))
        //        {
        //            sr.ReadLine();
        //            sr.ReadLine();

        //            string line = "";
        //            while ((line = sr.ReadLine()) != null)
        //            {
        //                string[] values = line.Split(',');

        //                foreach (string value in values)
        //                {
        //                    if (!double.TryParse(value, out _)) // trying to parse each value to double
        //                    {
        //                        throw new InvalidDataException("The CSV file contains a non-numeric value.");
        //                    }
        //                }

        //                ringLines.Add(line);
        //            }
        //        }
        //    }
        //    catch (InvalidDataException e)
        //    {
        //        // Handling specific CSV format errors
        //        MessageBox.Show(e.Message, "CSV Format Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //        ringLines = new List<string>();
        //    }
        //    catch (Exception e)
        //    {
        //        // Handling other generic errors
        //        MessageBox.Show("An error occurred while reading the CSV file: " + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //        ringLines = new List<string>();
        //    }
        //}

        //public static void ReadFiles(string path) //Reads csv file
        //{
        //    try
        //    {
        //        using (StreamReader sr = new StreamReader(path))
        //        {
        //            sr.ReadLine();
        //            sr.ReadLine();

        //            string line = "";                
        //            while ((line = sr.ReadLine()) != null)
        //            {
        //                ringLines.Add(line);
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        ringLines = new List<string>();
        //    }
        //}

        //public static class CSVFileReader
        //{
        //    public static List<string> ReadCSVFile(string path)
        //    {
        //        List<string> lines = new List<string>();

        //        try
        //        {
        //            using (StreamReader sr = new StreamReader(path))
        //            {
        //                string line;

        //                while ((line = sr.ReadLine()) != null)
        //                {
        //                    lines.Add(line);
        //                }
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            // Handle exceptions as needed
        //        }

        //        return lines;
        //    }
        //}

        public static void ringContentSeparator() //Sort the contents of the csv file.     
        {
            if (ringLines.Count == 0)
            {
                Console.WriteLine("No data to process in ringLines."); // Log to console or use other logging
                // Optionally, show a message to the user, if appropriate in your application context
                return;
            }


            int xCount = ringLines.Count;
            int yCount = ringLines[0].Split(',').Length;
            groupedRings = new char[yCount, xCount];
            char[,] ungroupedRings = new char[xCount, yCount];

            for (int x = 0; x < xCount; x++)
            {
                string[] ringElements = ringLines[x].Split(',');

                if (ringElements.Length < yCount)
                {
                    // Handle cases where a line has fewer columns than expected
                    // Possibly show an error message, log it, or throw a custom exception.
                    throw new InvalidDataException($"Line {x + 1} has fewer columns ({ringElements.Length}) than expected ({yCount}).");
                }

                for (int y = 0; y < yCount; y++)
                {
                    ungroupedRings[x, y] = stringToChar(ringElements[y]);
                }
            }

            for (int y = 0; y < yCount; y++)
            {
                for (int x = 0; x < xCount; x++)
                {
                    groupedRings[y, x] = ungroupedRings[x, y];
                }
            }
        }


        //public static void ringContentSeparator() //Sort the contents of the csv file.
        //{
        //    int xCount = ringLines.Count;
        //    int yCount = ringLines[0].Split(',').Length;
        //    groupedRings = new char[yCount, xCount];
        //    char[,] ungroupedRings = new char[xCount, yCount];

        //    for (int x = 0; x < xCount; x++)
        //    {
        //        string[] ringElements = ringLines[x].Split(',');
        //        for (int y = 0; y < yCount; y++)
        //        {
        //            ungroupedRings[x, y] = stringToChar(ringElements[y]);
        //        }
        //    }

        //    for (int y = 0; y < yCount; y++)
        //    {
        //        for (int x = 0; x < xCount; x++)
        //        {
        //            groupedRings[y, x] = ungroupedRings[x, y];
        //        }
        //    }
        //}

        private static char stringToChar(string a) //Convert string to char
        {
            return Convert.ToChar(Convert.ToInt32(a));
        }

        public static int ringCount() //get the ring count
        {
            return groupedRings.GetLength(0) - 1;
        }

        public static int ringContentCount() //get the number of characters in one ring
        {
            return groupedRings.GetLength(1);
        }

        public static string countDisplayFormatter(int count) //format the number display
        {
            return String.Format("{0:00}", count);
        }

        public static void ringSelectionCounter(int index, char buttonFunction) //counter for ring selection
        {
            switch (buttonFunction)
            {
                case '+':
                    ringSelection[index]++;
                    if (ringSelection[index] > ringCount() - 1)
                        ringSelection[index] = 0;
                    break;
                case '-':
                    ringSelection[index]--;
                    if (ringSelection[index] < 0)
                        ringSelection[index] = ringCount() - 1;
                    break;
            }
        }

        public static void ringSettingsCounter(int index, char buttonFunction) //counter for ring settings
        {
            switch (buttonFunction)
            {
                case '+':
                    ringSettings[index]++;
                    if (ringSettings[index] > ringContentCount() - 1)
                        ringSettings[index] = 0;
                    break;
                case '-':
                    ringSettings[index]--;
                    if (ringSettings[index] < 0)
                        ringSettings[index] = ringContentCount() - 1;
                    break;
            }
        }

        public static void offsetRotors() //Offset the rotors after choosing the desired settings in rotors
        {
            int rotorIndex = 0;
            int length = groupedRings.GetLength(1);
            rotatingRings = new char[3, length];
            for (int x = 0; x < length; x++)
            {
                rotorIndex = x + ringSettings[0];
                if (rotorIndex >= length)
                    rotorIndex %= length;

                rotatingRings[0, x] = groupedRings[ringSelection[0] + 1, rotorIndex];
            }

            for (int x = 0; x < length; x++)
            {
                rotorIndex = x + ringSettings[1];
                if (rotorIndex >= length)
                    rotorIndex %= length;

                rotatingRings[1, x] = groupedRings[ringSelection[1] + 1, rotorIndex];
            }

            for (int x = 0; x < length; x++)
            {
                rotorIndex = x + ringSettings[2];
                if (rotorIndex >= length)
                    rotorIndex %= length;

                rotatingRings[2, x] = groupedRings[ringSelection[2] + 1, rotorIndex];
            }
        }

        public static void rotatingRotors() //responsible to rotate the rotors
        {
            int max = groupedRings.GetLength(1) - 1;
            ringSettings[0]++;
            if (ringSettings[0] > max)
            {
                ringSettings[0] = 0;
                ringSettings[1]++;
                if (ringSettings[1] > max)
                {
                    ringSettings[1] = 0;
                    ringSettings[2]++;
                    if (ringSettings[2] > max)
                    {
                        ringSettings[2] = 0;
                    }
                }
            }
            offsetRotors();
        }

        public static void reverseRotor() //responsible to rotate rotors in reverse
        {
            if (textboxInput.Count == 0)
            {

            }
            else
            {
                int max = groupedRings.GetLength(1) - 1;
                ringSettings[0]--;
                if (ringSettings[0] < 0)
                {
                    ringSettings[0] = max;
                    ringSettings[1]--;
                    if (ringSettings[1] < 0)
                    {
                        ringSettings[1] = max;
                        ringSettings[2]--;
                        if (ringSettings[2] < 0)
                        {
                            ringSettings[2] = max;
                        }
                    }
                }
                offsetRotors();
            }
        }

        public static void inputTextbox(char input) //saved the input charecters in a list
        {
            textboxInput.Add(input);
        }

        public static void outputTextbox(char input) //saved the output/encrypted charecters in a list
        {
            textboxOutput.Add(input);
        }

        public static string encrypted(char input) //returns the encrypted character
        {
            char output = '\0';
            string encrypted = "";
            inputTextbox(input);


            int x = 0;
            if (input == '^')
            {
                output = '^';
            }
            else

                for (x = 0; x < groupedRings.GetLength(1); x++)
                {
                    if (input == groupedRings[0, x])
                    {
                        output = rotatingRings[0, x];
                        break;
                    }
                }

            int y = 0;
            for (y = 0; y < groupedRings.GetLength(1); y++)
            {
                if (rotatingRings[0, x] == groupedRings[0, y])
                {
                    output = rotatingRings[1, y];
                    break;
                }
            }

            int z = 0;
            for (z = 0; z < groupedRings.GetLength(1); z++)
            {
                if (rotatingRings[1, y] == groupedRings[0, z])
                {
                    output = rotatingRings[2, z];
                    break;
                }
            }

            if (checkboxIsChecked) //reflector
            {
                int z1 = 0;
                for (z1 = 0; z1 < groupedRings.GetLength(1); z1++)
                {
                    if (rotatingRings[2, z] == groupedRings[0, z1])
                    {
                        output = rotatingRings[2, z1];
                        break;
                    }
                }

                int y1 = 0;
                for (y1 = 0; y1 < groupedRings.GetLength(1); y1++)
                {
                    if (rotatingRings[2, z1] == groupedRings[0, y1])
                    {
                        output = rotatingRings[1, y1];
                        break;
                    }
                }

                int x1 = 0;
                for (x1 = 0; x1 < groupedRings.GetLength(1); x1++)
                {
                    if (rotatingRings[1, y1] == groupedRings[0, x1])
                    {
                        output = rotatingRings[0, x1];
                        break;
                    }
                }
                rotatingRotors();
                outputTextbox(output);

                for (int a = 0; a < textboxOutput.Count; a++)
                {
                    encrypted = textboxOutput[a].ToString();
                }
                return encrypted;
            }
            else
            {
                rotatingRotors();
                outputTextbox(output);

                for (int a = 0; a < textboxOutput.Count; a++)
                {
                    encrypted = textboxOutput[a].ToString();
                }
                return encrypted;
            }
        }

        public static string backspaceInput() //removes the last element in the list
        {
            string output = "";
            if (textboxInput.Any())
            {
                textboxInput.RemoveAt(textboxInput.Count - 1);

                for (int x = 0; x < textboxInput.Count; x++)
                {
                    output += textboxInput[x];
                }
            }
            return output;
        }

        public static string backspaceOutput() //removes the last element in the list
        {
            string output = "";
            if (textboxOutput.Any())
            {
                textboxOutput.RemoveAt(textboxOutput.Count - 1);

                for (int x = 0; x < textboxOutput.Count; x++)
                {
                    output += textboxOutput[x];
                }
            }
            return output;
        }
    }
}