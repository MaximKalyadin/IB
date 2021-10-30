using IB1.Models;
using IB1.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace IB1
{
    /// <summary>
    /// Логика взаимодействия для ClientWindow.xaml
    /// </summary>
    public partial class ClientWindow : Window
    {
        private List<Client> clients = new List<Client>();
        Client client = new Client();
        /// <summary>
        /// ограничение на выбор пользоателем пароля(регулярные выражения)
        /// </summary>
        string pattern = @"([a-zA-Z0-9а-яА-Я]*[,;.-:!?]*)+";

        private readonly string path = @"C:\Users\Maxim\Desktop\ИБ\file.json";
        private readonly string pathEncrypt = @"C:\Users\Maxim\Desktop\ИБ\file.des";

        public ClientWindow(Client client)
        {
            InitializeComponent();
            DeserializedJson();
            this.client = client;
        }

        /// <summary>
        /// Десериализация json (временного файла)
        /// </summary>
        private void DeserializedJson()
        {
            if (File.Exists(pathEncrypt))
            {
                DES.DecryptFile(pathEncrypt, path);
                string json = File.ReadAllText(path);
                clients = JsonConvert.DeserializeObject<List<Client>>(json);
            }
        }


        /// <summary>
        /// Событие кнопки смены пароля
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (var el in clients)
            {
                if (el.Name.Equals(client.Name))
                {
                    if (string.IsNullOrEmpty(old_passw.Password))
                    {
                        if (!el.Password.Equals(old_passw.Password))
                        {
                            Incorrect("Вы не ввели старый пароль, введите старый пароль");
                        }
                    }
                    if (string.IsNullOrEmpty(new_passw.Password))
                    {
                        Incorrect("Для смены пароля введите новый пароль");
                    }
                    else if (string.IsNullOrEmpty(conf_passw.Password))
                    {
                        Incorrect("Введите подтврждение пароля");
                    }
                    else if (!string.IsNullOrEmpty(new_passw.Password) && new_passw.Password.Equals(conf_passw.Password))
                    {
                        if (el.IsLimit)
                        {
                            if (Regex.IsMatch(new_passw.Password, pattern, RegexOptions.IgnoreCase))
                            {
                                if (el.Password.Equals(""))
                                {
                                    el.Password = DES.ToSHA256(new_passw.Password);
                                    MessageBox.Show("Вы успешно сменили пароль", "Successfully", MessageBoxButton.OK, MessageBoxImage.None);
                                    old_passw.Password = "";
                                    new_passw.Password = "";
                                    conf_passw.Password = "";
                                }
                                else if (DES.ToSHA256(old_passw.Password).Equals(el.Password))
                                {
                                    el.Password = DES.ToSHA256(new_passw.Password);
                                    MessageBox.Show("Вы успешно сменили пароль", "Successfully", MessageBoxButton.OK, MessageBoxImage.None);
                                    old_passw.Password = "";
                                    new_passw.Password = "";
                                    conf_passw.Password = "";
                                }
                                else
                                {
                                    Incorrect("Данные не совпадают, введите все заново и не ошибайтесь");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Пароль должен содержать буквы, цифры и знаки препинания", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                        else
                        {
                            if (el.Password.Equals(""))
                            {
                                el.Password = DES.ToSHA256(new_passw.Password);
                                MessageBox.Show("Вы успешно сменили пароль", "Successfully", MessageBoxButton.OK, MessageBoxImage.None);
                                old_passw.Password = "";
                                new_passw.Password = "";
                                conf_passw.Password = "";
                            } 
                            else if (DES.ToSHA256(old_passw.Password).Equals(el.Password))
                            {
                                el.Password = DES.ToSHA256(new_passw.Password);
                                MessageBox.Show("Вы успешно сменили пароль", "Successfully", MessageBoxButton.OK, MessageBoxImage.None);
                                old_passw.Password = "";
                                new_passw.Password = "";
                                conf_passw.Password = "";
                            } 
                            else
                            {
                                Incorrect("Данные не совпадают, введите все заново и не ошибайтесь");
                            }
                        }
                    }
                    else
                    {
                        Incorrect("Данные не совпадают, введите все заново и не ошибайтесь");
                    }
                }
            }
        }

        /// <summary>
        /// метод для вывода различного вывода сообщений об ошибках
        /// </summary>
        /// <param name="text">Сообщение ошибки</param>
        private void Incorrect(string text)
        {
            MessageBox.Show(text, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            old_passw.Password = "";
            new_passw.Password = "";
            conf_passw.Password = "";
        }

        /// <summary>
        /// событие выхода из окна клиента (нажатие на кнопки выйти)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }


        /// <summary>
        /// событие закрытие окна клиента, чтобы все дейсвия во временном файле сохранить, удалить файл и данные перенести в зашифрованный
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closed(object sender, EventArgs e)
        {
            string json = JsonConvert.SerializeObject(clients);
            File.Delete(path);
            using (StreamWriter tw = new StreamWriter(path, true))
            {
                tw.WriteLine(json.ToString());
                tw.Close();
            }
            DES.EncryptFile(path, pathEncrypt);
            File.Delete(path);
        }
    }
}
