using System;

namespace MongoDriver.Monitoring
{     
    public class Monitor
    {
        public bool TryGetEventHandler<TEvent>(out Action<TEvent> handler)
        {
            throw new NotImplementedException();
        }
    }
}
