using System.Collections.Generic;
using System.Text;


class Program
{
    class folderPath
    {
        public string Path { get; set; }
        private string currertYear = DateTime.UtcNow.ToString("yyyy");
        private string currentMonth = DateTime.UtcNow.ToString("MMMM");
        
        public folderPath(string path)
        {
            Path = path;
        }
        
        
        public bool CheckYear()
        {
            if (Directory.Exists($"{Path}\\{currertYear}"))
            {
                Console.WriteLine("Year already exists, creating folder inside directory");
                return true;

            }
            return false;
        }
        public bool CheckMonth()
        {
            if (Directory.Exists($"{Path}\\{currertYear}\\{currentMonth}"))
            {
                Console.WriteLine("Month already exists, creating folder inside directory");
                return true;

            }
            return false;
        }
        

        
        
        
    }

    static void Main(string[] args)
    {
        folderPath path = new folderPath(@"C:\Users\maxra\Documents\PhotoApplicationTesting");
        bool monthExists = path.CheckMonth();
        bool yearExists = path.CheckYear();
        
        Console.WriteLine($"{monthExists} and {yearExists}");

        //
        // Console.Write("What folder name would you like to create");
        // string folderName = Console.ReadLine();
        





        
        Console.ReadLine();
    }

    // static void CreateFolder(string path, string folderName)
    // {
    //     string currentMonth = DateTime.UtcNow.ToString("MMMM");
    //     string currertYear = DateTime.UtcNow.ToString("YYYY");
    //
    //     if (Directory.Exists($"{path}\\{currentMonth}"))
    //     {
    //         Console.WriteLine("Month already exists, creating folder inside directory");
    //         Directory.CreateDirectory($"{path}\\{currentMonth}\\{folderName}");
    //     }
    //     else
    //     {
    //         Console.WriteLine("Creating current month folder and folder inside directory");
    //         Directory.CreateDirectory($"{path}\\{currentMonth}");
    //         Directory.CreateDirectory($"{path}\\{currentMonth}\\{folderName}");
    //     }
    //     // Directory.CreateDirectory($"{path}\\{folderName}");
    //     
    // }

}