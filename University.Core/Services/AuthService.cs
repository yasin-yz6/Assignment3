using Microsoft.AspNetCore.Identity;
using University.Core.DTOs;
using University.Core.Exceptions;
using University.Core.Forms;
using University.Core.Validations;
using University.Data.Entities.Identity;

namespace University.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly SignInManager<User> _signInManager;//from Identity
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        public AuthService(SignInManager<User> signInManager, UserManager<User> userManager, RoleManager<Role> roleManager) 
        {
            _roleManager = roleManager;
            _signInManager = signInManager;
            _userManager = userManager;
        }
        
        public async Task<UserDTO> Register(RegisterForm form)
        {
            if (form == null)
                throw new ArgumentNullException(nameof(form));
            var validation = FormValidator.Validate(form);
            if (!validation.IsValid)
                throw new BusinessException(validation.Errors);
            var userExists = await _userManager.FindByEmailAsync(form.Email);
            if (userExists != null)
                throw new BusinessException("User already exists with this email");
            //if the user is null (no user found)   
            var user = new User
            {
                Email = form.Email,
                UserName = form.Email,
                FirstName = form.FirstName,
                LastName = form.LastName
            };
            var result = await _userManager.CreateAsync(user, form.Password);//the password is here because of the stracture of the fn.
            if (!result.Succeeded)//if the creation isn't succeeded an error will be thrown
            {
                throw new BusinessException(result.Errors.GroupBy(x => x.Code)
                                                        .ToDictionary(x => x.Key, y => y.Select(a => a.Description).ToList()));
            }
            //looking if the role exists in the database or not
            if (!await _roleManager.RoleExistsAsync(form.Role)) 
                    await _roleManager.CreateAsync(new Role { Name = form.Role});
            
            await _userManager.AddToRoleAsync(user, form.Role);//assings the role to the user
            return new UserDTO
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                Phone = user.PhoneNumber,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed
            };
        }

        public async Task<UserDTO> Login(LoginForm form)
        {
            if (form == null)
                throw new ArgumentNullException(nameof(form));
            var validation = FormValidator.Validate(form);
            if (!validation.IsValid)
                throw new BusinessException(validation.Errors);
            
            var result = await _signInManager.PasswordSignInAsync(form.EmailAddress, form.Password, true, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(form.EmailAddress);
                if (user == null)
                    throw new BusinessException($"Unable to find account with email address: {form.EmailAddress}");
                var dto = new UserDTO
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    EmailConfirmed = user.EmailConfirmed,
                    Phone = user.PhoneNumber,
                    PhoneNumberConfirmed = user.PhoneNumberConfirmed
                };
                return dto;
            }
            else if (result.IsLockedOut)
                throw new BusinessException("Account is locked out");
            else if (result.IsNotAllowed)
                throw new BusinessException("Account is not allowed");
            throw new BusinessException("Invalid Login attemp");
        }
    }

    public interface IAuthService
    {
        Task<UserDTO> Login(LoginForm form);
        Task<UserDTO> Register(RegisterForm form);
    }
}
