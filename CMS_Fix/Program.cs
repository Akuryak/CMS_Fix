using System.Diagnostics;

internal class Program
{
    private static void Main(string[] args)
    {
        string defaultPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "Low\\Red Dot Games\\Car Mechanic Simulator 2021\\Save";
        if (Directory.Exists(defaultPath))
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(defaultPath);
            FileInfo[] files = directoryInfo.GetFiles();
            if (files.Count() > 0)
            {
                DateTime maxDate = DateTime.MinValue;
                FileInfo lastSaveFile = null;
                foreach (FileInfo file in files)
                {
                    if (file.CreationTime > maxDate)
                    {
                        maxDate = file.CreationTime;
                        lastSaveFile = file;
                    }
                }
                if (File.Exists(lastSaveFile.FullName))
                {
                    short count = 0;
                    byte[] bytes = File.ReadAllBytes(lastSaveFile.FullName);
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        if (count == 4)
                        {
                            bytes[i] = byte.MinValue;
                            break;
                        }
                        if (bytes[i] == byte.MinValue) count++;
                    }
                    File.WriteAllBytes(lastSaveFile.FullName, bytes);

                    DriveInfo[] driveInfos = DriveInfo.GetDrives();
                    bool check = false;
                    string gamePath = null;
                    if (File.Exists("GamePath.bin"))
                    {
                        Process.Start(File.ReadAllText("GamePath.bin"));
                    }
                    else
                    {
                        foreach (DriveInfo disk in driveInfos)
                        {
                            if (File.Exists(disk.Name + "Games\\Car Mechanic Simulator 2021\\Car Mechanic Simulator 2021.exe"))
                            {
                                gamePath = disk.Name + "Games\\Car Mechanic Simulator 2021\\Car Mechanic Simulator 2021.exe";
                                check = true; break;
                            }
                            else if (File.Exists(disk.Name + "Car Mechanic Simulator 2021\\Car Mechanic Simulator 2021.exe"))
                            {
                                gamePath = disk.Name + "Car Mechanic Simulator 2021\\Car Mechanic Simulator 2021.exe";
                                check = true; break;
                            }
                            else if (File.Exists(disk.Name + "Program Files\\Car Mechanic Simulator 2021\\Car Mechanic Simulator 2021.exe"))
                            {
                                gamePath = disk.Name + "Program Files\\Car Mechanic Simulator 2021\\Car Mechanic Simulator 2021.exe";
                                check = true; break;
                            }
                            else if (File.Exists(disk.Name + "Program Files (x86)\\Car Mechanic Simulator 2021\\Car Mechanic Simulator 2021.exe"))
                            {
                                gamePath = disk.Name + "Program Files (x86)\\Car Mechanic Simulator 2021\\Car Mechanic Simulator 2021.exe";
                                check = true; break;
                            }
                            else if (File.Exists("Car Mechanic Simulator 2021.exe"))
                            {
                                gamePath = "Car Mechanic Simulator 2021.exe";
                                check = true; break;
                            }
                            else check = false;
                        }
                        if (check)
                        {
                            if (gamePath != null)
                            {
                                Process.Start(gamePath);
                            }
                        }
                        else
                        {
                            string path = null;
                            while (!File.Exists(path))
                            {
                                Console.WriteLine("Путь до файла игры не найден, пропишите его вручную: ");
                                path = Console.ReadLine();
                            }
                            File.WriteAllText("GamePath.bin", path);
                            Console.WriteLine("Данные сохранены, перезапустите приложение.");
                        }
                    }
                }
                else Console.WriteLine("Файл сохранения не найден.");
            }
            else Console.WriteLine("Произошла ошибка. Папка с сохранениями пустая.");
        }
        else Console.WriteLine("Произошла ошибка. Папка с сохранениями не найдена.");
    }
}