namespace ebyteLearner.DTOs.PDF
{
    public class PDFDTO
    {
        public Guid Id { get; set; }
        public string PDFName { get; set; }
        public int PDFNumberPages { get; set; }
        public string PDFContent { get; set; }
        public Guid ModuleID { get; set; }
    }
}
