using System;
using System.IO;
using System.Security.Permissions;
using System.Threading.Tasks;

namespace IlegraChallange
{
    public static class SaleAnalyser
    {
        public static void Run()
        {
            ProcessFiles();
            RunFolderMonitor();
        }

        private static void ProcessFiles()
        {
            string[] filePaths = Directory.GetFiles(Path.Combine(Utils.GetHomePath(), "data/in"), "*.txt");
            foreach (var filePath in filePaths)
            {
                Task.Run(() => FileProcessor.ProcessAsync(filePath));
            }
        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        private static void RunFolderMonitor()
        {
            using FileSystemWatcher watcher = new FileSystemWatcher
            {
                Path = Path.Combine(Utils.GetHomePath(), "data", "in"),
                NotifyFilter = NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.FileName
                                 | NotifyFilters.DirectoryName,
                Filter = "*.txt"
            };
            watcher.Created += OnCreated;
            watcher.EnableRaisingEvents = true;

            Console.WriteLine("Analisando arquivos!");
            Console.WriteLine("Precione 'q' para encerrar.");
            while (Console.Read() != 'q') ;
        }

        private static void OnCreated(object source, FileSystemEventArgs e)
        {
            Task.Run(() => FileProcessor.ProcessAsync(e.FullPath));
        }
    }
}