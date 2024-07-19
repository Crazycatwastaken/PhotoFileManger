using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;


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

        public bool MoveRawPhotos(string destinationDirectory)
        {
            string sourceFile = "";
            string destFile = "";
            
            string RAWPattern = @"\.ARW$";
            string JPGPattern = @"\.JPG";
            List<string> AWRPhotos = new List<string>();
            List<string> JPGPhotos = new List<string>();
       
            string[] files = Directory.GetFiles(photoPath);
            foreach (string file in files)
            {
                string fileName = Path.GetFileName(file);
                if (Regex.IsMatch(fileName, RAWPattern))
                {
                    AWRPhotos.Add(fileName);
                }

                if (Regex.IsMatch(fileName, JPGPattern))
                {
                    JPGPhotos.Add(fileName);
                }
            }

            try
            {
                string importRAW = returnYN("Would you like to import RAW photos Y/N: ");
                string importJPG = returnYN("Would you like to import JPEG photos Y/N: ");

                switch (importRAW)
                {
                    case "Y":
                        Console.WriteLine("Importing RAW Files");
                        for (int i = 0; i < AWRPhotos.Count; i++)
                        {
                            Console.WriteLine($"Importing {AWRPhotos[i]}");
                            sourceFile = Path.Combine(photoPath, AWRPhotos[i]);
                            destFile = Path.Combine($"{destinationDirectory}\\Temp Bin\\RAW", AWRPhotos[i]);
                            File.Copy(sourceFile, destFile, true);
                        }

                        break;
                    case "N":
                        Console.WriteLine("Skipping importing RAW files.");
                        break;
                    default:
                        Console.WriteLine("Please enter Y or N");
                        break;
                }

                switch (importJPG)
                {
                    case "Y":
                        Console.WriteLine("Importing JPEG Files");
                        for (int i = 0; i < JPGPhotos.Count; i++)
                        {
                            Console.WriteLine($"Importing {JPGPhotos[i]}");
                            sourceFile = Path.Combine(photoPath, JPGPhotos[i]);
                            destFile = Path.Combine($"{destinationDirectory}\\Temp Bin\\JPEG", JPGPhotos[i]);
                            File.Copy(sourceFile, destFile, true);
                        }
                        break;
                    case "N":
                        Console.WriteLine("Skipping importing JPEG files.");
                        break;
                    default:
                        Console.WriteLine("Please enter Y or N");
                        break;
                }
           
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
    }
    

    static void Main(string[] args)
    {
        createFolderPath path = new createFolderPath(@"C:\Users\maxra\Documents\PhotoApplicationTesting");
        MovePhotos photoPath = new MovePhotos(@"D:\DCIM\100MSDCF");
        
        
        
        bool monthExists = path.CheckMonth();
        bool yearExists = path.CheckYear();
        bool folderExists = false;

        bool photosExist = photoPath.CheckPhotoFolder();
        bool finishedImportingPhotos = false;

        string PhotoDirectory = "";
        

        while (folderExists == false)
        {
            Console.Write("What folder name would you like to create: ");
            string folderName = Console.ReadLine();
            folderExists = path.CheckFolderName(folderName, out PhotoDirectory);
            Console.WriteLine(PhotoDirectory);
        }

        while (finishedImportingPhotos == false)
        {
            string importphotos = returnYN("Would you like to import photos Y/N: ");
          
            switch (importphotos)
            {
                case "Y":
                    Console.WriteLine("Importing Photos");
                    finishedImportingPhotos = photoPath.MoveRawPhotos(PhotoDirectory);
                    break;
                case "N":
                    finishedImportingPhotos = true;
                    break;
                default:
                    Console.WriteLine("Invalid input. Please enter Y or N.");
                    break;
            }
            
        }

       Console.WriteLine("Thank you for using the program");

        Console.ReadLine();
    }
    
    static string returnYN(string text)
    {
        Console.Write(text);
        return Console.ReadLine().ToUpper();
    }


}