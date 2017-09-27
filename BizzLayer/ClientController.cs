using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizzLayer
{
    public class ClientController
    {
        public static List<CLIENT> GetAllClients()
        {
            var dbContext = new ServiceSystemDataContext();
            List<CLIENT> cliList = new List<CLIENT>();
            var query = from c in dbContext.CLIENT
                        join a in dbContext.ADRES
                        on c.id_client equals a.id_cli
                        select c;
            foreach (var q in query) {
                CLIENT newCli = new CLIENT();
                newCli.id_client = q.id_client;
                newCli.name = q.name;
                newCli.fname = q.fname;
                newCli.lname = q.lname;
                newCli.tel = q.tel;
                cliList.Add(newCli);
            }

            return cliList;

        }

        public static List<CLIENT> GetClientsByCriteria(CLIENT criteria)
        {
            using (ServiceSystemDataContext dbContext = new ServiceSystemDataContext())
            {
                var clients = from c in dbContext.CLIENT
                              join a in dbContext.ADRES
                              on c.id_client equals a.id_cli
                              select c;

                if (criteria.name.Length != 0 && criteria.fname.Length != 0 && criteria.lname.Length != 0)
                {
                    clients = clients.Where(c => c.name == criteria.name && c.fname == criteria.fname && c.lname == criteria.lname);
                }
                else if (criteria.name.Length != 0 && criteria.fname.Length != 0)
                {
                    clients = clients.Where(c => c.name == criteria.name && c.fname == criteria.fname);
                }
                else if (criteria.name.Length != 0 && criteria.lname.Length != 0)
                {
                    clients = clients.Where(c => c.name == criteria.name && c.lname == criteria.lname);
                }
                else if (criteria.fname.Length != 0 && criteria.lname.Length != 0)
                {
                    clients = clients.Where(c => c.fname == criteria.fname && c.lname == criteria.lname);
                }
                else if (criteria.name.Length != 0)
                {
                    clients = clients.Where(c => c.name == criteria.name);
                }
                else if (criteria.fname.Length != 0)
                {
                    clients = clients.Where(c => c.fname == criteria.fname);
                }
                else if (criteria.lname.Length != 0)
                {
                    clients = clients.Where(c => c.lname == criteria.lname);
                }

                List<CLIENT> cliList = new List<CLIENT>();

                foreach (var q in clients)
                {
                    CLIENT newCli = new CLIENT();
                    newCli.id_client = q.id_client;
                    newCli.name = q.name;
                    newCli.fname = q.fname;
                    newCli.lname = q.lname;
                    newCli.tel = q.tel;
                    cliList.Add(newCli);
                }
                return cliList;
            }
        }

        public static CLIENT GetClientById(int id) {
            using (ServiceSystemDataContext dbContext = new ServiceSystemDataContext()) {
                var query = (from c in dbContext.CLIENT
                            where c.id_client == id
                            select c).FirstOrDefault();
                return query;
            }
        }

        public static bool DeleteClient(CLIENT client)
        {
            using (ServiceSystemDataContext dbContext = new ServiceSystemDataContext())
            {
                try
                {
                    var objects = from o in dbContext.OBJECT
                                  where o.id_cli == client.id_client
                                  select o;
                    foreach (var obj in objects)
                    {
                        ObjectController.DeleteObject(obj);
                    }

                    CLIENT cli = dbContext.CLIENT.SingleOrDefault(x => x.id_client == client.id_client);
                    dbContext.CLIENT.DeleteOnSubmit(cli);
                    ADRES adr = dbContext.ADRES.SingleOrDefault(x => x.id_cli == client.id_client);
                    dbContext.ADRES.DeleteOnSubmit(adr);
                    dbContext.SubmitChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public static ADRES GetClientAddress(CLIENT client) {
            using (ServiceSystemDataContext dbContext = new ServiceSystemDataContext())
            {
                var query = (from a in dbContext.ADRES
                            where a.id_cli == client.id_client
                            select a).FirstOrDefault();
                return query;
            }
        }

        public static bool UpdateClientData(CLIENT currClient, CLIENT client, ADRES address)
        {
            using (ServiceSystemDataContext dbContext = new ServiceSystemDataContext())
            {
                try
                {
                    CLIENT cli = dbContext.CLIENT.SingleOrDefault(x => x.id_client == currClient.id_client);
                    cli.name = client.name;
                    cli.fname = client.fname;
                    cli.lname = client.lname;
                    cli.tel = client.tel;
                    ADRES adr = dbContext.ADRES.SingleOrDefault(x => x.id_cli == currClient.id_client);
                    adr.street = address.street;
                    adr.post_code = address.post_code;
                    adr.city = address.city;
                    adr.nation = address.nation;
                    dbContext.SubmitChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public static bool InsertNewClient(CLIENT client, ADRES address)
        {
            var dc = new ServiceSystemDataContext();
            var newClient = new CLIENT
            {
                name = client.name,
                fname = client.fname,
                lname = client.lname,
                tel = client.tel
            };


            dc.CLIENT.InsertOnSubmit(newClient);

            try
            {
                dc.SubmitChanges();
                Console.WriteLine(newClient.id_client);
            }
            catch (Exception ex)
            {
                return false;
            }

            var db = new ServiceSystemDataContext();
            ADRES newAdress = new ADRES
            {
                id_cli = newClient.id_client,
                street = address.street,
                city = address.city,
                post_code = address.post_code,
                nation = address.nation
            };
            Console.WriteLine(newAdress.id_cli);
            db.ADRES.InsertOnSubmit(newAdress);

            try
            {
                db.SubmitChanges();

                return true;
            }
            catch (Exception ex)
            {
                using (ServiceSystemDataContext newDbContext = new ServiceSystemDataContext())
                {
                    CLIENT cli = newDbContext.CLIENT.SingleOrDefault(x => x.id_client == newClient.id_client);
                    newDbContext.CLIENT.DeleteOnSubmit(cli);
                    newDbContext.SubmitChanges();
                }
                return false;
            }


        }

    }
}
