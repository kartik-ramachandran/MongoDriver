using MongoDriver.Helper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MongoDriver.Action
{
    public class Update
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="Tkey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="fields"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static async Task<bool> MultipleDocument<T, Tkey, TValue>
            (Dictionary<Tkey, TValue> fields, string field, T value, string tableName)
        {
            return await UpdateHelper.UpdateMultipleDocument(fields, field, value, tableName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TFilter"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="filterField"></param>
        /// <param name="filterValue"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static async Task<bool> SingleDocument<TFilter, TValue>(string filterField, 
            TFilter filterValue, 
            string field, 
            TValue value,             
            string tableName)
        {
            return await UpdateHelper.UpdateSingleDocument(filterField, filterValue, field, value, tableName);
        }

        public static bool SingleDocumentSync<TFilter, TValue>(string filterField,
          TFilter filterValue,
          string field,
          TValue value,
          string tableName)
        {
            return UpdateHelper.UpdateSingleDocumentSync(filterField, filterValue, field, value, tableName);
        }
    }
}
