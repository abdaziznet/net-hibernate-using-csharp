using NHibernate;
using NHibernate_Tutorial.Helpers;
using NHibernate_Tutorial.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NHibernate_Tutorial
{
    public partial class FrmEmployee : Form
    {
        public FrmEmployee()
        {
            InitializeComponent();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            loadEmplpoyeeData();
        }

        private void loadEmplpoyeeData()
        {
            txtFirstName.Focus();
            ISession session = NHibernateHelper.OpenSession;

            using (session)
            {
                IQuery query = session.CreateQuery("FROM Employee");
                IList<Employee> empInfo = query.List<Employee>();
                dgViewEmployee.DataSource = empInfo;
            }
        }

        private void FrmEmployee_Load(object sender, EventArgs e)
        {
            loadEmplpoyeeData();
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            Employee employee = new Employee();
            SetEmployeeInfo(employee);
            ISession session = NHibernateHelper.OpenSession;

            using (session)
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    try
                    {
                        session.Save(employee);
                        transaction.Commit();
                        loadEmplpoyeeData();
                        clear();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show(ex.Message);
                        throw ex;
                    }

                }
            }
        }

        private void SetEmployeeInfo(Employee employee)
        {
            employee.FirstName = txtFirstName.Text;
            employee.LastName = txtLastName.Text;
            employee.Email = txtEmail.Text;
        }

        private void clear()
        {
            txtId.Text = string.Empty;
            txtFirstName.Text = string.Empty;
            txtLastName.Text = string.Empty;
            txtEmail.Text = string.Empty;
        }
    }
}
