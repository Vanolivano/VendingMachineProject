using System.Collections.Generic;
using System.Linq;
using VendingMachineProject.Models;
using VendingMachineProject.Models.Types;

namespace VendingMachineProject.Tests.Infrastructure
{
    public static class Fixture
    {
        public static Dictionary<CoinType, int> GetExpectedCountCoinTypes(int countOne = 5, int countTwo = 5, int countFive = 5, int countTen = 5)
        {
            var result = new Dictionary<CoinType, int>
            {
                { CoinType.One, countOne },
                { CoinType.Two, countTwo },
                { CoinType.Five, countFive },
                { CoinType.Ten, countTen }
            };

            return result.Where(x => x.Value != 0).ToDictionary(x=>x.Key, x=>x.Value);
        }

        public static List<Coin> GetExpectedCoins(int countOne = 0, int countTwo = 0, int countFive = 0, int countTen = 0)
        {
            var coins = new List<Coin>();
            coins.MultipleAdd(CoinType.One, countOne);
            coins.MultipleAdd(CoinType.Two, countTwo);
            coins.MultipleAdd(CoinType.Five, countFive);
            coins.MultipleAdd(CoinType.Ten, countTen);
            return coins;
        }

        private static void MultipleAdd(this List<Coin> coins, CoinType coinType, int number)
        {
            for (int i = 0; i < number; i++)
            {
                coins.Add(new Coin { CoinType = coinType });
            }
        }
    }
}
