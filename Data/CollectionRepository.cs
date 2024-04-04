using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

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

        public void AddCollection(Models.Collection collection)
        {
            File.WriteAllText(
                    FilepathFromCollection(collection.Name),
                    collection.ToText()
                );
        }

        public void DeleteCollection(string collectionName)
        {
            File.Delete(FilepathFromCollection(collectionName));
        }
    }
}
