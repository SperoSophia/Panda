using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Panda.Attributes
{
    public class SQLInject : Attribute
    {
        public virtual string Value { get; set; }
    }
}
