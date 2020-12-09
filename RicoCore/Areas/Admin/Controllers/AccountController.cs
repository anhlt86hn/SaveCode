using AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using RicoCore.Areas.Admin.Models;
using RicoCore.Data.Entities.System;
using RicoCore.Models;
using RicoCore.Services;
using RicoCore.Services.Systems.Accounts;
using RicoCore.Services.Systems.Accounts.Dtos;
using RicoCore.Services.Systems.Passwords;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace RicoCore.Areas.Admin.Controllers
{
    public class AccountController : BaseController
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IAccountService _accountService;
        private readonly IPasswordService _passwordService;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
        private readonly IAuthorizationService _authorizationService;
        private readonly ILogger<LoginWith2faViewModel> _logger;
        private readonly IViewRenderService _viewRenderService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailSender _emailSender;

        public AccountController(SignInManager<AppUser> signInManager,
            IHostingEnvironment hostingEnvironment,
            IConfiguration configuration,
            UserManager<AppUser> userManager,
            IAccountService accountService,
            IAuthorizationService authorizationService,
            IPasswordService passwordService,
            ILogger<LoginWith2faViewModel> logger,
            IViewRenderService viewRenderService,
            IEmailSender emailSender)
        {
            _signInManager = signInManager;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
            _accountService = accountService;
            _passwordService = passwordService;
            _authorizationService = authorizationService;
            _logger = logger;
            _viewRenderService = viewRenderService;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        public async Task<IActionResult> Index(int? kichcotrang, string sapxep, int trang = 1)
        {
            var result = await _authorizationService.AuthorizeAsync(User, "USER", Operations.Read);
            if (result.Succeeded == false)
                return new RedirectResult("/Admin/Account/AccessDenied");

            var accounts = new PagedResultAccountViewModel();
            if (kichcotrang == null)
                kichcotrang = _configuration.GetValue<int>("PageSize");

            //pageSize = 10;
            accounts.PageSize = kichcotrang;
            accounts.SortType = sapxep;
            accounts.Data = _accountService.GetAllPaging(string.Empty, trang, kichcotrang.Value, accounts.SortType);
            return View(accounts);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var result = await _authorizationService.AuthorizeAsync(User, "USER", Operations.Create);
            if (result.Succeeded == false)
                return new RedirectResult("/Admin/Account/AccessDenied");

            var model = new AccountViewModel();
            var passwords = _passwordService.GetAll();
            _passwordService.SetSelectListItemPasswords(model, passwords);
            return View(model);
        }

        [HttpPost]
        public IActionResult Create(AccountViewModel accountVm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var passwords = _passwordService.GetAll();
                    _passwordService.SetSelectListItemPasswords(accountVm, passwords);

                    if (_accountService.ValidateAddAccountOrder(accountVm))
                    {
                        ModelState.AddModelError("",
                            "Thứ tự đã tồn tại");
                        return View(accountVm);
                    }
                    var passwordContent = _passwordService.GetById(accountVm.PasswordId).Content;
                    accountVm.Password = passwordContent;
                    var hiddenPassword = "";
                    for (int i = 0; i < passwordContent.Length; i++)
                    {
                        hiddenPassword += "*";
                    }
                    accountVm.HiddenPassword = hiddenPassword;
                    //var accountVm = new AccountViewModel
                    //{
                    //    Id = 0,
                    //     Domain = model.Domain,
                    //      HiddenPassword = model.HiddenPassword,
                    //       Level = model.Level,
                    //        Note = model.Note,
                    //         Order = model.Order,
                    //          Password = model.Password,
                    //           PasswordId = model.PasswordId,
                    //            Phone = model.Phone,
                    //             SecurityEmail = model.SecurityEmail,
                    //              Url = model.Url,
                    //               UserName = model.UserName
                    //};
                    _accountService.Add(accountVm);
                    _accountService.Save();
                    return Redirect("/Admin/Account/Index");
                }
                else
                {
                    // IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                    // return new BadRequestObjectResult(allErrors);

                    // return BadRequest(ModelState.Select(x => x.Value.Errors).FirstOrDefault(y => y.Count > 0)?.First()
                    //    .ErrorMessage);
                    IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                    foreach (var item in allErrors)
                    {
                        var message = item.ErrorMessage;
                    }
                    return View(accountVm);
                }
            }
            catch (Exception)
            {
                return Redirect("/Admin/Account/Index");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var result = await _authorizationService.AuthorizeAsync(User, "USER", Operations.Update);
            if (result.Succeeded == false)
                return new RedirectResult("/Admin/Account/AccessDenied");

            var accountVm = _accountService.GetById(id);
            var passwords = _passwordService.GetAll();
            _passwordService.SetSelectListItemPasswords(accountVm, passwords);
            return View(accountVm);
        }

        [HttpPost]
        public IActionResult Update(AccountViewModel accountVm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var passwords = _passwordService.GetAll();
                    _passwordService.SetSelectListItemPasswords(accountVm, passwords);

                    if (_accountService.ValidateUpdateAccountOrder(accountVm))
                    {
                        ModelState.AddModelError("",
                            "Thứ tự đã tồn tại");
                        return View(accountVm);
                    }
                    var passwordContent = _passwordService.GetById(accountVm.PasswordId).Content;
                    accountVm.Password = passwordContent;
                    var hiddenPassword = "";
                    for (int i = 0; i < passwordContent.Length; i++)
                    {
                        hiddenPassword += "*";
                    }
                    accountVm.HiddenPassword = hiddenPassword;

                    //var accountVm = new AccountViewModel
                    //{
                    //    Id = createOrUpdateAccountVm.Id,
                    //    Domain = createOrUpdateAccountVm.Domain,
                    //    HiddenPassword = createOrUpdateAccountVm.HiddenPassword,
                    //    Level = createOrUpdateAccountVm.Level,
                    //    Note = createOrUpdateAccountVm.Note,
                    //    Order = createOrUpdateAccountVm.Order,
                    //    Password = createOrUpdateAccountVm.Password,
                    //    PasswordId = createOrUpdateAccountVm.PasswordId,
                    //    Phone = createOrUpdateAccountVm.Phone,
                    //    SecurityEmail = createOrUpdateAccountVm.SecurityEmail,
                    //    Url = createOrUpdateAccountVm.Url,
                    //    UserName = createOrUpdateAccountVm.UserName
                    //};
                    _accountService.Update(accountVm);
                    _accountService.Save();
                    return Redirect("/Admin/Account/Index");
                }
                else
                {
                    // IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                    // return new BadRequestObjectResult(allErrors);

                    // return BadRequest(ModelState.Select(x => x.Value.Errors).FirstOrDefault(y => y.Count > 0)?.First()
                    //    .ErrorMessage);
                    IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                    foreach (var item in allErrors)
                    {
                        var message = item.ErrorMessage;
                    }
                    return View(accountVm);
                }
            }
            catch (Exception)
            {
                return Redirect("/Admin/Account/Index");
            }
        }

        #region Login with 2FA

        [HttpGet]
        public async Task<IActionResult> LoginWith2fa()
        {
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();

            if (user == null)
            {
                throw new InvalidOperationException($"Unable to load two-factor authentication user.");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginWith2faAsync(LoginWith2faViewModel model, bool rememberMe, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            returnUrl = returnUrl ?? Url.Content("~/Admin/Account/AccessDenied");
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();

            if (user == null)
            {
                throw new InvalidOperationException($"Unable to load two-factor authentication user.");
            }

            var authenticatorCode = model.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);

            var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, rememberMe, model.RememberMachine);

            if (result.Succeeded)
            {
                _logger.LogInformation("User with ID '{UserId}' logged in with 2fa.", user.Id);
                return LocalRedirect(returnUrl);
            }
            else if (result.IsLockedOut)
            {
                _logger.LogWarning("User with ID '{UserId}' account locked out.", user.Id);
                return RedirectToPage("./Lockout");
            }
            else
            {
                _logger.LogWarning("Invalid authenticator code entered for user with ID '{UserId}'.", user.Id);
                ModelState.AddModelError(string.Empty, "Invalid authenticator code.");
                return View();
            }
        }

        #endregion Login with 2FA

        #region Login with Recovery Code

        [HttpGet]
        public async Task<IActionResult> LoginWithRecoveryCode(string returnUrl = null)
        {
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new InvalidOperationException($"Unable to load two-factor authentication user.");
            }

            //ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginWithRecoveryCodeAsync(LoginWithRecoveryCodeViewModel model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
                return View();

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new InvalidOperationException($"Không thể load ứng dụng authentication 2FA.");
            }

            var recoveryCode = model.RecoveryCode.Replace(" ", string.Empty);

            var result = await _signInManager.TwoFactorRecoveryCodeSignInAsync(recoveryCode);

            if (result.Succeeded)
            {
                _logger.LogInformation("Tài khoản có ID '{UserId}' đã đăng nhập với một mã code khôi phục.", user.Id);
                return LocalRedirect(returnUrl ?? Url.Content("~/Admin/Home"));
            }
            if (result.IsLockedOut)
            {
                _logger.LogWarning("Tài khoản có ID '{UserId}' đã bị khóa.", user.Id);
                return RedirectToPage("./Lockout");
            }
            else
            {
                _logger.LogWarning("Mã code khôi phục đã nhập vào là không hợp lệ với tài khoản có ID '{UserId}' ", user.Id);
                ModelState.AddModelError(string.Empty, "Mã code khôi phục đã nhập vào là không hợp lệ.");
                return View();
            }
        }

        #endregion Login with Recovery Code

        #region Forgot password

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToPage("./ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { area = "Identity", code },
                    protocol: Request.Scheme);

                var emailModel = new MailTemplateModel()
                {
                    EmailSubject = "Reset mật khẩu",
                    EmailContent = "Nội dung email: ",
                };
                var htmlContent = await _viewRenderService.RenderToStringAsync("_ForgotPasswordConfirmationEmail", emailModel);
                await _emailSender.SendEmailAsync(
                    model.Email,
                    "Reset Mật khẩu",
                    $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                return RedirectToPage("./ForgotPasswordConfirmation");
            }
            return View();
        }

        #endregion Forgot password



        #region Get Data API

        [HttpGet]
        public IActionResult GetAllPaging(string keyword, int page, int pageSize, string sort)
        {
            var model = _accountService.GetAllPaging(keyword, page, pageSize, sort);
            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetById(int id)
        {
            var model = _accountService.GetById(id);

            return new ObjectResult(model);
        }

        #endregion Get Data API

        [HttpPost]
        public IActionResult SetNewOrder()
        {
            var order = _accountService.SetNewOrder();
            return Json(new
            {
                order
            });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _authorizationService.AuthorizeAsync(User, "USER", Operations.Delete);
            if (result.Succeeded == false)
                return new RedirectResult("/Admin/Home/Index");

            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            else
            {
                _accountService.Delete(id);
                _accountService.Save();
                return new OkObjectResult(id);
            }
        }

        [HttpPost]
        public IActionResult MultiDelete(ICollection<string> selectedIds)
        {
            if (selectedIds != null)
            {
                _accountService.MultiDelete(selectedIds.ToList());
            }
            return Json(new { Result = true });
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Redirect("/Admin/Login/Index");
        }

        [HttpPost]
        public IActionResult ImportExcel(IList<IFormFile> files, int categoryId)
        {
            try
            {
                if (files != null && files.Count > 0)
                {
                    var file = files[0];
                    var filename = ContentDispositionHeaderValue
                                       .Parse(file.ContentDisposition)
                                       .FileName
                                       .Trim('"');

                    string folder = _hostingEnvironment.WebRootPath + $@"\uploaded\excels\accounts";
                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }
                    string filePath = Path.Combine(folder, filename);

                    using (FileStream fs = System.IO.File.Create(filePath))
                    {
                        file.CopyTo(fs);
                        fs.Flush();
                    }
                    _accountService.ImportExcel(filePath);
                    _accountService.Save();
                    return new OkObjectResult(filePath);
                }
                return new NoContentResult();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public IActionResult ExportExcel()
        {
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string directory = Path.Combine(sWebRootFolder, "export-files");
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            string sFileName = $"Accounts_{DateTime.Now:yyyyMMddhhmmss}.xlsx";
            string fileUrl = $"{Request.Scheme}://{Request.Host}/export-files/{sFileName}";
            FileInfo file = new FileInfo(Path.Combine(directory, sFileName));
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            }
            var accounts = _accountService.GetAllToExport();

            using (ExcelPackage package = new ExcelPackage(file))
            {
                // add a new worksheet to the empty workbook
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Accounts");
                worksheet.Cells["A1"].LoadFromCollection(accounts, true, TableStyles.Light1);
                worksheet.Cells.AutoFitColumns();
                package.Save(); //Save the workbook.
            }
            return new OkObjectResult(fileUrl);
        }
    }
}