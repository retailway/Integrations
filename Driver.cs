using System;

namespace RetailLib
{
    public abstract class Driver: IDisposable
    {
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
    }
}
