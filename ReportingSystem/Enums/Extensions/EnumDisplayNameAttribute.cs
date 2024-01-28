using System.ComponentModel;
using System.Resources;

namespace ReportingSystem.Enums.Extensions
{
    public class EnumDisplayNameAttribute(string displayName) : Attribute
    {
        public string DisplayName { get; } = displayName;
    }

    //public class EnumDisplayNameAttribute : Attribute
    //{
    //    public EnumDisplayNameAttribute(string resourceKey)
    //          : base(GetDisplayName(resourceKey))
    //    {

    //    }
    //    private static string GetDisplayName(string displayName)
    //    {
    //        return LocalizeCustom.GetText(displayName, typeof(Properties.Resources));
    //    }
    //    //public string DisplayName { get; } = displayName;
    //}


    //public class LocalizeCustom
    //{
    //    static ResourceManager _resourceManager;

    //    public static string GetText(string resourceKey, Type resourceType)
    //    {
    //        if (_resourceManager is null)
    //        {
    //            _resourceManager = new ResourceManager(resourceType);
    //        }
    //        string category = _resourceManager.GetString(resourceKey);
    //        return string.IsNullOrWhiteSpace(category) ? $"[[{resourceKey}]]" : category;
    //    }


    //}
}
