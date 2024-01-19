namespace ReportingSystem.Enums.Extensions
{
    public class EnumDisplayNameAttribute(string displayName) : Attribute
    {
        public string DisplayName { get; } = displayName;
    }
}
