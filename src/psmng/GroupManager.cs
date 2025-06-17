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

    public static void NewGroup(string _groupName)
    {
    }

    public static void GetGroup(string _groupName)
    {
    }

    public static void DelGroup(string _groupName)
    {
    }

    public static void ListGroup()
    {
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

