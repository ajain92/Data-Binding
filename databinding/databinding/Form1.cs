using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace databinding
{
    public partial class Form1 : Form
    {
        private String ConnectionString;
        private DataViewManager dsView;
        private DataSet ds;
        public Form1()
        {
            InitializeComponent();
            ConnectionString = "data source=PR-A317-41\\SQLSERVER2012;initial catalog=northwind;Integrated Security=true";
            SqlConnection cn = new SqlConnection(ConnectionString);

            ds = new DataSet("CustOrders");
            SqlDataAdapter da1 = new SqlDataAdapter("SELECT * FROM Customers", cn);
            da1.TableMappings.Add("Table", "Customers");
            da1.Fill(ds);

            SqlDataAdapter da2 = new SqlDataAdapter("SELECT * FORM Orders", cn);
            da2.TableMappings.Add("Table", "Orders");
            da2.Fill(ds);

            SqlDataAdapter da3 = new SqlDataAdapter("SELECT * FORM OrderDetails", cn);
            da3.TableMappings.Add("Table", "OrderDetails");
            da3.Fill(ds);

            string myMessage = "Table Mapping ";
            for (int i = 0; i < ds.Tables.Count; i++)
            {
                myMessage += i.ToString() + " " + ds.Tables[i].ToString() + " ";

            }
            System.Data.DataRelation relCustOrd;
            System.Data.DataColumn colMaster1;
            System.Data.DataColumn colDetail1;
            System.Data.DataColumn colDetail2;
            colMaster1 = ds.Tables["Customers"].Columns["CustomerID"];
            colDetail1 = ds.Tables["Orders"].Columns["CustomerID"];
            relCustOrd = new System.Data.DataRelation("RelCustOrd", colMaster1, colDetail1);
            ds.Relations.Add(relCustOrd);

            System.Data.DataRelation relOrdDet;
            System.Data.DataColumn colMaster2;
            colMaster2 = ds.Tables["Orders"].Columns["OrderIDID"];
            colDetail2 = ds.Tables["OrderDetails"].Columns["OrderID"];
            relOrdDet = new System.Data.DataRelation("RelOrdDet", colMaster2, colDetail2);
            ds.Relations.Add(relOrdDet);

            myMessage = "Relation Mapping : ";
            for (int i = 0; i < ds.Relations.Count; i++)
            {
                myMessage += i.ToString() + " " + ds.Relations[i].ToString() + " ";
            }

            txtContact.Text = myMessage;

            dsView = ds.DefaultViewManager;

            grdOrders.DataSource = dsView;
            grdOrders.DataSource = "Customer.RelCustOrd.RelOrdDet";

            cbCust.DataSource = dsView;
            cbCust.DataSource = "Customers.CompanyName";
            cbCust.DataSource = "Customers.CustomerID";

            txtContact.DataBindings.Add("Text", dsView, "Customers.ContactName");
            txtPhone.DataBindings.Add("Text", dsView, "Customers.Phone");
            txtFax.DataBindings.Add("Text", dsView, "Customers.Fax");


        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (this.BindingContext[dsView, "Customers"].Position > 0)
                this.BindingContext[dsView, "Customers"].Position--;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            CurrencyManager cm = (CurrencyManager)this.BindingContext[dsView, "Customers"];
            if (cm.Position < cm.Count - 1)
                cm.Position--;
        }

    }
}
