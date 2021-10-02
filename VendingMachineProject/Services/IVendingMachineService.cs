namespace VendingMachineProject.Services
{
    public interface IVendingMachineService
    {
        /// <summary>
        /// Перейти в раздел покупки товаров.
        /// </summary>
        public void BuyProduct();

        /// <summary>
        /// Получить текущую информацию:
        /// Баланс, количество и стоимость товаров.
        /// </summary>
        public void GetCurrentInfo();

        /// <summary>
        /// Начать операцию по внесению монет в автомат.
        /// </summary>
        public void InsertCoin();

        /// <summary>
        /// Получить список возможных команд.
        /// </summary>
        public void Intro();

        /// <summary>
        /// Вернуть монеты.
        /// </summary>
        public void ReturnCoins();

        /// <summary>
        /// Сбросить баланс, пополнить автомат.
        /// </summary>
        public void HardReset();

    }
}
