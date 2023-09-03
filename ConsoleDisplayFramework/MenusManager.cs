using static System.Console;
using Microsoft.ML;

using Patrikakis.Types;
using Patrikakis.Models;
using static Patrikakis.ConsoleDisplay.DisplayFramework;

namespace Patrikakis.ConsoleDisplay;

using PredictionEngine = PredictionEngine<PaperData, PaperPrediction>;
public static class Menus {
    static string SelectedModel { get; set; } = "TestModel_1_01";
    static string ModelPath { get; set; } = "./StoredModels/";
    public static void WelcomeMenu() {
        PrintRow();
        CenterText("UNIWA");
        CenterText("Master of Research");
        CenterText(); // New Line
        CenterText("\"Reviewer Finder\"");
        CenterText("A Machine Learning Project");

        CenterText(); // New Line
        CenterText(); // New Line
        SpaceAround("Stamatakis D.", "Prof. Patrikakis");

        PrintRow();
    }
    public static void OptionsMenu() {
        WriteLine(); // New Line

        PrintRow();
        LeftText("Select one of the following options,", 2);
        CenterText(); // New Line
        LeftText("1. Find Suitable Reviewer.", 5);
        LeftText("2. Display Current Datasheet Details.", 5);
        LeftText("3. Train a new Model.", 5);
        LeftText("4. Project's Description.", 5);
        LeftText("5. Terminate the Application.", 5);
        PrintRow();
    }
    public static void DisplayDataMenu() {
        Clear();

        PrintRow();
        LeftText("NOTE: If you are working with your own data,", 2);
        LeftText("first [Train a Model]. By doing this, a proccess", 2);
        LeftText("of writing external files is performed", 2);
        LeftText("which is necessary for this method to work.", 2);
        CenterText(); // New Line

        LeftText("Provide the [Path] of your data. Ex. C:\\Journals\\Authors", 2);

        string pathToData = Helpers.GetSafeUserInputABS_Dir(true);
        string[] authorsDirs = Directory.GetDirectories(pathToData);
        int checker = 0;

        foreach (string authorDir in authorsDirs) {
            string filePath = Path.Combine(authorDir, "RawKeywords", "AllKeywords.txt");
            if (File.Exists(filePath))
            {
                checker++;
            }
        }

        if (checker == authorsDirs.Length) {
            PrintRow();
            CenterText(); // New Line
            LeftText("You are probably qualified to run this method", 2);
        } else {
            PrintRow();
            CenterText(); // New Line
            LeftText("[Error]: You are NOT qualified to run this method", 2);
            CenterText(); // New Line
            PrintRow();
            LeftText("Press any key to go Main Menu...", 2);
            Read();
            OptionsMenu();
        }

        List<Author> authors = AuthorHandler.GetAuthorsFrom(pathToData);
        string header = $" ==> {"Author's Name",-25} | {"Scientific Field", -25} | {"Documents", -10} | {"Total Keywords", -14} | {"Score"}";
        
        PrintRow();
        CenterText(); // New Line
        LeftText(header);
        LeftText(Repeat('~', header.Length + 5));

        foreach ( Author author in authors ) 
        {
            string pathToAllKeywordsTxt = Path.Combine(pathToData, author.Name, "RawKeywords", "AllKeywords.txt");
            string keywords = KeywordExtractor.ReadTxtFile(pathToAllKeywordsTxt);
            
            string[] words = keywords.Split(", ");
            int keywordsCount = words.Length;
            int authorPapersNum = author.Papers.Count;

            double score = Helpers.CalculateKeywordRatio(authorPapersNum, keywordsCount);

            LeftText($" ==> {author.Name,-25} | {author.GetField(), -25} | {authorPapersNum, -10} | {keywordsCount, -14} | {score:F5}");
        }

        PrintRow();
        LeftText("Press any key to go Main Menu...", 2);
        Read();
        PrintRow();
    }

