
namespace Hartalega.FloorSystem.Business.Logic
{
    public class WasherBatch
    {
        static WasherBatch()
        { 

        }
        private string batchNumber;

        public string BatchNumber
        {
            get { return batchNumber; }
            set { batchNumber = value; }
        }
        private string gloveType;

        public string GloveType
        {
            get { return gloveType; }
            set { gloveType = value; }
        }
        private string size;

        public string Size
        {
            get { return size; }
            set { size = value; }
        }
    }
}
