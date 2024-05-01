using Demo.DTO;
using Demo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Account : ControllerBase
    {
        //create Account new user
        //check Account Valid "login"
        private readonly UserManager<ApplicationUser> usermanager;
        private readonly IConfiguration confg;

        public Account(UserManager<ApplicationUser> usermanager,IConfiguration confg)
        {
           this.usermanager=usermanager;
            this.confg = confg;
        }
        //register
        [HttpPost("Register")]
        public async Task<IActionResult> Registertion(Login Userdto)
        {
            if (ModelState.IsValid)
            {
              

                ApplicationUser user = new ApplicationUser();
                user.UserName = Userdto.UserName;
                
                IdentityResult result = await usermanager.CreateAsync(user, Userdto.Password);

                if (result.Succeeded)
                    return Ok("Account successfully added");
                else
                    return BadRequest(result.Errors);
            }

            return BadRequest(ModelState);
        }
        //login
        [HttpPost("login")]
        public async Task<IActionResult> Login(Login login)
        {
            if (ModelState.IsValid==true)
            {
                ApplicationUser user=await usermanager.FindByNameAsync(login.UserName);
                if (user != null) {
                   bool found = await usermanager.CheckPasswordAsync(user, login.Password); 
                    if (found)
                    {//claims token
                        var claims = new List<Claim>();
                        claims.Add(new Claim(ClaimTypes.Name, login.UserName));
                        claims.Add(new Claim(ClaimTypes.NameIdentifier,user.Id));
                        claims.Add(new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()));
                        //get role
                        var role = await usermanager.GetRolesAsync(user);
                        foreach(var itemrole in role)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, itemrole));
                        }
                        SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(confg["JWT:SecretKey"]));
                        SigningCredentials signingCredentials = new SigningCredentials(securityKey
                            , SecurityAlgorithms.HmacSha256);
                        //create token
                       JwtSecurityToken mytoken = new JwtSecurityToken(
                            issuer: confg["JWT:ValidIssuer"],
                            audience: confg["JWT:ValidAudiance"],
                            claims: claims,
                            expires: DateTime.UtcNow.AddDays(1),
                            signingCredentials: signingCredentials
                            );
                        return Ok(new
                        {
                          token=new JwtSecurityTokenHandler().WriteToken(mytoken),
                          expiration=mytoken.ValidTo
                        });
                    }
                }
                return Unauthorized();
            }
            return Unauthorized();
        }
    }
}