    // 🧪 Needs TESTING 🧪
    public static void ReviewerFinderMenu() {
        Clear();

        PrintRow();
        LeftText("You have selected the: [Find Suitable Reviewer] ", 2);
        CenterText(); // New Line
        // "Please provide the absolute of your PDF Document"
        LeftText("NOTE_1: The Path must contain only ASCII characters. ", 4);
        LeftText("NOTE_2: Running as 'Administrator' might fix some issues ", 4);
        CenterText(); // New Line
        PrintRow();

        LeftText("Provide the Absolute Path of you PDF file: ", 2);
        CenterText(); // New Line

        string pdfPath = PDFHandler.GetPDFPathFromUser();
        string fileName = Path.GetFileNameWithoutExtension(pdfPath);

        // 1. Get Document's Name from the path
        LeftTextAction($" Selected File: [{fileName}]", 2);
        CenterText(); // New Line

        // 2. Use PDFHandler to Extract the text content
        LeftTextAction(" 1) Extracting Text from PDF...", 2);
        CenterText(); // New Line
        string textContent = PDFHandler.GetTextFromPDF(pdfPath);

        // 3. Use RAKE to Extract the keywords, through KeywordExtractor.cs
        LeftTextAction(" 2) Extracting Keywords using RAKE...", 2);
        CenterText(); // New Line
        Keywords rake_keywords = KeywordExtractor.ExtractSingle(textContent);
        LeftTextAction($" Extracted : ({KeywordExtractor.TotalKeywordsCount}) keywords", 2);

        // 3.1 Stop Python Runtime
        // RAKE.LetTheShakeRest();

        // 4. Convert Keywords into a
        LeftTextAction(" 3) Converting Keywords Object into a Single string...", 2);
        CenterText(); // New LineA
        string singleStringKeywords = rake_keywords.GetStringVersion();

        // 5. Convert Keywords into a
        LeftTextAction(" 4) Transforming Data to AutoML friendly format...", 2);
        CenterText(); // New LineA
        PaperData formatedData = new PaperData() { Author = "CurrentUser", Keywords = singleStringKeywords };

        LeftTextAction("Select 1 of 3 options:", 2);
        LeftTextAction("1) Provide the [Path] that contains your models", 5);
        LeftTextAction("2) Enter the word [def] to use the Default one", 5);
        LeftTextAction("3) Enter the word [local] to see the locally stored models", 5);
        CenterText(); // New LineA

        string modelSelection;
        do
        {  
            modelSelection = Helpers.GetSafeUserInput();
            if (modelSelection == "def") {
                LeftTextAction("You have selected the: (Default Model)", 2);
                SelectedModel = "TestModel_1_01";
                ModelPath = Path.Combine(Directory.GetCurrentDirectory(), "StoredModels");
            } else if (modelSelection == "local") {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "StoredModels");
                List<string> models = Helpers.GetZipFilesInDirectory(path);
                int counter = 1;
                foreach (string model in models) {
                    LeftTextAction($" > {counter}) '{model}'", 5);
                    counter++;
                }
                CenterText(); // New LineA
                LeftTextAction("Select a Model by Entering its Number", 2);
                int choice = Helpers.GetSafeIntegerWithinRange(1, models.Count);

                SelectedModel = Path.GetFileNameWithoutExtension(models[choice - 1]);

                CenterText(); // New LineA
                LeftTextAction($"You have selected the: ({SelectedModel})", 2);
                ModelPath = path;
            } else if (Path.IsPathRooted(modelSelection)) {
                CenterText(); // New LineA
                List<string> models = Helpers.GetZipFilesInDirectory(modelSelection);
                int counter = 1;
                foreach (string model in models) {
                    LeftTextAction($"{counter}) '{model}'", 5);
                    counter++;
                }
                CenterText(); // New LineA
                LeftTextAction("Select a Model by Entering its Number", 2);
                int choice = Helpers.GetSafeIntegerWithinRange(1, models.Count);

                SelectedModel = Path.GetFileNameWithoutExtension(models[choice - 1]);

                CenterText(); // New LineA
                LeftTextAction($"You have selected the: ({SelectedModel})", 2);
                ModelPath = modelSelection;
            } else {
                LeftTextAction($">> Invalid input, please try again.", 2);
            }
        } while (!(modelSelection == "def" || modelSelection == "local" || Path.IsPathRooted(modelSelection)));

        // TODO: Below
        // 6. Load the Trained Model
        LeftTextAction(" 5) Loading the Trained Model...", 2);
        CenterText(); // New Line
        TrainedModel.LoadModel(ModelPath, SelectedModel);
        PaperPrediction result = TrainedModel.Predict(formatedData);


        // TODO: Below
        // 7. Display the Prediction Result
        LeftTextAction($" 6) The Most Suitable Reviewer is: [{result.Author}]", 2);
        CenterText(); // New Line

        LeftTextAction($" 7) The Scientific Field should be: [{Author.GetField(result.Author)}]", 2);
        CenterText(); // New Line

        CenterText(); // New Line
        LeftText("Press 'q' for (Main Menu) or 'Enter' to (repeat the process)", 2);
        PrintRow();        

        bool input = Helpers.WaitForUserKeyPress();

