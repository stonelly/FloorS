using System;
using System.Collections.Generic;
using System.Text;

namespace Hartalega.FloorSystem.Framework.BarCodeLib
{
    interface IBarcode
    {
        string Encoded_Value
        {
            get;
        }//Encoded_Value

        string RawData
        {
            get;
        }//Raw_Data

        List<string> Errors
        {
            get;
        }//Errors
    }
}
