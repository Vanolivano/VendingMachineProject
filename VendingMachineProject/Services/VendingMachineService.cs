using System;
using System.Linq;
using System.Text;
using VendingMachineProject.Models;
using VendingMachineProject.Models.Types;
using VendingMachineProject.Providers;

namespace VendingMachineProject.Services
{
    public class VendingMachineService : IVendingMachineService
    {
        private readonly IVendingMachineProvider _provider = new VendingMachineProvider();

        public void BuyProduct()
        {
            var assortment = _provider.GetAssortment();

            Console.WriteLine("Выберите продукт:");

            foreach ((ProductType productType, int count, int cost) in assortment)
            {
                Console.WriteLine($"{(int)productType} - {productType}({cost}$), в количестве: {count}");
            }
            var input = Console.ReadLine();

            if (!int.TryParse(input, out var intOutput))
            {
                Console.WriteLine("Неверный ввод. Попробуйте снова.");
                return;
            }

            try
            {
                var product = _provider.BuyProduct((ProductType)intOutput);
                Console.WriteLine($"Возьмите ваш: {product.ProductType}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void ReturnCoins()
        {
            var coins = _provider.ReturnCoins().GroupBy(x => x.CoinType).OrderByDescending(x => x.Key).ToDictionary(x => x.Key, x => x.Count());

            if (!coins.Any())
                Console.WriteLine("На вашем счету нет денежных средств.");

            var coinsReturned = new StringBuilder("Вот ваши монеты:\r\n");

            foreach (var coin in coins)
            {
                coinsReturned.AppendLine($"Номинал: {coin.Key}, количество: {coin.Value}");
            }

            Console.WriteLine(coinsReturned);
        }

        public void Intro()
        {
            Console.WriteLine("Список команд:");
            Console.WriteLine("1 - Получить текущую информацию о асортименте и узнать баланс.");
            Console.WriteLine("2 - Вставить монету.");
            Console.WriteLine("3 - Отмена. Вернуть монеты.");
            Console.WriteLine("4 - Купить продукт.");
            Console.WriteLine("5 - Сброс. Пополнить ассортимент автомата.");
            Console.WriteLine("X - Завершить работу.");
        }

        public void InsertCoin()
        {
            while (true)
            {
                Console.WriteLine("1 - Вставить монету: 1$.");
                Console.WriteLine("2 - Вставить монету: 2$.");
                Console.WriteLine("3 - Вставить монету: 5$.");
                Console.WriteLine("4 - Вставить монету: 10$.");
                Console.WriteLine("5 - Вернуться в главное меню.");
                var input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        Console.WriteLine($"Ваш текущий баланс: {_provider.PutCoin(new Coin { CoinType = CoinType.One })}.");
                        break;
                    case "2":
                        Console.WriteLine($"Ваш текущий баланс: {_provider.PutCoin(new Coin { CoinType = CoinType.Two })}.");
                        break;
                    case "3":
                        Console.WriteLine($"Ваш текущий баланс: {_provider.PutCoin(new Coin { CoinType = CoinType.Five })}.");
                        break;
                    case "4":
                        Console.WriteLine($"Ваш текущий баланс: {_provider.PutCoin(new Coin { CoinType = CoinType.Ten })}.");
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Неверный ввод. Попробуйте снова.");
                        break;
                }

            }
        }

        public void GetCurrentInfo()
        {
            var balance = _provider.GetBalance();

            var assortment = new StringBuilder("\r\n");

            var assortiment = _provider.GetAssortment();

            foreach ((ProductType productType, int productCount, int productPrice) in assortiment)
            {
                assortment.AppendLine($"{productType}({productPrice}$), в количестве: {productCount}");
            }
            Console.WriteLine($"Ваш баланс: {balance}. У нас в наличии: {assortment}");

        }

        public void HardReset()
        {
            _provider.HardReset();
            Console.WriteLine($"Ваш счет обнулен, список товаров пополнен.");
            GetCurrentInfo();
        }
    }
}
