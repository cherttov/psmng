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
                Console.WriteLine("psmng 0.1.5");
                return;
            }

            string command = args[0].ToLower();

            switch (command)
            {
                // Login entry commands
                case "new":
                    if (args.Length != 3)
                    {
                        ShowUsageError("new <login> <password>");
                        return;
                    }
                    PasswordManager.NewPassword(args[1], args[2]);
                    break;

                case "get":
                    if (args.Length != 2)
                    {
                        ShowUsageError("get <login>");
                        return;
                    }
                    PasswordManager.GetPassword(args[1]);
                    break;

                case "del":
                    if (args.Length != 2)
                    {
                        ShowUsageError("del <login>");
                        return;
                    }
                    PasswordManager.DelPassword(args[1]);
                    break;

                case "list":
                    if (args.Length != 1)
                    {
                        ShowUsageError("list");
                        return;
                    }
                    PasswordManager.ListPassword();
                    break;

                // Group entry commands
                case "new-group":
                    if (args.Length != 2)
                    {
                        ShowUsageError("new-group <group_name>");
                        return;
                    }
                    GroupManager.NewGroup(args[1]);
                    break;

                case "get-group":
                    if (args.Length != 2)
                    {
                        ShowUsageError("get-group <group_name>");
                        return;
                    }
                    GroupManager.GetGroup(args[1]);
                    break;

                case "del-group":
                    if (args.Length != 2)
                    {
                        ShowUsageError("del-group <group_name>");
                        return;
                    }
                    GroupManager.DelGroup(args[1]);
                    break;

                case "list-group":
                    if (args.Length != 1)
                    {
                        ShowUsageError("list-group");
                        return;
                    }
                    GroupManager.ListGroup();
                    break;

                case "add-togroup":
                    if (args.Length != 3)
                    {
                        ShowUsageError("add-togroup <group_name> <login>");
                        return;
                    }
                    GroupManager.AddToGroup(args[1], args[2]);
                    break;

                // Path commands
                case "get-path":
                    if (args.Length != 1)
                    {
                        ShowUsageError("get-path");
                        return;
                    }
                    PasswordManager.PathPassword();
                    break;

                // Master password commands
                case "set-masterpassword":
                    if (args.Length != 2)
                    {
                        ShowUsageError("set-masterpassword <new_password");
                        return;
                    }
                    MasterPassword.SetMasterPassword(args[1]);
                    break;

                case "get-masterpassword":
                    if (args.Length != 1)
                    {
                        ShowUsageError("get-masterpassword");
                        return;
                    }
                    MasterPassword.GetMasterPassword();
                    break;

                case "set-timeout":
                    if (args.Length != 2)
                    {
                        ShowUsageError("set-timeout <int_minutes>");
                        return;
                    }
                    Console.WriteLine("Work in progress...");
                    break;

                case "get-timeout":
                    if (args.Length != 1)
                    {
                        ShowUsageError("get-timeout");
                        return;
                    }
                    MasterPassword.GetTimeout();
                    break;

                // Help command
                case "help":
                    if (args.Length != 1)
                    {
                        ShowUsageError("help");
                        return;
                    }
                    ShowHelp();
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
            Console.WriteLine("  LOGIN:");
            Console.WriteLine("    new <login> <password>               Create new login entry with password.");
            Console.WriteLine("    get <login>                          Get selected login's password.");
            Console.WriteLine("    del <login>                          Delete selected login.");
            Console.WriteLine("    list                                 List all passwords.");
            Console.WriteLine("\n  GROUPS:");
            Console.WriteLine("    new-group <group_name>               Create new group.");
            Console.WriteLine("    get-group <group_name>               Show login entries in selected group.");
            Console.WriteLine("    del-group <group_name>               Delete existing group.");
            Console.WriteLine("    list-group                           List all groups.");
            Console.WriteLine("    add-togroup <group_name> <login>     Add login to group.");
            Console.WriteLine("\n  PATH:");
            Console.WriteLine("    get-path                             Get program path.");
            Console.WriteLine("\n  MASTER PASSWORD:");
            Console.WriteLine("    set-masterpassword <new_password>    Set new master password.");
            Console.WriteLine("    get-masterpassword                   Show current master password.");
            Console.WriteLine("    set-timeout <minutes>                Set master password timeout.");
            Console.WriteLine("    get-timeout                          Get master password timeout.");
            Console.WriteLine("\nOPTIONS:");
            Console.WriteLine("  -h                                     Show help.");
            Console.WriteLine("  --version                              Show program's version.");
        }

        static void ShowUsageError(string usage)
        {
            Console.WriteLine($"Invalid arguments.");
            Console.WriteLine($"USAGE:\n  psmng {usage}");
        }
    }
}
