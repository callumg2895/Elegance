using System;
using System.Collections.Generic;
using System.Text;

namespace Elegance.Core.Metadata
{
    internal class ObjectMetadataFactory
    {
        IDictionary<Type, ObjectMetadata> _metadataLookup;

        internal ObjectMetadataFactory()
        {
            _metadataLookup = new Dictionary<Type, ObjectMetadata>();
        }

        public ObjectMetadata GetMetdata(Type type)
        {
            if (!_metadataLookup.TryGetValue(type, out var metadata))
            {
                metadata = new ObjectMetadata(type);

                _metadataLookup.Add(type, metadata);
            }

            return metadata;
        }
    }
}
