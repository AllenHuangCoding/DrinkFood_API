using DrinkFood_API.Models;
using DrinkFood_API.Repository;
using DrinkFood_API.Utility;
using DataBase.Entities;
using CodeShare.Libs.BaseProject;

namespace DrinkFood_API.Services
{
    public class ImportService : BaseService
    {
        [Inject] private readonly BrandRepository _brandRepository;

        [Inject] private readonly StoreRepository _storeRepository;

        [Inject] private readonly OrderDetailRepository _orderDetailRepository;

        [Inject] private readonly OptionRepository _optionRepository;

        [Inject] private readonly AccountRepository _accountRepository;

        [Inject] private readonly DrinkFoodRepository _drinkFoodRepository;

        [Inject] private readonly CodeTableRepository _codeTableRepository;

        public ImportService(IServiceProvider provider) : base(provider)
        {
            provider.Inject(this);
        }

        public void ReadBrandStoreExcel(IFormFile File)
        {
            var brandType = _codeTableRepository.GetListByType("BrandType");

            var menuArea = _codeTableRepository.GetListByType("MenuArea");

            ReadCsv.Convert<BrandStoreCsvColumn>(File, (Records) =>
            {
                foreach (var item in Records)
                {
                    var brand = _brandRepository.FindOne(x =>
                        x.B_name == item.品牌名字
                    );
                    if (brand == null && brandType.FirstOrDefault(x => x.CT_desc == item.品牌類型) != null)
                    {
                        brand = _brandRepository.Create(new Brand
                        {
                            B_name = item.品牌名字,
                            B_line_id = item.品牌Line,
                            B_logo = item.品牌Logo網址,
                            B_official_url = item.品牌官網網址,
                            B_type = brandType.First(x => x.CT_desc == item.品牌類型).CT_id
                        });
                    }


                    var store = _storeRepository.FindOne(x =>
                        x.S_address == item.店家地址
                    );
                    if (store == null && brand != null && menuArea.FirstOrDefault(x => x.CT_desc == item.菜單區域) != null)
                    {
                        _storeRepository.Create(new Store
                        {
                            S_brand_id = brand.B_id,
                            S_name = item.店家名,
                            S_address = item.店家地址,
                            S_phone = item.店家電話,
                            S_menu_area_id = menuArea.First(x => x.CT_desc == item.菜單區域).CT_id
                        });
                    }
                }
            });
        }

        // Bard AI 指令：幫我把菜單上的飲料品項與價格整理成 "品項"、"價格" 欄位。若有分M或者L容量，要在品項內容後面加上(M)或(L)
        // 幫我把菜單上每種飲料的品項都分成兩欄，第一欄是品項名稱，第二欄是價格。如果有分M或者L容量的飲料，會在品項名稱後面加上 (M)或(L)，並在價格欄位中標示出相對應的金額。
        public void ReadDrinkFoodExcel(Guid MenuID, IFormFile File)
        {

        }

        public void ReadOrderDetailExcel(Guid OrderID, IFormFile File)
        {
            var options = _optionRepository.GetAll();
            var accounts = _accountRepository.GetAll();
            var drinkFoods = _drinkFoodRepository.GetAll();
            var payments = _codeTableRepository.GetListByType("Payment");

            ReadCsv.Convert<OrderDetailCsvColumn>(File, (Records) =>
            {
                foreach (var item in Records)
                {
                    Guid? drinkFoodID = drinkFoods.FirstOrDefault(x => x.DF_name.Contains(item.DrinkFoodName) && x.DF_price == item.Price)?.DF_id;
                    Guid? accountID = accounts.FirstOrDefault(x => x.A_brief == item.Name || x.A_name == item.Name)?.A_id;
                    Guid? sugarID = options.FirstOrDefault(x => x.O_name.Contains(item.Sugar))?.O_id;
                    Guid? iceID = options.FirstOrDefault(x => x.O_name.Contains(item.Ice))?.O_id;
                    Guid? sizeID = options.FirstOrDefault(x => x.O_name.Contains(item.Size))?.O_id;
                    Guid? paymentID = payments.FirstOrDefault(x => x.CT_desc.Contains(item.Payment))?.CT_id;

                    if (drinkFoodID.HasValue && accountID.HasValue && sugarID.HasValue && iceID.HasValue && sizeID.HasValue && paymentID.HasValue)
                    {
                        _orderDetailRepository.Create(new OrderDetail
                        {
                            OD_order_id = OrderID,
                            OD_drink_food_id = drinkFoodID.Value,
                            OD_sugar_id = sugarID.Value,
                            OD_ice_id = iceID.Value,
                            OD_size_id = sizeID.Value,
                            OD_payment_id = paymentID.Value,
                            OD_payment_datetime = DateTime.Now,
                            OD_account_id = accountID.Value,
                        });
                    }
                }
            });
        }


    }
}
