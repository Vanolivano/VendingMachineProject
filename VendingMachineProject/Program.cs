using System;
using VendingMachineProject.Services;

namespace VendingMachineProject
{
    class Program
    {
        private static readonly IVendingMachineService _service = new VendingMachineService();
        static void Main(string[] args)
        {
            Console.WriteLine("Вендинговый Автомат!");

            while (true)
            {
                _service.Intro();
                var input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        _service.GetCurrentInfo();
                        break;

                    case "2":
                        _service.InsertCoin();
                        break;

                    case "3":
                        _service.ReturnCoins();
                        break;

                    case "4":
                        _service.BuyProduct();
                        break;

                    case "5":
                        _service.HardReset();
                        break;
                    case "x":
                    case "X":
                        return;

                    default:
                        Console.WriteLine("Неверный ввод. Попробуйте снова.");
                        break;
                }
            }
        }
    }
}
