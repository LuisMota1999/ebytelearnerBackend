using QRCoder;
using ebyteLearner.DTOs.Module;
using ebyteLearner.Interfaces;
using ebyteLearner.Models;
using System.Drawing;
using System.IO;

namespace ebyteLearner.Services
{
    public interface ISessionService
    {
        Task CreateSession(CreateSessionRequestDTO request);
    }

    public class SessionService : ISessionService
    {
        private readonly ISessionRepository _sessionRepository;

        public SessionService(ISessionRepository sessionRepository)
        {
            _sessionRepository = sessionRepository;
        }

        public async Task CreateSession(CreateSessionRequestDTO request)
        {
            byte[] QRCode = await GenerateQRCodeBase64("Teste");

            await _sessionRepository.Create(request, QRCode);
        }

        private async Task<byte[]> GenerateQRCodeBase64(string data)
        {
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q))
            using (QRCode qrCode = new QRCode(qrCodeData))
            using (Bitmap qrCodeImage = qrCode.GetGraphic(20))
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    qrCodeImage.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                    byte[] imageBytes = memoryStream.ToArray();

                    return imageBytes;
                }
            }
        }
    }
}
