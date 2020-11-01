using Elegance.Core.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Elegance.Core.Metadata
{
    internal class ObjectMetadata
    {
        private readonly List<PropertyInfo> _allProperties;
        private readonly List<PropertyInfo> _complexProperties;
        private readonly List<PropertyInfo> _simpleProperties;

        private readonly Dictionary<PropertyInfo, PropertyMetadata> _propertyMetadataLookup;

        private ObjectMetadata()
        {
            _allProperties = new List<PropertyInfo>();
            _complexProperties = new List<PropertyInfo>();
            _simpleProperties = new List<PropertyInfo>();

            _propertyMetadataLookup = new Dictionary<PropertyInfo, PropertyMetadata>();
        }

        public ObjectMetadata(Type type, ObjectMetadata parent)
            : this()
        {
            Type = type;
            Parent = parent;

            foreach (var property in type.GetProperties())
            {
                var metaData = new PropertyMetadata(property, this);

                if (metaData.IsComplex)
                {
                    _complexProperties.Add(property);
                }
                else
                {
                    _simpleProperties.Add(property);
                }

                _propertyMetadataLookup.Add(property, metaData);
                _allProperties.Add(property);
            }
        }

        public Type Type { get; private set; }
        public ObjectMetadata Parent { get; private set; }

        public List<PropertyInfo> GetProperties()
        {
            return _allProperties;
        }

        public List<PropertyInfo> GetComplexProperties()
        {
            return _complexProperties;
        }

        public List<PropertyInfo> GetSimpleProperties()
        {
            return _simpleProperties;
        }

        public PropertyMetadata GetMetadata(PropertyInfo property)
        {
            _propertyMetadataLookup.TryGetValue(property, out var metadata);

            return metadata;
        }
    }
}
