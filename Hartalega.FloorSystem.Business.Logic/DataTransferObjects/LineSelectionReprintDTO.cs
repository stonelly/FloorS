
namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    /// <summary>
    /// Lin Selection for Manual Print Screen
    /// </summary>
    public class LineSelectionReprint
    {
        public string LineId { get; set; }
        public string Hour { get; set; }
        public string LTTier { get; set; }
        public string LBTier { get; set; }
        public string RTTier { get; set; }
        public string RBTier { get; set; }
        public string LTSize { get; set; }
        public string LBSize { get; set; }
        public string RTSize { get; set; }
        public string RBSize { get; set; }
        public string ReasonforReprinting { get; set; }
        public bool IsDoubleFormer { get; set; }
    }
}
