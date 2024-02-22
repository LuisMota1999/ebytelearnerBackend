namespace ebyteLearner.DTOs.PDF
{
    public class UpdatePDFRequestDTO
    {
        public string PDFName { get; set; }
        public int PDFNumberPages { get; set; }
        public string PDFContent { get; set; }
        public Guid ModuleID { get; set; }
    }
}
