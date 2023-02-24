using System;

namespace Hartalega.BarcodeScannerIntegrator.IR_Reader
{
    public class IRReaderOperationException : Exception
    {
        public IRReaderOperationException(string ErrorMessage, Exception exception)
            : base(ErrorMessage, exception)
        {

        }
    }
}