using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using TicketProject.Properties;

namespace TicketProject
{
    public partial class Authorization : Form
    {
        /// <summary>
        /// Объявим переменную для неудачного входа. Для логгина создаем переменную ID. Создаем переменную для логгирования
        /// </summary>
        int FailAccess = 0;
        int ID = 0;
        string project_Action;

        /// <summary>
        /// Объявим переменные для дальнейшей работы с адаптерами
        /// </summary>
        TicketProgDataSet.UsersDataTable dataUsers;
        TicketProgDataSet.LoggingDataTable dataLogging;

        public Authorization()
        {
            MaximizeBox = false;
            InitializeComponent();
        }


        private void Authorization_Load(object sender, EventArgs e)
        {
            //Создаем отдельную строку для истории логгинга. Получаем все данные из логгинга. Считаем колличество записей. Создаем отдельную запись.

            dataLogging = this.loggingTableAdapter1.GetData();
            TicketProgDataSet.LoggingRow LoggingHR;
            try
            {
                for (int i = 0; i < dataLogging.Count; i++)
                {
                    LoggingHR = dataLogging.ElementAt(i);

                    /*Удалить текущую запись
                    this.loginHistoryTableAdapter1.Delete
                                            (loginHR.ID, loginHR.Login, loginHR.TimeLogin, loginHR.ResultLogin);*/


                }
            }
            catch
            {
                MessageBox.Show("Не удалось удалить лог из истории логирования");
            }

            //Получение всех записей из таблицы Users
            dataUsers = this.usersTableAdapter1.GetData();

            //Количество записей
            int totalCount = dataUsers.Count;

            /*
            //Отобразить полученные записи в компоненте
            this.dataGridView1.DataSource = dataUsers;

            //Настроить компонент по ширине
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            //Разрешить добавлять новые строки
            dataGridView1.AllowUserToAddRows = false;

            //Настроить видимость нужных столбцов
            this.dataGridView1.Columns["ID"].Visible = false;

            //Изменить заголовки столбцов таблицы
            this.dataGridView1.Columns["IDRole"].HeaderText = "Роль";
            this.dataGridView1.Columns["Email"].HeaderText = "Логин";
            this.dataGridView1.Columns["Password"].HeaderText = "Пароль";
            this.checkBoxHidePass.Checked = false;
            */
        }

        /// <summary>
        /// Скрытие пароля при вводе
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxHidePass_CheckedChanged_1(object sender, EventArgs e)
        {
            if (checkBoxHidePass.Checked)
                textBoxPassword.UseSystemPasswordChar = false;
            else
                textBoxPassword.UseSystemPasswordChar = true;
        }

        /// <summary>
        /// Добавляем кнопку выхода из приложения для более простого и понятного интерфейса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        /// <summary>
        /// Создаем кнопку для успешной авторизации пользователя, записи системы логгирования и ограничения, накладывающиеся на попытку входа без данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonEnter_Click(object sender, EventArgs e)
        {
            //Исходные данные
            string log, pas;
            pas = this.textBoxPassword.Text;
            log = this.textBoxLogin.Text;
            dataUsers = this.usersTableAdapter1.GetData();

            //Дата для истории логгинга
            DateTime dt = DateTime.Now;
            //Взять только время из Даты
            string timeString = dt.ToLongTimeString();

            //Наложить на все записи фильтр на совпадение по логину и паролю
            var filter = dataUsers.Where(rec => rec.Email == log && rec.Password == pas);
            // Нет записей – совпадение логина+пароля не найдено
            if (filter.Count() == 0)
            {
                //Санкции при неверном вводе данных
                FailAccess++;
                if (FailAccess == 4)
                    Thread.Sleep(10000);
                if (FailAccess == 7)
                    Thread.Sleep(15000);
                if (FailAccess == 9)
                    this.Close();
                try
                {
                    ID++;
                    //Добавить в таблицу логгирования попытку входа в систему
                    loggingTableAdapter1.Insert(ID, TimeSpan.Parse(timeString), project_Action);
                }
                catch
                {
                    MessageBox.Show("Ошибка при вводе Email`а или Пароля");
                }
                else
                    try
                    {
                        //Добавить в таблицу логгирования успешный вход в систему
                        loggingTableAdapter1.Insert(ID, TimeSpan.Parse(timeString), project_Action);
                    }
                    catch
                    {
                        MessageBox.Show("Неудачная попытка логгирования");
                    }
            }


            /// <summary>
            /// Используем общий класс для ролей, пытаемся войти в систему (использование класса, правда без метода =) )
            /// </summary>
            ClassTotal.idUser = filter.ElementAt(0).ID;
            ClassTotal.idRole = filter.ElementAt(0).IDRole;
            ClassTotal.login = filter.ElementAt(0).Email;
            //Переход к формам в зависимости от роли
            switch (ClassTotal.idRole)
            {
                case 1:
                    MessageBox.Show("Вы  авторизовались как Продавец");
                    FormSeller formSeller = new FormSeller();
                    this.Hide();
                    formSeller.ShowDialog();
                    this.Show();
                    break;
                case 2:
                    MessageBox.Show("Вы  авторизовались как Организатор");
                    FormOrganizer formOrganizer = new FormOrganizer();
                    this.Hide();
                    formOrganizer.ShowDialog();
                    this.Show();
                    break;
                case 3:
                    MessageBox.Show("Вы  авторизовались как Администратор");
                    FormAdministrator formAdmin = new FormAdministrator();
                    this.Hide();
                    formAdmin.ShowDialog();
                    this.Show();
                    break;
            }
        }
    }
}
