
using Interfaces.Synchronizer;
using task.models;

namespace Concrete.Synchronizer
{
    public class Synchronizer : ISynchronizer
    {
        private Settings _settings;
        public Synchronizer(Settings settings)
        {
            _settings = settings;
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
                        // Log here
                    }
                    catch (Exception ex)
                    {
                        // log failure
                    }
                }

                if (status == -1)
                {
                    try
                    {
                        File.Delete(file.Path);
                        filesStatus.Remove(item.Key);
                    }
                    catch (Exception ex)
                    {
                        // log failure
                    }
                }
            }
        }

        private Dictionary<string, (SynchronizedFile, int)> PopulateFileStatusDictionary(List<SynchronizedFile> sourceFilesInfo, List<SynchronizedFile> replicaFilesInfo)
        {
            var checkedFiles = new Dictionary<string, (SynchronizedFile, int)>();
            for (var i = 0; i < sourceFilesInfo.Count; i++)
            {
                // add all source files in the dictionary, by default they are the newwer version, hence marked with 1
                if (!checkedFiles.ContainsKey(sourceFilesInfo[i].Name))
                    checkedFiles.Add(sourceFilesInfo[i].Name, (sourceFilesInfo[i], 1));
            }

            for (var i = 0; i < replicaFilesInfo.Count; i++)
            {
                var replicaFileKey = replicaFilesInfo[i].Name;

                // if dictionary contains replica fileName
                if (checkedFiles.ContainsKey(replicaFileKey))
                {
                    // we check if file in source has changed
                    if (checkedFiles[replicaFileKey].Item1.DateChanged == replicaFilesInfo[i].DateChanged)
                        // if not, clean up dictionary, we won't override the file in replica
                        checkedFiles.Remove(replicaFileKey);
                }
                // if the source doesn't contain the replica file
                else
                {
                    // we add in dictionary marked as -1, we have to remove it from replica folder
                    checkedFiles.Add(replicaFileKey, (replicaFilesInfo[i], -1));
                }
            }

            return checkedFiles;
        }

        private List<SynchronizedFile> MapFiles(string[] files)
        {
            List<SynchronizedFile> result = new List<SynchronizedFile>();

            foreach (string filePath in files)
            {
                FileInfo fileInfo = new FileInfo(filePath);

                SynchronizedFile file = new SynchronizedFile
                {
                    Name = Path.GetFileName(filePath),
                    Path = fileInfo.FullName,
                    DateChanged = fileInfo.LastWriteTime
                };

                result.Add(file);
            }

            return result;
        }

    }

    public class SynchronizedFile
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public DateTime DateChanged { get; set; }
    }
}

/*
    So what I need to do.

    Source folder needs to have replica folder with the same files
        - Problems: content files in source might change, so I need to read all the lines too or just replace replica files everytime.
                content files might get removed, so I need to save the file names when they get removed
                new files will just get added on replica 

    So I have a folder source,
        My program might run, might not run but when it runs, I do a sync on start.
            I run sync that will do the following thing.
                if file x is found, x is updated with the source file
                if file x is not found in the replica, x was added, I add in replica
                if files not found in source, files were deleted

                Use binary search, considering files are in alphabetical order.

                generate a log entry for each case x was updated at hh/mm/yy,
                                                   x was deleted
                                                   x was added 

        what the heck is md5

    Problem when sync happens, paths should pe disallowed for changing till the sync is complete
    Problem if I change the source directory, should I move all files too?
*/
