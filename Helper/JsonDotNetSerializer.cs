using MongoDB.Bson;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MongoDriver.Helper
{
    internal class JsonDotNetSerializer
    {
        private readonly JsonSerializer _jsonSerializer;

        public JsonDotNetSerializer()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new EmptyDateToMinDateConverter());
            _jsonSerializer = JsonSerializer.CreateDefault(settings);
        }

        /// <summary>
        /// Root property value; only required if trying to access nested information or an array is hanging off a property
        /// </summary>
        internal string RootProperty { get; set; }

        internal T Deserialize<T>(string data)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                return default(T);
            }

            JToken o = JToken.Parse(data);
            return Deserialize<T>(o);
        }      

        internal T Deserialize<T>(JToken token)
        {
            if (token == null)
            {
                return default(T);
            }

            T result = string.IsNullOrWhiteSpace(RootProperty) ? token.ToObject<T>(_jsonSerializer) : token[RootProperty].ToObject<T>(_jsonSerializer);
            return result;
        }
    }

    internal class EmptyDateToMinDateConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            DateTime date = (DateTime)value;
            if (date == DateTime.MinValue)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteValue(date);
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (CanConvert(objectType))
            {
                if (string.IsNullOrWhiteSpace(reader.Value.ToString()))
                {
                    return DateTime.MinValue;
                }

                return DateTime.Parse(reader.Value.ToString());
            }
            return null;
        }

        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(DateTime));
        }
    }
}