        if (input) {
            Clear();
            OptionsMenu();
        } 
        else {
            ReviewerFinderMenu();
        }
    }
    
    // 🧪 Needs TESTING 🧪
    public static void TrainNewModelMenu() {
        Clear();

        PrintRow();
        LeftText("You have selected the: [Train a new Model] ", 2);
        CenterText(); // New Line
        // "Please provide the absolute of your PDF Document"
        LeftText("Provide the [Absolute Path] of your Authors Directory.", 2);
        LeftText("The Directory should much the structure below: ", 2);
        CenterText(); // New Line
        ExampleDir();
        CenterText(); // New Line

        // Gets the absolute path from User
        string inputPathToData = Helpers.GetSafeUserInputABS_Dir(true);

        // Goes to that Path and: 
        //  1.  Creates Author Objects,
        //  2.  Extracts all the Keywords,
        //  3.  Stores them in files: .txt, .tsv and .json
        List<Author> collectedAuthors = CollectData.From(inputPathToData);

        List<PaperData> formattedData = PrepareDataForML.Refine(collectedAuthors);

        LeftText("AutoML requires some time to figure out the suitable algorithm and train the model.", 2);

        LeftText("Please provide the time in (seconds). Recommendations:", 2);
        LeftText("> Teting: 20~30 seconds", 5);
        LeftText("> Small Datasheet (~100-150 docs): 100~120 seconds", 5);
        LeftText("> Big Datasheet (200+ docs): 240+ seconds", 5);

        uint trainTime = Helpers.GetSafeUserInputAsUInt();
        CenterText(); // New Line

        LeftText("Enter a name for the Model:", 2);
        string modelName = Helpers.GetSafeUserInput();

        LeftText("Commencing Training...", 2);
        UntrainedModel.TrainModel(formattedData, trainTime, modelName);
        
        CenterText(); // New Line
        LeftText("Training Completed!", 2);

        string pathToModel = Directory.GetCurrentDirectory();
        // string pathToModel = Helpers.GetRunningDirectory();
        CenterText(); // New Line
        LeftText($@"The model can be found here: [{pathToModel}\{modelName}.zip]", 2);

        CenterText(); // New Line
        LeftText("Press 'q' for (Main Menu) or 'Enter' to (repeat the process)", 2);
        PrintRow();   
        
        bool input = Helpers.WaitForUserKeyPress();

        if (input) {
            Clear();
            OptionsMenu();
        } 
        else {
            TrainNewModelMenu();
        }     

    }
    public static void DescriptionMenu() {
        Clear();

        PrintRow();
        LeftText($"{"Creator:", -15} D. Stamatakis", 2);
        LeftText($"{"Supervisor:", -15} Prof. Patrikakis", 2);
        LeftText($"{"Subject:", -15} Machine Learning", 2);
        CenterText(); // New Line

        LeftText("Tools Used:", 2);
        LeftText(Repeat('~', "Tools Used:".Length), 2);
        LeftText($"{"ML.NET", -15}", 5);
        LeftTextAction($"{"An Free Open-souce Machine Learning Framework", -15}", 5);
        CenterText(); // New Line

        LeftText($"{"Python.NET", -15}", 5);
        LeftTextAction($"{"An Free Open-souce Python Runtime for C#", -15}", 5);
        CenterText(); // New Line

        LeftText($"{"PdfPig", -15}", 5);
        LeftTextAction($"{"An Free Open-souce PDF Manipulation Library", -15}", 5);
        CenterText(); // New Line
        
        LeftText($"{"RAKE Algorithm (rake_nltk)", -15}", 5);
        LeftTextAction($"{"An Free Open-souce Python Module that implements RAKE", -15}", 5);
        CenterText(); // New Line

        LeftText($"{"TF-IDF Algorithm (ML.NET)", -15}", 5);
        LeftTextAction($"{"A Feature of Text_Featurizing_Estimator Class from ML.NET", -15}", 5);
        CenterText(); // New Line

        LeftText($"{"AutoML (ML.NET)", -15}", 5);
        LeftTextAction($"{"Provides methods and processes for automatically selecting the best algorithm given the task.", -15}", 5);
        CenterText(); // New Line

        LeftText(Repeat('~', "Provides methods and processes for automatically selecting the best algorithm given the task.".Length + 8), 2);
        CenterText(); // New Line

        LeftText("Getting Started:", 2);
        LeftText(Repeat('~', "Getting Started:".Length), 2);

        LeftText("Sadly, there are a few things that have to be done manually:", 2);
        CenterText(); // New Line

        LeftTextAction("1. Search for ('Cntl+F') [Runtime.PythonDLL =] which is located at the Rake.cs file", 2);
        CenterText(); // New Line

        LeftTextAction("2. Once found, insert the ABS Path of your python3.XX.dll", 2);
        CenterText(); // New Line

        LeftTextAction("3. You will have to download and install .NET 7.0 or 6.0, just Google it", 2);
        CenterText(); // New Line

        LeftTextAction("4. Add more...", 2);
        CenterText(); // New Line


        LeftText(Repeat('~', "Provides methods and processes for automatically selecting the best algorithm given the task.".Length + 8), 2);
        CenterText(); // New Line

        LeftText("For more Details visit the GitHub Repository: ", 2);
        CenterText(); // New Line

        LeftText("Thank you for reading! Happy Coding!", 2);
        CenterText(); // New Line
        
        LeftText("Press any key to go Main Menu...", 2);
        Read();
        PrintRow();


    }
    public static void ExampleDir() {
        LeftText("(rootDir) [This is the requested Path]");
        
        Indent(1, "Author_1 (Folder)");
        Indent(2, "Document_1.pdf");
        Indent(2, "Document_2.pdf");
        Indent(2, "Document_N.pdf");
        
        Indent(1, "Author_N (Folder)");
        Indent(2, "Document_1.pdf");
        Indent(2, "Document_2.pdf");
        Indent(2, "Document_N.pdf");
    }          
}