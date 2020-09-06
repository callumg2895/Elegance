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
        private protected readonly static Dictionary<Type, DbType> _dbTypeMappings;

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

        private protected readonly Type _valueType;
        private protected readonly string _name;
        private protected readonly T _value;
        private protected readonly DbType _dbType;

        internal DbQueryParameter(string name, T value, DbType? dbTypeOverride = null)
        {
            var type = typeof(T);

            _name = name;
            _value = value;
            _valueType = type.IsEnum
                ? type.GetEnumUnderlyingType()
                : type;

            if (dbTypeOverride.HasValue)
            {
                _dbType = dbTypeOverride.Value;
            }
            else if (_dbTypeMappings.TryGetValue(_valueType, out var dbType))
            {
                _dbType = dbType;
            }
            else
            {
                /*
                 * In theory, the IConvertible interfact should restrict the _valueType to be one of 
                 * the primitive C# types, or a string (one of the rare few reference types that implements
                 * the IConvertible interface. So we shouldn't get here under normal circumstances.
                 * 
                 * TODO - work out what to do if this test fails, i.e the user defines their own type that
                 * they want to use as a parameter. Perhaps define some sort of default behaviour?
                 */

                throw new ArgumentException($"Neither type '{type.Name}' or its related underlying types have a standard conversion to '{nameof(DbType)}' available");
            }

        }

        public void AddToCommand(IDbCommand command)
        {
            var commandParameter = command.CreateParameter();

            commandParameter.ParameterName = _name;
            commandParameter.Value = _value;
            commandParameter.DbType = _dbType;

            command.Parameters.Add(commandParameter);
        }
    }
}
