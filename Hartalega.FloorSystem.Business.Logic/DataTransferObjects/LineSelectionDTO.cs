
namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    /// <summary>
    /// Line Seclection DTO
    /// </summary>
    public class LineSelectionDTO
    {
        public string Id { get; set; }
        public string LineId { get; set; }
        public bool StartPrint { get; set; }
        public string LineStartDateTime { get; set; }
        public string GloveType { get; set; }
        public string GloveSize { get; set; }
        public string TierSide { get; set; }
        public bool IsDoubleFormer { get; set; }
        public bool IsPrintByFormer { get; set; }
    }

  
}
