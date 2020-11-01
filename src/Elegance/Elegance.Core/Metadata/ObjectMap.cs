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
        private readonly IDbDataReader _reader;

        private int _hash;

        private ObjectMap()
        {
            _primitiveValues = new List<object>(); 
            _complexValues = new List<ObjectMap>();
            _complexValuesLookup = new Dictionary<PropertyInfo, ObjectMap>();

            _hash = 0;
        }

        internal ObjectMap(IDbDataReader reader, Type type, ObjectMap parent = null)
            : this()
        {
            _reader = reader;

            if (typeof(IList).IsAssignableFrom(type))
            {
                type = type.GenericTypeArguments[0];
            }

            MetaData = new ObjectMetadata(type, parent?.MetaData);
            Type = type;
            Value = Activator.CreateInstance(type);

            ReadSimpleProperties();
            ReadComplexProperties();
        }

        internal ObjectMap(object obj, Type type, ObjectMap parent = null)
            : this()
        {
            _reader = null;

            if (typeof(IList).IsAssignableFrom(type))
            {
                type = type.GenericTypeArguments[0];
            }

            MetaData = new ObjectMetadata(type, parent?.MetaData);
            Type = type;
            Value = obj;

            ReadSimpleProperties();
            ReadComplexProperties();
        }

        public ObjectMetadata MetaData { get; private set; }
        public Type Type { get; private set; }
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

            foreach (var property in MetaData.GetComplexProperties())
            {
                var propertyMetadata = MetaData.GetMetadata(property);

                if (!propertyMetadata.IsCollection)
                {
                    continue;
                }

                // Need to get the original value as an object map somehow
                var originalValueCollection = (IList)property.GetValue(Value);
                var originalValueObjectMapCollection = new List<ObjectMap>();
                
                foreach (var value in originalValueCollection)
                {
                    originalValueObjectMapCollection.Add(new ObjectMap(value, property.PropertyType, this));
                }

                var currentValueEntryObjectMap = objectMap._complexValuesLookup[property];

                if (originalValueObjectMapCollection.Contains(currentValueEntryObjectMap))
                {
                    var index = originalValueObjectMapCollection.IndexOf(currentValueEntryObjectMap);
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
            if (objectMap._primitiveValues.Count != _primitiveValues.Count)
            {
                return false;
            }

            for (int i = 0; i < _primitiveValues.Count; i++)
            {
                if (objectMap._primitiveValues[i].Equals(_primitiveValues[i]))
                {
                    continue;
                }

                return false;
            }

            return true;
        }

        private void ReadSimpleProperties()
        {
            foreach (var property in MetaData.GetSimpleProperties())
            {
                var propertyMetadata = MetaData.GetMetadata(property);
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
            foreach (var property in MetaData.GetComplexProperties())
            {
                object propertyValue = null;

                var type = property.PropertyType;
                var propertyMetadata = MetaData.GetMetadata(property);
                var parentObject = this;

                if (_reader == null)
                {
                    propertyValue = property.GetValue(Value);

                    if (typeof(IList).IsAssignableFrom(type))
                    {
                        propertyValue = ((IList)propertyValue)[0];
                    }
                }

                var childObject = _reader == null
                    ? new ObjectMap(propertyValue, type, parentObject)
                    : new ObjectMap(_reader, type, parentObject);

                if (parentObject.Value == null || childObject.Value == null)
                {
                    continue;
                }

                if (propertyMetadata.IsCollection)
                {
                    var collectionProperty = parentObject.MetaData
                        .GetComplexProperties()
                        .FirstOrDefault(p => typeof(IList).IsAssignableFrom(p.PropertyType) && p.PropertyType.GenericTypeArguments[0] == childObject.Type);
                    var collectionInstance = Activator.CreateInstance(collectionProperty.PropertyType);
                    var collectionValue = Convert.ChangeType(childObject.Value, childObject.Type);

                    ((IList)collectionInstance).Add(collectionValue);

                    collectionProperty.SetValue(parentObject.Value, collectionInstance);
                }
                else
                {
                    parentObject.MetaData
                        .GetComplexProperties()
                        .FirstOrDefault(p => p.PropertyType == childObject.Type)?
                        .SetValue(parentObject.Value, childObject.Value);
                }

                childObject.MetaData
                    .GetComplexProperties()
                    .FirstOrDefault(p => p.PropertyType == parentObject.Type)?
                    .SetValue(childObject.Value, parentObject.Value);

                _complexValues.Add(childObject);
                _complexValuesLookup.Add(property, childObject);
            }
        }
    }
}
