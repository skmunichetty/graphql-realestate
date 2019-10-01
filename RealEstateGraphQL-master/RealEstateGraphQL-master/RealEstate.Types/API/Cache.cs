using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstate.Types
{
    public class ResponseCache : ConcurrentDictionary<Uri, Task<ISwapiResponse>>
    {
    }
}
