using DataBase.Entities;
using Microsoft.EntityFrameworkCore;
using CodeShare.Libs.GenericEntityFramework;

namespace DataBase
{
    public class EFContext : ReadWriteEFContext
    {
        public virtual DbSet<Account> Account { get; set; }

        public virtual DbSet<AccountLogin> AccountLogin { get; set; }

        public virtual DbSet<OfficeMember> AccoutOfficeDetail { get; set; }

        public virtual DbSet<ApiLog> ApiLog { get; set; }

        public virtual DbSet<Brand> Brand { get; set; }

        public virtual DbSet<CodeTable> CodeTable { get; set; }

        public virtual DbSet<Company> Company { get; set; }

        public virtual DbSet<DrinkFood> DrinkFood { get; set; }

        public virtual DbSet<DrinkFoodScore> DrinkFoodScore { get; set; }

        public virtual DbSet<Menu> Menu { get; set; }

        public virtual DbSet<Office> Office { get; set; }

        public virtual DbSet<OfficeMember> OfficeMember { get; set; }

        public virtual DbSet<Option> Option { get; set; }

        public virtual DbSet<OptionDetail> OptionDetail { get; set; }

        public virtual DbSet<Order> Order { get; set; }

        public virtual DbSet<OrderDetail> OrderDetail { get; set; }

        public virtual DbSet<Store> Store { get; set; }
    }
}