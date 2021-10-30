using IB1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Newtonsoft.Json;
using System.IO;
using IB1.Service;

namespace IB1
{
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        private List<Client> clients = new List<Client>();
        private readonly string path = @"C:\Users\Maxim\Desktop\ИБ\file.json";
        private readonly string pathEncrypt = @"C:\Users\Maxim\Desktop\ИБ\file.des";


        public AdminWindow()
        {
            InitializeComponent();
            DeserializedJson();
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
            LoadData();
        }

        /// <summary>
        /// метод отображение списка пользователей слева
        /// </summary>
        private void LoadData()
        {
            listbox.ItemsSource = null;
            listbox.ItemsSource = clients;
        }

        /// <summary>
        /// событие добавление нового пользователя
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(add_client.Text))
            {
                Incorrect("Для добавления нового пользователя введите его имя");
            } 
            else
            {
                clients.Add(new Client()
                {
                    Id = clients.Count,
                    Name = add_client.Text,
                    Login = add_client.Text,
                    Password = "",
                    IsAdmin = false,
                    IsBlock = false,
                    IsLimit = false
                });
                add_client.Text = "";
                LoadData();
            }
        }

        /// <summary>
        /// событие блокировки пользователя
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(block_client.Text))
            {
                Incorrect("Для блокировки пользователя введите его имя");
            } 
            else
            {
                bool temp = false;
                foreach (var el in clients)
                {
                    if (el.Name.Equals(block_client.Text))
                    {
                        el.IsBlock = true;
                        temp = true;
                        block_client.Text = "";
                        LoadData();
                    }
                }
                if (!temp)
                {
                    Incorrect("Пользователь не найден");
                }
            }
        }

        /// <summary>
        /// событие наложение ограничений пользователя
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(add_restrict.Text))
            {
                Incorrect("Для ввода ограничений пользователя введите его имя");
            }
            else
            {
                bool temp = false;
                foreach (var el in clients)
                {
                    if (el.Name.Equals(add_restrict.Text))
                    {
                        el.IsLimit = true;
                        temp = true;
                        add_restrict.Text = "";
                        LoadData();
                    }
                }
                if (!temp)
                {
                    Incorrect("Пользователь не найден");
                }
            }
        }

        /// <summary>
        /// событие смены пароля админа
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(old_passw.Password))
            {
                if (!clients[0].Password.Equals(old_passw.Password))
                {
                    Incorrect("Вы не ввели старый пароль, введите старый пароль");
                }
            }
            if (string.IsNullOrEmpty(new_passw.Password))
            {
                Incorrect("Для смены пароля введите новый пароль");
            }
            else if(string.IsNullOrEmpty(confirm_passw.Password))
            {
                Incorrect("Введите подтврждение пароля");
            }
            else if(!string.IsNullOrEmpty(new_passw.Password) && new_passw.Password.Equals(confirm_passw.Password))
            {
                if (clients[0].Password.Equals(""))
                {
                    clients[0].Password = DES.ToSHA256(new_passw.Password);
                    MessageBox.Show("Вы успешно сменили пароль", "Successfully", MessageBoxButton.OK, MessageBoxImage.None);
                    old_passw.Password = "";
                    new_passw.Password = "";
                    confirm_passw.Password = "";
                } else if (DES.ToSHA256(old_passw.Password).Equals(clients[0].Password))
                {
                    clients[0].Password = DES.ToSHA256(new_passw.Password);
                    MessageBox.Show("Вы успешно сменили пароль", "Successfully", MessageBoxButton.OK, MessageBoxImage.None);
                    old_passw.Password = "";
                    new_passw.Password = "";
                    confirm_passw.Password = "";

                } else
                {
                    Incorrect("Данные не совпадают, введите все заново и не ошибайтесь");
                }
            }
            else
            {
                Incorrect("Данные не совпадают, введите все заново и не ошибайтесь");
            }

        }

        /// <summary>
        ///  метод для вывода различного вывода сообщений об ошибках
        /// </summary>
        /// <param name="text">Сообщение ошибки</param>
        private void Incorrect(string text)
        {
            MessageBox.Show(text, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            old_passw.Password = "";
            new_passw.Password = "";
            confirm_passw.Password = "";
        }

        /// <summary>
        /// событие выхода из окна клиента (нажатие на кнопки выйти)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        /// <summary>
        /// событие закрытие окна админа, чтобы все дейсвия во временном файле сохранить, удалить файл и данные перенести в зашифрованный
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closed(object sender, EventArgs e)
        {
            string json = JsonConvert.SerializeObject(clients);
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
