using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Examining.Pages
{
    public class LoginModel  : PageModel
    {
        private readonly IAccountService _service;
        public const string SessionKeyName = "_Name";
        public LoginModel(IAccountService servie) 
        {
               
                 this._service = servie;
        }

        // public IActionResult OnPost()
        //     {
                
        //         if (!ModelState.IsValid)
        //         {
        //             return Page();
        //         }

        //         if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        //         {
        //             msg = "Hãy nhập tài khoản và mật khẩu";
        //             return Page();
        //         }
        //         if(!AccountExists(username))
        //         {
        //              msg = "tài khoản không tồn tại";
        //             return Page();
                    
        //         }
        //         if(!_service.GetAccount(username).Password.Equals(password))
        //         {
        //                 msg = "sai tên tài khoản hoặc mật khẩu";
        //                 return Page();
        //         }

        //          HttpContext.Session.SetString(SessionKeyName, username);
        //          return RedirectToPage("Index");
        //     }

        [HttpPost]
        public IActionResult OnGet(string returnUrl)
        {
            if(!HttpContext.User.Identity.IsAuthenticated)
                return Page();
            
            HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);
            
            return RedirectToPage("Login");
        }

        [HttpPost]
        
        public IActionResult Onpost( string returnUrl)
        {
            bool isUservalid = false;

            Account user = _service.GetAccount(username);

            if(user!=null)
            {
                isUservalid = true;
            }

        //Identity
            if(ModelState.IsValid && isUservalid)
            {
                var claims = new List<Claim>();

                claims.Add(new Claim(ClaimTypes.Name, user.Username));
                claims.Add(new Claim(ClaimTypes.Role, user.Roles));
                var identity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.
        AuthenticationScheme);

                var principal = new ClaimsPrincipal(identity);

                var props = new AuthenticationProperties();
                props.IsPersistent = RememberMe;

                HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.
        AuthenticationScheme,
                    principal, props).Wait();
            
                    
                    return RedirectToPage("Index");
            }
        else
            {
                ViewData["message"] = "Tài khoản hoặc mật khẩu không đúng";
            }
            return Page();
        }

        


        private bool AccountExists(string id)
        {
            return _service.GetAccount(id) != null; 
        }
        
        [TempData]
         public string msg{get; set;}
         [Required]
         [BindProperty]
         public string username{get; set;}
         [Required]
         [BindProperty]
         public string password{get; set;}
         [Required]
         [BindProperty]
         public bool RememberMe { get; set; }
        
    }
}