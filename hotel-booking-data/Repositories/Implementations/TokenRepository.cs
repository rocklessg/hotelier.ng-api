using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using hotel_booking_data.Contexts;
using hotel_booking_data.Repositories.Abstractions;
using hotel_booking_models;
using Microsoft.EntityFrameworkCore;

namespace hotel_booking_data.Repositories.Implementations
{
    public class TokenRepository : GenericRepository<RefreshToken>, ITokenRepository
    {
        private readonly HbaDbContext _context;
        private readonly DbSet<RefreshToken> _tokens;
        public TokenRepository(HbaDbContext context) : base(context)
        {
            _context = context;
            _tokens = _context.Set<RefreshToken>();
        }

        public AppUser GetUserByRefreshToken(string token)
        {
            var user = _context.Users.SingleOrDefault(u => u.RefreshTokens.Any(x => x.Token == token));
            if (user == null)
            {
                throw new Exception("Invalid token");
            }

            return user;
        }
    }
}
