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

namespace IB1
{
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        private List<Client> clients = new List<Client>();

        public AdminWindow()
        {
            InitializeComponent();
            DeserializedJson();
        }

        private void DeserializedJson()
        {
            string path = @"C:\Users\Maxim\Desktop\ИБ\file.json";

            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                clients = JsonConvert.DeserializeObject<List<Client>>(json);
            }
            LoadData();
        }

        private void LoadData()
        {
            listbox.ItemsSource = null;
            listbox.ItemsSource = clients;
        }

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
            else if(!string.IsNullOrEmpty(new_passw.Password) && new_passw.Password.Equals(confirm_passw.Password) && old_passw.Password.Equals(clients[0].Password))
            {
                clients[0].Password = new_passw.Password;
                MessageBox.Show("Вы успешно сменили пароль", "Successfully", MessageBoxButton.OK, MessageBoxImage.None);
                old_passw.Password = "";
                new_passw.Password = "";
                confirm_passw.Password = "";
            }
            else
            {
                Incorrect("Данные не совпадают, введите все заново и не ошибайтесь");
            }

        }

        private void Incorrect(string text)
        {
            MessageBox.Show(text, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            old_passw.Password = "";
            new_passw.Password = "";
            confirm_passw.Password = "";
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            string path = @"C:\Users\Maxim\Desktop\ИБ\file.json";
            if (File.Exists(path))
            {
                string json = JsonConvert.SerializeObject(clients);
                File.Delete(path);
                using (StreamWriter tw = new StreamWriter(path, true))
                {
                    tw.WriteLine(json.ToString());
                    tw.Close();
                }
            }
        }
    }
}
