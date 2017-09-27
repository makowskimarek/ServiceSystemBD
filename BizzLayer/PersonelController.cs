using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizzLayer
{
    public class PersonelController
    {
        public class Personel
        {
            public int id_pers { get; set; }
            public String username { get; set; }
            public String password { get; set; }
            public String first_name { get; set; }
            public String last_name { get; set; }
            public DateTime retire_date { get; set; }
            public String role { get; set; }
        }

        public static List<PERSONEL> GetWorkers()
        {
            using (ServiceSystemDataContext dbContext = new ServiceSystemDataContext())
            {
                var query = (from p in dbContext.PERSONEL
                             where p.role == "wrk"
                             select p).ToList();
                return query;
            }
        }

        public static bool RegisterAccount(Personel new_account)
        {
            using (ServiceSystemDataContext dbContext = new ServiceSystemDataContext())
            {
                PERSONEL new_personel = new PERSONEL
                {
                    fname = new_account.first_name,
                    lname = new_account.last_name,
                    username = new_account.username,
                    pass = new_account.password,
                    role = new_account.role,
                    dt_retire = new_account.retire_date
                };

                dbContext.PERSONEL.InsertOnSubmit(new_personel);
                try
                {
                    dbContext.SubmitChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }

            }

        }

        public static List<PERSONEL> GetAllAccounts()
        {
            using (ServiceSystemDataContext dbContext = new ServiceSystemDataContext())
            {
                var query = (from p in dbContext.PERSONEL
                             select p).ToList();
                return query;
            }
        }

        public static bool DeleteAccount(PERSONEL account)
        {
            using (ServiceSystemDataContext dbContext = new ServiceSystemDataContext())
            {
                try
                {
                    PERSONEL personel = dbContext.PERSONEL.SingleOrDefault(x => x.id_pers == account.id_pers);
                    dbContext.PERSONEL.DeleteOnSubmit(personel);
                    dbContext.SubmitChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public static List<PERSONEL> GetAccountsWithCriteria(PersonelController.Personel srchCriteria)
        {
            using (ServiceSystemDataContext dbContext = new ServiceSystemDataContext())
            {
                try
                {
                    var persons = from p in dbContext.PERSONEL
                                  select p;

                    if (srchCriteria.first_name.Length != 0 && srchCriteria.last_name.Length != 0 && srchCriteria.role.Length != 0)
                    {
                        persons = persons.Where(p => p.fname == srchCriteria.first_name && p.lname == srchCriteria.last_name && p.role == srchCriteria.role);
                    }
                    else if (srchCriteria.first_name.Length != 0 && srchCriteria.last_name.Length != 0)
                    {
                        persons = persons.Where(p => p.fname == srchCriteria.first_name && p.lname == srchCriteria.last_name);
                    }
                    else if (srchCriteria.first_name.Length != 0 && srchCriteria.role.Length != 0)
                    {
                        persons = persons.Where(p => p.fname == srchCriteria.first_name && p.role == srchCriteria.role);
                    }
                    else if (srchCriteria.last_name.Length != 0 && srchCriteria.role.Length != 0)
                    {
                        persons = persons.Where(p => p.lname == srchCriteria.last_name && p.role == srchCriteria.role);
                    }
                    else if (srchCriteria.last_name.Length != 0)
                    {
                        persons = persons.Where(p => p.lname == srchCriteria.last_name);
                    }
                    else if (srchCriteria.first_name.Length != 0)
                    {
                        persons = persons.Where(p => p.fname == srchCriteria.first_name);
                    }
                    else if(srchCriteria.role.Length != 0)
                    {
                        persons = persons.Where(p => p.role == srchCriteria.role);
                    }


                    return persons.ToList();
                }
                catch (Exception ex) {
                    return null;
                }
            }
        }

        public static bool ChangePersonelItemData(PERSONEL account, PersonelController.Personel accountChanges) {
            using (ServiceSystemDataContext dbContext = new ServiceSystemDataContext())
            {
                try
                {
                    PERSONEL personel = dbContext.PERSONEL.SingleOrDefault(x => x.id_pers == account.id_pers);
                    personel.fname = accountChanges.first_name;
                    personel.lname = accountChanges.last_name;
                    personel.username = accountChanges.username;
                    personel.pass = accountChanges.password;
                    personel.dt_retire = accountChanges.retire_date;
                    personel.role = accountChanges.role;
                    dbContext.SubmitChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
                
            }
        }
    }
}
