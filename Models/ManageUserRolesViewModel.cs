using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace SMS.Models
{
    public class ManageUserRolesViewModel
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public bool Selected { get; set; }
    }
}
