using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class DataManager
{
    public static string fileLoginPath = Path.Combine(GetProjectRoot(), "data", "psmng_store.json");
    public static string fileGroupPath = Path.Combine(GetProjectRoot(), "data", "groups.json");
    public static string masterKeyPath = Path.Combine(GetProjectRoot(), "data", "master.key");

    public static byte[] masterKey = Array.Empty<byte>();

    // Constructor on run
    static DataManager()
    {
        string directory = Path.GetDirectoryName(fileLoginPath)!;

        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        if (!File.Exists(fileLoginPath))
        {
            File.WriteAllText(fileLoginPath, "[]");
        }

        if (!File.Exists(fileGroupPath))
        {
            File.WriteAllText(fileGroupPath, "[]");
        }

        if (!File.Exists(masterKeyPath))
        {
            GenerateMasterKey();
        }
        LoadMasterKey();
    }

    // Get absolute path to project root
    public static string GetProjectRoot()
    {
        var _dir = new DirectoryInfo(AppContext.BaseDirectory);

        while (_dir != null)
        {
            string csprojPath = Path.Combine(_dir.FullName, "psmng.csproj");
            if (File.Exists(csprojPath))
            {
                return _dir.FullName;
            }

            _dir = _dir.Parent;
        }
        throw new Exception("Project root not found.");
    }

    // Generate and store MasterKey
    private static void GenerateMasterKey()
    {
        using var rnd = RandomNumberGenerator.Create();

        masterKey = new byte[32];
        rnd.GetBytes(masterKey);

        string encoded = Convert.ToBase64String(masterKey);
        File.WriteAllText(masterKeyPath, encoded);
    }

    // Load MasterKey
    private static void LoadMasterKey()
    {
        string readKey = File.ReadAllText(masterKeyPath);
        masterKey = Convert.FromBase64String(readKey);
    }
}
