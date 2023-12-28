using System.Diagnostics;
using System.IO.Compression;
using Tomlyn;

namespace PluginManagerObs.Classes
{
    internal class ControllerPlugins
    {
        public string obsPath;
        public string pluginsPath;
        public List<Plugin> listPlugins;

        private List<Plugin> listPluginsFull;

        private PluginsContext dbHandler;
        public ControllerPlugins()
        {
            obsPath = string.Empty;
            pluginsPath = string.Empty;

            listPlugins = new();
            listPluginsFull = new();

            dbHandler = new();
            dbHandler.Database.EnsureCreatedAsync().Wait();
        }

        public bool populatePlugins()
        {
            if (pluginsPath == string.Empty) return false;
            listPlugins.Clear();
            listPluginsFull.Clear();
            foreach (string file in Directory.EnumerateFiles(pluginsPath))
            {
                // Validate plugin zips
                string[] splitName = file.Split('\\');
                string simpleName = splitName[splitName.Length - 1];
                simpleName = simpleName.Substring(0, simpleName.Length - 4);
                // Add validated zips
                Plugin p = new() {
                    Name = simpleName
                };
                listPlugins.Add(p);
                listPluginsFull.Add(p);
            }
            return true;
        }

        public bool addPlugins(string name_)
        {
            try
            {
                // Check if already exists
                if (obsPath == string.Empty && pluginsPath == string.Empty) return false;
                string name = $"{name_}.zip";
                using (ZipArchive zip = ZipFile.Open(pluginsPath + name, ZipArchiveMode.Read))
                {
                    foreach (ZipArchiveEntry zipEntry in zip.Entries)
                    {
                        string zipWin = zipEntry.FullName.Replace('/', '\\');

                        if (zipEntry.FullName.Last<char>() == '/')
                        {
                            if (!Directory.Exists(obsPath + zipWin))
                            {
                                Directory.CreateDirectory(obsPath + zipWin);
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
                            if (!Directory.Exists(obsPath + justPath))
                            {
                                Directory.CreateDirectory(obsPath + justPath);
                            }
                            zipEntry.ExtractToFile(obsPath + zipWin);
                        }
                    }
                }
                foreach (Plugin p in listPlugins)
                {
                    if (p.Name == name_)
                    {
                        p.IsInstalled = true;
                        break;
                    }
                }
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
                            Debug.WriteLine(ss);
                            pluginFolder = ss;
                        }
                        string zipWin = zipEntry.FullName.Replace('/', '\\');
                        File.Delete(obsPath + zipWin);
                    }
                }
                Directory.Delete(obsPath +dpPath+ pluginFolder,true);
            }
            foreach (Plugin p in listPlugins)
            {
                if (p.Name == name_)
                {
                    p.IsInstalled = false;
                    break;
                }
            }
            return true;
        }

        // Private, later
        public void vanityRemoval()
        {
            vanityCheck(obsPath,0);
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
                Debug.WriteLine($"{path} DELETED!");
            }
            else
            {
                foreach (string dir in directories)
                {
                    Debug.WriteLine($"{space}{dir}");
                    vanityCheck(dir, tabs + 1);
                }
                var entries = Directory.EnumerateFileSystemEntries(path);
                if(entries.Count() == 0)
                {
                    Directory.Delete(path, false);
                    Debug.WriteLine($"{path} DELETED!");
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

                obsPath = (string)model["obspath"];
                obsPath = obsPath.Replace('/', '\\');
                pluginsPath = (string)model["pluginspath"];
                pluginsPath = pluginsPath.Replace('/', '\\');
                return true;
            }
            return false;
        }

        public bool savePaths()
        {
            string settings = "settings.tml";
            string toml = $"obspath = \"{obsPath.Replace('\\', '/')}\"\n";
            toml += $"pluginspath = \"{pluginsPath.Replace('\\', '/')}\"";

            using (StreamWriter sw = new StreamWriter(settings))
            {
                sw.Write(toml);
            }
            return true;
        }
    }
}

