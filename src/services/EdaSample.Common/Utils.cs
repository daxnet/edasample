using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdaSample.Common
{
    public static class Utils
    {
        private static Lazy<Type[]> SimpleTypesInternal = new Lazy<Type[]>(() =>
        {
            var types = new[]
                           {
                              typeof (Enum),
                              typeof (string),
                              typeof (char),
                              typeof (Guid),

                              typeof (bool),
                              typeof (byte),
                              typeof (short),
                              typeof (int),
                              typeof (long),
                              typeof (float),
                              typeof (double),
                              typeof (decimal),

                              typeof (sbyte),
                              typeof (ushort),
                              typeof (uint),
                              typeof (ulong),

                              typeof (DateTime),
                              typeof (DateTimeOffset),
                              typeof (TimeSpan),
                          };


            var nullableTypes = from t in types
                                where t != typeof(Enum) && t != typeof(string)
                                select typeof(Nullable<>).MakeGenericType(t);

            return types.Concat(nullableTypes).ToArray();
        });

        public static bool IsSimpleType(this Type src)
        {
            if (src == null)
            {
                throw new ArgumentNullException(nameof(src));
            }

            return src.IsEnum ||
                (src.IsGenericType &&
                    src.GetGenericTypeDefinition() == typeof(Nullable<>) &&
                    src.GetGenericArguments().First().IsEnum) ||
                SimpleTypesInternal.Value.Contains(src);
        }

        public static void ConcurrentDictionarySafeRegister<TKey, TValue>(TKey key, TValue value, ConcurrentDictionary<TKey, List<TValue>> registry)
        {
            if (registry.TryGetValue(key, out List<TValue> registryItem))
            {
                if (registryItem != null)
                {
                    if (!registryItem.Contains(value))
                    {
                        registry[key].Add(value);
                    }
                }
                else
                {
                    registry[key] = new List<TValue> { value };
                }
            }
            else
            {
                registry.TryAdd(key, new List<TValue> { value });
            }
        }
    }
}
