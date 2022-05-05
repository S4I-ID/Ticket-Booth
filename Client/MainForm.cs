using CommonDomain;
using Networking;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class MainForm : Form
    {
        private ClientController controller;
        internal MainForm(ClientController serviceController)
        {
            InitializeComponent();
            this.controller = serviceController;
            controller.updateEvent += clientUpdate;
        }

        public void clientUpdate(object sender, ClientEventArgs e)
        {
            if (e.GetEventType() == ClientEvent.UpdateShowList)
            {
                dataGridViewMain.BeginInvoke((Action)delegate 
                { 
                    updateMainGrid((List<Show>)e.GetData());
                    DateTime date = datePickerFilter.Value;
                    if (date != null)
                        updateFilteredGrid(controller.GetShowsByDate(date));
                });
            }
            if (e.GetEventType()== ClientEvent.UpdateFilteredList)
            {
                dataGridViewMain.BeginInvoke((Action)delegate { updateFilteredGrid((List<Show>)e.GetData()); });
            }
        }

        private void MainFormLoad(object sender, EventArgs e)
        {
            dataGridViewMain.ColumnCount = 7;
            dataGridViewMain.Columns[0].Name = "Artist name";
            dataGridViewMain.Columns[1].Name = "Show name";
            dataGridViewMain.Columns[2].Name = "Address";
            dataGridViewMain.Columns[3].Name = "Time";
            dataGridViewMain.Columns[4].Name = "Available seats";
            dataGridViewMain.Columns[5].Name = "Sold seats";
            dataGridViewMain.Columns[6].Name = "ID";
            dataGridViewMain.Columns[6].Visible = false;
            dataGridViewMain.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            filteredDataGridView.ColumnCount = 7;
            filteredDataGridView.Columns[0].Name = "Artist name";
            filteredDataGridView.Columns[1].Name = "Show name";
            filteredDataGridView.Columns[2].Name = "Address";
            filteredDataGridView.Columns[3].Name = "Time";
            filteredDataGridView.Columns[4].Name = "Available seats";
            filteredDataGridView.Columns[5].Name = "Sold seats";
            filteredDataGridView.Columns[6].Name = "ID";
            filteredDataGridView.Columns[6].Visible = false;
            filteredDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            updateMainGrid(controller.GetAllShows());
        }

        private void updateMainGrid(List <Show> shows)
        {
            dataGridViewMain.Rows.Clear();
            int i = 0;
            foreach (Show show in shows)
            {
                dataGridViewMain.Rows.Add
                    (show.GetArtistName(), show.GetShowName(), show.GetAddress(), show.GetStartTime().ToString(),
                     show.GetAvailableSeats().ToString(), show.GetSoldSeats().ToString(), show.GetId().ToString()
                     );
                if (show.GetAvailableSeats() == show.GetSoldSeats())
                    dataGridViewMain.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                else
                    dataGridViewMain.Rows[i].DefaultCellStyle.BackColor = Color.White;
                i++;
            }
        }

        private void updateFilteredGrid(List<Show> shows)
        {
            filteredDataGridView.Rows.Clear();
            int i = 0;
            foreach (Show show in shows)
            {
                filteredDataGridView.Rows.Add
                    (show.GetArtistName(), show.GetShowName(), show.GetAddress(), show.GetStartTime().ToString(),
                     show.GetAvailableSeats().ToString(), show.GetSoldSeats().ToString(), show.GetId().ToString()
                     );
                if (show.GetAvailableSeats() == show.GetSoldSeats())
                    filteredDataGridView.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                else
                    filteredDataGridView.Rows[i].DefaultCellStyle.BackColor = Color.White;
                i++;
            }
        }

        private void buyTicketsClick(object sender, EventArgs e)
        {
            try
            {
                string name = textBoxName.Text;
                int ticketsBought = int.Parse(textBoxTickets.Text);
                int showId = int.Parse((string)dataGridViewMain.SelectedRows[0].Cells[6].Value);

                if (name == "") throw new Exception("Invalid name!");

                controller.AddSaleToShow(name, ticketsBought, showId);

                //updateMainGrid();
                //updateFilteredGrid(datePickerFilter.Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void filterDateClick(object sender, EventArgs e)
        {
            updateFilteredGrid(controller.GetShowsByDate(datePickerFilter.Value));
        }

        private void logoutClick(object sender, EventArgs e)
        {
            this.Hide();
            controller.Logout();
            Form1 login = new Form1(controller);
            login.FormClosed += (s, args) => this.Close();
            login.Show();
        }

        private new void Closed(object sender, FormClosingEventArgs e)
        {
            Console.WriteLine("Window manually closed. Logging user out...");
            if (e.CloseReason == System.Windows.Forms.CloseReason.UserClosing)
            {
                controller.Logout();
                controller.updateEvent -= clientUpdate;
                Form1 login = new Form1(controller);
                login.FormClosed += (s, args) => this.Close();
                login.Show();
            }
        }
    }
}
