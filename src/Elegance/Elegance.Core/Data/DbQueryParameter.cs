using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Elegance.Core.Interface;

namespace Elegance.Core.Data
{
    internal class DbQueryParameter<T> : IDbQueryParameter<T>
    {
        private readonly static Dictionary<Type, DbType> _dbTypeMappings;

        static DbQueryParameter()
        {
            _dbTypeMappings = new Dictionary<Type, DbType>()
            {
                { typeof(DateTime),     DbType.DateTime2    },

                { typeof(string),       DbType.AnsiString   },

                { typeof(bool),         DbType.Boolean      },
                { typeof(byte),         DbType.Byte         },
                { typeof(sbyte),        DbType.SByte        },
                { typeof(short),        DbType.Int16        },
                { typeof(ushort),       DbType.UInt16       },
                { typeof(int),          DbType.Int32        },
                { typeof(uint),         DbType.UInt32       },
                { typeof(long),         DbType.Int64        },
                { typeof(ulong),        DbType.UInt64       },
                { typeof(double),       DbType.Double       },
                { typeof(float),        DbType.Single        },
                { typeof(decimal),      DbType.Decimal      },
            };
        }

        public string Name { get; private set; }
        public T Value { get; private set; }
        public DbType DbType { get; private set; }
        public ParameterDirection Direction { get; private set; }
        public int Size { get; private set; }

        private readonly bool _isNullable;
        private readonly Type _valueType;

        internal DbQueryParameter(string name, T value, IDbQueryParameterOptions options)
        {
            var type = typeof(T);
            var underlyingType = Nullable.GetUnderlyingType(type);

            Name = name;
            Value = value;
            Direction = options?.DirectionOverride ?? ParameterDirection.Input;
            Size = options?.SizeOverride ?? InterpretSize();

            _isNullable = underlyingType != null;

            if (_isNullable)
            {
                _valueType = underlyingType.IsEnum
                    ? underlyingType.GetEnumUnderlyingType()
                    : underlyingType;
            }
            else
            {
                _valueType = type.IsEnum
                    ? type.GetEnumUnderlyingType()
                    : type;
            }


            if (options != null && options.DbTypeOverride.HasValue)
            {
                DbType = options.DbTypeOverride.Value;
            }
            else if (_dbTypeMappings.TryGetValue(_valueType, out var dbType))
            {
                DbType = dbType;
            }
            else
            {
                throw new ArgumentException($"No override was provided for '{nameof(DbType)}', and neither type '{type.Name}' or its related underlying types have a standard conversion to '{nameof(DbType)}' available");
            }
        }

        public object GetValueForParameter()
        {
            if (_isNullable)
            {
                return Value == null 
                    ? (object)DBNull.Value 
                    : (object)Value;
            }
            else
            {
                return (object)Value;
            }
        }

        private int InterpretSize()
        {
            if (_valueType == typeof(string))
            {
                var valueString = (Value as string);
                var valueSize = string.IsNullOrEmpty(valueString)
                    ? -1
                    : valueString.ToCharArray().Length;

                return valueSize;
            }

            return -1;
        }
    }
}
