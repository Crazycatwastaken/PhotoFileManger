using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;


class Program
{
    class createFolderPath
    {
        public string Path { get; set; }
        private string _currertYear = DateTime.UtcNow.ToString("yyyy");
        private string _currentMonth = DateTime.UtcNow.ToString("MMMM");
        
        public createFolderPath(string path)
        {
            Path = path;
        }
        
        public bool CheckYear()
        {
            if (Directory.Exists($"{Path}\\{_currertYear}"))
            {
                return false;

            }
            Directory.CreateDirectory($"{Path}\\{_currertYear}");
            return true;
        }
        public bool CheckMonth()
        {
            if (Directory.Exists($"{Path}\\{_currertYear}\\{_currentMonth}"))
            {
                return false;

            }
            Directory.CreateDirectory($"{Path}\\{_currertYear}\\{_currentMonth}");
            return true;
        }
        
        public bool CheckFolderName(string folderName, out string workingDirectory)
        {
            string currentDirectory = $"{Path}\\{_currertYear}\\{_currentMonth}\\{folderName}";
            if (Directory.Exists(currentDirectory))
            {
                Console.WriteLine("Folder name already exists please choose another name");
                workingDirectory = "";
                return false;

            }
            Directory.CreateDirectory(currentDirectory);
            Directory.CreateDirectory($"{currentDirectory}\\Photos");
            Directory.CreateDirectory($"{currentDirectory}\\Edited");
            Directory.CreateDirectory($"{currentDirectory}\\Temp Bin");
            Directory.CreateDirectory($"{currentDirectory}\\Temp Bin\\JPEG");
            Directory.CreateDirectory($"{currentDirectory}\\Temp Bin\\RAW");
            workingDirectory = currentDirectory;
            return true;
        }
        
    }

    class MovePhotos
    {
        public string photoPath { get; set; }
        
        public MovePhotos(string path)
        {
            photoPath = path;
        }

        public bool CheckPhotoFolder()
        {
            if (Directory.Exists($"{photoPath}"))
            {
                return true;

            }
            return false;
        }

        // public bool MoveRawPhotos(string destinationDirectory)
        // {
        //     try
        //     {
        //         Directory.Move(photoPath, destinationDirectory);
        //     }
        //     catch (Exception e)
        //     {
        //         Console.WriteLine(e.Message);
        //     }
        // }
    }
    

    static void Main(string[] args)
    {
        createFolderPath path = new createFolderPath(@"C:\Users\maxra\Documents\PhotoApplicationTesting");
        MovePhotos photoPath = new MovePhotos(@"D:\DCIM\100MSDCF");
        
        
        
        bool monthExists = path.CheckMonth();
        bool yearExists = path.CheckYear();
        bool folderExists = false;

        bool photosExist = photoPath.CheckPhotoFolder();
        bool importingPhotos = false;

        string PhotoDirectory = "";
        
        Console.WriteLine($"{monthExists} and {yearExists}");

        while (folderExists == false)
        {
            Console.Write("What folder name would you like to create: ");
            string folderName = Console.ReadLine();
            folderExists = path.CheckFolderName(folderName, out PhotoDirectory);
            Console.WriteLine(PhotoDirectory);
        }

        while (importingPhotos == false)
        {
            Console.Write("Would you like to import photos Y/N: ");
            string importphotos = Console.ReadLine();
            if (importphotos == "Y")
            {
                Console.WriteLine("Importing Photos");
                // photoPath.MoveRawPhotos(path.Path);
            }
            else if (importphotos == "N")
            {
                importingPhotos = true;
                Console.WriteLine("Exiting program");
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter Y or N.");
            }
        }

        // if (!photosExist == true)
        // {
        //     Console.WriteLine("Please check the SD of photos");
        // }
        // else
        // {
        //     Console.WriteLine("Photo Directory loaded.");
        // }

        
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