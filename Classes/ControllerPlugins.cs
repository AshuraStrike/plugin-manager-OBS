using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Reflection.Metadata.Ecma335;
using Microsoft.EntityFrameworkCore;
using PluginManagerObs.Models;
using Tomlyn;

namespace PluginManagerObs.Classes
{
    internal class ControllerPlugins
    {
        private OBSPath obsPath;
        public string pluginsPath;
        public List<Plugin> listPlugins;

        private List<Plugin> listPluginsFull;

        private PluginsContext dbHandler;
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
            if(query.Count() == 0 )
            {
                obsPath.Path = obsPath_;
                dbHandler.OBSPaths.Add(obsPath);
                dbHandler.SaveChanges();
            }
            else
            {
                obsPath = query.First();
            }
            return true;
        }

        public bool populatePlugins()
        {
            if (pluginsPath == string.Empty) return false;
            listPlugins.Clear();
            listPluginsFull.Clear();

            // Validate plugin zips
            foreach (string file in Directory.EnumerateFiles(pluginsPath))
            {
                bool data = false, plugins = false;
                using (ZipArchive zip = ZipFile.Open(file, ZipArchiveMode.Read))
                {
                    foreach (ZipArchiveEntry zipEntry in zip.Entries)
                    {
                        if (zipEntry.ToString().Contains("data/")) data = true;
                        if (zipEntry.ToString().Contains("obs-plugins/")) plugins = true;
                    }
                }
                if (data && plugins)
                {
                    string[] splitName = file.Split('\\');
                    string simpleName = splitName[splitName.Length - 1];
                    simpleName = simpleName.Substring(0, simpleName.Length - 4);
                    // Add validated zips
                    Plugin p = new()
                    {
                        Name = simpleName
                    };
                    //listPlugins.Add(p);
                    listPluginsFull.Add(p);
                }
            }

            var query = dbHandler.Plugins.Where(p => p.OBSPathId == obsPath.OBSPathId);
            for (int i = 0;i<listPluginsFull.Count;i++)
            {
                foreach (Plugin plu in query)
                {
                    if (listPluginsFull[i].Name == plu.Name)
                    {
                        /*plug.PluginId = plu.PluginId;
                        plug.IsInstalled = plu.IsInstalled;
                        plug.InstalledDate = plu.InstalledDate;
                        plug.OBSPath = plu.OBSPath;*/
                        listPluginsFull[i] = plu;
                        break;
                    }
                }
                listPlugins.Add(listPluginsFull[i]);
            }
            return true;
        }

        public bool addPlugins(string name_)
        {
            try
            {
                // Check if already exists
                if (obsPath.Path == string.Empty && pluginsPath == string.Empty) return false;
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
                            zipEntry.ExtractToFile(obsPath.Path + zipWin);
                        }
                    }
                }
                Plugin plugin = new();
                for (int i=0; i<listPlugins.Count; i++)
                {
                    if (listPlugins[i].Name == name_)
                    {
                        plugin = listPlugins[i];
                        plugin.IsInstalled = true;
                        plugin.IsInstalled = true;
                        plugin.InstalledDate = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeMilliseconds();
                        plugin.OBSPathId = obsPath.OBSPathId;
                        break;
                    }
                }
                dbHandler.Add(plugin);
                dbHandler.SaveChanges();
            }catch (Exception e)
            {
                Debug.Write("IO Exception, while adding plugin "+ name_ + e.ToString());
                return false;
            }
            return true;
        }

        public bool uninstallPlugin(string name_)
        {
            // Exception ret false
            string name = $"{name_}.zip";
            string pluginFolder = string.Empty;
            string dpPath = "data/obs-plugins/";

            using (ZipArchive zip = ZipFile.Open(pluginsPath + name, ZipArchiveMode.Read))
            {
                foreach (ZipArchiveEntry zipEntry in zip.Entries)
                {
                    int len = zipEntry.FullName.Length-1;
                    string fullName = zipEntry.FullName;
                    int end=0;

                    if (zipEntry.FullName.Last() != '/')
                    {
                        if (pluginFolder == string.Empty && fullName.Contains(dpPath) && len > 16)
                        {
                            for(int i = 18; i < len; i++)
                            {
                                if (fullName[i] == '/')
                                {
                                    end = i;
                                    break;
                                }
                            }
                            string ss = fullName.Substring(17, end - 17);
                            Debug.WriteLine($"Substring plugin name: {ss}");
                            pluginFolder = ss;
                        }
                        string zipWin = zipEntry.FullName.Replace('/', '\\');
                        if (File.Exists(obsPath.Path + zipWin))
                            File.Delete(obsPath.Path + zipWin);
                    }
                }
                if (Directory.Exists(obsPath.Path + dpPath + pluginFolder))
                    Directory.Delete(obsPath.Path + dpPath+ pluginFolder,true);
            }
            foreach (Plugin p in listPlugins)
            {
                if (p.Name == name_)
                {
                    p.IsInstalled = false;
                    p.InstalledDate = 0;
                    var query = dbHandler.Plugins.Where(plugin => plugin.Name == name_ && plugin.OBSPathId == obsPath.OBSPathId);
                    if (query.Count() > 0)
                    {
                        dbHandler.Plugins.Remove(p);
                        dbHandler.SaveChanges();
                    }
                    break;
                }
            }

            return true;
        }

        // Private, later
        public void vanityRemoval()
        {
            vanityCheck(obsPath.Path,0);
        }

        private void vanityCheck(string path,int tabs)
        {
            string space = "";
            for(int i = 0; i < tabs; i++) { space += "   "; }
            // If no file && no dir, DELETE
            // else
            var files = Directory.EnumerateFiles(path);
            var directories = Directory.EnumerateDirectories(path);
            if (files.Count() == 0 && directories.Count() == 0)
            {
                Directory.Delete(path,false);
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
                if(entries.Count() == 0)
                {
                    Directory.Delete(path, false);
                    Debug.WriteLine($"Vanity {path} DELETED!");
                }
            }
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
            if(File.Exists(obsPath + @"bin\64bit\obs64.exe") || File.Exists(obsPath + @"bin\32bit\obs.exe")) exists = true;
            return exists;
        }

        public string getObsPath() => obsPath.Path;
    }
}

