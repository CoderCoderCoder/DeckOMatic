namespace DeckOMatic
{
    using System;
    using System.Collections.Generic;
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
                serializer.Converters.Add(new CardSetJsonConverter());
            }

            return serializer;
        }

        /// <summary>
        /// Converter that changes card IDs to card names
        /// </summary>
        private class CardSetJsonConverter : JsonConverter
        {
            /// <summary>
            /// Determines whether this instance can convert the specified object type
            /// </summary>
            public override bool CanConvert(Type objectType)
            {
                // See if the type is (or is a subclass of) CardSet
                while (objectType != null)
                {
                    if (objectType == typeof(CardSet))
                    {
                        return true;
                    }

                    objectType = objectType.BaseType;
                }

                return false;
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
                List<string> cardNames = new List<string>();
                foreach (string cardId in (CardSet)value)
                {
                    cardNames.Add(Cards.All[cardId].Name);
                }

                serializer.Serialize(writer, cardNames);
            }
        }
    }
}
