
using Interfaces.Logger;
using Interfaces.Synchronizer;
using task.Interfaces;
using task.models;

namespace Concrete.Synchronizer
{
    public class Synchronizer : ISynchronizer
    {
        private Settings _settings;
        private ILogger _logger;
        private IDisplay _display;
        public Synchronizer(Settings settings, ILogger logger, IDisplay display)
        {
            _settings = settings;
            _logger = logger;
            _display = display;
        }

        public void ScheduledSync()
        {
            throw new NotImplementedException();
        }

        public void Sync()
        {
            var sourcePath = @_settings.SourcePath;
            var replicaPath = @_settings.ReplicaPath;

            var sourceFilePaths = Directory.GetFiles(sourcePath);
            var sourceFilesInfo = MapFiles(sourceFilePaths);

            var replicaFiles = Directory.GetFiles(replicaPath);
            var replicaFilesInfo = MapFiles(replicaFiles);

            var filesStatus = PopulateFileStatusDictionary(sourceFilesInfo, replicaFilesInfo);

            foreach (var item in filesStatus)
            {
                var file = item.Value.Item1;
                var status = item.Value.Item2;

                if (status == 1)
                {
                    var fileToUpdate = file.Path;
                    var destinationFilePath = Path.Combine(replicaPath, Path.GetFileName(fileToUpdate));

                    try
                    {
                        File.Copy(fileToUpdate, destinationFilePath, true);

                        var message = $"{file.Name} updated successfully!";
                        _display.Show(message);
                        _logger.LogAction(message);
                    }
                    catch (Exception ex)
                    {
                        var exceptionMessage = $"Files failed to update!\nException:\n ";
                        _display.Show(exceptionMessage + ex.Message);
                        _logger.LogAction(exceptionMessage + ex);
                    }
                }

                if (status == -1)
                {
                    try
                    {
                        File.Delete(file.Path);
                        filesStatus.Remove(item.Key);

                        var message = $"{file} from replica removed since is no longer in the source folder";
                        _display.Show(message);
                        _logger.LogAction(message);
                    }
                    catch (Exception ex)
                    {
                        var message = $"Replica file failed to get deleted\nnException: ";
                        _display.Show(message + ex.Message);
                        _logger.LogAction(message + ex);

                    }
                }
            }
        }

        private Dictionary<string, (SynchronizedFile, int)> PopulateFileStatusDictionary(List<SynchronizedFile> sourceFilesInfo, List<SynchronizedFile> replicaFilesInfo)
        {
            var filesDueToUpdate = new Dictionary<string, (SynchronizedFile, int)>();
            for (var i = 0; i < sourceFilesInfo.Count; i++)
            {
                // add all source files in the dictionary, by default they are the newwer version, hence marked with 1
                if (!filesDueToUpdate.ContainsKey(sourceFilesInfo[i].Name))
                    filesDueToUpdate.Add(sourceFilesInfo[i].Name, (sourceFilesInfo[i], 1));
            }

            for (var i = 0; i < replicaFilesInfo.Count; i++)
            {
                var replicaFileKey = replicaFilesInfo[i].Name;

                // if dictionary contains replica fileName
                if (filesDueToUpdate.ContainsKey(replicaFileKey))
                {
                    // we check if file in source has changed
                    if (filesDueToUpdate[replicaFileKey].Item1.DateChanged == replicaFilesInfo[i].DateChanged)
                        // if not, clean up dictionary, we won't override the file in replica
                        filesDueToUpdate.Remove(replicaFileKey);
                }
                // if the source doesn't contain the replica file
                else
                {
                    // we add in dictionary marked as -1, we have to remove it from replica folder
                    filesDueToUpdate.Add(replicaFileKey, (replicaFilesInfo[i], -1));
                }
            }

            return filesDueToUpdate;
        }

        private List<SynchronizedFile> MapFiles(string[] files)
        {
            List<SynchronizedFile> synchronizedFiles = new List<SynchronizedFile>();

            foreach (string filePath in files)
            {
                FileInfo fileInfo = new FileInfo(filePath);

                SynchronizedFile file = new SynchronizedFile
                {
                    Name = Path.GetFileName(filePath),
                    Path = fileInfo.FullName,
                    DateChanged = fileInfo.LastWriteTime
                };

                synchronizedFiles.Add(file);
            }

            return synchronizedFiles;
        }

    }

    public class SynchronizedFile
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public DateTime DateChanged { get; set; }
    }
}

