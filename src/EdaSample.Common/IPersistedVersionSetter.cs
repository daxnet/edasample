using System;
using System.Collections.Generic;
using System.Text;

namespace EdaSample.Common
{
    public interface IPersistedVersionSetter
    {
        long PersistedVersion { set; }
    }
}
