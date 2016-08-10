namespace DeckOMatic
{
    using System;
    using HearthDb;
    using Newtonsoft.Json;

    public static class Serialization
    {
        /// <summary>
        /// Get a JSON serializer
        /// </summary>
        /// <param name="useCardNames">True to replace card IDs with card names</param>
        public static JsonSerializer GetSerializer(bool useCardNames)
        {
            var serializer = new JsonSerializer();

            if (useCardNames)
            {
                serializer.Converters.Add(new CardIdJsonConverter());
            }

            return serializer;
        }

        /// <summary>
        /// Converter that changes card IDs to card names
        /// </summary>
        private class CardIdJsonConverter : JsonConverter
        {
            private JsonSerializer defaultSerializer = new JsonSerializer();

            /// <summary>
            /// Determines whether this instance can convert the specified object type
            /// </summary>
            public override bool CanConvert(Type objectType)
            {
                return (objectType == typeof(string));
            }

            /// <summary>
            /// Reads a JSON representation of the object
            /// </summary>
            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Writes a JSON representation of the object
            /// </summary>
            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                string stringValue = (string)value;
                if (Cards.All.ContainsKey(stringValue))
                {
                    stringValue = Cards.All[stringValue].Name;
                }

                this.defaultSerializer.Serialize(writer, stringValue);
            }
        }
    }
}
