using System;
using System.Collections.Generic;
using System.Text;
using DCRF.Core;
using System.IO;
using System.Reflection;
using System.Collections;
using DCRF.Interface;
using DCRF.Attributes;
using System.Reflection.Emit;

namespace BlockBroker
{
    /// <summary>
    /// This class is used to load object instances from assembly files (.dll) noting the referenced assemblies
    /// </summary>
    public class AssemblyLoader
    {
        private Dictionary<string, Assembly> assemblyCache = new Dictionary<string, Assembly>();

        public AssemblyLoader()
        {
            //since the Block may need this assembly, we should manually redirect this reference
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
        }

        /// <summary>
        /// Loads the Block from specified folder. Usually the folder of a Block is pre-specified
        /// in Block descriptior's ID but this method is also used to load Block assembly
        /// from its source folder when installing a Block
        /// </summary>
        /// <param name="Block"></param>
        /// <param name="folder"></param>
        /// <returns></returns>
        public IContainedBlock LoadBlock(string assemblyName, string className, string folder, object[] args)
        {
            string path = Path.Combine(folder, assemblyName);

            Assembly ass = loadAssembly(path, folder, assemblyName);

            object result = ass.CreateInstance(className,false, BindingFlags.Default, null, args, null, null);

            if (result == null)
            {
                //the assembly does not have the given class
                return null;
            }

            if (!(result is IContainedBlock))
            {
                throw new Exception("Invalid Block. Blocks' main classes must implement IBlock");
            }

            return result as IContainedBlock;
        }

        private Assembly loadAssembly(string path, string folder, string assemblyName)
        {
            if (!File.Exists(path))
            {
                return null;
            }

            if (!assemblyCache.ContainsKey(path))
            {
                //try to lookup in the currently loaded assemblies of the current appdomain
                foreach (Assembly item in AppDomain.CurrentDomain.GetAssemblies())
                {
                    //AB creates dynamic in-mem assembleis which have no location
                    if (item is AssemblyBuilder) continue;

                    if (item.Location == path)
                    {
                        assemblyCache[path] = item;
                    }
                }
            }

            if (!assemblyCache.ContainsKey(path))
            {
                //if not found, load it 
                try
                {
                    Assembly result = null;
                    try
                    {
                        byte[] data = File.ReadAllBytes(path);
                        result = Assembly.Load(data);
                    }
                    catch (FileLoadException exc)
                    {
                        //I replaced above two lines with below line when faced with
                        //"Unverifiable code failed policy check" exception while loading
                        //System.Data.SQLite.DLL
                        result = Assembly.LoadFrom(path);
                    }

                    assemblyCache[path] = result;

                    //for each reference of this assembly set path of the Block in app domain data
                    //so when resolving its reference the method can lookup there
                    foreach (AssemblyName an in result.GetReferencedAssemblies())
                    {
                        if (an.Name.ToLower() == "mscorlib" || an.Name.StartsWith("System."))
                        {
                            //these are in GAC
                            continue;
                        }

                        string uniqueName = an.FullName;

                        //store folder of the assembly we are loading so if it requires some dependent assemblies
                        //we will look into that folder first
                        AppDomain.CurrentDomain.SetData(uniqueName, folder);
                    }
                }
                catch
                {
                    return null;
                }
            } //end of if not in cache

            if (!assemblyCache.ContainsKey(path))
            {
                return null;
            }

            return assemblyCache[path];
        }

        public void ClearCache()
        {
            assemblyCache.Clear();
        }


        /// <summary>
        /// Returns a list of class names (including namespace) that are blocks (implement IBlock)
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="folder"></param>
        /// <returns></returns>
        public List<string> GetBlocks(string assemblyName, string folder)
        {
            List<string> result = new List<string>();

            string path = Path.Combine(folder, assemblyName);
            Assembly ass = loadAssembly(path, folder, assemblyName);

            if (ass == null)
            {
                //seems that this is not a .NET based dll
                return result;
            }

            Type[] types = ass.GetTypes();

            foreach (Type type in types)
            {
                if (type.IsAbstract || type.IsInterface) continue;

                Type[] interfaces = type.GetInterfaces();

                foreach (Type iface in interfaces)
                {
                    if (iface.Equals(typeof(IBlock)))
                    {
                        if (type.GetCustomAttributes(typeof(BlockHandleAttribute), true).Length > 0)
                        {
                            //we suppose that the name of the type is unique among types in the assembly
                            result.Add(type.Namespace + "." + type.Name);
                        }
                        break;
                    }
                }
            }

            return result;

        }

        private Hashtable libraries = null;
        private string libFolder = null;

        /// <summary>
        /// A key,value collection to indicate key=dll file, value=path to container assembly
        /// </summary>
        /// <param name="files"></param>
        public void SetGeneralLibrariesPath(Hashtable files, string libFolder)
        {
            libraries = files;
            this.libFolder = libFolder;
        }

        /// <summary>
        /// Method to handle reference resolution event. This methods uses libraries variable and folder stored in appdomain
        /// data (given when calling Load method) to find the referenced assembly.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string name = args.Name;

            if (libraries != null)
            {
                string fileName = name.Split(',')[0];
                object filePath = libraries[fileName];

                if (filePath != null)
                {
                    //TODO: test this code
                    return loadAssembly(filePath.ToString(), Path.GetDirectoryName(filePath.ToString()), name);
                }
            }

            if (libFolder != null)
            {
                string fileName = name.Split(',')[0]+".dll";
                string filePath = Path.Combine(libFolder, fileName);

                if (File.Exists(filePath))
                {
                    return loadAssembly(filePath, Path.GetDirectoryName(filePath), name);
                }
            }

            //find origianl folder of the requesting assembly
            object data = AppDomain.CurrentDomain.GetData(name);

            if (data != null)
            {
                string fileName = name.Split(',')[0] + ".dll";

                return loadAssembly(Path.Combine(data.ToString(), fileName), data.ToString(), name);
            }

            return null;
        }
    }
}
