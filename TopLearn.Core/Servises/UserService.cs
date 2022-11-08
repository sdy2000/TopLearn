﻿using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopLearn.Core.Convertors;
using TopLearn.Core.DTOs;
using TopLearn.Core.Generators;
using TopLearn.Core.Security;
using TopLearn.Core.Servises.Interfaces;
using TopLearn.DataLayer.Context;
using TopLearn.DataLayer.Entities.User;
using TopLearn.DataLayer.Entities.Wallet;

namespace TopLearn.Core.Servises
{
    public class ViewRenderService : IUserService
    {
        private TopLearnContext _context;

        public ViewRenderService(TopLearnContext context)
        {
            _context = context;
        }


        public bool IsExistUserName(string userName)
        {
            return _context.Users.Any(u => u.UserName == userName);
        }

        public bool IsExistEmail(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }

        public int AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return user.UserId;
        }

        public User LoginUser(LoginViewModel login)
        {
            string hashPassword = PasswordHelper.EncodePasswordMd5(login.Password);
            string email = FixedText.FixEmail(login.Email);

            return _context.Users.SingleOrDefault(u => u.Email == email && u.Password == hashPassword);
        }

        public User GetUserByEmail(string email)
        {
            return _context.Users.SingleOrDefault(u => u.Email == email);
        }

        public User GetUserByUserName(string userName)
        {
            return _context.Users.SingleOrDefault(u => u.UserName == userName);
        }

        public User GetUserByActiveCode(string activeCode)
        {
            return _context.Users.SingleOrDefault(u => u.ActiveCode == activeCode);
        }

        public User GetUserById(int userId)
        {
            return _context.Users.Find(userId);
        }

        public void UpdateUser(User user)
        {
            _context.Update(user);
            _context.SaveChanges();
        }

        public bool ActiveAccount(string activeCode)
        {
            var user = _context.Users.SingleOrDefault(u => u.ActiveCode == activeCode);
            if (user == null || user.IsActive)
                return false;

            user.IsActive = true;
            user.ActiveCode = NameGenerator.GenerateUniqCode();
            _context.SaveChanges();


            return true;
        }

        public int GetUserIdByUserName(string userName)
        {
            return _context.Users.Single(u => u.UserName == userName).UserId;
        }

