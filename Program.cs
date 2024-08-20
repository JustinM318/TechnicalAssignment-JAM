using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChipSecuritySystem
{
    internal class Program
    {
        // Random number generator for random chip generation
        private static readonly Random Rand = new Random();

        // List of all possible colors
        private static readonly List<ColorChip> Chips = new List<ColorChip>();

        private static void Main()
        {
            // menu options
            Console.WriteLine("ApexDefense A4-91 / Master Panel Controls\n");
            Console.WriteLine("1. Try your luck with randomly generated chips.");
            Console.WriteLine("2. Manually enter chips.");
            Console.WriteLine("3. Use preset chip pattern. EX: [Blue, Yellow] [Yellow, Red] [Red, Green].");
            Console.WriteLine("4. Exit control panel");

            // Validate input is within menu range
            int option;
            while (!int.TryParse(Console.ReadLine(), out option) || (option != 1 && option != 2 && option != 3 && option != 4))
            {
                Console.WriteLine("Invalid input. Please enter 1, 2 or 3.");
            }

            Console.WriteLine(option);
            // Run case based on menu option
            switch (option)
            {
                case 1:
                    createRandomChips();
                    break;
                case 2:
                    manualInput();
                    break;
                case 3:
                    createPresetChips();
                    break;
                case 4:
                    while (true)
                    {
                        Console.WriteLine("Exiting master panel controls, press enter to exit...");
                        Console.ReadLine();
                        return;
                    }
            }

            Console.WriteLine("\n[---Selected Chips---]\n");

            // Show selected chips
            foreach (var chip in Chips)
            {
                Console.WriteLine("[" + chip + "]");
            }

            // create a hashset to store the chips for comparison
            var chipSet = new HashSet<ColorChip>();

            // Check if provided chips can unlock master panel
            foreach (var chip in Chips.Where(chip => chip.StartColor == Color.Blue))
            {
                // Add chip to set and check if a chain is found
                chipSet.Add(chip);
                if (FindChain(chip, chipSet))
                {
                    Console.WriteLine("\n### Master panel unlocked! ###\n");
                    return;
                }

                // Remove chip from chipSet if no chain is found
                chipSet.Remove(chip);
            }

            // Show error message if no chain is found
            Console.WriteLine("\n" + Constants.ErrorMessage + "!\n");
            return;

        }


        // Create random chips using Random
        private static void createRandomChips()
        {
            for (var i = 0; i < 3; i++)
            {
                var startColor = (Color)Rand.Next(0, 6);
                var endColor = (Color)Rand.Next(0, 6);
                Chips.Add(new ColorChip(startColor, endColor));
            }
        }

        // Create preset chips
        private static void createPresetChips()
        {
            Chips.Add(new ColorChip(Color.Blue, Color.Yellow));
            Chips.Add(new ColorChip(Color.Yellow, Color.Red));
            Chips.Add(new ColorChip(Color.Red, Color.Green));
        }

        // List all possible colors and prompt user to enter colors for chips
        private static void manualInput()
        {
            Console.WriteLine('\n' + "Enter colors for four chips. Choices:");
            var allColors = Enum.GetValues(typeof(Color));
            for (var i = 0; i < allColors.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {allColors.GetValue(i)}");
            }
            for (var i = 0; i < 3; i++)
            {
                Console.WriteLine($"\nChip #{i + 1}");
                Color startColor = GetColor(nameof(startColor));
                Color endColor = GetColor(nameof(endColor));
                Chips.Add(new ColorChip(startColor, endColor));
            }
        }

        // Ensure user enters valid color based on the enum, return color
        private static Color GetColor(string colorType)
        {
            Console.WriteLine($"Enter color:");
            int option;
            while (!int.TryParse(Console.ReadLine(), out option) || option < 1 || option > 6)
            {
                Console.WriteLine("Invalid color. Please enter a number between 1 and 6.");
            }
            return (Color)(option - 1);
        }

        // Recursively find chain of chips
        private static bool FindChain(ColorChip lastChip, ISet<ColorChip> chipSet)
        {
            if (lastChip.EndColor == Color.Green)
            {
                return true;
            }
            // Check if chain is possible
            foreach (var nextChip in Chips.Where(c => c.StartColor == lastChip.EndColor))
            {
                // Check if chip is already in set
                if (!chipSet.Add(nextChip)) continue;

                if (FindChain(nextChip, chipSet))
                {
                    return true;
                }

                // Remove chip from set if no chain is found
                chipSet.Remove(nextChip);
            }
            return false;
        }

    }
}
