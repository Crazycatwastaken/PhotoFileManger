using System.Collections.Generic; 


class Program
{
  
    static void Main(string[] args)
    {
        Dictionary<int, string> dictionary_name = new Dictionary<int, string>();
        dictionary_name.Add(123, "Test");
        Console.WriteLine(dictionary_name[123]);

        string path = @"C:\Users\maxra\Documents\PhotoApplicationTesting";

        Console.WriteLine(DateTime.Today);

        Console.Write("What folder name would you like to create");
        string FolderName = Console.ReadLine();
        Directory.CreateDirectory($"{path}\\{FolderName}");


        
        Console.ReadLine();
    }
}