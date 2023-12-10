using DataBase.Entities;
using Microsoft.EntityFrameworkCore;
using CodeShare.Libs.GenericEntityFramework;
using DataBase.View;

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

        #region View
        public virtual DbSet<ViewAccount> ViewAccount { get; set; }
        public virtual DbSet<ViewDrinkFood> ViewDrinkFood { get; set; }
        public virtual DbSet<ViewMenu> ViewMenu { get; set; }
        public virtual DbSet<ViewOffice> ViewOffice { get; set; }
        public virtual DbSet<ViewOrder> ViewOrder { get; set; }
        public virtual DbSet<ViewOrderDetail> ViewOrderDetail { get; set; }
        public virtual DbSet<ViewStore> ViewStore { get; set; }

        #endregion
    }
}