using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.AutoML;

using Patrikakis;
using Patrikakis.Types;
using Patrikakis.Tests;
using Patrikakis.Models;
using static Patrikakis.ConsoleDisplay.Menus;

bool shouldTerminate = false;
WelcomeMenu();
do
{
    OptionsMenu();
    int userChoice = Helpers.GetSafeIntegerWithinRange(1, 5);
    switch (userChoice)
    {
        case 1:
            ReviewerFinderMenu(); // C:\Users\Jimzord12\Desktop\MscRes\Thesys_Final_v3.0.pdf
            break;
        case 2:
            DisplayDataMenu(); // C:\FastPaths\AuthorsAndPapers
            break;
        case 3:
            TrainNewModelMenu();
            break;
        case 4:
            DescriptionMenu();
            break;
        case 5:
            shouldTerminate = true;
            break;

        default:
            throw new ArgumentOutOfRangeException();
    }

} while (!shouldTerminate);
