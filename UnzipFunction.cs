using System;
using System.IO;
using System.IO.Compression;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

public static class UnzipFunction
{
    [FunctionName("UnzipFunction")]
    public static void Run([BlobTrigger("your-container/{name}", Connection = "AzureWebJobsStorage")] Stream myBlob, string name, ILogger log)
    {
        log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");

        try
        {
            // Define the directory where you want to extract the files
            string extractPath = Path.Combine(Path.GetTempPath(), "extracted");

            // Create the directory if it doesn't exist
            if (!Directory.Exists(extractPath))
            {
                Directory.CreateDirectory(extractPath);
            }

            // Extract the zip file
            using (ZipArchive archive = new ZipArchive(myBlob, ZipArchiveMode.Read))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    // Extract each file to the specified directory
                    string filePath = Path.Combine(extractPath, entry.FullName);
                    entry.ExtractToFile(filePath);
                }
            }

            log.LogInformation("Files extracted successfully.");
        }
        catch (Exception ex)
        {
            log.LogError($"Error occurred: {ex.Message}");
        }
    }
}
