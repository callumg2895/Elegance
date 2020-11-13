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

        private ObjectMetadata _parent;

        private ObjectMetadata()
        {
            _allProperties = new List<PropertyInfo>();
            _complexProperties = new List<PropertyInfo>();
            _simpleProperties = new List<PropertyInfo>();

            _propertyMetadataLookup = new Dictionary<PropertyInfo, PropertyMetadata>();
        }

        public ObjectMetadata(Type type)
            : this()
        {
            Type = type;

            BuildPropertyMetadata();
        }

        public Type Type { get; private set; }

        public ObjectMetadata Parent 
        { 
            get 
            { 
                return _parent;
            }

            set 
            {
                if (_parent == null)
                {
                    _parent = value;

                    BuildPropertyMetadata();
                }
            } 
        }

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

        private void BuildPropertyMetadata()
        {
            _allProperties.Clear();
            _complexProperties.Clear();
            _simpleProperties.Clear();
            _propertyMetadataLookup.Clear();

            foreach (var property in Type.GetProperties())
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
    }
}
