namespace AwesomeGamingRacing.Models.Enums
{
    public static class EnumHelper
    {
        public struct EnumDisplayName<T>
        {
            public T EnumValue { get; set; }
            public string Name { get; set; }
        }

        public static List<EnumDisplayName<T>> GetEnumDisplayNames<T>() where T : Enum
        {
            List<EnumDisplayName<T>> result = new List<EnumDisplayName<T>>();
            T[] enumVals = (T[])Enum.GetValues(typeof(T));
            foreach (T enumVal in enumVals)
            {
                EnumDisplayName<T> enumDisplayName = new EnumDisplayName<T>();
                enumDisplayName.EnumValue = enumVal;
                enumDisplayName.Name = enumVal.GetAttribute<DisplayName>().Name;
                result.Add(enumDisplayName);
            }
            return result;
        }

        public static TAttribute GetAttribute<TAttribute>(this Enum value) where TAttribute : Attribute
        {
            var enumType = value.GetType();
            var name = Enum.GetName(enumType, value);
            return enumType.GetField(name).GetCustomAttributes(false).OfType<TAttribute>().SingleOrDefault();
        }
    }
}