        public string SaveOrUpdateAvatar(IFormFile UserAvatar, string AvatarName= "Defult.jpg")
        {
            if (UserAvatar != null)
            {
                string imagePath = "";
                if (AvatarName != "Defult.jpg")
                {
                    imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", AvatarName);
                    if (File.Exists(imagePath))
                        File.Delete(imagePath);
                }
                AvatarName = NameGenerator.GenerateUniqCode() + Path.GetExtension(UserAvatar.FileName);
                imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", AvatarName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    UserAvatar.CopyTo(stream);
                }

                return AvatarName;
            }
            else if (AvatarName != "Defult.jpg")
            {
                return AvatarName;
            }
            else
            {
                return "Defult.jpg";
            }
        }

        public string GetUserNameById(int userId)
        {
            return _context.Users.Single(u => u.UserId == userId).UserName;
        }
        public string GetEmailById(int userId)
        {
            return _context.Users.Single(u => u.UserId == userId).Email;
        }

        public void DeleteUser(int userId)
        {
            User user = GetUserById(userId);
            user.IsDelete = true;
            user.IsActive = false;
            UpdateUser(user);
        }









        public InformationUserViewModel GetUserInformation(string userName)
        {
            var user = GetUserByUserName(userName);
            InformationUserViewModel information = new InformationUserViewModel()
            {
                UserName = user.UserName,
                Email = user.Email,
                RegisterDate = user.RegisterDate,
                Wallet = BalanceUserWallet(userName)
            };
            return information;
        }

        public SideBarUserPanelViewModel GetSideBarUserPanelData(string userName)
        {
            return _context.Users
                .Where(u => u.UserName == userName)
                .Select(u => new SideBarUserPanelViewModel()
                {
                    UserName = u.UserName,
                    RegisterDate = u.RegisterDate,
                    ImageName = u.UserAvatar
                })
                .Single();
        }

        public EditProfileViewModel GetDataForEditProfileUser(string userName)
        {
            return _context.Users
                .Where(u => u.UserName == userName)
                .Select(u => new EditProfileViewModel()
                {
                    UserName = u.UserName,
                    Email = u.Email,
                    AvatarName = u.UserAvatar
                })
                .Single();
        }

        public void EditProfile(string userName, EditProfileViewModel profile)
        {

            var user = GetUserByUserName(userName);
            user.UserName = profile.UserName;
            user.Email = profile.Email;
            user.UserAvatar = SaveOrUpdateAvatar(profile.UserAvatar, profile.AvatarName); 


            UpdateUser(user);
        }

        public bool CompareOldPassword(string OldPassword, string userName)
        {
            string hashOldPassword = PasswordHelper.EncodePasswordMd5(OldPassword);

            return _context.Users.Any(u => u.UserName == userName && u.Password == hashOldPassword);
        }

        public void ChangeUserPassword(string newPassword, string userName)
        {
            var user = GetUserByUserName(userName);
            user.Password = PasswordHelper.EncodePasswordMd5(newPassword);

            UpdateUser(user);
        }

        public int BalanceUserWallet(string userName)
        {
            int userId = GetUserIdByUserName(userName);
            var enter = _context.Wallets
                .Where(w => w.UserId == userId && w.TypeId == 1 && w.IsPay)
                .Select(w => w.Amount)
                .ToList();

            var exit = _context.Wallets
                .Where(w => w.UserId == userId && w.TypeId == 2 && w.IsPay)
                .Select(w => w.Amount)
                .ToList();

            // it's better create table in database for this code
            return (enter.Sum() - exit.Sum());
        }

        public List<WalletViewModel> GetWalletUser(string userName)
        {
            int userId = GetUserIdByUserName(userName);

            return _context.Wallets
                .Where(w => w.UserId == userId && w.IsPay)
                .Select(w => new WalletViewModel()
                {
                    Amount = w.Amount,
                    Type = w.TypeId,
                    Description = w.Description,
                    DateTime = w.CreateDate
                })
                .ToList();
        }

        public int ChargeWallet(string userName, int amount, string discription, bool isPay = false)
        {
            Wallet wallet = new Wallet()
            {
                Amount = amount,
                CreateDate = DateTime.Now,
                Description = discription,
                IsPay = isPay,
                TypeId = 1,
                UserId = GetUserIdByUserName(userName)
            };


            return AddWallet(wallet);
        }

        public int AddWallet(Wallet wallet)
        {
            _context.Wallets.Add(wallet);
            _context.SaveChanges();

            return wallet.WalletId;
        }

        public Wallet GetWalletByWalletId(int walletId)
        {
            return _context.Wallets.Find(walletId);
        }


        public void UpdateWallet(Wallet wallet)
        {
            _context.Wallets.Update(wallet);
            _context.SaveChanges();
        }





        //////////////////////




        public UsersForAdminViewModel GetUsers(int pageId = 1, string filterEmail = "", string filterUserName = "")
        {
            IQueryable<User> result = _context.Users;


            if (!string.IsNullOrEmpty(filterEmail))
            {
                result = result.Where(u => u.Email.Contains(filterEmail));
            }

            if (!string.IsNullOrEmpty(filterUserName))
            {
                result = result.Where(u => u.UserName.Contains(filterUserName));
            }

            // Show Item In Page
            int take = 20;
            int skip = (pageId - 1) * take;


            UsersForAdminViewModel list = new UsersForAdminViewModel();
            list.CurrentPage = pageId;
            list.PageCount = result.Count() / take;
            list.Users = result.OrderBy(u => u.RegisterDate).Skip(skip).Take(take).ToList();


            return list;
        }

        public UsersForAdminViewModel GetDeleteUsers(int pageId = 1, string filterEmail = "", string filterUserName = "")
        {
            IQueryable<User> result = _context.Users.IgnoreQueryFilters().Where(u=>u.IsDelete);


            if (!string.IsNullOrEmpty(filterEmail))
            {
                result = result.Where(u => u.Email.Contains(filterEmail));
            }

            if (!string.IsNullOrEmpty(filterUserName))
            {
                result = result.Where(u => u.UserName.Contains(filterUserName));
            }

            // Show Item In Page
            int take = 20;
            int skip = (pageId - 1) * take;


            UsersForAdminViewModel list = new UsersForAdminViewModel();
            list.CurrentPage = pageId;
            list.PageCount = result.Count() / take;
            list.Users = result.OrderBy(u => u.RegisterDate).Skip(skip).Take(take).ToList();


            return list;
        }

        public int AddUserFromAdmin(CreateUserViewModel user)
        {
            User addUser = new User();

            addUser.Password = PasswordHelper.EncodePasswordMd5(user.Password);
            addUser.ActiveCode = NameGenerator.GenerateUniqCode();
            addUser.UserName = user.UserName;
            addUser.Email = FixedText.FixEmail(user.Email);
            addUser.IsActive = true;
            addUser.RegisterDate = DateTime.Now;


            addUser.UserAvatar = SaveOrUpdateAvatar(user.UserAvatar);

           


            return AddUser(addUser);
        }

        public EditUserViewModel GetUserForShowInEditMode(int userId)
        {
            return _context.Users
                 .Where(u => u.UserId == userId)
                 .Select(u => new EditUserViewModel()
                 {
                     UserId = u.UserId,
                     UserName = u.UserName,
                     Email = u.Email,
                     AvatarName = u.UserAvatar,
                     UserRoles = u.UserRoles.Select(r => r.RoleId).ToList()
                 })
                 .Single();

        }

        public void EditUserFromAdmin(EditUserViewModel editUser)
        {
            User user = GetUserById(editUser.UserId);
            user.UserName = editUser.UserName;
            user.Email = editUser.Email;
            user.UserAvatar = SaveOrUpdateAvatar(editUser.UserAvatar, editUser.AvatarName);
            if (!string.IsNullOrEmpty(editUser.Password))
            {
                user.Password = PasswordHelper.EncodePasswordMd5(editUser.Password);
            }

            UpdateUser(user);
        }
    }
}
