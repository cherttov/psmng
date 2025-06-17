using System;
using System.IO;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using Aes = System.Security.Cryptography.Aes;
using System.Text;
using System.Text.Json;
using System.Collections.Generic;
using System.Data;


public class PasswordEntry
{
    public required string login { get; set; }
    public required string password { get; set; }
    public required string iv { get; set; }
    // public string? group { get; set; }
}

public static class PasswordManager
{
    // Path to store.json & master.key
    private static string filePath = Path.Combine(GetProjectRoot(), "data", "psmng_store.json");
    private static string masterKeyPath = Path.Combine(GetProjectRoot(), "data", "master.key");

    private static byte[] _masterKey = Array.Empty<byte>();

    // Constructor on run
    static PasswordManager()
    {
        string directory = Path.GetDirectoryName(filePath)!;

        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "[]");
        }

        if (!File.Exists(masterKeyPath))
        {
            GenerateMasterKey();
        }

        LoadMasterKey();
    }

    // Get absolute path to project root
    private static string GetProjectRoot()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);

        while (dir != null)
        {
            var csprojPath = Path.Combine(dir.FullName, "psmng.csproj");
            if (File.Exists(csprojPath))
            {
                return dir.FullName;
            }

            dir = dir.Parent;
        }
        throw new Exception("Project root not found.");
    }

    // Add command
    public static void NewPassword(string _login, string _password)
    {
        var _iv = GenerateIV();
        var _encryptedPassword = EncryptData(_password, Convert.FromBase64String(_iv));
        var _passwordEntries = JsonDeserializer();

        // Rewrites existing entry || adds new entry
        var existingEntry = _passwordEntries.FirstOrDefault(entry => entry.login.Equals(_login, StringComparison.OrdinalIgnoreCase));
        if (existingEntry != null)
        {
            existingEntry.password = _encryptedPassword;
            existingEntry.iv = _iv;
            Console.WriteLine($"{_login} password has been updated to {_password}");
        }
        else
        {
            var newEntry = new PasswordEntry
            {
                login = _login,
                password = _encryptedPassword,
                iv = _iv
            };

            _passwordEntries.Add(newEntry);

            Console.WriteLine($"Added {_login} with password {_password}");
        }

        string updatedJson = JsonSerializer.Serialize(_passwordEntries, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, updatedJson);
    }

    // Get command
    public static void GetPassword(string _login)
    {
        var _passwordEntries = JsonDeserializer();
        var _entryToRead = _passwordEntries.FirstOrDefault(e => e.login == _login);

        if (_entryToRead != null)
        {
            var _ivToPass = Convert.FromBase64String(_entryToRead.iv);
            var _decrypted = DecryptData(_entryToRead.password, _ivToPass);

            Console.WriteLine($"LOGIN: {_login}");
            Console.WriteLine($"PASSWORD: {_decrypted}");
        }
        else
        {
            Console.WriteLine($"Login {_login} not found.");
        }
    }

    // Del command
    public static void DelPassword(string _login)
    {
        var _passwordEntries = JsonDeserializer();
        var _entryToRemove = _passwordEntries.FirstOrDefault(e => e.login == _login);

        if (_entryToRemove != null)
        {
            _passwordEntries.Remove(_entryToRemove);

            string updatedJson = JsonSerializer.Serialize(_passwordEntries, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, updatedJson);
            Console.WriteLine($"Deleted {_login} from database.");
        }
        else
        {
            Console.WriteLine($"Login {_login} not found.");
        }
    }

    // List command
    public static void ListPassword()
    {
        var _passwordEntries = JsonDeserializer();

        Console.WriteLine("PASSWORD LIST:");

        foreach (var entry in _passwordEntries)
        {
            Console.WriteLine($"  {entry.login} : {entry.password}");
        }
    }

    // Path command
    public static void PathPassword()
    {
        Console.WriteLine($"psmng path : {filePath}");
    }


    // Json deserializer
    private static List<PasswordEntry> JsonDeserializer()
    {
        List<PasswordEntry> _passwordEntries = new();

        if (File.Exists(filePath))
        {
            string _json = File.ReadAllText(filePath);
            if (!string.IsNullOrEmpty(_json))
            {
                _passwordEntries = JsonSerializer.Deserialize<List<PasswordEntry>>(_json)!;
            }
        }

        return _passwordEntries;
    }


    // Encrypt data
    private static string EncryptData(string plainText, byte[] _iv)
    {
        using Aes aes = Aes.Create();
        aes.Key = _masterKey;
        aes.IV = _iv;

        using var encryptor = aes.CreateEncryptor();
        using var ms = new MemoryStream();
        using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
        using var sw = new StreamWriter(cs);

        sw.Write(plainText);
        sw.Close();

        return Convert.ToBase64String(ms.ToArray());
    }

    // Decrypt data
    private static string DecryptData(string encryptedText, byte[] _iv)
    {
        using Aes aes = Aes.Create();
        aes.Key = _masterKey;
        aes.IV = _iv;

        using var decryptor = aes.CreateDecryptor();
        byte[] buffer = Convert.FromBase64String(encryptedText);

        using var ms = new MemoryStream(buffer);
        using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using var sr = new StreamReader(cs);

        return sr.ReadToEnd();
    }


    // Generate and store MasterKey
    private static void GenerateMasterKey()
    {
        using var rnd = RandomNumberGenerator.Create();

        _masterKey = new byte[32];
        rnd.GetBytes(_masterKey);

        string encoded = Convert.ToBase64String(_masterKey);
        File.WriteAllText(masterKeyPath, encoded);
    }

    // Generate and return IV
    private static string GenerateIV()
    {
        using var rnd = RandomNumberGenerator.Create();

        var _iv = new byte[16];
        rnd.GetBytes(_iv);

        string encoded = Convert.ToBase64String(_iv);

        return encoded;
    }

    private static void LoadMasterKey()
    {
        string readKey = File.ReadAllText(masterKeyPath);
        _masterKey = Convert.FromBase64String(readKey);
    }
}


