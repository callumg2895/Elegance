using Elegance.Core.Attributes;
using Elegance.Core.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Elegance.Core.Metadata
{
    internal class PropertyMetadata
    {
        private static readonly HashSet<Type> _primitiveTypes;

        static PropertyMetadata()
        {
            _primitiveTypes = new HashSet<Type>()
            {
                typeof(string),

                typeof(sbyte),
                typeof(byte),
                typeof(short),
                typeof(ushort),
                typeof(int),
                typeof(uint),
                typeof(long),
                typeof(ulong),

                typeof(float),
                typeof(double),
                typeof(decimal),

                typeof(DateTime),
                typeof(Guid),
            };
        }

        private PropertyMetadata()
        {
            Aliases = new List<string>();
        }

        public PropertyMetadata(PropertyInfo property, ObjectMetadata objectMetadata)
            : this()
        {
            var propertyType = property.PropertyType;

            IsCollection = typeof(IList).IsAssignableFrom(propertyType);

            if (IsCollection)
            {
                propertyType = propertyType.GenericTypeArguments[0];          
            }

            var nullUnderlyingType = Nullable.GetUnderlyingType(propertyType);
            var enumUnderlyingType = (nullUnderlyingType ?? propertyType).IsEnum
                ? (nullUnderlyingType ?? propertyType).GetEnumUnderlyingType()
                : null;

            ObjectMetadata = objectMetadata;
            Property = property;
            IsComplex = !_primitiveTypes.Contains(propertyType) && !_primitiveTypes.Contains(nullUnderlyingType) && !_primitiveTypes.Contains(enumUnderlyingType);
            IsEnum = enumUnderlyingType != null;
            Type = propertyType;
            UnderlyingType = enumUnderlyingType ?? nullUnderlyingType ?? propertyType;

            UpdateAliasOptions();
        }

        public ObjectMetadata ObjectMetadata { get; private set; }
        public PropertyInfo Property { get; private set; }
        public bool IsComplex { get; private set; }
        public bool IsCollection { get; private set; }
        public bool IsEnum { get; private set; }
        public Type Type { get; private set; }
        public Type UnderlyingType { get; private set; }
        public List<string> Aliases {get; private set;}

        private void UpdateAliasOptions()
        {
            var aliasOptions = new List<string>();
            var columnName = (Property.GetCustomAttribute(typeof(ColumnAttribute)) as ColumnAttribute)?.Name;
            var alternateAliases = Property.GetCustomAttributes()
                .Where(p => p is AlternateAliasAttribute)
                .Select(p => (p as AlternateAliasAttribute).Name)
                .Where(n => !string.IsNullOrWhiteSpace(n));

            if (!string.IsNullOrWhiteSpace(columnName))
            {
                Aliases.Add(columnName);
            }

            Aliases.Add(Property.Name);
            Aliases.AddRange(alternateAliases);

            var currentParent = ObjectMetadata.Parent;
            var currentType = ObjectMetadata.Type;

            while (currentParent != null && currentType != null)
            {
                var aliases = currentParent
                    .GetComplexProperties()
                    .Select(p => currentParent.GetMetadata(p))
                    .Where(p => p.Type == currentType)
                    .SelectMany(p => p.Aliases)
                    .SelectMany(parentAlias => Aliases.Select(alias => $"{parentAlias}_{alias}"))
                    .ToList();

                Aliases.AddRange(aliases);

                currentParent = currentParent?.Parent;
                currentType = currentParent?.Type;
            }
        }
    }
}
