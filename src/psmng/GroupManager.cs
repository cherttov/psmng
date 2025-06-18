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


public class GroupEntry
{
    public required string groupName {  get; set; }
}

public static class GroupManager 
{
    private static string filePath = DataManager.fileGroupPath;

    // Constructor on run
    static GroupManager()
    {
    }

    // New command
    public static void NewGroup(string _groupName)
    {
        var _groupEntries = JsonDeserializer();

        var _existingEntry = _groupEntries.FirstOrDefault(e => e.groupName.Equals(_groupName, StringComparison.OrdinalIgnoreCase));
        if (_existingEntry != null)
        {
            Console.WriteLine($"Group '{_groupName}' already exists.");
        }
        else
        {
            var _newEntry = new GroupEntry() { groupName = _groupName };
            _groupEntries.Add(_newEntry);

            Console.WriteLine($"Group '{_groupName}' has been created.");
        }

        string _updatedJson = JsonSerializer.Serialize(_groupEntries, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, _updatedJson);
    }

    // Get command
    public static void GetGroup(string _groupName)
    {
        var _groupEntries = JsonDeserializer();
        var _entryToRead = _groupEntries.FirstOrDefault(e => e.groupName == _groupName);

        if (_entryToRead != null)
        {
            Console.WriteLine($"Logins in group '{_groupName}':");
            Console.WriteLine($"  work in progress...");
        }
        else
        {
            Console.WriteLine($"Group '{_groupName}' not found");
        }
    }

    // Del command
    public static void DelGroup(string _groupName)
    {
        var _groupEntries = JsonDeserializer();
        var _entryToRemove = _groupEntries.FirstOrDefault(e => e.groupName == _groupName);

        if (_entryToRemove != null)
        {
            _groupEntries.Remove(_entryToRemove);

            string _updatedJson = JsonSerializer.Serialize(_groupEntries, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, _updatedJson);
            Console.WriteLine($"Group '{_groupName}' has been deleted.");
        }
        else
        {
            Console.WriteLine($"Group '{_groupName}' not found");
        }
    }

    // List command
    public static void ListGroup()
    {
        var _groupEntries = JsonDeserializer();

        Console.WriteLine("GROUPS LIST:");

        foreach (var entry in _groupEntries)
        {
            Console.WriteLine($"  {entry.groupName}");
        }
    }

    private static List<GroupEntry> JsonDeserializer()
    {
        List<GroupEntry> _entries = new();

        if (File.Exists(filePath))
        {
            string _json = File.ReadAllText(filePath);
            if (!string.IsNullOrEmpty(_json))
            {
                _entries = JsonSerializer.Deserialize<List<GroupEntry>>(_json)!;
            }
        }
        return _entries;
    }
}

