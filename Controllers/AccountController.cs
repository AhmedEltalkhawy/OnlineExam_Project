

using OnlineExamProject.ViewModel;

namespace OnlineExamProject.Controllers
{
    public class AccountController : Controller
    {
        private IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> usermanager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> usermanager, SignInManager<ApplicationUser> signInManager)
        {
            this.unitOfWork = unitOfWork;
            this.usermanager = usermanager;
            this.signInManager = signInManager;
        }

        public IActionResult Index()
        {
            var users = unitOfWork.Users.GetAll();
            return View("Index", users);
        }

        public IActionResult Admins()
        {
            var users = unitOfWork.Users.FindAll(u => u.IsAdmin == true);
            return View("Index", users);
        }

        public IActionResult Students()
        {
            var users = unitOfWork.Users.FindAll(u => u.IsAdmin == false);
            return View("Index", users);
        }

        public IActionResult Search(string Name)
        {

            var users = unitOfWork.Users.FindAll(c => EF.Functions.Like(c.UserName, $"%{Name}%"));
            return View("Index", users);

        }


        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveRegister(RegisterUserViewModel viewmodel)
        {
            var existingUser = await usermanager.FindByNameAsync(viewmodel.UserName);

            if (existingUser != null)
            {
                ModelState.AddModelError("", "The Name is already in use.");
                return View("Register", viewmodel);
            }
            if (ModelState.IsValid)
            {

                //mapping 

                ApplicationUser appUser = new ApplicationUser();

                appUser.UserName = viewmodel.UserName;
                appUser.PasswordHash = viewmodel.Password;
                appUser.PhoneNumber = viewmodel.PhoneNumber;
                appUser.Email = viewmodel.Email;
                appUser.IsAdmin = viewmodel.Role == "Admin" ? true : false;
                appUser.IsDeleted = false;


                IdentityResult result = await usermanager.CreateAsync(appUser, viewmodel.Password);

                if (result.Succeeded)
                {

                    if (viewmodel.Role == "Admin")
                    {
                        await usermanager.AddToRoleAsync(appUser, "admin");
                    }
                    else if (viewmodel.Role == "User")
                    {
                        await usermanager.AddToRoleAsync(appUser, "user");

                    }
                   
                    //Create cookies appuser to extract information 
                    //false to make it per session , true presistent
                    await signInManager.SignInAsync(appUser, false);

                    //return Content("register success");

                    return RedirectToAction("Index", "Home");
                }

                // if errors occuar while create 
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }

            }
            return View("Register", viewmodel);
        }
        [HttpGet]
        public IActionResult Login()
        {
            LoginUserViewModel model = new LoginUserViewModel();
            return View("Login", model);
        }
        [HttpPost]
        public async Task<IActionResult> SaveLogin(LoginUserViewModel UserViewModel)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser appuser = await usermanager.FindByNameAsync(UserViewModel.Name);
                if (appuser != null)
                {
                    bool found = await usermanager.CheckPasswordAsync(appuser, UserViewModel.Password);
                    bool IsDeleted = appuser.IsDeleted;

                    if (found == true && IsDeleted == false)
                    {

                        List<Claim> claims = new List<Claim>();
                        //claims.Add(new Claim("UserAddress", appuser.Address));

                        await signInManager.SignInWithClaimsAsync(appuser, UserViewModel.RememberMe, claims);

                        return RedirectToAction("Index", "Home");
                    }
                }

            }
            ModelState.AddModelError("", "UserName or Password are wrong");
            //return RedirectToAction("Login");
            return View("Login", UserViewModel);

        }
        public async Task<IActionResult> SignOut()
        {
            await signInManager.SignOutAsync();
            return View("Login");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string Name)
        {
            ApplicationUser user = await usermanager.FindByNameAsync(Name);

            if(user is null)
            {
                return NotFound("this user does not exist");
            }
            user.IsDeleted = true;

            //if(user.IsAdmin == true)
            //{
            //    await usermanager.RemoveFromRoleAsync(user, "admin");
            //}
            //else
            //{
            //    await usermanager.RemoveFromRoleAsync(user, "user");

            //}


            //var result = await usermanager.DeleteAsync(user);
            unitOfWork.Complete();

            return RedirectToAction("Index");



        }

        [HttpGet]
        public async Task<IActionResult> UndoDelete(string Name)
        {
            ApplicationUser user = await usermanager.FindByNameAsync(Name);

            if (user is null)
            {
                return NotFound("this user does not exist");
            }
            user.IsDeleted = false;

            //if(user.IsAdmin == true)
            //{
            //    await usermanager.RemoveFromRoleAsync(user, "admin");
            //}
            //else
            //{
            //    await usermanager.RemoveFromRoleAsync(user, "user");

            //}


            //var result = await usermanager.DeleteAsync(user);

            unitOfWork.Complete();

            return RedirectToAction("Index");



        }

    }
}
