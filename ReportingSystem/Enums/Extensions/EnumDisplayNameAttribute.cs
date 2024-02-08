using System.ComponentModel;
using System.Resources;

namespace ReportingSystem.Enums.Extensions
{
    //public class EnumDisplayNameAttribute(string displayName) : Attribute
    //{
    //    public string DisplayName { get; } = displayName;
    //}

    public class EnumDisplayNameAttribute : DescriptionAttribute
    {

        ResourceManager _resourceManager;
        private readonly string _resourceKey;

        public EnumDisplayNameAttribute(string resourceKey, Type resourceType)
        {
            _resourceManager = new ResourceManager(resourceType);
            _resourceKey = resourceKey;
        }
        public override string Description
        {
            get
            {
                string description = _resourceManager.GetString(_resourceKey);
                return string.IsNullOrWhiteSpace(description) ? $"{_resourceKey}" : description;
            }
        }


    }
}
