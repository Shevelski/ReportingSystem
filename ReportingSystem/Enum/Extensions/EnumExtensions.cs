using System.Reflection;

namespace ReportingSystem.Enum.Extensions
{
    public static class EnumExtensions
    {
        public static string? GetDisplayName(this System.Enum value)
        {
            FieldInfo? field = value.GetType().GetField(value.ToString());
            if (field != null)
            {
                EnumDisplayNameAttribute? attribute = field.GetCustomAttribute<EnumDisplayNameAttribute>();
                return attribute != null ? attribute.DisplayName : value.ToString();
            }
            return null;  
        }
    }
}
