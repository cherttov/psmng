using System;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0 || args[0] == "-h" || args[0] == "-?" || args[0] == "/h" || args[0] == "/?")
        {
            ShowHelp();
            return;
        }

        string command = args[0].ToLower();

        switch (command)
        {
            case "add":
                if (args.Length != 3)
                {
                    Console.WriteLine("Invalid arguments.\nUSAGE:\n    psmng add <login> <password>");
                    return;
                }
                PasswordManager.AddPassword(args[1], args[2]);
                break;

            case "del":
                if (args.Length != 2)
                {
                    Console.WriteLine("Invalid arguments.\nUSAGE:\n    psmng del <login>");
                    return;
                }
                PasswordManager.DelPassword(args[1]);
                break;

            case "get":
                if (args.Length != 2)
                {
                    Console.WriteLine("Invalid arguments.\nUSAGE:\n    psmng get <login>");
                    return;
                }
                PasswordManager.GetPassword(args[1]);
                break;

            case "list":
                if (args.Length != 1)
                {
                    Console.WriteLine("Invalid arguments.\nUSAGE:\n    psmng list");
                    return;
                }
                PasswordManager.ListPassword();
                break;

            case "path":
                if (args.Length != 1)
                {
                    Console.WriteLine("Invalid arguments.\nUSAGE:\n    psmng path");
                    return;
                }
                PasswordManager.PathPassword();
                break;
            case "gui":
                if (args.Length != 1)
                {
                    Console.WriteLine("Invalid arguments.\nUSAGE:\n    psmng gui");
                    return;
                }
                Console.WriteLine("Work in progress.");
                break;

            default:
                Console.WriteLine("Unknown command.");
                ShowHelp();
                break;
        }
    }

    static void ShowHelp()
    {
        Console.WriteLine("USAGE:\n    psmng <command> [options]");
        Console.WriteLine("\nCOMMANDS:");
        Console.WriteLine("    add <login> <password>     Add new login with password.");
        Console.WriteLine("    del <login>                Delete selected login.");
        Console.WriteLine("    get <login>                Get selected login's password.");
        Console.WriteLine("    list                       List all password.");
        Console.WriteLine("    path                       Shows program path.");
        Console.WriteLine("    gui                        Open GUI (graphical user interface).");
        Console.WriteLine("\nOPTIONS:");
        Console.WriteLine("    -h                         Show help.");
    }
}