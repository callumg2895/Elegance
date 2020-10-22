using Elegance.Core.Attributes;
using Elegance.Core.Data;
using Elegance.Core.Interface;
using System;
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
        private readonly IDbDataReader _reader;

        private int _hash;

        private ObjectMap()
        {
            _primitiveValues = new List<object>();
            
            _hash = 0;
        }

        internal ObjectMap(IDbDataReader reader, Type type, ObjectMap parent = null)
            : this()
        {
            _reader = reader;

            MetaData = new ObjectMetadata(type, parent?.MetaData);
            Type = type;
            Value = Activator.CreateInstance(type);

            ReadSimpleProperties();
            ReadComplexProperties();
        }

        public ObjectMetadata MetaData { get; private set; }
        public Type Type { get; private set; }
        public object Value { get; private set; }

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
            return objectMap._primitiveValues.Equals(_primitiveValues);
        }

        private void ReadSimpleProperties()
        {
            foreach (var property in MetaData.GetSimpleProperties())
            {
                var propertyMetadata = MetaData.GetMetadata(property);
                var alias = propertyMetadata.Aliases.FirstOrDefault(a => _reader.HasValue(a));
                var propertyValue = _reader.GetValue(alias);

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
                var type = property.PropertyType;
                var parentObject = this;
                var childObject = new ObjectMap(_reader, type, parentObject);

                if (parentObject.Value == null || childObject.Value == null)
                {
                    continue;
                }

                parentObject.MetaData
                    .GetComplexProperties()
                    .FirstOrDefault(p => p.PropertyType == childObject.Type)?
                    .SetValue(parentObject.Value, childObject.Value);

                childObject.MetaData
                    .GetComplexProperties()
                    .FirstOrDefault(p => p.PropertyType == parentObject.Type)?
                    .SetValue(childObject.Value, parentObject.Value);
            }
        }
    }
}
