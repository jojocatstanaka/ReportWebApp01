using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ReportWebApp01.Models
{
    // ApplicationUser クラスにさらにプロパティを追加すると、ユーザーのプロファイル データを追加できます。詳細については、https://go.microsoft.com/fwlink/?LinkID=317594 を参照してください。
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // authenticationType が CookieAuthenticationOptions.AuthenticationType で定義されているものと一致している必要があります
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // ここにカスタム ユーザー クレームを追加します
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<ReportWebApp01.Models.Department> Departments { get; set; }

        public System.Data.Entity.DbSet<ReportWebApp01.Models.Employee> Employees { get; set; }

        public System.Data.Entity.DbSet<ReportWebApp01.Models.Steps> Steps { get; set; }

        public System.Data.Entity.DbSet<ReportWebApp01.Models.Corprate> Corprates { get; set; }

        public System.Data.Entity.DbSet<ReportWebApp01.Models.FitbitAccount> FitbitAccounts { get; set; }
    }
}