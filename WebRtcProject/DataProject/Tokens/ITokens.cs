using DataProject.Data.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProject.Tokens
{
    public interface ITokens
    {
        public Task<JWT_RefreshToken> UpdateTokens(string refToken);

    }
}
