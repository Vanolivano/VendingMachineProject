using System;
using System.Collections.Generic;
using System.Linq;
using VendingMachineProject.Models;
using VendingMachineProject.Models.Types;
using VendingMachineProject.Storages;

namespace VendingMachineProject.Providers
{
    public class VendingMachineProvider : IVendingMachineProvider
    {
        public VendingMachineProvider()
        {
            HardReset();
        }
        public Product BuyProduct(ProductType productType)
        {
            var product = VendingMachineStore.Products.FirstOrDefault(x => x.ProductType == productType);

            if (product == null)
                throw new InvalidOperationException($"Товар: {productType} нет в наличии.");

            var price = VendingMachineStore.Prices[product.ProductType];

            if (VendingMachineStore.CurentBalance < price)
                throw new InvalidOperationException("У вас на счету не достаточно средств для совершения покупки.");

            if(!IsCanGiveChange(VendingMachineStore.CurentBalance - price))
                throw new InvalidOperationException("Покупка отменена, автомат не сможет выдать сдачу.");

            VendingMachineStore.CurentBalance -= price;
            VendingMachineStore.Products.Remove(product);

            return product;
        }

        public List<(ProductType, int, int)> GetAssortment()
        {
            var result = new List<(ProductType, int, int)>();

            foreach(var productTypeGroup in VendingMachineStore.Products.GroupBy(x => x.ProductType))
            {
                var productCount = productTypeGroup.Count();
                var productPrice = VendingMachineStore.Prices[productTypeGroup.Key];

                result.Add((productTypeGroup.Key, productCount, productPrice));
            }

            return result.OrderBy(x=>x.Item1).ToList();
        }

        public int GetBalance() => VendingMachineStore.CurentBalance;

        public void HardReset()
        {
            VendingMachineStore.Coins = new List<Coin>();
            VendingMachineStore.Products = new List<Product>();
            VendingMachineStore.CurentBalance = 0;
            VendingMachineStore.Prices = new Dictionary<ProductType, int> 
            {
                {ProductType.Cocoa, 50 },
                {ProductType.Coffee, 48 },
                {ProductType.Tea, 35 },
                {ProductType.Water, 22 }
            };

            for (int i = 0; i < 5; i++)
            {
                VendingMachineStore.Coins.AddRange(new List<Coin>()
                {
                    new Coin { CoinType = CoinType.One },
                    new Coin { CoinType = CoinType.Two },
                    new Coin { CoinType = CoinType.Five },
                    new Coin { CoinType = CoinType.Ten }
                });

                VendingMachineStore.Products.AddRange(new List<Product>()
                {
                    new Product { ProductType = ProductType.Cocoa},
                    new Product { ProductType = ProductType.Coffee},
                    new Product { ProductType = ProductType.Tea},
                    new Product { ProductType = ProductType.Water}
                });
            }
        }

        public int PutCoin(Coin coin)
        {
            return VendingMachineStore.CurentBalance += (int)coin.CoinType;
        }

        public List<Coin> ReturnCoins()
        {
            var coinCounts = VendingMachineStore.Coins.GroupBy(x => x.CoinType).OrderByDescending(x=>x.Key).ToDictionary(x => x.Key, x => x.Count());

            if (VendingMachineStore.CurentBalance == 0)
                return new List<Coin>();

            var outputs = new List<Coin>();

            foreach(var item in coinCounts)
            {
                if (item.Value == 0)
                    continue;

                var count = Math.DivRem(VendingMachineStore.CurentBalance, (int)item.Key, out var remainder);

                if (count == 0)
                    continue;

                count = Math.Min(count, item.Value);

                for(int i = 0; i < count; i++)
                {
                    var coin = VendingMachineStore.Coins.First(x => x.CoinType == item.Key);
                    VendingMachineStore.Coins.Remove(coin);

                    outputs.Add(coin);
                }

                VendingMachineStore.CurentBalance -= count * (int)item.Key;

                if (VendingMachineStore.CurentBalance == 0)
                    break;
            }

            VendingMachineStore.CurentBalance = 0;
            return outputs;
        }

        public void SetBalance(int sum) => VendingMachineStore.CurentBalance = sum;

        public Dictionary<CoinType, int> GetCoinBalance() =>
            VendingMachineStore.Coins.GroupBy(x => x.CoinType).OrderByDescending(x => x.Key).ToDictionary(x => x.Key, x => x.Count());

        private bool IsCanGiveChange(int sum)
        {
            if (sum == 0)
                return true;

            var coinCounts = VendingMachineStore.Coins.GroupBy(x => x.CoinType).OrderByDescending(x => x.Key).ToDictionary(x => x.Key, x => x.Count());
            
            foreach(var coinTypeCount in coinCounts)
            {
                if (coinTypeCount.Value == 0)
                    continue;

                var coinNumber = Math.DivRem(sum, (int)coinTypeCount.Key, out var remainder);

                if (coinNumber == 0)
                    continue;

                coinNumber = Math.Min(coinNumber, coinTypeCount.Value);

                sum -= coinNumber * (int)coinTypeCount.Key;
                if (sum == 0)
                    return true;
            }
            return false;
        }
    }
}
