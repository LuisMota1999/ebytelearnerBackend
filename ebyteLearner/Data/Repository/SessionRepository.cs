using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ebyteLearner.DTOs.Module;
using ebyteLearner.Helpers;
using ebyteLearner.Models;
using System;
using System.Collections.Generic;

namespace ebyteLearner.Interfaces
{
    public interface ISessionRepository
    {
        Task Create(CreateSessionRequestDTO request, byte[] QRCode);
        Task Update(Session session);
    }

    public class SessionRepository : ISessionRepository
    {
        private readonly DBContextService _dbContext;
        private readonly ILogger<SessionRepository> _logger;
        private readonly IMapper _mapper;
        public SessionRepository(DBContextService dbContext, ILogger<SessionRepository> logger, IMapper mapper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task Create(CreateSessionRequestDTO request, byte[] QRCode)
        {
            if (_dbContext.Session.Any(x => x.StartSessionDate <= request.EndSessionDate && x.EndSessionDate >= request.StartSessionDate))
                throw new AppException("End session date cannot be greater than the start session date.");

            if (_dbContext.Session.Any(x => (x.StartSessionDate <= request.EndSessionDate && x.EndSessionDate >= request.StartSessionDate) && x.SessionModuleID == request.SessionModuleID))
                throw new AppException("There is already a session registered within the specified date range for the module with ID " + request.SessionModuleID + ".");

            var session = _mapper.Map<Session>(request);
            session.QRCode = QRCode;
            _dbContext.Session.Add(session);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Update(Session session)
        {
            _dbContext.Entry(session).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}