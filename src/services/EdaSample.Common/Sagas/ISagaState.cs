using EdaSample.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdaSample.Common.Sagas
{
    public interface ISagaState
    {
        string Serialize();

        void Deserialize(string serializedData);
    }
}
