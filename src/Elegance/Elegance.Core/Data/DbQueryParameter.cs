using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;
using Elegance.Core.Interface;

namespace Elegance.Core.Data
{
    internal class DbQueryParameter<T> : IDbQueryParameter
        where T : IConvertible
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
                { typeof(float),        DbType.Int16        },
                { typeof(decimal),      DbType.Decimal      },
            };
        }

        public string Name { get; private set; }

        private readonly Type _valueType;
        private readonly T _value;
        private readonly DbType _dbType;
        private readonly ParameterDirection? _direction;
        private readonly int? _size;

        internal DbQueryParameter(string name, T value, IDbQueryParameterOptions options)
        {
            var type = typeof(T);

            Name = name;

            _value = value;
            _valueType = type.IsEnum
                ? type.GetEnumUnderlyingType()
                : type;
            _direction = options?.DirectionOverride;
            _size = options?.SizeOverride ?? InterpretSize();

            if (options != null && options.DbTypeOverride.HasValue)
            {
                _dbType = options.DbTypeOverride.Value;
            }
            else if (_dbTypeMappings.TryGetValue(_valueType, out var dbType))
            {
                _dbType = dbType;
            }
            else
            {
                throw new ArgumentException($"No override was provided for '{nameof(DbType)}', and neither type '{type.Name}' or its related underlying types have a standard conversion to '{nameof(DbType)}' available");
            }
        }

        public IDbDataParameter AddToCommand(IDbCommand command)
        {
            var commandParameter = command.CreateParameter();

            commandParameter.ParameterName = Name;
            commandParameter.Value = _value;
            commandParameter.DbType = _dbType;

            if (_direction.HasValue)
            {
                commandParameter.Direction = _direction.Value;
            }

            if (_size.HasValue)
            {
                commandParameter.Size = _size.Value;
            }

            command.Parameters.Add(commandParameter);

            return commandParameter;
        }

        private int? InterpretSize()
        {
            if (_valueType == typeof(string))
            {
                var valueString = (_value as string);
                var valueSize = string.IsNullOrEmpty(valueString)
                    ? -1
                    : valueString.ToCharArray().Length;

                return valueSize;
            }

            return null;
        }
    }
}
