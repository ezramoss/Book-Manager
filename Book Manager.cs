using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Book_Manager_Project
{
    public partial class Form1 : Form
    {
        MySqlConnection sqlConn = new MySqlConnection();
        MySqlCommand sqlComm = new MySqlCommand();
        DataTable sqlTable = new DataTable();
        MySqlDataAdapter sqlAdatper = new MySqlDataAdapter();
        DataSet ds = new DataSet();
        MySqlDataReader sqlReader;

        String server = "";
        String username = "";
        String password = "";
        String database = "books";

        public Form1()
        {
            InitializeComponent();
        }

        private void displayError(String error)
        {
            logBox.SelectionColor = Color.Red;
            logBox.AppendText(error);
            logBox.SelectionColor = Color.Black;
        }

        private void clearLog_Click(object sender, EventArgs e)
        {
            logBox.Clear();
        }

        //------------------
        //SQL Functions
        //------------------

        private void setConnection_Click(object sender, EventArgs e)
        {
            server = serverBox.Text;
            username = usernameBox.Text;
            password = passwordBox.Text;
            logBox.AppendText("Connection set\r\n");
        }

        private void addBook(object sender, EventArgs e)
        {
            String sqlQuery;
            sqlConn.ConnectionString = "server=" + server + ";" + "user id =" +
                username + ";" + "password=" + password + ";" + "database=" + database;
            try
            {
                sqlConn.Open();
                
                sqlQuery = "insert into books.books (ISBN, Title, Author, Publisher, Quantity)" +
                    "values(" + isbnBox.Text + ",'" + titleBox.Text + "','" + authorBox.Text +
                    "','" + publisherBox.Text + "'," + quantityBox.Text + ")";

                sqlComm = new MySqlCommand(sqlQuery, sqlConn);
                sqlReader = sqlComm.ExecuteReader();
                sqlConn.Close();
                logBox.AppendText("Added book\r\n");
            }
            catch(Exception ex)
            {
                displayError("Failed to add book\r\n");
            }
            finally
            {
                sqlConn.Close();
            }
        }

        private void deleteBook(object sender, EventArgs e)
        {
            sqlConn.ConnectionString = "server=" + server + ";" + "user id =" +
                username + ";" + "password=" + password + ";" + "database=" + database;
            try
            {
                sqlConn.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = sqlConn;
                cmd.CommandText = "delete from books.books where ISBN = " + Convert.ToInt32(isbnBox.Text);

                foreach(DataGridViewRow book in dataGridView1.SelectedRows)
                {
                    dataGridView1.Rows.RemoveAt(book.Index);
                }
                cmd.ExecuteNonQuery();
                logBox.AppendText("Book deleted\r\n");
                sqlConn.Close();

            }
            catch (Exception ex)
            {
                displayError("Failed to delete book\r\n");
            }
            finally
            {
                sqlConn.Close();
            }
        }

        private void clearQuery(object sender, EventArgs e)
        {
            titleBox.Clear();
            authorBox.Clear();
            publisherBox.Clear();
            isbnBox.Clear();
            quantityBox.Clear();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                isbnBox.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                titleBox.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                authorBox.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                publisherBox.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                quantityBox.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            }
            catch (Exception ex)
            {
                displayError("Failed to load book\r\n");
            }
        }

        private void queryBooks(object sender, EventArgs e)
        {
            sqlConn.ConnectionString = "server=" + server + ";" + "user id =" +
            username + ";" + "password=" + password + ";" + "database=" + database;
            try
            {
                sqlConn.Open();
                sqlComm.Connection = sqlConn;
                sqlComm.CommandText = "Select * From books.books";
                sqlReader = sqlComm.ExecuteReader();
                sqlTable.Load(sqlReader);
                sqlReader.Close();
                sqlConn.Close();
                dataGridView1.DataSource = sqlTable;
            }
            catch (Exception ex)
            {
                displayError("Failed to query books\r\n");
            }
        }

        private void update_Click(object sender, EventArgs e)
        {
            sqlConn.ConnectionString = "server=" + server + ";" + "user id =" +
                username + ";" + "password=" + password + ";" + "database=" + database;
            try
            {
                sqlConn.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = sqlConn;
                cmd.CommandText = "Update books.books set ISBN = " + Convert.ToInt32(isbnBox.Text) +
                    ", Title = '" + titleBox.Text + "', Author = '" + authorBox.Text + "', Publisher = '" +
                    publisherBox.Text + "', Quantity = " + Convert.ToInt32(quantityBox.Text) + 
                    " where ISBN = " + Convert.ToInt32(isbnBox.Text);

                cmd.ExecuteNonQuery();
                if(cmd.ExecuteNonQuery() == 1)
                {
                    logBox.AppendText("Book data updated\r\n");
                }
                else
                {
                    displayError("Failed to update book\r\n");
                }
                sqlConn.Close();
            }
            catch (Exception ex)
            {
                displayError("Failed to update book\r\n");
            }
            finally
            {
                sqlConn.Close();
            }
        }
    }
}
