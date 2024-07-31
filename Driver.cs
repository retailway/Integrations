using RetailTypes;
using System;
using RetailTypes.Elements;

namespace RetailLib
{
    public abstract class Driver: IDisposable
    {
        #region Статические переменные
        /// <summary>
        /// ИНН ОФД
        /// </summary>
        public static string FiscalOperator;
        /// <summary>
        /// ИНН компании
        /// </summary>
        public static string CompanyVatin;
        /// <summary>
        /// Регистрационный номер ККТ
        /// </summary>
        public static string RegNumber;
        /// <summary>
        /// Номер ФН
        /// </summary>
        public static string TotalStorage;
        /// <summary>
        /// Создавать электронные документы
        /// </summary>
        public static bool IsElectronically;
        #endregion
        #region Абстрактные переменные
        public abstract bool IsConnected { get; }
        #endregion
        #region Абстрактные методы
        /// <summary>
        /// Деактивация драйвера
        /// </summary>
        public abstract void Dispose();
        /// <summary>
        /// Открытие смены
        /// </summary>
        public abstract void StartSession();
        /// <summary>
        /// Закрытие смены
        /// </summary>
        public abstract void EndSession();
        /// <summary>
        /// Выгрузка данных
        /// </summary>
        public abstract void PullInfo();
        /// <summary>
        /// Открытие чека
        /// </summary>
        /// <param name="receipt">Информация о чеке</param>
        public abstract void OpenReceipt(Receipt receipt);
        /// <summary>
        /// Добавление позиции
        /// </summary>
        /// <param name="pos">Информация о позиции</param>
        public abstract void AddPosition(Position pos);
        /// <summary>
        /// Добавление позиции
        /// </summary>
        /// <param name="pos">Информация о позиции</param>
        public abstract void SetCodes(Codes codes);
        /// <summary>
        /// Закрытие чека
        /// </summary>
        /// <param name="receipt">Информации о чеке</param>
        public abstract void CloseReceipt(Receipt receipt);
        /// <summary>
        /// Округление итога чека
        /// </summary>
        public abstract void RoundSum(Position[] positions);
        /// <summary>
        /// Отмена чека
        /// </summary>
        public abstract void CancelReceipt();
        /// <summary>
        /// Оплата чека
        /// </summary>
        /// <param name="type">Вид оплаты</param>
        /// <param name="sum">Сумма оплаты</param>
        public abstract void Payment(int type, int sum);
        /// <summary>
        /// Авторизация
        /// </summary>
        /// <param name="cashier">ФИО кассира</param>
        /// <param name="vatin">ИНН кассира</param>
        /// <param name="passwd">Пароль</param>
        public abstract void Auth(string cashier, string vatin, string passwd);
        #endregion
    }
}
