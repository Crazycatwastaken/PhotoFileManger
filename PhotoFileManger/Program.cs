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
                Console.Write("Would you like to import RAW photos Y/N: ");
                string importRAW = Console.ReadLine().ToUpper();
                
                Console.Write("Would you like to import JPEG photos Y/N: ");
                string importJPG = Console.ReadLine().ToUpper();

                if (importRAW == "Y")
                {
                    Console.WriteLine("Importing RAW Files");
                    for (int i = 0; i < AWRPhotos.Count; i++)
                    {
                        Console.WriteLine($"Importing {AWRPhotos[i]}");
                        sourceFile = Path.Combine(photoPath, AWRPhotos[i]);
                        destFile = Path.Combine($"{destinationDirectory}\\Temp Bin\\RAW", AWRPhotos[i]);
                        File.Copy(sourceFile, destFile, true);
                    }
                }
                else if (importRAW == "N")
                {
                    Console.WriteLine("Skipping importing RAW files.");
                }
                else
                {
                    Console.WriteLine("Please enter Y or N");
                }
                
                if (importJPG == "Y")
                {
                    Console.WriteLine("Importing JPEG Files");
                    for (int i = 0; i < JPGPhotos.Count; i++)
                    {
                        Console.WriteLine($"Importing {JPGPhotos[i]}");
                        sourceFile = Path.Combine(photoPath, JPGPhotos[i]);
                        destFile = Path.Combine($"{destinationDirectory}\\Temp Bin\\JPEG", JPGPhotos[i]);
                        File.Copy(sourceFile, destFile, true);
                    }
                }
                else if (importJPG == "N")
                {
                    Console.WriteLine("Skipping importing RAW files.");
                }
                else
                {
                    Console.WriteLine("Please enter Y or N");
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
                Console.WriteLine(photoPath.MoveRawPhotos(PhotoDirectory));
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

        
        Console.ReadLine();
    }


}