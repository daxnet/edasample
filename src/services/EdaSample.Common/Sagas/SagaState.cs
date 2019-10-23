using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;

namespace EdaSample.Common.Sagas
{
    public abstract class SagaState : ISagaState
    {
        #region Public Methods

        public void Deserialize(string serializedData)
        {
            if (!string.IsNullOrEmpty(serializedData))
            {
                var propertyEntries = serializedData.Split(';');
                foreach (var propertyEntry in propertyEntries)
                {
                    var propertyKeyValuePair = propertyEntry.Split('=');
                    var propertyName = propertyKeyValuePair[0];
                    var propertyValue = propertyKeyValuePair[1];
                    var property = this.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
                    if (property?.CanWrite ?? false)
                    {
                        property.SetValue(this, ConvertPropertyValue(propertyValue, property.PropertyType));
                    }
                }
            }
        }

        public string Serialize()
        {
            var serializedData = new List<string>();
            var serializableProperties = from p in this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                         where p.CanRead && p.PropertyType.IsSimpleType()
                                         select p;
            foreach (var prop in serializableProperties)
            {
                object val = prop.GetValue(this);
                if (val is string str && string.IsNullOrEmpty(str))
                {
                    val = "<empty>";
                }
                else if (val == null)
                {
                    val = "<null>";
                }

                serializedData.Add($"{prop.Name}={val}");
            }

            return string.Join(";", serializedData.ToArray());
        }

        #endregion Public Methods

        #region Private Methods

        private object ConvertPropertyValue(string value, Type propertyType)
        {
            if (propertyType == typeof(string) && string.Equals(value, "<empty>"))
            {
                return "";
            }
            else if (string.Equals(value, "<null>"))
            {
                return null;
            }
            else
            {
                var convertMethod = typeof(Convert).GetMethod($"To{propertyType.Name}", BindingFlags.Public | BindingFlags.Static);
                return convertMethod?.Invoke(null, new[] { value });
            }
        }

        #endregion Private Methods
    }
}
