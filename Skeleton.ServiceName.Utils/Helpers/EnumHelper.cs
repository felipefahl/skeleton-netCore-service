using Skeleton.ServiceName.Utils.CustomAttribute;
using Skeleton.ServiceName.Utils.Interfaces;
using Skeleton.ServiceName.Utils.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Resources;

namespace Skeleton.ServiceName.Utils.Helpers
{
    public static class EnumHelper
    {
        public static bool HasValue<T>(Enum value)
        {
            var t = typeof(T);
            if (!t.IsEnum)
                throw new InvalidOperationException("The generic type T must be an Enum type.");
            return Enum.IsDefined(typeof(T), value);
        }

        public static string GetEnumDescription(Enum value)
        {
            try
            {
                var fi = value.GetType().GetField(value.ToString());

                var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attributes.Any())
                {
                    var attribute = attributes[0];

                    ResourceManager resources = null;

                    if (attribute is LocalizedEnumAttribute)
                        resources = new ResourceManager(((LocalizedEnumAttribute)attribute).NameResourceType);
                    else
                        resources = new ResourceManager(typeof(Global));

                    var description = string.Empty;

                    if (resources != null)
                        description = resources.GetString(attribute.Description);

                    if (string.IsNullOrEmpty(description))
                    {
                        return attribute.Description;
                    }

                    return description;
                }

                return value.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Função para retornar uma lista de valores de um enumerador para ser utilizado em um dropdownlist
        /// </summary>
        /// <typeparam name="TDropDown">Tipo da lista que vai ser utilizada no dropdown</typeparam>
        /// <typeparam name="TEnum">Tipo de enumerador</typeparam>
        /// <typeparam name="TKey">Tipo da chave</typeparam>
        /// <typeparam name="TValue">Tipo do valor</typeparam>
        /// <returns></returns>
        public static List<TDropDown> GetListOfEnum<TDropDown, TEnum, TKey, TValue>()
            where TDropDown : IEnumDropDownListable<TKey, TValue>, new()
        {

            var typesRoom = new List<TDropDown>();

            var resource = new ResourceManager(typeof(Global));

            foreach (var value in Enum.GetValues(typeof(TEnum)))
            {
                object v = GetEnumDescription((Enum)value);
                if (string.IsNullOrEmpty((string)v))
                    v = resource.GetString(Enum.GetName(typeof(TEnum), value));
                object key = Convert.ToInt64(Enum.Parse(typeof(TEnum), value.ToString()));
                typesRoom.Add(new TDropDown
                {
                    Key = (TKey)key,
                    Value = (TValue)v
                });
            }

            return typesRoom.OrderBy(x => x.Value).ToList();

        }
    }
}
