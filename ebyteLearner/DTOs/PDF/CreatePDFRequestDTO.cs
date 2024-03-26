namespace ebyteLearner.DTOs.PDF
{
    public class CreatePDFRequestDTO
    {
        public string PDFName { get; set; }
        public int PDFNumberPages { get; set; }
        public string PDFContent { get; set; }
        public long PDFLength { get; set; }
        public Guid ModuleID { get; set; }
    }
}
