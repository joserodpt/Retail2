using Retail2.Classes;
using Retail2.Forms.Admin.Order;
using Retail2.Forms.Admin.Products;
using Retail2.Forms.Admin.Profile;
using Retail2.Forms.Admin.Settings;
using Retail2.Managers;
using Retail2.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Retail2.Forms.Admin
{
    public partial class Administrar : UI2
    {
        User log;

        List<User> users = new List<User>();
        List<Product> products = new List<Product>();
        List<Category> categories = new List<Category>();
        List<Classes.Profile> profiles = new List<Classes.Profile>();
        List<Classes.Order> orders = new List<Classes.Order>();

        Dictionary<int, User> levelUser = new Dictionary<int, User>();
        Dictionary<int, Product> levelProduct = new Dictionary<int, Product>();
        Dictionary<int, Category> levelCategory = new Dictionary<int, Category>();
        Dictionary<int, Classes.Profile> levelProfile = new Dictionary<int, Classes.Profile>();
        Dictionary<int, Classes.Order> levelOrder = new Dictionary<int, Classes.Order>();

        public Administrar(User u)
        {
            log = u;

            InitializeComponent();

            this.Size = Databases.getSize(SettingsManager.getWindowSize(1));
        }

        private void updateAll()
        {
            updateUserList();
            updateCategoryList();
            updateProductList();
            updateProfileList();
            updateOrderList();
        }

        private void updateOrderList()
        {
            orders.Clear();
            levelOrder.Clear();
            orders = OrderManager.loadOrders();

            dataGridView3.Rows.Clear();
            dataGridView3.DataSource = null;

            dataGridView3.ColumnCount = 11;
            dataGridView3.Columns[0].Name = "ID";
            dataGridView3.Columns[0].Width = 110;
            dataGridView3.Columns[1].Name = "Criador";
            dataGridView3.Columns[2].Name = "Valor";
            dataGridView3.Columns[3].Name = "Mesa";
            dataGridView3.Columns[4].Name = "Data Criado";
            dataGridView3.Columns[5].Name = "Data Fechado";
            dataGridView3.Columns[6].Name = "Ocorrência";
            dataGridView3.Columns[7].Name = "Info Ocorrência";
            dataGridView3.Columns[8].Name = "Detalhes de Pagamento";
            dataGridView3.Columns[9].Name = "Conta Associada";
            dataGridView3.Columns[10].Name = "Tipo";

            int count = 1;
            foreach (Classes.Order o in orders)
            {
                if (checkBox1.Checked == o.DONE)
                {
                    levelOrder.Add(count, o);
                    count += 1;

                    ArrayList row = new ArrayList();
                    row.Add(o.ID + "");
                    row.Add(UserManager.getUserFirstName(o.CREATORUSERID));
                    row.Add(o.VALUE + "€");
                    String str;
                    int table = o.TABLE;
                    if (table == -1)
                    {
                        str = "Nenhuma";
                    }
                    else
                    {
                        str = "" + table;
                    }
                    row.Add(str);
                    row.Add(o.DATECREATED);
                    row.Add(o.DATECLOSED);
                    String str2;
                    if (o.OCCURRENCE == true)
                    {
                        str2 = "Sim";
                    }
                    else
                    {
                        str2 = "Não";
                    }
                    row.Add(str2);
                    row.Add(o.OCCURRENCEINFO);
                    row.Add(o.PAYMENTDETAILS);
                    if (o.PEOPLEPROFILEID != null)
                    {
                        if (o.PEOPLEPROFILEID != "Nenhum")
                        {
                            Classes.Profile f = ProfileManager.getProfile(o.PEOPLEPROFILEID);
                            row.Add(f.FIRSTNAME + " " + f.LASTNAME);
                        }
                        else
                        {
                            row.Add("Nenhuma");
                        }
                    }
                    else
                    {
                        row.Add("-");
                    }
                    String str3;
                    if (o.ORDERTYPE == 0)
                    {
                        str3 = "Fatura";
                    }
                    else
                    {
                        str3 = "Mesas";
                    }
                    row.Add(str3);
                    dataGridView3.Rows.Add(row.ToArray());
                }
            }
            dataGridView3.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;

            DataGridViewButtonColumn btn = new DataGridViewButtonColumn();
            btn.HeaderText = "Produtos";
            btn.Name = "produtos";
            btn.Text = "Clica para ver";
            btn.UseColumnTextForButtonValue = true;
            dataGridView3.Columns.Add(btn);

            DataGridViewButtonColumn btn2 = new DataGridViewButtonColumn();
            btn2.HeaderText = "Eventos";
            btn2.Name = "event";
            btn2.Text = "Clica para ver";
            btn2.UseColumnTextForButtonValue = true;
            dataGridView3.Columns.Add(btn2);

            //checker

            foreach (DataGridViewRow rw in this.dataGridView3.Rows)
            {
                for (int i = 0; i < rw.Cells.Count; i++)
                {
                    if (rw.Cells[i].Value == null || rw.Cells[i].Value == DBNull.Value || String.IsNullOrWhiteSpace(rw.Cells[i].Value.ToString()))
                    {
                        rw.Cells[i].Value = "-";
                    }
                }
            }

            //checker

            foreach (DataGridViewRow rw in this.dataGridView1.Rows)
            {
                for (int i = 0; i < rw.Cells.Count; i++)
                {
                    if (rw.Cells[i].Value == null || rw.Cells[i].Value == DBNull.Value || String.IsNullOrWhiteSpace(rw.Cells[i].Value.ToString()))
                    {
                        rw.Cells[i].Value = "-";
                    }
                }
            }

            //checker

            foreach (DataGridViewRow rw in this.dataGridView2.Rows)
            {
                for (int i = 0; i < rw.Cells.Count; i++)
                {
                    if (rw.Cells[i].Value == null || rw.Cells[i].Value == DBNull.Value || String.IsNullOrWhiteSpace(rw.Cells[i].Value.ToString()))
                    {
                        rw.Cells[i].Value = "-";
                    }
                }
            }

            //checker

            foreach (DataGridViewRow rw in this.dataGridView4.Rows)
            {
                for (int i = 0; i < rw.Cells.Count; i++)
                {
                    if (rw.Cells[i].Value == null || rw.Cells[i].Value == DBNull.Value || String.IsNullOrWhiteSpace(rw.Cells[i].Value.ToString()))
                    {
                        rw.Cells[i].Value = "-";
                    }
                }
            }
        }

        private void updateProfileList()
        {
            profiles = ProfileManager.loadProfiles();
            levelProfile.Clear();

            comboBox3.Items.Clear();

            dataGridView4.DataSource = null;

            DataTable dbUsers = new DataTable();
            dbUsers.Columns.Add("Primeiro Nome", typeof(string));
            dbUsers.Columns.Add("Último Nome", typeof(string));
            dbUsers.Columns.Add("Morada 1", typeof(string));
            dbUsers.Columns.Add("Morada 2", typeof(string));
            dbUsers.Columns.Add("Cidade", typeof(string));
            dbUsers.Columns.Add("Freguesia", typeof(string));
            dbUsers.Columns.Add("NIF", typeof(string));
            dbUsers.Columns.Add("Telefone", typeof(string));
            dbUsers.Columns.Add("Email", typeof(string));
            dbUsers.Columns.Add("Referencia", typeof(string));
            dbUsers.Columns.Add("Info", typeof(string));
            dbUsers.Columns.Add("Data de Criação", typeof(string));

            dataGridView4.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;

            int count = 0;
            foreach (Classes.Profile u in profiles)
            {
                comboBox3.Items.Add(u.FIRSTNAME + " " + u.LASTNAME);

                levelProfile.Add(count, u);
                count += 1;

                dbUsers.Rows.Add(u.FIRSTNAME, u.LASTNAME, u.ADRESS1, u.ADRESS2, u.CITY, u.STATE, u.FISCAL, u.PHONE, u.EMAIL, u.REFERENCE, u.INFO, u.DATECREATED);
            }

            dataGridView4.DataSource = dbUsers;
        }

        private void updateCategoryList()
        {
            categories = CategoryManager.loadCategories();
            levelCategory.Clear();

            listBox6.Items.Clear();

            int count = 0;
            foreach (Category c in categories)
            {
                levelCategory.Add(count, c);
                count += 1;

                listBox6.Items.Add(c.NAME);
            }

            comboBox1.Items.Clear();
            comboBox1.Items.Add("Todas");
            foreach (Category c in categories)
            {
                comboBox1.Items.Add(c.NAME);
            }

            comboBox1.SelectedIndex = 0;
        }


        private void updateProductList()
        {
            products = ProductManager.loadProducts();
            levelProduct.Clear();

            dataGridView2.DataSource = null;

            DataTable dbUsers = new DataTable();
            dbUsers.Columns.Add("Nome", typeof(string));
            dbUsers.Columns.Add("Descrição", typeof(string));
            dbUsers.Columns.Add("Preço", typeof(Double));
            dbUsers.Columns.Add("Stock Ilimitado", typeof(Boolean));
            dbUsers.Columns.Add("Stock", typeof(string));
            dbUsers.Columns.Add("Info", typeof(string));
            dbUsers.Columns.Add("Data Adicionado", typeof(string));
            dbUsers.Columns.Add("Usa Overlay", typeof(Boolean));
            dbUsers.Columns.Add("Identificador", typeof(string));
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;

            int count = 0;
            foreach (Product p in products)
            {
                if (comboBox1.Text == p.CATEGORY)
                {
                    levelProduct.Add(count, p);
                    count += 1;

                    dbUsers.Rows.Add(p.NAME, p.DESCRIPTION, p.PRICE, p.unlimitedSTOCK, p.STOCK, p.INFO, p.DATEADDED, p.useOverlay, p.IDENTIFIER);
                }
                if (comboBox1.Text == "Todas")
                {
                    levelProduct.Add(count, p);
                    count += 1;

                    dbUsers.Rows.Add(p.NAME, p.DESCRIPTION, p.PRICE, p.unlimitedSTOCK, p.STOCK, p.INFO, p.DATEADDED, p.useOverlay, p.IDENTIFIER);
                }
            }

            dataGridView2.DataSource = dbUsers;
        }

        private void PictureBox6_Click(object sender, EventArgs e)
        {
            AddUser p = new AddUser();
            p.FormClosing += new FormClosingEventHandler(updAdminUserActions);
            p.Show();
        }

        private void updAdminUserActions(object sender, FormClosingEventArgs e)
        {
            if (Data.editdone1 == true)
            {
                updateUserList();
                Data.editdone1 = false;
            }
        }

        private void updateUserList()
        {
            users = UserManager.loadUsers();
            levelUser.Clear();

            comboBox4.Items.Clear();

            dataGridView1.DataSource = null;

            DataTable dbUsers = new DataTable();
            dbUsers.Columns.Add("Primeiro Nome", typeof(string));
            dbUsers.Columns.Add("Último Nome", typeof(string));
            dbUsers.Columns.Add("Admin", typeof(Boolean));
            dbUsers.Columns.Add("Online", typeof(Boolean));
            dbUsers.Columns.Add("Último Login", typeof(string));
            dbUsers.Columns.Add("Permissões", typeof(string));
            dbUsers.Columns.Add("Info", typeof(string));
            dbUsers.Columns.Add("Identificador", typeof(string));
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;

            int count = 0;
            foreach (User u in users)
            {
                comboBox4.Items.Add(u.FIRSTNAME + " " + u.LASTNAME);

                levelUser.Add(count, u);
                count += 1;

                List<String> final = new List<string>();
                String perms = u.PERMISSIONS;
                string[] tok = perms.Split(';');
                if (tok[0] == "True")
                {
                    final.Add("Mesas");
                }
                if (tok[1] == "True")
                {
                    final.Add("Faturas");
                }
                if (tok[2] == "True")
                {
                    final.Add("Stocks");
                }

                dbUsers.Rows.Add(u.FIRSTNAME, u.LASTNAME, u.ADMIN, u.ONLINE, u.LOGININFO, string.Join(", ", final.ToArray()), u.INFO, u.IDENTIFIER);
            }

            dataGridView1.DataSource = dbUsers;
        }

        private void PictureBox8_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell != null)
            {
                int i = dataGridView1.CurrentCell.RowIndex;

                EditUser p = new EditUser(levelUser[i], false);
                p.FormClosing += new FormClosingEventHandler(updAdminUserActions);
                p.Show();
            }
        }

        private void PictureBox7_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell != null)
            {
                int i = dataGridView1.CurrentCell.RowIndex;

                DialogResult result = MessageBox.Show("Eliminar " + levelUser[i].FIRSTNAME + " ?", "Confirmar", MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Yes)
                {
                    UserManager.deleteUser(levelUser[i]);
                    updateUserList();
                }
            }
        }

        private void DefeniçõesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SettingsForm f = new SettingsForm();
            f.ShowDialog();
        }

        private void PictureBox17_Click(object sender, EventArgs e)
        {
            AddCategory p = new AddCategory();
            p.FormClosing += new FormClosingEventHandler(updAdminCategoryActions);
            p.Show();
        }

        private void updAdminCategoryActions(object sender, FormClosingEventArgs e)
        {
            updateCategoryList();
        }

        private void PictureBox13_Click(object sender, EventArgs e)
        {
            if (listBox6.SelectedItem != null)
            {
                int i = listBox6.SelectedIndex;

                EditCategory p = new EditCategory(levelCategory[i]);
                p.FormClosing += new FormClosingEventHandler(updAdminCategoryActions);
                p.Show();
            }
        }

        private void PictureBox16_Click(object sender, EventArgs e)
        {
            if (listBox6.SelectedItem != null)
            {
                int i = listBox6.SelectedIndex;

                DialogResult result = MessageBox.Show("Eliminar " + levelCategory[i].NAME + " ?", "Confirmar", MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Yes)
                {
                    CategoryManager.deleteCat(levelCategory[i]);
                    updateCategoryList();
                }
            }
        }

        private void PictureBox3_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex < comboBox1.Items.Count - 1)
            {
                comboBox1.SelectedIndex = comboBox1.SelectedIndex + 1;
            }
        }

        private void PictureBox2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > 0)
            {
                comboBox1.SelectedIndex = comboBox1.SelectedIndex - 1;
            }
        }

        private void Administrar_Load(object sender, EventArgs e)
        {
            updateAll();

            comboBox4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            comboBox4.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox4.AutoCompleteSource = AutoCompleteSource.ListItems;
            comboBox3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            comboBox3.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox3.AutoCompleteSource = AutoCompleteSource.ListItems;

            foreach(Zone z in TableManager.loadZones())
            {
                richTextBox1.Text += z.NAME + "\n";
            }
        }

        private void PictureBox12_Click(object sender, EventArgs e)
        {
            AddProduct p = new AddProduct();
            p.FormClosing += new FormClosingEventHandler(updAdminProductActions);
            p.Show();
        }

        private void updAdminProductActions(object sender, FormClosingEventArgs e)
        {
            updateProductList();
        }

        private void DataGridView2_Click(object sender, EventArgs e)
        {
            if (dataGridView2.CurrentCell != null)
            {
                int i = dataGridView2.CurrentCell.RowIndex;

                pictureBox1.Image = Databases.getImage(levelProduct[i].IDENTIFIER);
            }
        }

        private void PictureBox10_Click(object sender, EventArgs e)
        {
            if (dataGridView2.CurrentCell != null)
            {
                int i = dataGridView2.CurrentCell.RowIndex;

                EditProduct p = new EditProduct(levelProduct[i]);
                p.FormClosing += new FormClosingEventHandler(updAdminProductActions);
                p.Show();
            }
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateProductList();
        }

        private void PictureBox11_Click(object sender, EventArgs e)
        {
            if (dataGridView2.CurrentCell != null)
            {
                int i = dataGridView2.CurrentCell.RowIndex;

                DialogResult result = MessageBox.Show("Eliminar " + levelProduct[i].NAME + " ?", "Confirmar", MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Yes)
                {
                    ProductManager.deleteProduct(levelProduct[i]);
                    updateProductList();
                }
            }
        }

        private void PictureBox15_Click(object sender, EventArgs e)
        {
            AddProfile p = new AddProfile();
            p.FormClosing += new FormClosingEventHandler(updAdminProfileActions);
            p.Show();
        }

        private void updAdminProfileActions(object sender, FormClosingEventArgs e)
        {
            if (Data.editdone2 == true)
            {
                updateProfileList();
                Data.editdone2 = false;
            }
        }

        private void PictureBox9_Click(object sender, EventArgs e)
        {
            if (dataGridView4.CurrentCell != null)
            {
                int i = dataGridView4.CurrentCell.RowIndex;

                EditProfile p = new EditProfile(levelProfile[i]);
                p.FormClosing += new FormClosingEventHandler(updAdminProfileActions);
                p.Show();
            }
        }

        private void ComboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = comboBox4.SelectedIndex;
            dataGridView1.ClearSelection();
            dataGridView1.Rows[i].Selected = true;
        }

        private void ComboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = comboBox3.SelectedIndex;
            dataGridView4.ClearSelection();
            dataGridView4.Rows[i].Selected = true;
        }

        private void PictureBox14_Click(object sender, EventArgs e)
        {
            if (dataGridView4.CurrentCell != null)
            {
                int i = dataGridView4.CurrentCell.RowIndex;

                DialogResult result = MessageBox.Show("Eliminar " + levelProfile[i].FIRSTNAME + " ?", "Confirmar", MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Yes)
                {
                    ProfileManager.deleteProfile(levelProfile[i]);
                    updateProfileList();
                }
            }
        }

        private void SToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            updateAll();
        }

        private void ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateOrderList();
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            updateOrderList();
        }

        private void DataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 11)
            {
                int cel = e.RowIndex + 1;
                if (levelOrder.ContainsKey(cel))
                {
                    ShowData d = new ShowData(0, Databases.uncompactTable(levelOrder[cel].CONTENTLIST), null);
                    d.ShowDialog();
                }
            }
            if (e.ColumnIndex == 12)
            {
                int cel = e.RowIndex + 1;
                if (levelOrder.ContainsKey(cel))
                {
                    ShowData d = new ShowData(1, null, Databases.uncompactList(levelOrder[cel].EVENTS));
                    d.ShowDialog();
                }
            }
        }

        private void PictureBox4_Click(object sender, EventArgs e)
        {
            MesasAdmin m = new MesasAdmin();
            m.ShowDialog();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            TableManager.flushZones();

            for (int i = 0; i < richTextBox1.Lines.Length; i++)
            {
                String line = richTextBox1.Lines[i];
                if (String.IsNullOrEmpty(line) != true)
                {
                    if (TableManager.zoneExists(line))
                    {
                        Zone z = new Zone();
                        z.NAME = line;
                        TableManager.updateZone(z);
                    }
                    else
                    {
                        Zone z = new Zone();
                        z.NAME = line;
                        TableManager.saveZone(z);
                    }
                }
            }

            TableManager.loadTables();
        }
    }
}

