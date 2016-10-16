using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Panda.Attributes;

namespace Panda
{
    public class IPandaEntity
    {
        [AUTO_INCREMENT]
        public int Id { get; set; }
    }
}
