using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPlateFormLibary.Struct
{
    public struct TestPara
    {
        public int curIndex;
        public string config;
        public string methodName;
        public string classNameKey;
    }

    public struct Reflectpara
    {
        internal Type type;
        internal object classInstance;
    }
}
