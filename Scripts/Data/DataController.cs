using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

namespace Hykudoru
{
    [Serializable]
    public abstract class PersistantData<T>
    {
        public abstract T GetData();
    }

    public interface ILocalResource
    {
        string FolderName { get; set; }
        string FileName { get; set; }
        string FileExtension { get; set; }
        string GetPath();
    }

    public class LocalResource : ILocalResource
    {
        static readonly string root;
        public string FolderName { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }

        static LocalResource()
        {
            root = Application.persistentDataPath;
        }

        public LocalResource(string folderName, string fileName, string fileExtension)
        {
            FolderName = folderName;
            FileName = fileName;
            FileExtension = fileExtension;
        }

        public string GetPath()
        {
            return Path.Combine(Path.Combine(root, FolderName), FileName + FileExtension);
        }
    }

    public abstract class DataSerializer
    {
        public abstract void Save<T>(T data, string path);
        public abstract T Load<T>(string path);
        public abstract void Delete(string path);
    }

    public class JsonSerializer : DataSerializer
    {
        public override void Save<T>(T data, string path)
        {
            Debug.Log("JSON SAVE");
        }

        public override T Load<T>(string path)
        {
            Debug.Log("JSON LOAD");
            return (T)new object();
        }

        public override void Delete(string path)
        {
            Debug.Log("JSON DELETE");
        }
    }

    public class BinarySerializer : DataSerializer
    {
        static BinaryFormatter binaryFormatter;

        static BinarySerializer()
        {
            binaryFormatter = new BinaryFormatter();
        }

        public BinarySerializer() { }

        public override void Save<T>(T data, string path)
        {
            Debug.Log("BINARY SAVE");

            using (FileStream fileStream = File.Open(path, FileMode.OpenOrCreate))
            {
                binaryFormatter.Serialize(fileStream, data);
            }
        }

        public override T Load<T>(string path)
        {
            Debug.Log("BINARY LOAD");
            using (FileStream fileStream = File.Open(path, FileMode.Open))
            {
                return (T)binaryFormatter.Deserialize(fileStream);
            }
        }

        public override void Delete(string path)
        {
            Debug.Log("BINARY DELETE");
        }
    }

    public static class DataController
    {
        private static DataSerializer Serializer { get; set; }
        static StringBuilder stringBuilder;

        static DataController()
        {
            Serializer = new BinarySerializer();
            stringBuilder = new StringBuilder();
        }

        public static void Save<T>(T data, string folderName, string file)
        {
            if (data != null && TypeSerializable<T>())
            {
                string folderPath = Path.Combine(Application.persistentDataPath, folderName);
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                    Debug.Log("Created Folder " + folderName);
                }

                string path = Path.Combine(folderPath, file);

                Serializer.Save(data, path);
            }
        }

        public static void Save<T>(T data, ILocalResource resource)
        {
            Save<T>(data, resource.FolderName, resource.FileName + resource.FileExtension);
        }

        public static T Load<T>(string filePath)
        {
            object obj = null;
            
            if (File.Exists(filePath))
            {
                obj = Serializer.Load<T>(filePath);
            }

            return (T)obj;
        }

        public static T Load<T>(ILocalResource resource)
        {
            return Load<T>(Path.Combine(Path.Combine(Application.persistentDataPath, resource.FolderName), resource.FileName + resource.FileExtension));
        }

        public static void Delete(string folderName, int fileIndex)
        {
            //serializer.Delete(path);
        }

        public static bool TypeSerializable(Type type)
        {
            Debug.Log("Serializable(Type type)");
            return type.IsDefined(typeof(SerializableAttribute), false);
        }

        public static bool TypeSerializable<T>()
        {
            Debug.Log("Serializable<T>()");
            return typeof(T).IsDefined(typeof(SerializableAttribute), false);
        }

        public static bool TypeSerializable<T>(T any) where T : new()
        {
            if (any == null)
            {
                any = new T();
            }

            Debug.Log("Serializable<T>(T any)");
            return any.GetType().IsDefined(typeof(SerializableAttribute), false);
        }
    }
}