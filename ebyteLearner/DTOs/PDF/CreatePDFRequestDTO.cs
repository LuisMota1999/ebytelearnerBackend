namespace ebyteLearner.DTOs.NewFolder
{
    public class CreatePDFRequestDTO
    {
        public string PDFName { get; set; }
        public int PDFNumberPages { get; set; }
        public string PDFContent { get; set; }
        public long PDFLength { get; set; }
        public Guid ModuleID { get; set; }
        public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.Now;
    }
}
