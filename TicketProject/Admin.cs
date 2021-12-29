using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TicketProject
{
    public partial class Admin : Form
    {
        public Admin()
        {
            InitializeComponent();
        }

        //Кнопка выхода из приложения
        private void buttonExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// Добавляем кнопку входа в систему Логирования, доступаня только Администратору
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonLogging_Click(object sender, EventArgs e)
        {
            FormLogging formlogs = new FormLogging();
            this.Hide();
            formlogs.ShowDialog();
            this.Show();
        }

        /// <summary>
        /// Переход на форму Организаций, которые работают с администратором, либо организации которая проводит мероприятие
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonOrganizer_Click(object sender, EventArgs e)
        {
            FormOrganizer formorgan = new FormOrganizer();
            this.Hide();
            formorgan.ShowDialog();
            this.Show();
        }
    }
}
