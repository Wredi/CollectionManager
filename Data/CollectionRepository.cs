using System;
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

        public List<string> GetDefaultColumnNames()
        {
            return new List<string> { "Image", "Name", "Price", "Status", "Valuation" };
        }

        public void AddCollection(string collectionName)
        {

        }

        public void DeleteCollection(string collectionName)
        {

        }
    }
}
