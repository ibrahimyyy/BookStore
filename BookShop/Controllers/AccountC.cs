using BookShop.Models;
using BookShop.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BookShop.Controllers
{

    
    public class AccountC : Controller
    {
        private readonly IAccountRepository _accountRepository;
        public AccountC(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }
        
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpUserModel userModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountRepository.CreateUserAsync(userModel);
                    if(!result.Succeeded)
                    {
                         foreach (var errors in result.Errors)
                         {
                              ModelState.AddModelError("",errors.Description);
                         }
                      return View(userModel);
                    }
                 ModelState.Clear();
                 return RedirectToAction("ConfirmEmail", new { email = userModel.Email });
            }
            return View(userModel);
        }

        [Route("login")]
        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LogIn(LogInModel logInModel , string returnUrl)
        {
            ViewBag.Isnotallowed = false;
            if (ModelState.IsValid)
            {
                var result = await _accountRepository.PasswordSignInAsync(logInModel);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl))
                        return LocalRedirect(returnUrl);//this to return to last page after i login.

                    return RedirectToAction("Index", "Home");
                }
                if (result.IsNotAllowed)
                    ViewBag.Isnotallowed = true;

                else if (result.IsLockedOut)
                {
                    ViewBag.Isnotallowed = false;
                    ModelState.AddModelError("", "Account has blocked. Try another some time");
                }
                else
                {
                    ViewBag.Isnotallowed = false;
                    ModelState.AddModelError("", "Invalid credintials");
                }
            }            
            return View(logInModel);
        }

        public async Task<IActionResult> Logout()
        {
            await _accountRepository.SignoutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Route("ChangePass")]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost("ChangePass")]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel changePasswordModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountRepository.ChangePassword(changePasswordModel);
                if (result.Succeeded)
                {
                    ViewBag.IsSuccess = true;
                    ModelState.Clear();
                    return View();
                }
                foreach (var errors in result.Errors)
                    ModelState.AddModelError("",errors.Description);
            }
            return View();  
        }
        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string  uid , string token , string email)
        {
            EmailConfirmModel model = new EmailConfirmModel
            {
                Email=email
            };
            if (!string.IsNullOrEmpty(uid) && !string.IsNullOrEmpty(token))
            {
                token = token.Replace(' ' , '+');
                var result = await _accountRepository.ConfirmEmailAsync(uid, token);
                if (result.Succeeded)
                {
                    model.EmailVerified = true;
                }
            }
            return View(model);
        }

        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(EmailConfirmModel model)
        {
            var user = await _accountRepository.GetUserByEmailAsync(model.Email);
            if (user != null)
            {
                if (user.EmailConfirmed)
                {
                    model.IsConfirmed = true;
                    return View(model);
                }
                await _accountRepository.GenerateEmailConfirmationTokenAsync(user);
                model.EmailSent = true;
                ModelState.Clear();
            }
            else
                ModelState.AddModelError("", "somthing went wrong");

            return View(model);
        }
        [AllowAnonymous , HttpGet("forget-password")]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [AllowAnonymous, HttpPost("forget-password")]
        public async Task<ActionResult> ForgetPassword(ForgetPasswordModel model)
        {
            if(ModelState.IsValid)
            {
                var user = await _accountRepository.GetUserByEmailAsync(model.Email);
                if(user != null)
                {
                    await _accountRepository.GenerateForgetPasswordTokenAsync(user);                    
                    ModelState.Clear();
                    model.EmailSent = true;
                }
            }

            return View(model);
        }
        [AllowAnonymous, HttpGet("reset-password")]
        public IActionResult ResetPassword(string uid , string token)
        {
            ResetPasswordModel resetPasswordModel = new ResetPasswordModel
            {
                UserId = uid,
                Token = token
            };
            return View(resetPasswordModel);
        }
        [AllowAnonymous, HttpPost("reset-password")]
        public async Task<ActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                model.Token = model.Token.Replace(' ', '+');
                var result = await _accountRepository.ResetPasswordAsync(model);
                if(result.Succeeded)
                {
                    ModelState.Clear();
                    model.IsSuccess = true;
                    return View(model);
                }

                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult AddRoles()
        {
            ViewBag.IsSuccess = false;
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> AddRoles(AddRolesModel addRolesModel)
        {
            if(ModelState.IsValid)
            {
                addRolesModel.RoleName = addRolesModel.RoleName.Trim();
                var result = await _accountRepository.AddRoels(addRolesModel);
                if(result.Succeeded)
                {
                    ViewBag.IsSuccess = true;
                    ModelState.Clear();
                    return View();
                }
            }
            ViewBag.IsSuccess = false;
            ModelState.AddModelError("", "Invaild cridntial.");
            return View();
        }
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> ShowRoles()
        {
            return View(await _accountRepository.GetAllRoles());
        }
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> EditeRoles(string roleid)
        {
            if(!string.IsNullOrEmpty(roleid))
            {
                var data = await _accountRepository.GetRole(roleid);
                return View(data);
            }
            return View();
        }
        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        public async Task<IActionResult> EditeRoles(RoleinfoModel model)
        {
            model.RoleName = model.RoleName.Trim();
            if (ModelState.IsValid)
            {
                var result = await _accountRepository.EditeRole(model);
                if(result.Succeeded)
                {
                    ModelState.Clear();
                    return RedirectToAction(nameof(ShowRoles));
                }                
            }
            ModelState.AddModelError("", "error");
            return View();
        }
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> DeleteRole(string roleid)
        {
            if(!string.IsNullOrEmpty(roleid))
               await _accountRepository.Deleterole(roleid);
            return RedirectToAction(nameof(ShowRoles));
        }

        [Authorize(Roles ="SuperAdmin")]
        public async Task<IActionResult> AllUserInfo()
        {
            var data = await _accountRepository.GetAllUsersWithRoles();
            ViewBag.search = false;
            ViewBag.none = false;
            return View(data);
        }
        [HttpPost]
        public async Task<IActionResult> AllUserInfo(string email)
        {
            if(ModelState.IsValid)
            {
                    ViewBag.search = true;
                    email = email.Trim();
                    email = email.ToLower();
                if (!string.IsNullOrEmpty(email))
                {
                    var data = await _accountRepository.UsersSearch(email);
                    if (data.Count() == 0)
                    {
                        ViewBag.none = true;
                        AllUsersInfo x = new AllUsersInfo();
                        data.Add(x);
                        return View(data);
                    }
                    ViewBag.none = false;
                    return View(data);
                }
            }
            return View();
        }
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> ManageUserRoles(string userid)
        {
            UserRoleModel data = new UserRoleModel();
            data.UserId = userid;
            data.OldRoles = await _accountRepository.GetUserRolesNames(userid);
            data.Email = (await _accountRepository.GetUserByIdAsync(userid)).Email;            
            data.SelectedRoles = await _accountRepository.SelectedRoles(userid);            
            data.AllRoles = await _accountRepository.GetAllRoles();
            return View(data);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> ManageUserRoles(UserRoleModel userRoleModel)
        {
            var result = await _accountRepository.EditeTheRolesOfUser(userRoleModel);
            if(result.Succeeded)
            {
                return RedirectToAction(nameof(AllUserInfo));
            }
            return View();
        }
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> DeleteAccount(string userid)
        {
            if(!string.IsNullOrEmpty(userid))
            {
                var result = await _accountRepository.DeleteUserAccount(userid);
                if(result.Succeeded)
                {
                    return RedirectToAction(nameof(AllUserInfo));
                }
            }
            return View();
        }
    }
}