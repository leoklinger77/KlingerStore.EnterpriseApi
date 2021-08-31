﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using System;

namespace KSE.Cart.Extensions
{
    public class CustomSerializationProvider : IBsonSerializationProvider
    {
        public IBsonSerializer GetSerializer(Type type)
        {
            if (type == typeof(ObjectId))
            {
                return new SafeObjectIdSerializer();
            }

            return null;
        }
    }

    public class SafeObjectIdSerializer : ObjectIdSerializer
    {
        public SafeObjectIdSerializer() : base() { }

        public override ObjectId Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var bsonReader = context.Reader;

            var bsonType = bsonReader.GetCurrentBsonType();

            switch (bsonType)
            {
                case BsonType.Binary:
                    {

                        var value = bsonReader
                                .ReadBinaryData()
                                .AsGuid
                                .ToString()
                                .Replace("-", "")
                                .Substring(0, 24);

                        return new ObjectId(value);
                    }
            }

            return base.Deserialize(context, args);
        }
    }
}
