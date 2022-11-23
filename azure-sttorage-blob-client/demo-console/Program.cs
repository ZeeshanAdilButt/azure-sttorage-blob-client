using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure; // Namespace for CloudConfigurationManager
using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.Blob; // Namespace for Table storage types
using System.Diagnostics;
using System.IO;
using Microsoft.WindowsAzure.Storage.Auth;

namespace _404NotFound
{
    class Program
    {
        static void Main(string[] args)
        {
            
            // Parse the connection string and return a reference to the storage account.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString"));

            var path = "{file path here}";

            try
            {
                CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference("{containername}");

                using (FileStream fs = new FileStream(path,
                        FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite,
                        4096, FileOptions.RandomAccess | FileOptions.None))
                {
                    Console.WriteLine("Temp file = {0}", fs.Name);
                    Console.WriteLine("Uploading to Blob storage as blob '{0}'", Path.GetFileName(path));
                    var finalFileName = Path.GetFileName(path);

                    CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(finalFileName);

                    cloudBlockBlob.UploadFromStream(fs);
                    Console.WriteLine("Upload completed on {0} UTC", DateTime.Now.ToUniversalTime().ToString());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message.ToString());
                Console.WriteLine("TimeStamp: {0} UTC", DateTime.Now.ToUniversalTime().ToString());
            }
        }
    }
}
