using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using IB1.Models;
using IB1.Service;
using Newtonsoft.Json;

namespace IB1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int IsChecked = 0;
        private List<Client> clients = new List<Client>();
        readonly string path = @"C:\Users\Maxim\Desktop\ИБ\file.json";
        private readonly string pathEncrypt = @"C:\Users\Maxim\Desktop\ИБ\file.des";

        public MainWindow()
        {
            InitializeComponent();
            CreateOrUpdateFile();
        }

        public void CreateOrUpdateFile()
        {

            if (File.Exists(pathEncrypt))
            {
                DES.DecryptFile(pathEncrypt, path);
                string json = File.ReadAllText(path);
                clients = JsonConvert.DeserializeObject<List<Client>>(json);
            }
            else
            {
                Client client = new Client()
                {
                    Name = "admin",
                    Id = 1,
                    IsAdmin = true,
                    IsBlock = false,
                    IsLimit = false,
                    Login = "admin",
                    Password = ""
                };
                clients.Add(client);
                string json = JsonConvert.SerializeObject(clients);
                using (StreamWriter tw = new StreamWriter(path, true))
                {
                    tw.WriteLine(json.ToString());
                    tw.Close();
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (login.Text.ToLower().Equals("admin"))
            {
                if (string.IsNullOrEmpty(clients[0].Password))
                {
                    AdminWindow admin = new AdminWindow();
                    admin.Show();
                    Close();
                }
                else if (DES.ToSHA256(passw.Password).Equals(clients[0].Password))
                {
                    AdminWindow admin = new AdminWindow();
                    admin.Show();
                    Close();
                }
                else
                {
                    IncorrectLoginOrPassword();
                }
            }
            else
            {
                bool temp = false;
                foreach (var el in clients)
                {
                    if (el.Name.Equals(login.Text))
                    {
                        temp = true;
                        if (string.IsNullOrEmpty(el.Password))
                        {
                            ClientWindow client = new ClientWindow(el);
                            client.Show();
                            Close();
                        }
                        else if (DES.ToSHA256(passw.Password).Equals(el.Password))
                        {
                            if (el.IsBlock)
                            {
                                MessageBox.Show("Данный пользователь заблокирован", "Ошибка входа", MessageBoxButton.OK, MessageBoxImage.Error);
                            } 
                            else
                            {
                                ClientWindow client = new ClientWindow(el);
                                client.Show();
                                Close();
                            }
                        }
                        else
                        {
                            IncorrectLoginOrPassword();
                        }
                    }
                }
                if (!temp)
                {
                    IncorrectLoginOrPassword();
                }
            }
        }

        public void IncorrectLoginOrPassword()
        {
            IsChecked++;
            if (IsChecked == 3)
            {
                Close();
            }
            else
            {
               MessageBox.Show("Не правильно введен логин или пароль, введите заново логин и пароль", "Ошибка входа",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                login.Text = "";
                passw.Password = "";
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Автоp: Калядин Максим ПИбд-41. Вариант: 9", "Информация", MessageBoxButton.OK , MessageBoxImage.Information);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            DES.EncryptFile(path, pathEncrypt);
            File.Delete(path);
        }
    }
}
