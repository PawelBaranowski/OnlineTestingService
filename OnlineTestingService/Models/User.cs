using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineTestingService.Models
{
    public class User
    {
        public Object UserID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsCandidateManager { get; set; }
        public bool IsTestDefiner { get; set; }
        public bool IsTestReviewer { get; set; }
        public bool IsCandidate { get; set; }

        public const string ADMIN = "Admin";
        public const string CANDIDATE_MANAGER = "CandidateManager";
        public const string TEST_DEFINER = "TestDefiner";
        public const string TEST_REVIEWER = "TestReviewer";
        public const string CANDIDATE = "Candidate";

        public User()
        {
        }

        public User(string userName, string email, string password, string[] roles, Object userId = null)
        {
            if (this.UserID == null)
            {
                this.UserID = userId;
            }
            else
            {
                this.UserID = System.Guid.NewGuid();
            }
            this.UserName = userName;
            this.Email = email;
            this.Password = password;
            IsAdmin = false;
            IsCandidateManager = false;
            IsTestDefiner = false;
            IsTestReviewer = false;
            SetRoles(roles);
        }

        /// <summary>
        /// Gets roles of the user.
        /// </summary>
        /// <param name="myUser">User whose roles you want to retrieve.</param>
        /// <returns>Table with names of roles which user has.</returns>
        public string[] GetRoles()
        {
            List<string> roles = new List<string>();
            if (IsAdmin)
            {
                roles.Add(ADMIN);
            }
            if (IsCandidateManager)
            {
                roles.Add(CANDIDATE_MANAGER);
            }
            if (IsTestDefiner)
            {
                roles.Add(TEST_DEFINER);
            }
            if (IsTestReviewer)
            {
                roles.Add(TEST_REVIEWER);
            }
            if (IsCandidate)
            {
                roles.Add(CANDIDATE);
            }
            return roles.ToArray();
        }

        internal void SetRoles(string[] roles)
        {
            foreach (string role in roles)
            {
                switch (role)
                {
                    case "Admin":
                        IsAdmin = true;
                        break;
                    case "CandidateManager":
                        IsCandidateManager = true;
                        break;
                    case "TestDefiner":
                        IsTestDefiner = true;
                        break;
                    case "TestReviewer":
                        IsTestReviewer = true;
                        break;
                    case CANDIDATE:
                        IsCandidate = true;
                        break;
                    default:
                        break;
                }
            }
        }
    }


}