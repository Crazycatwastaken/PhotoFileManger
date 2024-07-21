using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Text.Json;
using System.Text.Json.Serialization;



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
            CheckMonth();
            CheckYear();
        }
        
        private bool CheckYear()
        {
            if (Directory.Exists($"{Path}\\{_currertYear}"))
            {
                return false;
            }
            Directory.CreateDirectory($"{Path}\\{_currertYear}");
            return true;
        }
        private bool CheckMonth()
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

    class DataStore
    {
        public string Data { get; set; }
        
        public DataStore(string data)
        {
            Data = data;
        }
        
        public class PhotoInfo
        {
            public DateTimeOffset Date { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
            public double Size { get; set; }    
            public string? CameraMaker { get; set; }
            public string? CameraModel { get; set; }
            public int? IsoSpeed { get; set; }
            public string? FStop { get; set; }
            public string? ExposureTime { get; set; }
            public string? ExposureBias { get; set; }
            public string? ExposureProgram { get; set; }
            public string? MeteringMode { get; set; }
            public string? FlashMode { get; set; }
            public int? FocalLength { get; set; } 
        }

        

        public void CheckDataStore()
        {
            if (Directory.Exists(Data))
            {
                Console.WriteLine("Found data store");
            }
            else
            {
                // File.Create(Data).Close();
                Console.WriteLine($"Created data store @{Data}");
            }

        }

        public void writeData()
        {

            string filePath = Data;
            string readJsonString = File.ReadAllText(filePath);
            List<PhotoInfo> photoInfoList = JsonSerializer.Deserialize<List<PhotoInfo>>(readJsonString);
            foreach (PhotoInfo photoInfo in photoInfoList)
            {
                Console.WriteLine($"Date: {photoInfo.Date}");
                Console.WriteLine($"Width: {photoInfo.Width}");
                // ... and so on for other properties
            }

           
            var TestPhotoInfo = new PhotoInfo
            {
                Date = DateTime.Parse("2020-08-01"),
                Width = 154,
                Height = 420,
                Size = 23.5,
                CameraMaker = "Sony",
                CameraModel = "ILCE-6400",
                IsoSpeed = 900,
                FStop = "f/16",
                ExposureTime = "1/160",
                ExposureBias = "0 step",
                ExposureProgram = "Manual",
                MeteringMode = "Pattern",
                FlashMode = "Flash",
                FocalLength = 17
            };
            
            
            //
            photoInfoList.Add(TestPhotoInfo);
            string jsonString = JsonSerializer.Serialize(photoInfoList);
            // Console.WriteLine(Data);
            // var readJson = File.ReadAllText(Data);
            // Console.WriteLine(readJson);
            //
            // string jsonString2 = File.ReadAllText(Data);
            // PhotoInfo photoInfo = JsonSerializer.Deserialize<PhotoInfo>(jsonString2);
            //
            // Console.WriteLine($"The file reads: {photoInfo}");

            //
            // string serializedData = JsonConvert.SerializeObject(myList);
            // File.WriteAllText(readJson, serializedData);
            
            Console.WriteLine("Writing");
            try
            {
                
                File.WriteAllText(Data, jsonString);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Console.WriteLine("Written");
            // Console.WriteLine(jsonString);
        }
    }
    

    static void Main(string[] args)
    {
        createFolderPath path = new createFolderPath(@"C:\Users\maxra\Documents\PhotoApplicationTesting");
        MovePhotos photoPath = new MovePhotos(@"D:\DCIM\100MSDCF");
        DataStore dataStore = new DataStore($"{path.Path}\\Datastore.json");
        dataStore.CheckDataStore();
        dataStore.writeData();
        // dataStore.writeData();
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