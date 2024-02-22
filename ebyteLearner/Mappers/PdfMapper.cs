using AutoMapper;
using ebyteLearner.DTOs.PDF;
using ebyteLearner.Models;

namespace ebyteLearner.Mappers
{
    public class PdfMapper : Profile
    {
        public PdfMapper()
        {
            // Pdf -> CreatePDFRequestDTO
            CreateMap<Pdf, CreatePDFRequestDTO>();

            // CreatePDFRequestDTO -> Pdf
            CreateMap<CreatePDFRequestDTO, Pdf>();

            // PDFDTO -> Pdf
            CreateMap<PDFDTO, Pdf>();

            // Pdf -> PDFDTO
            CreateMap<Pdf, PDFDTO>();
        }
    }
}

