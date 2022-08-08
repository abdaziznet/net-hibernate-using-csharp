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

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            ISession session = NHibernateHelper.OpenSession;

            using (session)
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    try
                    {
                        IQuery query = session.CreateQuery("FROM Employee where id = '" + txtId.Text + "'");
                        Employee employee = query.List<Employee>()[0];
                        SetEmployeeInfo(employee);
                        session.Update(employee);
                        transaction.Commit();

                        loadEmplpoyeeData();
                        clear();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        throw ex;
                    }
                }

            }
        }

        private void dgViewEmployee_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgViewEmployee.RowCount <= 1 || e.RowIndex < 0)
                return;

            string id = dgViewEmployee[0, e.RowIndex].Value.ToString();

            if (string.IsNullOrEmpty(id))
                return;

            IList<Employee> empInfo = getDataFromEmployee(id);
            txtId.Text = empInfo[0].Id.ToString();
            txtFirstName.Text = empInfo[0].FirstName.ToString();
            txtLastName.Text = empInfo[0].LastName.ToString();
            txtEmail.Text = empInfo[0].Email.ToString();
        }

        private IList<Employee> getDataFromEmployee(string id)
        {
            ISession session = NHibernateHelper.OpenSession;
            using (session)
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    try
                    {
                        IQuery query = session.CreateQuery("FROM Employee where Id = '" + id + "'");
                        return query.List<Employee>();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        throw ex;
                    }

                }
            }
        }
    }
}
