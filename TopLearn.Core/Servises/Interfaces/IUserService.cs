using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopLearn.Core.DTOs;
using TopLearn.DataLayer.Entities.User;
using TopLearn.DataLayer.Entities.Wallet;

namespace TopLearn.Core.Servises.Interfaces
{
    public interface IUserService
    {
        bool IsExistUserName(string userName);
        bool IsExistEmail(string email);
        int AddUser(User user);
        User LoginUser(LoginViewModel login);
        User GetUserByEmail(string email);
        User GetUserByUserName(string userName);
        User GetUserByActiveCode(string activeCode);
        User GetUserById(int userId);
        void UpdateUser(User user);
        bool ActiveAccount(string activeCode);
        int GetUserIdByUserName(string userName);
        string SaveOrUpdateAvatar(IFormFile? UserAvatar, string? AvatarName);
        string GetUserNameById(int userId);
        string GetEmailById(int userId);
        void DeleteUser(int userId);


        #region User panel

        InformationUserViewModel GetUserInformation(string userName);
        public SideBarUserPanelViewModel GetSideBarUserPanelData(string userName);
        EditProfileViewModel GetDataForEditProfileUser(string userName);
        void EditProfile(string userName, EditProfileViewModel profile);
        bool CompareOldPassword(string OldPassword, string userName);
        void ChangeUserPassword(string newPassword,string userName);

        #endregion


        #region Wallet

        int BalanceUserWallet(string userName);
        List<WalletViewModel> GetWalletUser(string userName);
        int ChargeWallet(string userName,int amount,string discription, bool isPay=false);
        int AddWallet(Wallet wallet);
        Wallet GetWalletByWalletId(int walletId);
        void UpdateWallet(Wallet wallet);

        #endregion


        #region Admin Panel

        UsersForAdminViewModel GetUsers(int pageId = 1, string filterEmail = "", string filterUserName = "");
        UsersForAdminViewModel GetDeleteUsers(int pageId = 1, string filterEmail = "", string filterUserName = "");
        int AddUserFromAdmin(CreateUserViewModel user);
        EditUserViewModel GetUserForShowInEditMode(int userId);
        void EditUserFromAdmin(EditUserViewModel editUser);

        #endregion
    }
}
