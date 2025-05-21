using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DeliveryApp
{
    public partial class Form1 : Form
    {
        private string connectionString = "Server=localhost;Database = database;User Id=root;Password=toor;";

        public Form1()
        {
            InitializeComponent(); 
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            int orderId;
            if (!int.TryParse(textBox1.Text, out orderId))
            {
                MessageBox.Show("Введите корректный номер заказа.");
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM Orders WHERE order_id = @OrderId";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@OrderId", orderId);

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    var dataTable = new System.Data.DataTable();
                    adapter.Fill(dataTable);

                    dataGridView1.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}");
                }
            }
        }
    }
}

private void Button2_Click(object sender, EventArgs e)
{
    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        try
        {
            connection.Open();
            string query = "SELECT * FROM Orders WHERE status = 'доставлен' ORDER BY order_date";
            SqlCommand command = new SqlCommand(query, connection);

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            var dataTable = new System.Data.DataTable();
            adapter.Fill(dataTable);

            dataGridView1.DataSource = dataTable;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка: {ex.Message}");
        }
    }
}

private void Button3_Click(object sender, EventArgs e)
{
    string plannedDate = textBox2.Text;

    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        try
        {
            connection.Open();
            string query = @"
                SELECT o.order_id, o.order_date, o.customer_name, c.courier_name, c.courier_phone
                FROM Orders o
                JOIN Couriers c ON o.courier_id = c.courier_id
                WHERE DATE(o.planned_delivery_date) = @PlannedDate";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PlannedDate", plannedDate);

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            var dataTable = new System.Data.DataTable();
            adapter.Fill(dataTable);

            dataGridView1.DataSource = dataTable;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка: {ex.Message}");
        }
    }
}

private void Button4_Click(object sender, EventArgs e)
{
    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        try
        {
            connection.Open();
            string query = @"
                SELECT product_type, COUNT(*) AS completed_orders_count
                FROM Orders
                WHERE status = 'доставлен'
                GROUP BY product_type";

            SqlCommand command = new SqlCommand(query, connection);

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            var dataTable = new System.Data.DataTable();
            adapter.Fill(dataTable);

            dataGridView1.DataSource = dataTable;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка: {ex.Message}");
        }
    }
}

private void Button5_Click(object sender, EventArgs e)
{
    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        try
        {
            connection.Open();
            string query = @"
                WITH CourierOrderCount AS (
                    SELECT courier_id, COUNT(*) AS order_count
                    FROM Orders
                    WHERE status = 'доставлен'
                    GROUP BY courier_id
                )
                SELECT c.courier_name, c.courier_phone, coc.order_count
                FROM CourierOrderCount coc
                JOIN Couriers c ON coc.courier_id = c.courier_id
                WHERE coc.order_count = (SELECT MAX(order_count) FROM CourierOrderCount)";

            SqlCommand command = new SqlCommand(query, connection);

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            var dataTable = new System.Data.DataTable();
            adapter.Fill(dataTable);

            dataGridView1.DataSource = dataTable;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка: {ex.Message}");
        }
    }
}

private void Button6_Click(object sender, EventArgs e)
{
    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        try
        {
            connection.Open();
            string query = @"
                SELECT 
                    o.order_id,
                    o.order_date,
                    o.customer_name,
                    c.courier_name,
                    c.courier_phone,
                    DATEDIFF(MINUTE, CONCAT(o.planned_delivery_date, ' ', o.planned_delivery_time), CONCAT(o.actual_delivery_date, ' ', o.actual_delivery_time)) AS delay_minutes
                FROM Orders o
                JOIN Couriers c ON o.courier_id = c.courier_id
                WHERE DATEDIFF(MINUTE, CONCAT(o.planned_delivery_date, ' ', o.planned_delivery_time), CONCAT(o.actual_delivery_date, ' ', o.actual_delivery_time)) >= 15
                ORDER BY delay_minutes DESC";

            SqlCommand command = new SqlCommand(query, connection);

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            var dataTable = new System.Data.DataTable();
            adapter.Fill(dataTable);

            dataGridView1.DataSource = dataTable;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка: {ex.Message}");
        }
    }
}

