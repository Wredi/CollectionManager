using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Compression;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
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

        public bool CheckDuplicates(string collectionName, string nameToCheck, string nameToSkip)
        {
            Collection coll = LoadCollection(collectionName);
            return CheckDuplicates(coll.Items.Select(s => s.GetName() == nameToSkip ? null : s.GetName()), new List<string> { nameToCheck });
        }

        public bool CheckDuplicates(IEnumerable<Item> names, IEnumerable<Item> namesToCheck)
        {
            return CheckDuplicates(names.Select(s => s.GetName()), namesToCheck.Select(s => s.GetName()));
        }

        public bool CheckDuplicates(IEnumerable<string> names, IEnumerable<string> namesToCheck)
        {
            foreach (var name in names)
            {
                foreach (var item in namesToCheck)
                {
                    if (item == name)
                    {
                        return true;
                    }
                }
            }
            return false;
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
            foreach (var i in Directory.EnumerateFiles(_filesDirectory, $"{collectionName}.image.*"))
            {
                File.Delete(i);
            }
            File.Delete(FilepathFromCollection(collectionName));
        }

        public void ZipCollection(Stream stream, string collectionName)
        {
            string[] imageFiles = Directory.GetFiles(_filesDirectory, $"{collectionName}.image.*");

            using (ZipArchive zip = new ZipArchive(stream, ZipArchiveMode.Create, true))
            {
                zip.CreateEntryFromFile(FilepathFromCollection(collectionName), $"{collectionName}.collection.txt");

                foreach (string imageFile in imageFiles)
                {
                    zip.CreateEntryFromFile(imageFile, Path.GetFileName(imageFile));
                }
            }
        }

        public string UnzipAndMergeCollection(string filePath, string dstCollection)
        {
            string dstCollectionPath = FilepathFromCollection(dstCollection);
            using (ZipArchive zip = ZipFile.Open(filePath, ZipArchiveMode.Read))
            {
                foreach (ZipArchiveEntry entry in zip.Entries)
                {
                    if (entry.FullName.EndsWith(".collection.txt", StringComparison.OrdinalIgnoreCase))
                    {
                        using (StreamReader reader = new StreamReader(entry.Open()))
                        {
                            using (StreamReader readerDst = new StreamReader(dstCollectionPath))
                            {
                                string importedColumns = reader.ReadLine();
                                string importedCollectionToAppend = reader.ReadToEnd();

                                Collection currColl = Collection.FromText("", readerDst.ReadToEnd());
                                Collection collToCheck = Collection.FromText("", importedColumns + importedCollectionToAppend);

                                if(CheckDuplicates(currColl.Items, collToCheck.Items))
                                {
                                    return "There are duplicated names in the imported collection";
                                }
                                if (currColl.Columns.SequenceEqual(collToCheck.Columns))
                                {
                                    return "Column layout is different in imported collection";
                                }
                                File.AppendAllText(dstCollectionPath, importedCollectionToAppend);
                            }
                        }
                    }
                    else if (entry.FullName.Contains(".image."))
                    {
                        string newImageName = $"{dstCollection}{entry.FullName.Substring(entry.FullName.IndexOf('.'))}";
                        entry.ExtractToFile(Path.Combine(_filesDirectory, newImageName), true);
                    }
                }
            }
            return string.Empty;
        }
    }
}
