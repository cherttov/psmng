using psmng.src.psmng;
using System;
using System.Data;
using System.Drawing;

class Program
{
    static void Main(string[] args)
    {
        _ = MasterPassword.isEntered;
        _ = DataManager.masterKey;
        CommandProcessor.Process(args);
    }
}