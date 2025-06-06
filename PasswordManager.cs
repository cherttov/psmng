using System;
using System.IO;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Collections.Generic;

public static class PasswordManager
{
    // Path to data files (root_dir, data_dir, file)
    private static string filePath = Path.Combine(GetProjectRoot(), "data", "psmng_store.json");
    private static string masterKeyPath = Path.Combine(GetProjectRoot(), "data", "master.key");

    // Private & public MasterKey for encryption
    private static byte[] key = Array.Empty<byte>();
    private static byte[] iv = Array.Empty<byte>();

    // Static constructor on first run
    static PasswordManager()
    {
        string directory = Path.GetDirectoryName(filePath)!;

        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "{}");
        }

        if (!File.Exists(masterKeyPath))
        {
            GenerateMasterKey();
        }

        LoadMasterKey();
    }

    // Get absolute path To project file
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
    public static void AddPassword(string login, string password)
    {
        var encryptedPassword = EncryptData(password);
        var oldJson = JsonDeserializer();

        // Add or update the password
        oldJson[login] = encryptedPassword; // Dict[key] = value

        // Save back to file
        string updatedJson = JsonSerializer.Serialize(oldJson, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, updatedJson);

        Console.WriteLine($"Added {login} with password {password}");
    }

    // Delete command
    public static void DelPassword(string login)
    {
        var oldJson = JsonDeserializer();

        // Runs Remove() & returns false if didn't Remove()
        if (oldJson.Remove(login))
        {
            string updatedJson = JsonSerializer.Serialize(oldJson, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, updatedJson);
            Console.WriteLine($"Deleted {login} from database.");
        }
        else
        {
            Console.WriteLine($"Login {login} not found.");
        }
        
    }

    // Get command
    public static void GetPassword(string login)
    {
        var json = JsonDeserializer();

        foreach (var row in json)
        {
            if (row.Key == login)
            {
                var decrypted = DecryptData(row.Value);
                Console.WriteLine($"login: {row.Key}");
                Console.WriteLine($"password: {decrypted}");
                return;
            }
        }
        Console.WriteLine("Login not found.");
    }

    // List command
    public static void ListPassword()
    {
        var json = JsonDeserializer();

        Console.WriteLine("PASSWORD LIST:");

        foreach (var row in json)
        {
            Console.WriteLine($"    {row.Key} : {row.Value}");
        }
    }

    // Path command
    public static void PathPassword()
    {
        Console.WriteLine($"psmng path : {filePath}");
    }

    // Json deserializer
    public static Dictionary<string, string> JsonDeserializer()
    {
        // New Dictionary
        Dictionary<string, string> deserializedJson = new();

        // If file already has data, load it
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            if (!string.IsNullOrEmpty(json))
            {
                // Converts json string into a usable dictionary
                deserializedJson = JsonSerializer.Deserialize<Dictionary<string, string>>(json)!;
            }
        }

        return deserializedJson;
    }

    // Encrypt data
    private static string EncryptData(string plainText)
    {
        using System.Security.Cryptography.Aes aes = System.Security.Cryptography.Aes.Create();
        aes.Key = key;
        aes.IV = iv;

        using var encryptor = aes.CreateEncryptor();
        using var ms = new MemoryStream();
        using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
        using var sw = new StreamWriter(cs);

        sw.Write(plainText);
        sw.Close();

        return Convert.ToBase64String(ms.ToArray());
    }

    // Decrypt data
    private static string DecryptData(string EncryptedText)
    {
        using System.Security.Cryptography.Aes aes = System.Security.Cryptography.Aes.Create();
        aes.Key = key;
        aes.IV = iv;

        using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        byte[] buffer = Convert.FromBase64String(EncryptedText);
        using var ms = new MemoryStream(buffer);
        using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using var sr = new StreamReader(cs);

        return sr.ReadToEnd();
    }

    // Generate & store MasterKey & IV
    private static void GenerateMasterKey()
    {
        using var rnd = RandomNumberGenerator.Create();

        key = new byte[32];
        iv = new byte[16];

        rnd.GetBytes(key);
        rnd.GetBytes(iv);

        string encoded = Convert.ToBase64String(key) + "\n" + Convert.ToBase64String(iv);
        File.WriteAllText(masterKeyPath, encoded);
    }

    // Loading MasterKey & IV
    private static void LoadMasterKey()
    {
        string[] lines = File.ReadAllLines(masterKeyPath);
        key = Convert.FromBase64String(lines[0]);
        iv = Convert.FromBase64String(lines[1]);
        if (lines.Length < 2)
        {
            throw new Exception("Invalid master.key file format.");
        }
    }
}