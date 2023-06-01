using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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

namespace ADO.NET_Task1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var conStr = ConfigurationManager.ConnectionStrings["conStr"].ConnectionString;
            SqlConnection sqlServer = new(connectionString: conStr);

            SqlDataReader? reader = null;

            try
            {
                sqlServer.Open();
                SqlCommand cmd = new("Select FirstName From Authors", sqlServer);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string? item = reader["FirstName"].ToString();
                    AuthorComboBox.Items.Add(item);
                }

            }
            finally
            {
                sqlServer.Close();
                reader?.Close();
            }



            try
            {
                sqlServer.Open();
                SqlCommand cmd = new("Select Name From Categories", sqlServer);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string? item = reader["Name"].ToString();
                    CategoriesComboBox.Items.Add(item);
                }

            }
            finally
            {
                sqlServer.Close();
                reader?.Close();
            }




        }

        private void AuthorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string? selectedAuthor = AuthorComboBox.SelectedItem.ToString();

            string conStr = ConfigurationManager.ConnectionStrings["conStr"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(conStr))
            {
                try
                {
                    string query = "Select * From Books Inner Join Authors On Books.Id_Author = Authors.Id WHERE Authors.FirstName = @AuthorName";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@AuthorName", selectedAuthor);
                    connection.Open();


                    SqlDataReader reader = command.ExecuteReader();
                    BooksListBox.Items.Clear();
                    while (reader.Read())
                    {
                        string? bookName = reader["Name"].ToString();
                        BooksListBox.Items.Add(bookName);
                    }
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void CategoriesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string? selectedCategory = CategoriesComboBox.SelectedItem.ToString();

            string conStr = ConfigurationManager.ConnectionStrings["conStr"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(conStr))
            {

                try
                {
                    string query = "Select * From Books Inner Join Categories On Books.Id_Category = Categories.Id WHERE Categories.Name = @CategoryName";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@CategoryName", selectedCategory);
                    connection.Open();


                    SqlDataReader reader = command.ExecuteReader();
                    BooksListBox_Categories.Items.Clear();
                    while (reader.Read())
                    {
                        string? bookName = reader["Name"].ToString();
                        BooksListBox_Categories.Items.Add(bookName);
                    }
                }
                finally
                {
                    connection.Close();
                }
                
            }
        }
    }
}
