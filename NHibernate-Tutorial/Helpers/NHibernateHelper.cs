using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NHibernate_Tutorial.Helpers
{
    public class NHibernateHelper
    {
        private static ISessionFactory _sessionFactory;
        private static object syncRoot = new Object();

        public static ISession OpenSession
        {
            get
            {
                if (_sessionFactory == null)
                {
                    lock (syncRoot)
                    {
                        if (_sessionFactory == null)
                        {
                            _sessionFactory = BuildSessionFactory();
                        }
                    }
                }
                return _sessionFactory.OpenSession();
            }

        }

        private static ISessionFactory BuildSessionFactory()
        {
            try
            {
                string conString = ConfigurationManager.AppSettings["ConnectionString"];
                return Fluently.Configure().Database(MsSqlConfiguration.MsSql7.ConnectionString(conString)).Mappings(m => m.FluentMappings.AddFromAssemblyOf<Program>()).ExposeConfiguration(BuildSchema).BuildSessionFactory();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw ex;
            }
        }

        //create schema
        private static AutoPersistenceModel CreateMappings()
        {
            return AutoMap.Assembly(System.Reflection.Assembly.GetCallingAssembly()).Where(t => t.Namespace == "NHibernate_Tutorial.Model");
        }

        private static void BuildSchema(NHibernate.Cfg.Configuration config)
        {

        }
    }
}
