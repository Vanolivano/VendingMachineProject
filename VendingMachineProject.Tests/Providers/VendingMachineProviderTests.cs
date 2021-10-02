using FluentAssertions;
using System.Collections.Generic;
using VendingMachineProject.Models;
using VendingMachineProject.Models.Types;
using VendingMachineProject.Providers;
using VendingMachineProject.Tests.Infrastructure;
using Xunit;

namespace VendingMachineProject.Tests.Providers
{
    public class VendingMachineProviderTests
    {
        [Theory]
        [InlineData(CoinType.One, 1)]
        [InlineData(CoinType.Two, 2)]
        [InlineData(CoinType.Five, 5)]
        [InlineData(CoinType.Ten, 10)]
        public void PutCoinTest(CoinType coinType, int expected)
        {
            var client = new VendingMachineProvider();
            var testCoin = new Coin { CoinType = coinType };

            var actual = client.PutCoin(testCoin);

            actual.Should().Be(expected);
        }

        [Theory]
        [InlineData(ProductType.Cocoa)]
        [InlineData(ProductType.Water)]
        [InlineData(ProductType.Tea)]
        [InlineData(ProductType.Coffee)]
        public void BuyProductTest(ProductType productType)
        {
            var client = new VendingMachineProvider();
            client.SetBalance(50);
            var expected = new Product { ProductType = productType };

            var actual = client.BuyProduct(productType);

            actual.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [MemberData(nameof(ReturnCoinsTestCases))]
        public void ReturnCoinsTest(int balance, List<Coin> expectedChange, Dictionary<CoinType, int> expectedCoinBalance)
        {
            var client = new VendingMachineProvider();
            client.SetBalance(balance);

            var actualChange = client.ReturnCoins();

            actualChange.Should().HaveCount(expectedChange.Count);
            actualChange.Should().BeEquivalentTo(expectedChange);

            client.GetBalance().Should().Be(0);

            var actualCoinBalance = client.GetCoinBalance();

            actualCoinBalance.Should().BeEquivalentTo(expectedCoinBalance);
        }

        public static IEnumerable<object[]> ReturnCoinsTestCases()
        {
            yield return new object[]
            {
                10, Fixture.GetExpectedCoins(countTen: 1), Fixture.GetExpectedCountCoinTypes(countTen: 4)
            };

            yield return new object[]
            {
                5, Fixture.GetExpectedCoins(countFive: 1), Fixture.GetExpectedCountCoinTypes(countFive: 4)
            };

            yield return new object[]
            {
                2, Fixture.GetExpectedCoins(countTwo: 1), Fixture.GetExpectedCountCoinTypes(countTwo: 4)
            };

            yield return new object[]
            {
                1, Fixture.GetExpectedCoins(countOne: 1), Fixture.GetExpectedCountCoinTypes(countOne: 4)
            };

            yield return new object[]
            {
                11, Fixture.GetExpectedCoins(countOne: 1, countTen: 1), Fixture.GetExpectedCountCoinTypes(countOne: 4, countTen: 4)
            };

            yield return new object[]
            {
                18, Fixture.GetExpectedCoins(countOne: 1, countTwo: 1, countFive: 1, countTen: 1), Fixture.GetExpectedCountCoinTypes(countOne: 4, countTwo: 4, countFive: 4, countTen: 4)
            };

            yield return new object[]
            {
                68, Fixture.GetExpectedCoins(countOne: 1, countTwo: 1, countFive: 3, countTen: 5), Fixture.GetExpectedCountCoinTypes(countOne: 4, countTwo: 4, countFive: 2, countTen: 0)
            };

            yield return new object[]
            {
                90, Fixture.GetExpectedCoins(countOne: 5, countTwo: 5, countFive: 5, countTen: 5), Fixture.GetExpectedCountCoinTypes(countOne: 0, countTwo: 0, countFive: 0, countTen: 0)
            };
        }
    }
}
