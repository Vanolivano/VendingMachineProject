using System.Collections.Generic;
using VendingMachineProject.Models;
using VendingMachineProject.Models.Types;

namespace VendingMachineProject.Providers
{
    public interface IVendingMachineProvider
    {
        /// <summary>
        /// Получить информацию о текущем балансе.
        /// </summary>
        public int GetBalance();

        /// <summary>
        /// Получить текущий список товаров.
        /// </summary>
        /// <returns>Список товаров(Тип товара, количество товара, цена товара) </returns>
        public List<(ProductType, int, int)> GetAssortment();

        /// <summary>
        /// Вставить монету в автомат.
        /// </summary>
        /// <param name="coin">Монета.</param>
        /// <returns>Текущий баланс.</returns>
        public int PutCoin(Coin coin);

        /// <summary>
        /// Вернуть монеты с текущего баланса.
        /// </summary>
        public List<Coin> ReturnCoins();

        /// <summary>
        /// Купить продукт.
        /// </summary>
        /// <param name="productType">Тип покупаемого продукта.</param>
        public Product BuyProduct(ProductType productType);

        /// <summary>
        /// Операция сброса к исходному кол-ву товаров и денег.
        /// </summary>
        public void HardReset();
    }
}
