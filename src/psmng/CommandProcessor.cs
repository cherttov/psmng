using System;

namespace psmng.src.psmng
{
    public static class CommandProcessor
    {
        public static void Process(string[] args)
        {
            if (args.Length == 0 || args[0] == "-h" || args[0] == "-?" || args[0] == "-help" || args[0] == "/h" || args[0] == "/?" || args[0] == "/help")
            {
                ShowHelp();
                return;
            }
            else if (args[0] == "--version")
            {
                Console.WriteLine("psmng 0.0.9");
                return;
            }

            string command = args[0].ToLower();

            switch (command)
            {
                // Login entry commands
                case "new":
                    if (args.Length != 3)
                    {
                        ShowUsageError("psmng new <login> <password>");
                        return;
                    }
                    PasswordManager.NewPassword(args[1], args[2]);
                    break;

                case "get":
                    if (args.Length != 2)
                    {
                        ShowUsageError("psmng get <login>");
                        return;
                    }
                    PasswordManager.GetPassword(args[1]);
                    break;

                case "del":
                    if (args.Length != 2)
                    {
                        ShowUsageError("psmng del <login>");
                        return;
                    }
                    PasswordManager.DelPassword(args[1]);
                    break;

                case "list":
                    if (args.Length != 1)
                    {
                        ShowUsageError("psmng list");
                        return;
                    }
                    PasswordManager.ListPassword();
                    break;

                // Group entry commands
                case "newgroup": // <--     !!!
                    if (args.Length != 2)
                    {
                        ShowUsageError("psmng newgroup <group_name>");
                        return;
                    }
                    GroupManager.NewGroup(args[1]);
                    break;

                case "getgroup": // <--     !!!
                    if (args.Length != 2)
                    {
                        ShowUsageError("psmng getgroup <group_name>");
                        return;
                    }
                    GroupManager.GetGroup(args[1]);
                    break;

                case "delgroup": // <--     !!!
                    if (args.Length != 2)
                    {
                        ShowUsageError("psmng delgroup <group_name>");
                        return;
                    }
                    GroupManager.DelGroup(args[1]);
                    break;

                case "listgroup": // <--     !!!
                    if (args.Length != 1)
                    {
                        ShowUsageError("psmng listgroup");
                        return;
                    }
                    GroupManager.ListGroup();
                    break;

                case "addtogroup": // <--     !!!
                    if (args.Length != 3)
                    {
                        ShowUsageError("psmng addtogroup <group_name> <login>");
                        return;
                    }
                    GroupManager.AddToGroup(args[1], args[2]);
                    break;

                // Path commands
                case "getpath":
                    if (args.Length != 1)
                    {
                        ShowUsageError("psmng data-path");
                        return;
                    }
                    PasswordManager.PathPassword();
                    break;
                
                // Default case
                default:
                    Console.WriteLine("ERROR: Unknown command.\n");
                    ShowHelp();
                    break;
            }
        }
        static void ShowHelp()
        {
            Console.WriteLine("USAGE:\n  psmng <command> [options]");
            Console.WriteLine("\nCOMMANDS:");
            Console.WriteLine("\n  LOGIN:");
            Console.WriteLine("    new <login> <password>              Create new login entry with password.");
            Console.WriteLine("    get <login>                         Get selected login's password.");
            Console.WriteLine("    del <login>                         Delete selected login.");
            Console.WriteLine("    list                                List all passwords.");
            Console.WriteLine("\n  GROUPS:");
            Console.WriteLine("    newgroup <group_name>               Create new group.");
            Console.WriteLine("    getgroup <group_name>               Show login entries in selected group.");
            Console.WriteLine("    delgroup <group_name>               Delete existing group.");
            Console.WriteLine("    listgroup                           List all groups.");
            Console.WriteLine("    addtogroup <group_name> <login>     Add login to group.");
            Console.WriteLine("\n  PATH:");
            Console.WriteLine("    getpath                             Get program path.");
            Console.WriteLine("\nOPTIONS:");
            Console.WriteLine("  -h                                    Show help.");
            Console.WriteLine("  --version                             Show program's version.");
        }

        static void ShowUsageError(string usage)
        {
            Console.WriteLine($"Invalid arguments.");
            Console.WriteLine($"USAGE:\n    {usage}");
        }
    }
}
