using System;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Library
{
    public partial class Form1 : Form
    {
        MyLibEntities db;
        public Form1()
        {
            InitializeComponent();
            db = new MyLibEntities();
            db.Books.Load();
            dataGridView1.DataSource = db.Books.Local.ToBindingList();
            
        }

        private void button1_Click(object sender,EventArgs e)
        {
            dataGridView1.DataSource = null;
            List<Book> books = new List<Book>();
            var group = db.Books.Where(p => p.Pages > 350);
            foreach (var item in group)
            {
                
                 books.Add(item);
              
            }
            dataGridView1.DataSource = books;
        }

        private void button2_Click(object sender,EventArgs e)
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = db.Books.Local.ToBindingList();
        }

        public int h = 10;

        private void button3_Click(object sender,EventArgs e)
        {
            h = 12;
            button4_Click(sender,e);
            h = 10;
            return;
            List<Book> books = new List<Book>();
      
            dataGridView1.DataSource = null;


            //System.Data.SqlClient.SqlParameter param = new System.Data.SqlClient.SqlParameter("@id","%1%");
            //var book1 = db.Database.SqlQuery<Book>("SELECT * FROM db.Book WHERE Id",1);
            //foreach (var b in book1)
            //    books.Add(b);

            var result1 = "SELECT * FROM Title WHERE Count >= {0}";
            h = 10;
            var query = db.Database.SqlQuery<Book>(result1,h);

            dataGridView1.DataSource = books;
        }

        public void button4_Click(object sender,EventArgs e)
        {
            dataGridView1.DataSource = null;
            List<Book> books = new List<Book>();
            var group = from c in db.Books
                        where c.Count < h
                        select c;
            foreach (var item in group)
            {

                books.Add(item);

            }
            dataGridView1.DataSource = books;
        }

        private void button5_Click(object sender,EventArgs e)
        {
            Form2 tmForm = new Form2();
            DialogResult result = tmForm.ShowDialog(this);

            if (result == DialogResult.Cancel)
                return;

            Book book = new Book();
            book.Author = tmForm.textBox1.Text;
            book.Title = tmForm.textBox2.Text;
            book.Pages = Convert.ToInt32( tmForm.textBox3.Text);
            book.Count = Convert.ToInt32(tmForm.textBox4.Text);

            int max = db.Books.Count();
            book.Id = max;
            db.Books.Add(book);
            db.SaveChanges();
            MessageBox.Show("Новый объект добавлен");
        }

        private void button6_Click(object sender,EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int index = dataGridView1.SelectedRows[0].Index;
                int id = 0;
                bool converted = Int32.TryParse(dataGridView1[0,index].Value.ToString(),out id);
                if (converted == false)
                    return;

                Book user = db.Books.Find(id);
                db.Books.Remove(user);
                db.SaveChanges();

                MessageBox.Show("Объект удален");
            }
        }

        private void button7_Click(object sender,EventArgs e)
        {

            dataGridView1.DataSource = null;
            List<Book> books = new List<Book>();
            var groups = db.Books.GroupBy(p => p.Title);
            foreach (var g in groups)
            {
                foreach (var p in g)
                    books.Add(p);
            }
            dataGridView1.DataSource = books;
        }

        private void button8_Click(object sender,EventArgs e)
        {
            dataGridView1.DataSource = null;
            List<Book> books = new List<Book>();
            var groups = from p in db.Books
                         group p by p.Author;
            foreach (var g in groups)
            {
                foreach (var p in g)
                    books.Add(p);
            }
            dataGridView1.DataSource = books;
        }
    }
}
