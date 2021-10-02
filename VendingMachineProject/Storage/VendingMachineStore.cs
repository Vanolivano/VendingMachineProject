using System.Collections.Generic;
using VendingMachineProject.Models;
using VendingMachineProject.Models.Types;

namespace VendingMachineProject.Storages
{
    public static class VendingMachineStore
    {
        public static int CurentBalance { get; set; }

        public static List<Coin> Coins { get; set; }

        public static List<Product> Products { get; set; }

        public static Dictionary<ProductType, int> Prices { get; set; }
    }
}
