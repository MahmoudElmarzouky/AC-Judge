﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using ACJudge.Areas.Identity.Data;
using ACJudge.Data.Models;
using ACJudge.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ACJudge.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<AuthUser> _userManager;
        private readonly SignInManager<AuthUser> _signInManager;
        private readonly IUserRepository<User> _userRepository;
        private readonly IWebHostEnvironment _hosting;

        public IndexModel(
            UserManager<AuthUser> userManager,
            SignInManager<AuthUser> signInManager,
            IUserRepository<User> userRepository,
            IWebHostEnvironment hosting)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._userRepository = userRepository;
            this._hosting = hosting;
            AddCountries();
        }

        public string Username { get; set; }
        public List<string> Countries { get; set; }

        void AddCountries()
        {
            Countries = new List<string>();
            CultureInfo[] cultureInfos = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
            foreach (CultureInfo cultureInfo in cultureInfos)
            {
                RegionInfo regionInfo = new(cultureInfo.LCID);
                if (!Countries.Contains(regionInfo.EnglishName))
                {
                    Countries.Add(regionInfo.EnglishName);
                }
            }

            Countries.Remove("Israel");
            Countries.Add("Palastine");
            Countries.Sort();
        }

        [TempData] public string StatusMessage { get; set; }

        [BindProperty] public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Display(Name = "Last Name")]
            [Required]
            public string LastName { get; set; }

            [Display(Name = "Country")]
            [Required]
            public string Country { get; set; }

            [Range(1, 9999, ErrorMessage = "You must Set Your Birth Date")]
            [Display(Prompt = "BirthDate")]
            public int BirthDate { get; set; }

            [Display(Name = "Profile Picture")]
            public string PhotoUrl { get; set; }

            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            public IFormFile photoFile { get; set; }
        }

        private async Task LoadAsync(AuthUser authUser)
        {
            var userName = await _userManager.GetUserNameAsync(authUser);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(authUser);
            string userIdentityId = await _userManager.GetUserIdAsync(authUser);
            var userTable = _userRepository.Find(userIdentityId);
            Username = userName;
            Input = new InputModel
            {
                PhoneNumber = phoneNumber, FirstName = userTable.FirstName,
                LastName = userTable.LastName,
                Country = userTable.Country,
                PhotoUrl = userTable.PhotoUrl,
                BirthDate = userTable.BirthDateYear
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            string userIdentityId = await _userManager.GetUserIdAsync(user);
            var userTable = _userRepository.Find(userIdentityId);
            string firstName = Input.FirstName;
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            string imageFullName = null;
            if (Input.photoFile != null)
            {
                Guid imageGuid = Guid.NewGuid();
                string extension = Path.GetExtension(Input.photoFile.FileName);
                imageFullName = imageGuid + extension;
                string newPath = _hosting.WebRootPath + "/img/Profile/" + imageFullName;
                string oldPath = null;
                if (Input.PhotoUrl != null)
                {
                    oldPath = _hosting.WebRootPath + "/img/Profile/" + Input.PhotoUrl;
                }

                using (FileStream fileStream = new FileStream(newPath, FileMode.Create))
                {
                    Input.photoFile.CopyTo(fileStream);
                    if (oldPath != null)
                        System.IO.File.Delete(oldPath);
                }
            }

            User newUser = new User
            {
                UserId = userTable.UserId,
                UserIdentityId = userTable.UserIdentityId, FirstName = Input.FirstName, LastName = Input.LastName,
                Country = Input.Country, PhotoUrl = imageFullName, BirthDateYear = Input.BirthDate
            };
            _userRepository.Update(newUser);
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}