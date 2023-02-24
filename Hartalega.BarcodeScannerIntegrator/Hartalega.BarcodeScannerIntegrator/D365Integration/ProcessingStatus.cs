using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Hartalega.BarcodeScannerIntegrator.D365Integration
{
    public enum ProcessingStatus : int
    {
        NotStarted = 0,
        
        Ready = 1,
        
        InProgress = 2,
        
        Completed = 3,
        
        Error = 4,
        
        OnHold = 5,
    }
}
