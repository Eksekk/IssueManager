using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace IssueManager.Static_classes
{
    public static class ExtensionMethods
    {
        public static bool IsFieldValid(this ModelStateDictionary dict, string fieldName)
        {
            return dict.ContainsKey(fieldName) && dict.GetFieldValidationState(fieldName) == ModelValidationState.Valid;
        }

        public static V GetValueOrDefault<K, V> (this Dictionary<K, V> dict, K key, V defaultValue)
        {
            if (dict.ContainsKey(key))
            {
                return dict[key];
            }
            return defaultValue;
        }

        // now same, but for ViewData dictionary
        public static dynamic GetValueOrDefault<M>(this ViewDataDictionary<M> viewData, string key, dynamic value)
        {
            if (viewData.ContainsKey(key))
            {
                return viewData[key];
            }
            return value;
        }

        // for TempData
        public static dynamic GetValueOrDefault(this ITempDataDictionary tempData, string key, dynamic value)
        {
            if (tempData.ContainsKey(key))
            {
                return tempData[key];
            }
            return value;
        }
    }
}
