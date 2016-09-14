using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDriver.Helper
{
    public class DocumentHelper
    {        
        public static async Task<bool> UploadFile(string fileName, string mimeType, string tableName, string patientId)
        {
            var bytes = File.ReadAllBytes(fileName);

            var options = new GridFSUploadOptions
            {
                ChunkSizeBytes = 64512,
                ContentType = mimeType,
                Metadata = new BsonDocument
                {
                    { "contentType", mimeType},
                    {"resolution", "1080P" },
                    { "copyrighted", false},
                    { "patientid", patientId}
                }
            };

            var id = await CommonHelper.MongoGridFs.UploadFromBytesAsync(fileName, bytes, options);

            return true;
        }

        public static async Task<byte[]> DownloadFile(string fileName, string tableName)
        {
            var fileBytes = await CommonHelper.MongoGridFs.DownloadAsBytesByNameAsync(fileName);

            return fileBytes;
        }

        public static async Task<List<GridFSFileInfo>> FindAllFiles(int limit = 100, bool sortAscending = true)
        {
            var filter = Builders<GridFSFileInfo>.Filter.Empty;

            var sort = Builders<GridFSFileInfo>.Sort.Ascending(x => x.UploadDateTime);

            if (!sortAscending)
            {
                sort = Builders<GridFSFileInfo>.Sort.Descending(x => x.UploadDateTime);
            }

            var options = new GridFSFindOptions
            {
                Limit = limit,
                Sort = sort
            };

            var fileInfoList = new List<GridFSFileInfo>();

            var result = await CommonHelper.MongoGridFs.Find(filter, options).ToListAsync();

            foreach (var value in result)
            {
                fileInfoList.Add(value);

            }

            return fileInfoList;
        }

        public static async Task<ObjectId> FindFiles(string fileName, DateTime? startDate, DateTime? endDate, int limit = 100)
        {
            var filter = Builders<GridFSFileInfo>.Filter.And(
                Builders<GridFSFileInfo>.Filter.Eq(x => x.Filename, fileName),
                Builders<GridFSFileInfo>.Filter.Gte(x => x.UploadDateTime, startDate.Value),
                Builders<GridFSFileInfo>.Filter.Lt(x => x.UploadDateTime, endDate.Value));
            var sort = Builders<GridFSFileInfo>.Sort.Descending(x => x.UploadDateTime);
            var options = new GridFSFindOptions
            {
                Limit = limit,
                Sort = sort
            };

            GridFSFileInfo fileInfo = null;

            using (var cursor = await CommonHelper.MongoGridFs.FindAsync(filter, options))
            {
                fileInfo = (await cursor.ToListAsync()).FirstOrDefault();
            }

            return fileInfo.Id;
        }

        public static async Task<ObjectId> FindFilesFrom(string fileName, DateTime? startDate, int limit = 100)
        {
            var filter = Builders<GridFSFileInfo>.Filter.And(Builders<GridFSFileInfo>.Filter.Eq(x => x.Filename, fileName),
                Builders<GridFSFileInfo>.Filter.Gte(x => x.UploadDateTime, startDate.Value));

            var sort = Builders<GridFSFileInfo>.Sort.Descending(x => x.UploadDateTime);
            var options = new GridFSFindOptions
            {
                Limit = limit,
                Sort = sort
            };

            GridFSFileInfo fileInfo = null;

            using (var cursor = await CommonHelper.MongoGridFs.FindAsync(filter, options))
            {
                fileInfo = (await cursor.ToListAsync()).FirstOrDefault();
            }

            return fileInfo.Id;
        }

        public static async Task<ObjectId> FindFiles(string fileName, int limit = 100)
        {
            var filter = Builders<GridFSFileInfo>.Filter.And(Builders<GridFSFileInfo>.Filter.Eq(x => x.Filename, fileName));
            var sort = Builders<GridFSFileInfo>.Sort.Descending(x => x.UploadDateTime);
            var options = new GridFSFindOptions
            {
                Limit = limit,
                Sort = sort
            };

            GridFSFileInfo fileInfo = null;

            using (var cursor = await CommonHelper.MongoGridFs.FindAsync(filter, options))
            {
                fileInfo = (await cursor.ToListAsync()).FirstOrDefault();
            }

            if (fileInfo == null) return new ObjectId();

            return fileInfo.Id;
        }

        public static async Task<ObjectId> FindFilesBefore(string fileName, DateTime? endDate, int limit = 100)
        {
            var filter = Builders<GridFSFileInfo>.Filter.And(Builders<GridFSFileInfo>.Filter.Eq(x => x.Filename, fileName),
                Builders<GridFSFileInfo>.Filter.Lt(x => x.UploadDateTime, endDate.Value));

            var sort = Builders<GridFSFileInfo>.Sort.Descending(x => x.UploadDateTime);
            var options = new GridFSFindOptions
            {
                Limit = limit,
                Sort = sort
            };

            GridFSFileInfo fileInfo = null;

            using (var cursor = await CommonHelper.MongoGridFs.FindAsync(filter, options))
            {
                fileInfo = (await cursor.ToListAsync()).FirstOrDefault();
            }

            return fileInfo.Id;
        }

        public static async Task<bool> DeleteFile(ObjectId id)
        {
            await CommonHelper.MongoGridFs.DeleteAsync(id);

            return true;
        }

        public static async void DropBucket()
        {
            await CommonHelper.MongoGridFs.DropAsync();
        }

        public static async Task<bool> RenameFile(ObjectId id, string newFileName)
        {
            //Call find file for getting the ID first
            await CommonHelper.MongoGridFs.RenameAsync(id, newFileName);

            return true;
        }

        public static async Task<bool> RenameAllFiles(string oldFileName, string newFileName)
        {
            var filter = Builders<GridFSFileInfo>.Filter.Eq(x => x.Filename, oldFileName);

            var filesCursor = await CommonHelper.MongoGridFs.FindAsync(filter);
            var files = await filesCursor.ToListAsync();

            foreach (var file in files)
            {
                await CommonHelper.MongoGridFs.RenameAsync(file.Id, newFileName);
            }

            return true;
        }        
    }
}
