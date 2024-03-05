using ebyteLearner.Models;
using pt.portugal.eid;
using ebyteLearner.DTOs.Auth;
using BCryptNet = BCrypt.Net.BCrypt;
using AutoMapper;
using ebyteLearner.Helpers;


namespace ebyteLearner.Services
{
    public interface IAuthService
    {
        UserDTO RegisterUser(RegisterRequestDTO request);
        Task<AuthResponseDTO> LoginCitizenCard();
        AuthResponseDTO LoginCredentials(AuthRequestDTO credentials);
        void ForgotPassword();
    }

    public class AuthService: IAuthService
    {
        private readonly DBContextService _dbContext;
        private readonly ILogger<UserService> _logger;
        private readonly IJwtUtils _jwtUtils;
        private readonly ICacheService _cacheService;
        private readonly IMapper _mapper;
        private PTEID_ReaderSet readerSet = null;
        private PTEID_ReaderContext readerContext = null;
        private PTEID_EIDCard eidCard = null;
        private PTEID_EId eid = null;

        public AuthService(DBContextService dbContext, ILogger<UserService> logger, IMapper mapper, IJwtUtils jwtUtils, ICacheService cacheService)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _jwtUtils = jwtUtils ?? throw new ArgumentNullException(nameof(jwtUtils));
            _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
        }

        public UserDTO RegisterUser(RegisterRequestDTO request)
        {
            // validate
            if (_dbContext.User.Any(x => x.Username == request.Username))
                throw new AppException("Username '" + request.Username + "' is already taken");

            if (IsValidEmail(request.Email) == false)
                throw new AppException("Email '" + request.Email + "' is not valid");

            // map model to new user object
            var user = _mapper.Map<User>(request);

            // hash password
            user.Password = BCryptNet.HashPassword(request.Password);
            _logger.LogInformation($"User found: PW={user.Password}, NIF={user.NIF}");
            // save user
            _dbContext.User.Add(user);
            _dbContext.SaveChanges();

            var userDTO = _mapper.Map<UserDTO>(user);

            return userDTO;
        }

        public async Task<AuthResponseDTO?> LoginCitizenCard()
        {
            try
            {
                PTEID_ReaderSet.initSDK();

                readerSet = PTEID_ReaderSet.instance();

                readerContext = readerSet.getReader();

                eidCard = readerContext.getEIDCard();
                eid = eidCard.getID();

                string NIF = eid.getTaxNo();

                if (NIF == null)
                    throw new AppException("NIF is invalid to read");

                User user = _dbContext.User.SingleOrDefault(u => u.NIF == NIF);

                if (user != null)
                {
                    _logger.LogInformation($"User found: Id={user.Id}, Name={user.NIF}");
                    var response = _mapper.Map<AuthResponseDTO>(user);
                    response.AccessToken = _jwtUtils.GenerateJwtToken(user);
                    return response;
                }
                else
                {
                    throw new AppException("User not found");
                }
            }
            catch (Exception ex)
            {
                throw new AppException($"Error authenticating user: {ex.Message}");
            }
            finally
            {
                readerContext?.releaseCard();
            }
        }

        public AuthResponseDTO LoginCredentials(AuthRequestDTO credentials)
        {
            
            var user = _dbContext.User.SingleOrDefault(x => x.Username == credentials.Username);

            if (user == null || !BCryptNet.Verify(credentials.Password, user.Password))
                return null;
               
            // Mapear o objeto User para UserDTO
            var userDTO = _mapper.Map<UserDTO>(user);

            // Gerar o token de acesso
            var accessToken = _jwtUtils.GenerateJwtToken(user);

            // Construir a resposta com o objeto UserDTO e o token de acesso
            var response = new AuthResponseDTO
            {
                User = userDTO,
                AccessToken = accessToken,
                Role = user.UserRole.ToString(),
            };

            return response;
        }

        public void ForgotPassword()
        {
            return;
        }

        public void Logout()
        {
            return;
        }

        private bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false; 
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }
    }
}
