using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Data.SqlClient;

namespace OnlineTestingService.Models
{
    /// <summary>
    /// This class is used for managing users.
    /// </summary>
    public class UserManagement
    {
        public static string DbFile { get { return AppDomain.CurrentDomain.GetData("DataDirectory").ToString() + "\\ASPNETDB.MDF"; } }

        private SqlMembershipProvider membershipProvider;
        private SqlRoleProvider roleProvider;
        private static UserManagement instance;

        public static UserManagement GetInstance()
        {
            if (instance == null)
            {
                return instance = new UserManagement();
            }
            else
            {
                return instance;
            }
        }

        private UserManagement()
        {
            membershipProvider = (SqlMembershipProvider)Membership.Provider;
            roleProvider = (SqlRoleProvider)Roles.Provider;
        }
        /// <summary>
        /// Retrieves all users stored in database as Models/User object.
        /// </summary>
        /// <returns>List of all users as Model.User object.</returns>
        public List<User> GetAllUsers()
        {
            List<User> myUsers = new List<Models.User>();

            int total;
            List<MembershipUser> users = new List<MembershipUser>();
            if (GetCount() > 0)
            {
                MembershipUserCollection collection = membershipProvider.GetAllUsers(0, GetCount(), out total);
                foreach (MembershipUser user in collection)
                {
                    string[] roles = roleProvider.GetRolesForUser(user.UserName);
                    User myUser = new Models.User(user.UserName, user.Email, GetHashPassword(user), roles, user.ProviderUserKey);
                    myUsers.Add(myUser);
                }
            }            
            return myUsers;
        }
        /// <summary>
        /// There is no usage of this function now. Probably will be removed.
        /// </summary>
        /// <param name="user">User whose hash password you want to retrieve.</param>
        /// <returns>Hash password of user.</returns>
        private string GetHashPassword(MembershipUser user)
        {
            string hash = string.Empty;

            SqlConnection sqlConnection =
                new SqlConnection(
                    System.Configuration
                    .ConfigurationManager
                    .ConnectionStrings["ApplicationServices"]
                    .ConnectionString);
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand("SELECT Password FROM aspnet_Membership WHERE UserId = @userId",
                sqlConnection);
            sqlCommand.Parameters.AddWithValue("@userId", user.ProviderUserKey);

            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

            if (sqlDataReader.HasRows)
            {
                sqlDataReader.Read();
                if (!sqlDataReader.IsDBNull(0))
                {
                    hash = sqlDataReader.GetString(0);
                }
            }
            sqlDataReader.Close();
            sqlDataReader.Dispose();
            sqlCommand.Cancel();
            sqlCommand.Dispose();
            sqlConnection.Close();
            sqlConnection.Dispose();

            return hash;
        }

        /// <summary>
        /// Counts all users in database.
        /// </summary>
        /// <returns>Total number of users.</returns>
        protected int GetCount()
        {
            int total = 0;

            SqlConnection sqlConnection =
                new SqlConnection(
                    System.Configuration
                    .ConfigurationManager
                    .ConnectionStrings["ApplicationServices"]
                    .ConnectionString);
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand("SELECT COUNT(UserId) FROM aspnet_Users", sqlConnection);

            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

            if (sqlDataReader.HasRows)
            {
                sqlDataReader.Read();
                if (!sqlDataReader.IsDBNull(0))
                {
                    total = sqlDataReader.GetInt32(0);
                }
            }
            sqlDataReader.Close();
            sqlDataReader.Dispose();
            sqlCommand.Cancel();
            sqlCommand.Dispose();
            sqlConnection.Close();
            sqlConnection.Dispose();

            return total;
        }

        /// <summary>
        /// Adds user to database.
        /// </summary>
        /// <param name="user">User to add. Should have specified properties.</param>
        /// <param name="status">Status of adding operation.</param>
        internal void AddUser(User user, out MembershipCreateStatus status)
        {
            if (user.Password == null || user.Password == string.Empty)
            {
                user.Password = membershipProvider.GeneratePassword();
            }
            MembershipUser newUser = membershipProvider.CreateUser(
                user.UserName,
                user.Password,
                user.Email,
                null,
                null,
                true,
                user.UserID,
                out status);
            //string newPassword = newUser.ResetPassword();
            //send email with login and password.
            BusinessLogic.Mailer.Instance.SendCustomNotification(new BusinessLogic.Entities.EmailAddress(user.Email), "Dane do logowania", string.Format("Twój login: {0}. \n Twoje hasło: {1}", user.UserName, user.Password), null);
            AssignToRoles(user);
        }

        /// <summary>
        /// Assign myUser to roles based on his properties.
        /// </summary>
        /// <param name="myUser">User to assign.</param>
        internal void AssignToRoles(User myUser)
        {
            //MembershipUser dbUser = GetUser(myUser);
            if (myUser.GetRoles().Length > 0)
            {
                roleProvider.AddUsersToRoles(new string[1] { myUser.UserName }, myUser.GetRoles());
            }            
        }
        
        /// <summary>
        /// Gets a MembershipUser from a User.
        /// </summary>
        /// <param name="myUser"></param>
        /// <returns>MembershipUser from database.</returns>
        private MembershipUser GetUser(User myUser)
        {
            return membershipProvider.GetUser(myUser.UserName, false);
        }

        /// <summary>
        /// Depraceted.?
        /// </summary>
        /// <param name="myUser"></param>
        /// <returns></returns>
        internal string ResetPassword(User myUser)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deletes user by user name.
        /// </summary>
        /// <param name="userName">String with username.</param>
        /// <returns>Result of deleting.</returns>
        internal bool DeleteUser(string userName)
        {            
            return membershipProvider.DeleteUser(userName, true);
        }

        internal bool UpdateUser(User user)
        {
            bool updated = false;

            MembershipUser dbUser = GetUser(user);
            if (user.Email.Length > 0)
            {
                dbUser.Email = user.Email;
                membershipProvider.UpdateUser(dbUser);
            }
            //brutal way of update roles.
            //fisrtly clean all user's roles...
            if (roleProvider.GetRolesForUser(dbUser.UserName).Length > 0)
            {
                roleProvider.RemoveUsersFromRoles(new string[1] { dbUser.UserName },
                    roleProvider.GetRolesForUser(dbUser.UserName));
            }
            //... then add user to new roles.
            if (user.GetRoles().Length > 0)
            {
                roleProvider.AddUsersToRoles(new string[1] { dbUser.UserName },
                    user.GetRoles());
            }

            return updated;
        }

        public void Setup()
        {
            roleProvider.CreateRole(User.ADMIN);
            roleProvider.CreateRole(User.CANDIDATE_MANAGER);
            roleProvider.CreateRole(User.TEST_DEFINER);
            roleProvider.CreateRole(User.TEST_REVIEWER);
        }
    }
}