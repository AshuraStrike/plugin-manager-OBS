using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Reflection.Metadata.Ecma335;
using System.Xml.Linq;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using PluginManagerObs.Models;
using Tomlyn;

namespace PluginManagerObs.Classes
{
    internal class ControllerPlugins
    {
        private DBOBSPathEntry obsPath;
        public string pluginsPath;
        public List<Plugin> listPlugins;

        private List<Plugin> listPluginsFull;

        private DBPluginsContext dbHandler;
        public ControllerPlugins()
        {
            obsPath = new()
            {
                Path = string.Empty
            };
            pluginsPath = string.Empty;

            listPlugins = new();
            listPluginsFull = new();

            dbHandler = new();
            dbHandler.Database.EnsureCreatedAsync().Wait();
        }

        public bool setObsPath(string obsPath_)
        {
            var query = dbHandler.OBSPaths.Where(o => o.Path == obsPath_);
            Debug.WriteLine($"Query contents: {query.Count()}");
            if (query.Count() == 0)
            {
                obsPath = new() { Path = obsPath_ };
                dbHandler.OBSPaths.Add(obsPath);
                dbHandler.SaveChanges();
            }
            else
            {
                obsPath = query.First();
            }
            return true;
        }

        public bool populatePluginLists()
        {
            if (pluginsPath == string.Empty) return false;
            listPlugins.Clear();
            listPluginsFull.Clear();

            foreach (string file in Directory.EnumerateFiles(pluginsPath))
            {
                string extension = file.Substring(file.Length - 3, 3);
                if (extension == "zip")
                {
                    // Validate plugin zips
                    if (validateZip(file))
                    {
                        string[] splitName = file.Split('\\');
                        string simpleName = splitName[splitName.Length - 1];
                        simpleName = simpleName.Substring(0, simpleName.Length - 4);
                        // Add validated zips
                        Plugin p = new()
                        {
                            Name = simpleName,
                            Installed = PluginInstallationType.NOT_INSTALLED
                        };
                        listPluginsFull.Add(p);
                    }
                }
            }

            var pluginQuery = dbHandler.Plugins.Where(p => p.OBSPathId == obsPath.OBSPathId);
            for (int i = 0; i < listPluginsFull.Count; i++)
            {
                foreach (DBPluginEntry pluginQueryEle in pluginQuery)
                {
                    if (listPluginsFull[i].Name == pluginQueryEle.Name)
                    {
                        listPluginsFull[i].dbEntry = pluginQueryEle;
                        listPluginsFull[i].Installed = listPluginsFull[i].dbEntry.IsInstalled ? PluginInstallationType.INSTALLED : PluginInstallationType.NOT_INSTALLED;
                        break;
                    }
                }
                if (listPluginsFull[i].dbEntry == null)
                {
                    listPluginsFull[i].dbEntry = new()
                    {
                        Name = listPluginsFull[i].Name
                    };
                }
                var temp = determinePluginState(listPluginsFull[i].Name);
                if (listPluginsFull[i].Installed == PluginInstallationType.INSTALLED)
                {
                    switch (temp)
                    {
                        case PluginInstallationStateType.NOT_INSTALLED:
                        case PluginInstallationStateType.FILES_PRESENT:
                            listPluginsFull[i].Installed = PluginInstallationType.INSTALLED_MODIFIED;
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    switch (temp)
                    {
                        case PluginInstallationStateType.FILES_PRESENT:
                            listPluginsFull[i].Installed = PluginInstallationType.FILES_PRESENT;
                            break;
                        case PluginInstallationStateType.INSTALLED:
                            listPluginsFull[i].Installed = PluginInstallationType.MANUALLY_INSTALLED;
                            break;
                        default:
                            break;
                    }
                }
                listPlugins.Add(listPluginsFull[i]);
            }
            return true;
        }

        private PluginInstallationStateType determinePluginState(string simpleName)
        {
            bool someFound = false;
            bool allMatching = true;
            using (ZipArchive zip = ZipFile.Open(pluginsPath + simpleName + ".zip", ZipArchiveMode.Read))
            {
                foreach (ZipArchiveEntry zipEntry in zip.Entries)
                {
                    if (zipEntry.FullName.Last() == '/') { continue; }
                    if (File.Exists(obsPath.Path + zipEntry.FullName))
                    {
                        someFound = true;
                        if (allMatching && zipEntry.Length != new FileInfo(obsPath.Path + zipEntry.FullName).Length)
                        {
                            allMatching = false;
                        }
                    }
                    else
                    {
                        allMatching = false;
                    }
                }
            }
            if (allMatching)
            {
                return PluginInstallationStateType.INSTALLED;
            }
            else if (someFound)
            {
                return PluginInstallationStateType.FILES_PRESENT;
            }
            else
            {
                return PluginInstallationStateType.NOT_INSTALLED;
            }
        }

        public PluginInstallationType getInstallStateOfPlugin(string name_)
        {
            foreach (Plugin p in listPlugins)
            {
                if (p.Name == name_)
                {
                    return p.Installed;
                }
            }
            return PluginInstallationType.NOT_INSTALLED;
        }

        public bool addPlugins(string name_)
        {
            try
            {
                if (obsPath.Path == string.Empty && pluginsPath == string.Empty) return false;
                foreach (Plugin p in listPlugins)
                {
                    if (p.Name == name_)
                    {
                        if (p.Installed == PluginInstallationType.FILES_PRESENT)
                        {
                            DialogResult dr = MessageBox.Show("Files from this plugin are present in the OBS directory, but they were not installed with the Plugin Manager.\nProbably a different version was installed manually in the OBS directory. Continuing might leave files from the previous manual installation in the OBS directory.\nDo you want to continue?", "Other version detected in OBS", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                            if (dr != DialogResult.Yes)
                            {
                                return true;
                            }
                        }
                    }
                }
                string name = $"{name_}.zip";
                using (ZipArchive zip = ZipFile.Open(pluginsPath + name, ZipArchiveMode.Read))
                {
                    foreach (ZipArchiveEntry zipEntry in zip.Entries)
                    {
                        string zipWin = zipEntry.FullName.Replace('/', '\\');

                        if (zipEntry.FullName.Last<char>() == '/')
                        {
                            if (!Directory.Exists(obsPath.Path + zipWin))
                            {
                                Directory.CreateDirectory(obsPath.Path + zipWin);
                            }
                        }
                        else
                        {
                            // benchmark split vs substring
                            string[] path = zipWin.Split('\\');
                            string justPath = string.Empty;
                            for (int i = 0; i < path.Length - 1; i++)
                            {
                                justPath += path[i] + '\\';
                            }
                            if (!Directory.Exists(obsPath.Path + justPath))
                            {
                                Directory.CreateDirectory(obsPath.Path + justPath);
                            }
                            zipEntry.ExtractToFile(obsPath.Path + zipWin, true);
                        }
                    }
                }
                Plugin plugin = new();
                for (int i = 0; i < listPlugins.Count; i++)
                {
                    if (listPlugins[i].Name == name_)
                    {
                        plugin = listPlugins[i];
                        plugin.Installed = PluginInstallationType.INSTALLED;
                        plugin.dbEntry.IsInstalled = true;
                        plugin.dbEntry.InstalledDate = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeMilliseconds();
                        plugin.dbEntry.OBSPathId = obsPath.OBSPathId;
                        break;
                    }
                }
                dbHandler.Add(plugin.dbEntry);
                dbHandler.SaveChanges();
            }
            catch (Exception e)
            {
                Debug.Write("IO Exception, while adding plugin " + name_ + e.ToString());
                return false;
            }
            return true;
        }

        public bool uninstallPlugin(string name_)
        {
            string name = $"{name_}.zip";

            foreach (Plugin p in listPlugins)
            {
                if (p.Name == name_)
                {
                    if (p.Installed == PluginInstallationType.NOT_INSTALLED)
                    {
                        // nothing to do
                        return true;
                    }
                }
            }


            using (ZipArchive zip = ZipFile.Open(pluginsPath + name, ZipArchiveMode.Read))
            {
                // Remove files
                foreach (ZipArchiveEntry zipEntry in zip.Entries)
                {
                    if (zipEntry.FullName.Last() != '/')
                    {
                        string zipWin = zipEntry.FullName.Replace('/', '\\');
                        if (File.Exists(obsPath.Path + zipWin))
                        {
                            try
                            {
                                File.Delete(obsPath.Path + zipWin);
                            }
                            catch (Exception e) when (e is IOException ||
                                                       e is UnauthorizedAccessException)
                            {
                                Debug.WriteLine($"Error deleting file {zipWin}: {e}");
                                return false;
                            }
                        }
                    }
                }
                // Remove empty directories in second iteration
                foreach (ZipArchiveEntry zipEntry in zip.Entries.Reverse())
                {
                    if (zipEntry.FullName.Last() == '/')
                    {
                        string dirpath = obsPath.Path + zipEntry.FullName;
                        if (Directory.Exists(dirpath) && Directory.GetFileSystemEntries(dirpath).Length == 0)
                        {
                            try
                            {
                                Directory.Delete(dirpath, true);
                            }
                            catch (IOException e)
                            {
                                Debug.WriteLine($"Error deleting directory {dirpath}: {e}");
                                return false;
                            }
                        }
                    }
                }
            }
            markPluginUninstalled(name_);

            return true;
        }

        public void markPluginUninstalled(string name_)
        {
            foreach (Plugin p in listPlugins)
            {
                if (p.Name == name_)
                {
                    p.Installed = PluginInstallationType.NOT_INSTALLED;
                    p.dbEntry.IsInstalled = false;
                    p.dbEntry.InstalledDate = 0;
                    var query = dbHandler.Plugins.Where(dbPlugin => dbPlugin.Name == name_ && dbPlugin.OBSPathId == obsPath.OBSPathId);
                    if (query.Count() > 0)
                    {
                        dbHandler.Plugins.Remove(p.dbEntry);
                        dbHandler.SaveChanges();
                    }
                    break;
                }
            }
        }

        public bool copyPluginZip(string file)
        {
            string extension = file.Substring(file.Length - 3, 3);
            if (extension == "zip")
            {
                int separatorPos = file.LastIndexOf('\\') + 1;
                string nameAndExtension = file.Substring(separatorPos, file.Length - separatorPos);
                // TODO Check valid plugin before copy
                try
                {
                    if (validateZip(file))
                        File.Copy(file, pluginsPath + nameAndExtension);
                    else
                        return false;
                }
                catch (IOException e)
                {
                    Debug.WriteLine($"Could not copy file {nameAndExtension} : " + e.ToString());
                    return false;
                }
            }
            else
            {
                return false;
            }
            return true;
        }

        // TODO, remove? Private, later
        public void vanityRemoval()
        {
            vanityCheck(obsPath.Path, 0);
        }

        private void vanityCheck(string path, int tabs)
        {
            string space = "";
            for (int i = 0; i < tabs; i++) { space += "   "; }
            // If no file && no dir, DELETE
            // else
            var files = Directory.EnumerateFiles(path);
            var directories = Directory.EnumerateDirectories(path);
            if (files.Count() == 0 && directories.Count() == 0)
            {
                Directory.Delete(path, false);
                Debug.WriteLine($"Vanity {path} DELETED!");
            }
            else
            {
                foreach (string dir in directories)
                {
                    Debug.WriteLine($"Vanity check dir: {space}{dir}");
                    vanityCheck(dir, tabs + 1);
                }
                var entries = Directory.EnumerateFileSystemEntries(path);
                if (entries.Count() == 0)
                {
                    Directory.Delete(path, false);
                    Debug.WriteLine($"Vanity {path} DELETED!");
                }
            }
        }

        public bool validateZip(string file)
        {
            using (ZipArchive zip = ZipFile.Open(file, ZipArchiveMode.Read))
            {
                foreach (ZipArchiveEntry zipEntry in zip.Entries)
                {
                    if (zipEntry.ToString().Contains("obs-plugins/")) return true;
                }
            }
            return false;
        }

        public void filterPlugins(string text)
        {
            listPlugins.Clear();
            text = text.ToLower();
            foreach (Plugin plugin in listPluginsFull)
            {
                if (plugin.Name.ToLower().Contains(text))
                {
                    listPlugins.Add(plugin);
                }
            }
        }

        public bool loadPaths()
        {
            string settings = "settings.tml";
            string toml = string.Empty;
            if (File.Exists(settings))
            {
                using (StreamReader sr = new StreamReader(settings))
                {
                    toml = sr.ReadToEnd();
                }
                var model = Toml.ToModel(toml);

                string obsPath_ = (string)model["obspath"];
                obsPath_ = obsPath_.Replace('/', '\\');

                setObsPath(obsPath_);

                pluginsPath = (string)model["pluginspath"];
                pluginsPath = pluginsPath.Replace('/', '\\');
                if (!System.IO.Directory.Exists(pluginsPath))
                {
                    pluginsPath = string.Empty;
                }
                return true;
            }
            return false;
        }

        public bool savePaths()
        {
            string settings = "settings.tml";
            string toml = $"obspath = \"{obsPath.Path.Replace('\\', '/')}\"\n";
            toml += $"pluginspath = \"{pluginsPath.Replace('\\', '/')}\"";

            using (StreamWriter sw = new StreamWriter(settings))
            {
                sw.Write(toml);
            }
            return true;
        }

        public bool validateObsPath(string obsPath)
        {
            bool exists = false;
            if (File.Exists(obsPath + @"bin\64bit\obs64.exe") || File.Exists(obsPath + @"bin\32bit\obs.exe")) exists = true;
            return exists;
        }

        public string getObsPath() => obsPath.Path;
    }
}

