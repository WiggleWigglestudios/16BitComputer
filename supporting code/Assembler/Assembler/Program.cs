// See https://aka.ms/new-console-template for more information


using System.Collections;

/* Todo:
 * comments
 * variables/labels
 * 
 */



string loadFilePath = "C:\\Users\\breck\\OneDrive\\Documents\\_circuits\\16 bit computer\\github\\16BitComputer\\supporting code\\Assembler\\programs\\";
string saveFilePath = "C:\\Users\\breck\\OneDrive\\Documents\\_circuits\\16 bit computer\\github\\16BitComputer\\supporting code\\Assembler\\Assembled files\\";



Console.WriteLine("Load file name with extension (ex: test.a)");

string loadFileName = Console.ReadLine();
loadFilePath += loadFileName;
if (!File.Exists(loadFilePath))
{
    Console.WriteLine(loadFileName + " File does not exist");
}
else
{
    Console.WriteLine("File found");
    Console.WriteLine("Save file name WITHOUT extension (ex: test)");

    string outputFileName = Console.ReadLine();
    bool approval = false;
    while ((File.Exists(saveFilePath + outputFileName + "H.bin") | File.Exists(saveFilePath + outputFileName + "L.bin")) && !approval)
    {
        Console.WriteLine(outputFileName + " already exists. Are you sure you want to override it? (y/n)");

        if (Console.ReadLine() == "y")
        {
            approval = true;
        }
        else
        {
            Console.WriteLine("Save file name WITHOUT extension (ex: test)");
            outputFileName = Console.ReadLine();
        }
    }

    saveFilePath += outputFileName;

    string fileText = File.ReadAllText(loadFilePath);
    char[] whitespaceChars = { ' ', '\t', '\n', '\v', '\f', '\r', '\0' };
    string currentToken = "";
    List<string> tokens = new List<string>();
    for (int i = 0; i < fileText.Length; i++)
    {
        bool whiteSpace = false;
        for (int c = 0; c < whitespaceChars.Length; c++)
        {
            if (whitespaceChars[c] == fileText[i])
            {
                c = whitespaceChars.Length;
                whiteSpace = true;
            }
        }
        if (whiteSpace)
        {
            if (currentToken.Length > 0)
            {
                tokens.Add(currentToken);
            }
            currentToken = "";
        }
        else if (fileText[i] == '=')
        {
            if (currentToken.Length > 0)
            {
                tokens.Add(currentToken);
            }
            tokens.Add("=");
            currentToken = "";

        }
        else
        {
            currentToken += fileText[i];
        }
    }



    List<Variable> variables = new List<Variable>();
    for (int i = 0; i < tokens.Count; i++)
    {
        Console.WriteLine(tokens[i]);
        //checks if it is a label
        if (tokens[i][tokens[i].Length - 1] == ':')
        {
            variables.Add(new Variable(tokens[i], -1));
            Console.WriteLine("Label");
        }

        //checks for variables either it contains = or next thing contains equal then it is a variable 
        if (i < tokens.Count - 2)
        {
            if (tokens[i].Contains('=') || tokens[i + 1].Contains('='))
            {
                if (tokens[i][0] != '=')
                {
                    variables.Add(new Variable(tokens[i], convertToShort(tokens[1 + 2])));
                    Console.WriteLine("variable");
                }
            }
        }


        //checks if it is a number by checking if leading char # then if it is a number or if it is $ or %
        if (tokens[i][0] == '#')
        {
            Console.WriteLine("Number");
        }

        //checks if it is an address either a label or just a number



        //checks if it is a register
        //still have to check for mar pc sp etc.
        if (tokens[i][0] == 'r' && (tokens[i][1] == '0' || tokens[i][1] == '1' || tokens[i][1] == '2' || tokens[i][1] == '3' || tokens[i][1] == '4' || tokens[i][1] == '5' || tokens[i][1] == '6' || tokens[i][1] == '7' || tokens[i][1] == '8' || tokens[i][1] == '9'))
        {
            Console.WriteLine("Register");
        }


        //checks if it is a register address
        if (tokens[i][0] == '[')
        {
            Console.WriteLine("Register Address");
        }

        


        //assembles instruction


    }
}






static short convertToShort(string input)
{
    return 0;
}


struct Variable
{
    public Variable(string _name, short _value)
    {
        name = _name;
        value = _value;
    }
    string name;
    short value;
}

