using Elegance.Core.Attributes;
using Elegance.Core.Data;
using Elegance.Core.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace Elegance.Core.Metadata
{
    internal class ObjectMap
    {
        private readonly List<object> _primitiveValues;
        private readonly List<ObjectMap> _complexValues;
        private readonly Dictionary<PropertyInfo, ObjectMap> _complexValuesLookup;
        private readonly Type _type;
        private readonly ObjectMetadataFactory _metadataFactory;
        private readonly ObjectMetadata _metadata;
        private readonly IDbDataReader _reader;

        private int _hash;
        private bool _isCollection;

        private ObjectMap(ObjectMetadataFactory metadataFactory, Type type, ObjectMap parent)
        {
            _hash = 0;
            _isCollection = typeof(IList).IsAssignableFrom(type);

            _metadataFactory = metadataFactory;
            _primitiveValues = new List<object>(); 
            _complexValues = new List<ObjectMap>();
            _complexValuesLookup = new Dictionary<PropertyInfo, ObjectMap>();
            _type = _isCollection
                ? type.GenericTypeArguments[0] 
                : type;
            _metadata = _metadataFactory.GetMetdata(_type);

            _metadata.Parent = parent?._metadata;
        }

        internal ObjectMap(ObjectMetadataFactory metadataFactory, IDbDataReader reader, Type type, ObjectMap parent = null)
            : this(metadataFactory, type, parent)
        {
            _reader = reader;

            Value = Activator.CreateInstance(_type);

            ReadSimpleProperties();
            ReadComplexProperties();
        }

        internal ObjectMap(ObjectMetadataFactory metadataFactory, object value, Type type, ObjectMap parent = null)
            : this(metadataFactory, type, parent)
        {
            _reader = null;

            Value = value;

            ReadSimpleProperties();
            ReadComplexProperties();
        }

        public object Value { get; private set; }

        public void Merge(ObjectMap objectMap)
        {
            if (!Equals(objectMap))
            {
                throw new Exception("Cannot merge with a different object map");
            }

            // We've got another object map with the same primitive properties, it's
            // just got a different set of complex properties, so they'll need merging too.
            // If we're merging, the only thing that could possibly be different are the 
            // many to one mappings.

            foreach (var property in _metadata.GetComplexProperties())
            {
                var propertyMetadata = _metadata.GetMetadata(property);

                if (!propertyMetadata.IsCollection)
                {
                    continue;
                }

                // Need to get the original value as an object map somehow
                var currentValueEntryObjectMap = objectMap._complexValuesLookup[property];
                var originalValueCollection = (IList)property.GetValue(Value);
                var originalValueObjectMapCollection = new List<ObjectMap>();
              
                foreach (var value in originalValueCollection)
                {
                    originalValueObjectMapCollection.Add(new ObjectMap(_metadataFactory, value, property.PropertyType, this));
                }

                var index = originalValueObjectMapCollection.IndexOf(currentValueEntryObjectMap);

                if (index >= 0)
                {
                    var originalValueEntryObjectMap = originalValueObjectMapCollection[index];

                    originalValueEntryObjectMap.Merge(currentValueEntryObjectMap);
                    originalValueCollection.RemoveAt(index);
                    originalValueCollection.Insert(index, originalValueEntryObjectMap.Value);
                }
                else
                {
                    originalValueCollection.Add(currentValueEntryObjectMap.Value);
                }
            }
        }

        public override int GetHashCode()
        {
            return _hash;
        }

        public override bool Equals(object obj)
        {
            var comparisonObj = obj as ObjectMap;

            return comparisonObj == null
                ? false
                : Equals(comparisonObj);
        } 

        private bool Equals(ObjectMap objectMap)
        {
            bool areEqual = objectMap._primitiveValues.Count == _primitiveValues.Count;

            if (areEqual)
            {
                for (int i = 0; i < Math.Max(_primitiveValues.Count, _complexValues.Count); i++)
                {
                    areEqual &= (_primitiveValues.Count <= i || objectMap._primitiveValues[i].Equals(_primitiveValues[i]));
                    areEqual &= (_complexValues.Count <= i || _complexValues[i]._isCollection || objectMap._complexValues[i].Equals(_complexValues[i]));
                }
            }

            return areEqual;
        }

        private void ReadSimpleProperties()
        {
            foreach (var property in _metadata.GetSimpleProperties())
            {
                var propertyMetadata = _metadata.GetMetadata(property);
                var alias = propertyMetadata.Aliases.FirstOrDefault(a => _reader?.HasValue(a) ?? false);
                var propertyValue = _reader == null 
                    ? property.GetValue(Value) ?? DBNull.Value
                    : _reader.GetValue(alias);

                if (propertyValue == null)
                {
                    Value = null;
                    return;
                }
                else if (propertyValue == DBNull.Value)
                {
                    property.SetValue(Value, null);
                    continue;
                }

                var convertedUnderlyingValue = Convert.ChangeType(propertyValue, propertyMetadata.UnderlyingType);
                var convertedValue = propertyMetadata.IsEnum
                    ? Enum.Parse(propertyMetadata.Type, convertedUnderlyingValue.ToString())
                    : convertedUnderlyingValue;

                property.SetValue(Value, convertedValue);

                _primitiveValues.Add(convertedValue);
                _hash ^= convertedValue?.GetHashCode() ?? 0;
            }
        }

        private void ReadComplexProperties()
        {
            foreach (var property in _metadata.GetComplexProperties())
            {
                var type = property.PropertyType;
                ObjectMap childObject;
                ObjectMap parentObject = this;

                if (_reader == null)
                {
                    var propertyMetadata = _metadata.GetMetadata(property);
                    var propertyValue = property.GetValue(Value);

                    if (propertyMetadata.IsCollection)
                    {
                        propertyValue = ((IList)propertyValue)[0];
                    }

                    childObject = new ObjectMap(_metadataFactory, propertyValue, type, parentObject);
                }
                else
                {
                    childObject =  new ObjectMap(_metadataFactory, _reader, type, parentObject);
                }

                if (childObject.Value == null || parentObject.Value == null)
                {
                    continue;
                }

                parentObject.AddChild(property, childObject);
                childObject.AddParent(parentObject);

                _complexValues.Add(childObject);
                _complexValuesLookup.Add(property, childObject);
            }
        }

        private void AddChild(PropertyInfo property, ObjectMap child)
        {
            var propertyMetadata = _metadata.GetMetadata(property);
            var childValue = child.Value;

            if (propertyMetadata.IsCollection)
            {
                var collectionInstance = Activator.CreateInstance(property.PropertyType);
                var collectionValue = Convert.ChangeType(childValue, child._type);

                ((IList)collectionInstance).Add(collectionValue);

                childValue = collectionInstance;
            }

            property.SetValue(Value, childValue);
        }

        private void AddParent(ObjectMap parent)
        {
            _metadata
                .GetComplexProperties()
                .FirstOrDefault(p => p.PropertyType == parent._type)?
                .SetValue(Value, parent.Value);
        }
    }
}
