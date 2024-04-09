using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using CollectionManager.Models;

namespace CollectionManager.Data
{
    public class CollectionRepository
    {
        string _filesDirectory;

        public CollectionRepository(string filesDirectory)
        {
            _filesDirectory = filesDirectory;
        }

        public string FilepathFromCollection(string collectionName)
        {
            return Path.Combine(_filesDirectory, $"{collectionName}.collection.txt");
        }

        public IEnumerable<string> GetCollectionNames()
        {
            return Directory
                    .EnumerateFiles(_filesDirectory, "*.collection.txt")
                    .Select(Path.GetFileName)
                    .Select(filename => filename.Substring(0, filename.IndexOf('.')));
        }

        public void SaveCollection(Collection collection)
        {
            File.WriteAllText(
                    FilepathFromCollection(collection.Name),
                    collection.ToText()
                );
        }

        public Collection LoadCollection(string collectionName)
        {
            return Collection.FromText(collectionName, File.ReadAllText(FilepathFromCollection(collectionName)));
        }

        public void AddItem(string collectionName, Models.Item item)
        {
            File.AppendAllText(FilepathFromCollection(collectionName), item.ToText() + Environment.NewLine);
        }

        public string GetImagePath(string collectionName, string itemImagePath)
        {
            return Path.Combine(_filesDirectory, $"{collectionName}.image.{itemImagePath}");
        }

        public string SaveImageFromFile(string collectionName, string filePath)
        {
            string itemImagePath = Guid.NewGuid().ToString() + Path.GetExtension(filePath);
            File.Copy(filePath, GetImagePath(collectionName, itemImagePath));
            return itemImagePath;
        }

        public void EditItem(string collectionName, string itemNameToEdit, Item newItem)
        {
            Collection collection = LoadCollection(collectionName);
            collection.Items = collection.Items.Select(x => x.GetName() == itemNameToEdit ? newItem : x).ToList();
            SaveCollection(collection);
        }

        public void RenameCollection(string collectionName, string newName)
        {
            File.Move(FilepathFromCollection(collectionName), FilepathFromCollection(newName));
            foreach(var i in Directory.EnumerateFiles(_filesDirectory, $"{collectionName}.image.*"))
            {
                string fileName = Path.GetFileName(i);
                string directoryPath = Path.GetDirectoryName(i);
                int dotIndex = fileName.IndexOf('.');
                string newFileName = newName + fileName.Substring(dotIndex);
                string newFilePath = Path.Combine(directoryPath, newFileName);
                File.Move(i, newFilePath);
            }
        }

        public void RemoveItemImage(string collectionName, Models.Item item)
        {
            string imagePath = GetImagePath(collectionName, item.Values[0]);
            File.Delete(imagePath);
        }

        public void DeleteCollection(string collectionName)
        {
            File.Delete(FilepathFromCollection(collectionName));
        }
    }
}
