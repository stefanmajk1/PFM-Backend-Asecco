using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace PFMBackend.JSON
{
    public class StringValueProvider : IValueProvider
    {
        private PropertyInfo _targetProperty;
        private string _substitutionValue;

        public StringValueProvider(PropertyInfo targetProperty, string substitutionValue)
        {
            _targetProperty = targetProperty;
            _substitutionValue = substitutionValue;
        }

        
        public void SetValue(object target, object value)
        {
            _targetProperty.SetValue(target, value);
        }

        
        public object GetValue(object target)
        {
            object value = _targetProperty.GetValue(target);
            return value == null ? _substitutionValue : value;
        }
    }
}
