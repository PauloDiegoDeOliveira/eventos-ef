using System;
using System.Collections.Generic;

namespace ObjetivoEventos.Application.Utilities.Estados
{
    public static class EnumStateManager
    {
        public static bool StateVerifier(Type enumType, System.Enum key, System.Enum value, Dictionary<string, List<string>> rules)
        {
            if (EnumVerifier(enumType, key) && EnumVerifier(enumType, value) && rules.ContainsKey(key.ToString()) && rules[key.ToString()].Contains(value.ToString()))
                return true;

            return false;
        }

        public static bool StateVerifier(System.Enum key, System.Enum value, Dictionary<string, List<string>> rules)
        {
            if (rules.ContainsKey(key.ToString()) && rules[key.ToString()].Contains(value.ToString()))
                return true;

            return false;
        }

        public static bool EnumVerifier(Type enumType, System.Enum myEnum)
        {
            return myEnum.GetType() == enumType;
        }

        public static bool EnumVerifier(Type enumType, System.Enum enum1, System.Enum enum2)
        {
            return enum1.GetType() == enumType && enum2.GetType() == enumType;
        }
    }
}