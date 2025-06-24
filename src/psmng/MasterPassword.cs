using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using psmng.src.psmng;

public class MasterPassword
{
    private static readonly string filePath = DataManager.masterPasswordPath;
    private static readonly string sessionFile = DataManager.sessionPath;

    private static int timeout = 30;
    public static bool placeholder = true;

    // Constructor on run
    static MasterPassword()
    {
        // func CheckEntry()
        if (CheckSession())
        {
            return;
        }
        else
        {
            Console.WriteLine("MASTER PASSWORD:");
            Console.Write("> ");
            string? enteredPassword = Console.ReadLine();

            if (CheckEnteredPassword(enteredPassword))
            {
                CreateSessionFile();
            }
            else
            {
                Console.WriteLine("ERROR:\n  Wrong master password was provided.");
                Environment.Exit(0);
            }
        }
    }

    // Set master password
    public static void SetMasterPassword(string _newPassword)
    {
        File.WriteAllText(filePath, _newPassword);
        Console.WriteLine($"Master password has been changed to '{_newPassword}'");
    }

    // Get master password
    public static void GetMasterPassword()
    {
        string _masterPassword = LoadMasterPassword();
        Console.WriteLine($"CURRENT MASTER PASSWORD:\n  {_masterPassword}");
    }

    // Set timeout
    public static void SetTimeout(string _minutes)
    {
        if (int.TryParse(_minutes, out int _parsedMinutes) && _parsedMinutes >= 0)
        {
            timeout = _parsedMinutes; // new timeout file
            // combine all data files to 2 files, add more encryption, optimize program more
            Console.WriteLine($"Timeout has been changed to {_parsedMinutes}");
        }
        else
        {
            Console.WriteLine($"ERROR: Invalid timeout value, please enter positive number.");
        }
    }

    // Get timeout
    public static void GetTimeout()
    {
        Console.WriteLine($"CURRENT TIMEOUT:\n  {timeout} minutes.");
    }

    // Check if entered password matches master password
    private static bool CheckEnteredPassword(string? _input)
    {
        string _masterPassword = LoadMasterPassword();
        return _input == _masterPassword;
    }

    // Load Master Password
    private static string LoadMasterPassword()
    {
        string _masterPassword = File.ReadAllText(filePath);
        return _masterPassword;
    }

    // Create session authorization file
    private static void CreateSessionFile()
    {
        string _time = DateTime.UtcNow.ToString("O");
        File.WriteAllText(sessionFile, _time);
    }

    // Check session authorization file
    private static bool CheckSession()
    {
        if (!File.Exists(sessionFile))
        {
            return false;
        }

        try
        {
            string _time = File.ReadAllText(sessionFile);
            DateTime _authTime = DateTime.Parse(_time).ToUniversalTime();
            return (DateTime.UtcNow - _authTime).TotalMinutes < timeout;
        }
        catch
        {
            return false;
        }
    }
}

