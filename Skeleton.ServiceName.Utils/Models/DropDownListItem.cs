using Skeleton.ServiceName.Utils.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Skeleton.ServiceName.Utils.Models
{
    public class BaseDropDownListItem<TKey, TValue> : IEnumDropDownListable<TKey, TValue>
    {

        public TKey Key { get; set; }

        public TValue Value { get; set; }
    }

    public class DropDownListItem : BaseDropDownListItem<long, string>
    {
    }
}
