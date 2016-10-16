using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Panda.Attributes
{
    public class AUTO_INCREMENT : SQLInject
    {
        public override string Value { get; set; } = "IDENTITY(1,1) PRIMARY KEY";
    }
}
