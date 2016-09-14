using MongoDriver.Helper;
using System.Threading.Tasks;
using System;
using MongoDB.Bson;
using MongoDB.Driver.GridFS;
using System.Collections.Generic;

namespace MongoDriver.Document
{   
    public class Documents 
    {
        public static async Task<bool> UploadFile(string fileName, string mimeType, string tableName, string patientId)
        {
           return await DocumentHelper.UploadFile(fileName, mimeType, tableName, patientId);
        }

        public static async Task<byte[]> DownloadFile(string fileName, string tableName)
        {
            return await DocumentHelper.DownloadFile(fileName, tableName);
        }

        public static async Task<bool> DeleteFile(string fileName)
        {
            var objectId = await DocumentHelper.FindFiles(fileName);

            return await DocumentHelper.DeleteFile(objectId);
        }

        public static async void DeleteAllFile()
        {
            DocumentHelper.DropBucket();         
        }

        public static async Task<bool> FindFiles(string fileName)
        {
            var objectId = await DocumentHelper.FindFiles(fileName);

            return true;
        }

        public static async Task<List<GridFSFileInfo>> FindAllFiles()
        {
            var fileInfo = await DocumentHelper.FindAllFiles();

            return fileInfo;
        }

        public static async Task<ObjectId> FindFiles(string fileName, DateTime? startTime, DateTime? endTime)
        {
            return await DocumentHelper.FindFiles(fileName, startTime, endTime);
        }

        public static async Task<bool> RenameFile(string oldFileName, string newFileName)
        {
            var objectId = await DocumentHelper.FindFiles(oldFileName);

            if (objectId.Pid == 0) return false;

            return await DocumentHelper.RenameFile(objectId, newFileName);
        }

        public static async Task<bool> RenameAllFiles(string oldFileName, string newFileName)
        {
            return await DocumentHelper.RenameAllFiles(oldFileName, newFileName);
        }
    }
}
