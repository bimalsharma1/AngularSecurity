using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using PtcApi.Model;

namespace PtcApi.Security
{
    public  class SecurityManager
    {
        private JwtSettings _settings;
        public SecurityManager(JwtSettings settings)
        {
            _settings = settings;
        }
        public AppUserAuth ValidateUser(AppUser user)
        {
            AppUserAuth ret = new AppUserAuth();
            AppUser authUser = null;

            using (var db = new PtcDbContext())
            {
                //Attempt to validate user
                authUser = db.Users.Where(
                    u => u.UserName.ToLower() == user.UserName.ToLower()
                    && u.Password == user.Password).FirstOrDefault();
            }

            if(authUser != null)
            {
                //build usersecurity object
                ret = BuildUserAuthObject(authUser);
            }

            return ret;
        }

        public AppUserAuth GetNewUserClaims(AppUser user)
        {
            AppUserAuth ret = new AppUserAuth();
            AppUser authUser = null;
            try
            {
                using (var db = new PtcDbContext())
                {
                    if (user != null)
                    {
                        db.Users.Add(user);
                        db.SaveChanges();

                        authUser = db.Users.Where(
                            u => u.UserName.ToLower() == user.UserName.ToLower()
                            && u.Password == user.Password).FirstOrDefault();

                        AppUserClaim userClaim = new AppUserClaim();
                        userClaim.UserId = authUser.UserId;
                        userClaim.ClaimType = "CanAccessMenu";
                        userClaim.ClaimValue = "true";

                        db.Claims.Add(userClaim);
                        db.SaveChanges();

                        if(authUser != null) {
                            //build usersecurity object
                            ret = BuildUserAuthObject(authUser);
                        }  
                    }
                }
            } catch (Exception ex) {
                throw new Exception(
                    "Exception trying to create new user.", ex);
            }
            return ret;
        }

        protected List<AppUserClaim> GetUserClaims(AppUser authUser)
        {
            List<AppUserClaim> list = new List<AppUserClaim>();

            try
            {
                using (var db = new PtcDbContext())
                {
                    list = db.Claims.Where(
                        u => u.UserId == authUser.UserId).ToList();
                    
                }
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "Exception trying to retrieve user claims.", ex);
                
            }

            return list;
        }

        protected AppUserAuth BuildUserAuthObject(AppUser authUser)
        {
            AppUserAuth ret = new AppUserAuth();
            List<AppUserClaim> claims = new List<AppUserClaim>();

            //set user properties
            ret.UserName = authUser.UserName;
            ret.IsAuthenticated = true;
            ret.BearerToken = new Guid().ToString();

            //get all claims from this user
            ret.Claims = GetUserClaims(authUser);
            ret.BearerToken = BuildJwtToken(ret);
            return ret;
        }

        protected string BuildJwtToken(AppUserAuth authUser)
        {
            SymmetricSecurityKey key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_settings.Key));
            List<Claim> jwtClaims = new List<Claim>();

            //create standard JWT claims
            jwtClaims.Add(new Claim(JwtRegisteredClaimNames.Sub,
                authUser.UserName));

            jwtClaims.Add(new Claim(JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString()));

            //add custom claims
            jwtClaims.Add(new Claim("isAuthenticated",
                authUser.IsAuthenticated.ToString().ToLower()));
            //add custom claims from claims array
            foreach(var claim in authUser.Claims) {
                jwtClaims.Add(new Claim(claim.ClaimType, claim.ClaimValue));
            }


            //create jetsecuritytoken object
            var token = new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                claims: jwtClaims,
                notBefore: DateTime.UtcNow.AddMinutes(
                    _settings.MinutesToExpiration),
                expires:DateTime.UtcNow.AddMinutes(
                    _settings.MinutesToExpiration),
                signingCredentials:new SigningCredentials(key,
                        SecurityAlgorithms.HmacSha256)

            );

            //create string representation of jwt token
            return new JwtSecurityTokenHandler().WriteToken(token);

        }

    }
}