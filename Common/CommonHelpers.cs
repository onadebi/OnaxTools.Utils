﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using OnaxTools.Dto;
using System.IdentityModel.Tokens.Jwt;

namespace OnaxTools.Common
{
    public static class CommonHelpers
    {
        public static GenResponse<AppUser> ReadJwt(HttpContext context)
        {
            var objResp = new GenResponse<AppUser>();
            try
            {
                var authValues = context.Request.Headers["Authorization"];
                if (StringValues.IsNullOrEmpty(authValues) || !authValues.Any(m => m.StartsWith("Bearer", StringComparison.OrdinalIgnoreCase)))
                {
                    return GenResponse<AppUser>.Failed("Invalid token credentials");
                }
                var authHeader = authValues.FirstOrDefault(m => m != null && m.StartsWith("Bearer"));
                if (authHeader != null)
                {
                    string authToken = !string.IsNullOrWhiteSpace(authHeader) ? authHeader.Split(" ")[1] : string.Empty;
                    var jwtoken = new JwtSecurityTokenHandler().ReadJwtToken(authToken);
                    Dictionary<string, string> tokenValues = new();
                    foreach (var claim in jwtoken.Claims)
                    {
                        if (!tokenValues.ContainsKey(claim.Type))
                        {
                            tokenValues.Add(claim.Type, claim.Value);
                        }
                        else
                        {
                            tokenValues[claim.Type] = tokenValues[claim.Type] + "," + claim.Value;
                        }
                    }
                    if (tokenValues.Any())
                    {
                        var appUser = new AppUser
                        {
                            Email = tokenValues["Email"],
                            DisplayName = tokenValues["unique_name"],
                            Id = Convert.ToInt32(tokenValues["UserId"])
                        };
                        string userRoles = tokenValues["Role"];
                        appUser.Roles = !string.IsNullOrWhiteSpace(userRoles) ? userRoles.Split(',').ToList() : new();
                        objResp = GenResponse<AppUser>.Success(appUser);
                    }
                    else
                    {
                        objResp = GenResponse<AppUser>.Failed("Invalid token credentials");
                    }
                }
                else { objResp = GenResponse<AppUser>.Failed("Invalid token credentials"); }
            }
            catch (Exception ex)
            {
                OnaxTools.Logger.OutputException(ex);
                objResp = GenResponse<AppUser>.Failed("Invalid token credentials");
            }
            return objResp;
        }



    }

}
