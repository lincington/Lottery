using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    internal class MapsterHelper
    {
        string s = 123.Adapt<string>(); // 等同于: 123.ToString();

    }
}
