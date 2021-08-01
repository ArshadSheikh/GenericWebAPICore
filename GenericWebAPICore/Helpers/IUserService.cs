using DynamicAndGenericControllersSample.DB;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GenericWebAPICore.Helpers
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        IEnumerable<UserMaster> GetAll();
        UserMaster GetById(string id);
    }

    public class UserService : IUserService
    {
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        GenericRepository<UserMaster> Storage = null;

        private readonly AppSettings _appSettings;

        public UserService(IOptions<AppSettings> appSettings, GenericRepository<UserMaster> storage)
        {
            _appSettings = appSettings.Value;
            Storage = storage;
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var UserMaster = Storage.GetSingleOrDefault(x => x.Username == model.Username && x.Password == model.Password);

            // return null if user not found
            if (UserMaster == null) return null;

            // authentication successful so generate jwt token
            var token = generateJwtToken(UserMaster);

            return new AuthenticateResponse(UserMaster, token);
        }

        public IEnumerable<UserMaster> GetAll()
        {
            return Storage.GetAll();
        }

        public UserMaster GetById(string id)
        {
            return Storage.GetByID(id);
        }

        // helper methods

        private string generateJwtToken(UserMaster UserMaster)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", UserMaster.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
