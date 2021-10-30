using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IB1.Models
{
    /// <summary>
    /// Модель клиента
    /// </summary>
    public class Client
    {
        /// <summary>
        /// Уникальный идентификационный номер
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Флаг блокировки пользователя
        /// </summary>
        public bool IsBlock { get; set; }
        /// <summary>
        /// Флаг наложение ограничений на пользователя
        /// </summary>
        public bool IsLimit { get; set; }
        /// <summary>
        /// Логин пользователя/админа
        /// </summary>
        public string Login { get; set; }
        /// <summary>
        /// пароль пользователя/админа
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// флаг, показывающий заход на форму под админом или нет
        /// </summary>
        public bool IsAdmin { get; set; }
    }
}
