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
using System.Runtime.CompilerServices;


public class PasswordEntry
{
    public required string login { get; set; }
    public required string password { get; set; }
    public required string iv { get; set; }
    // public string? group { get; set; }
}

public static class PasswordManager
{
    private static readonly string filePath = DataManager.fileLoginPath;
    private static readonly byte[] masterKey = DataManager.masterKey;
    // Constructor on run
    static PasswordManager()
    {
    }

    // Add command
    public static void NewPassword(string _login, string _password)
    {
        string _iv = GenerateIV();
        string _encryptedPassword = EncryptData(_password, Convert.FromBase64String(_iv));
        var _passwordEntries = JsonDeserializer();

        // Rewrites existing entry || adds new entry
        var _existingEntry = _passwordEntries.FirstOrDefault(e => e.login.Equals(_login, StringComparison.OrdinalIgnoreCase));
        if (_existingEntry != null)
        {
            _existingEntry.password = _encryptedPassword;
            _existingEntry.iv = _iv;
            Console.WriteLine($"'{_login}' password has been updated to '{_password}'");
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

            Console.WriteLine($"Added '{_login}' with password '{_password}'");
        }

        string _updatedJson = JsonSerializer.Serialize(_passwordEntries, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, _updatedJson);
    }

    // Get command
    public static void GetPassword(string _login)
    {
        var _passwordEntries = JsonDeserializer();

        var _entryToRead = _passwordEntries.FirstOrDefault(e => e.login == _login);
        if (_entryToRead != null)
        {
            byte[] _ivToPass = Convert.FromBase64String(_entryToRead.iv);
            string _decrypted = DecryptData(_entryToRead.password, _ivToPass);

            string _group = GroupManager.CheckGroup(_login);

            Console.WriteLine($"LOGIN:\n  {_login}");
            Console.WriteLine($"GROUP:\n  {_group}");
            Console.WriteLine($"PASSWORD:\n  {_decrypted}");
        }
        else
        {
            Console.WriteLine($"Login '{_login}' not found.");
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

            string _updatedJson = JsonSerializer.Serialize(_passwordEntries, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, _updatedJson);
            Console.WriteLine($"Login '{_login}' has been deleted.");
        }
        else
        {
            Console.WriteLine($"Login '{_login}' not found.");
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
        string _dir = DataManager.GetProjectRoot();
        Console.WriteLine($"PSMNG DIRECTORY PATH:\n  '{_dir}'");
    }

    // Encrypt data
    private static string EncryptData(string plainText, byte[] _iv)
    {
        using Aes aes = Aes.Create();
        aes.Key = masterKey;
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
        aes.Key = masterKey;
        aes.IV = _iv;

        using var decryptor = aes.CreateDecryptor();
        byte[] buffer = Convert.FromBase64String(encryptedText);

        using var ms = new MemoryStream(buffer);
        using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using var sr = new StreamReader(cs);

        return sr.ReadToEnd();
    }

    // Generate and return IV
    private static string GenerateIV()
    {
        using var rnd = RandomNumberGenerator.Create();

        byte[] _iv = new byte[16];
        rnd.GetBytes(_iv);

        string encoded = Convert.ToBase64String(_iv);

        return encoded;
    }

    // Login existence check for GroupManager
    public static bool CheckLogin(string _login) 
    {
        var _passwordEntries = JsonDeserializer();

        var _entryToRead = _passwordEntries.FirstOrDefault(e => e.login == _login);
        if (_entryToRead != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // Json deserializer
    private static List<PasswordEntry> JsonDeserializer()
    {
        List<PasswordEntry> _entries = new();

        if (File.Exists(filePath))
        {
            string _json = File.ReadAllText(filePath);
            if (!string.IsNullOrEmpty(_json))
            {
                _entries = JsonSerializer.Deserialize<List<PasswordEntry>>(_json)!;
            }
        }
        return _entries;
    }
}


