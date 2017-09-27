using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizzLayer
{
    public class ObjectAndClient {
        public int id_client { get; set; }
        public int nr_obj { get; set; }
        public String code { get; set; }
        public String code_type { get; set; }
        public String name{ get; set; }
        public String fname{ get; set; }
        public String lname { get; set; }
        public int tel { get; set; }

    }
    public class ObjectController
    {
        public static bool AddNewObject(OBJECT obj)
        {
            using (ServiceSystemDataContext dbContext = new ServiceSystemDataContext())
            {
                dbContext.OBJECT.InsertOnSubmit(obj);
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

        public static bool UpdateObject(OBJECT obj)
        {
            using (ServiceSystemDataContext dbContext = new ServiceSystemDataContext())
            {
                try
                {
                    OBJECT newObj = dbContext.OBJECT.SingleOrDefault(x => x.nr_obj == obj.nr_obj);
                    Console.WriteLine(obj.code);
                    newObj.code = obj.code;
                    newObj.code_type = obj.code_type;
                    dbContext.SubmitChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public static OBJECT GetObjectById(int id)
        {
            using (ServiceSystemDataContext dbContext = new ServiceSystemDataContext())
            {
                try
                {
                    var query = (from o in dbContext.OBJECT
                                where o.nr_obj == id
                                select o).FirstOrDefault();
                    return query;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public static List<ObjectAndClient> GetObjectsByCriteria(CLIENT client, OBJECT obj)
        {
            using (ServiceSystemDataContext dbContext = new ServiceSystemDataContext())
            {
                var res = GetAllObjects();
                var query = from r in res
                             select r;
                if (client.name.Length != 0 && client.fname.Length != 0 && client.lname.Length != 0)
                {
                    query = query.Where(c => c.name == client.name && c.fname == client.fname && c.lname == client.lname);
                }
                else if (client.name.Length != 0 && client.fname.Length != 0)
                {
                    query = query.Where(c => c.name == client.name && c.fname == client.fname);
                }
                else if (client.name.Length != 0 && client.lname.Length != 0)
                {
                    query = query.Where(c => c.name == client.name && c.lname == client.lname);
                }
                else if (client.fname.Length != 0 && client.lname.Length != 0)
                {
                    query = query.Where(c => c.fname == client.fname && c.lname == client.lname);
                }
                else if (client.name.Length != 0)
                {
                    query = query.Where(c => c.name == client.name);
                }
                else if (client.fname.Length != 0)
                {
                    query = query.Where(c => c.fname == client.fname);
                }
                else if (client.lname.Length != 0)
                {
                    query = query.Where(c => c.lname == client.lname);
                }

                if (obj != null)
                {

                    if (obj.code.Length != 0 && obj.code_type.Length != 0)
                    {
                        query = query.Where(c => c.code == obj.code && c.code_type == obj.code_type);
                    }
                    else if (obj.code.Length != 0)
                    {
                        query = query.Where(c => c.code == obj.code);
                    }
                    else if (obj.code_type.Length != 0)
                    {
                        query = query.Where(c => c.code_type == obj.code_type);
                    }
                }
                
                return query.ToList();
            }
        }

        public static List<ObjectAndClient> GetAllObjects() {
            var dbContext = new ServiceSystemDataContext();
            var query = (from o in dbContext.OBJECT
                         join c in dbContext.CLIENT
                         on o.id_cli equals c.id_client
                         into oc from c in oc.DefaultIfEmpty()
                         select new {
                             id_client = c.id_client,
                             code = o.code,
                             code_type = o.code_type,
                             nr_obj = o.nr_obj,
                             name = c.name,
                             fname = c.fname,
                             lname = c.lname,
                             tel = c.tel
                         });

            List<ObjectAndClient> objAndClient = new List<ObjectAndClient>();
            foreach(var q in query) {
                ObjectAndClient newObjAndClient = new ObjectAndClient();
                newObjAndClient.id_client = q.id_client;
                newObjAndClient.code = q.code;
                newObjAndClient.nr_obj = q.nr_obj;
                newObjAndClient.code_type = q.code_type;
                newObjAndClient.tel = q.tel;
                newObjAndClient.name = q.name;
                newObjAndClient.fname = q.fname;
                newObjAndClient.lname = q.lname;
                objAndClient.Add(newObjAndClient);
            }
            return objAndClient;
        }

        public static void DeleteObject(OBJECT obj)
        {
            using (ServiceSystemDataContext dbContext = new ServiceSystemDataContext()) {
                try
                {
                    var requests = from r in dbContext.REQUEST
                                  where r.nr_obj == obj.nr_obj
                                  select r;
                    foreach (var req in requests)
                    {
                        RequestController.DeleteRequest(req);
                    }



                    OBJECT obj1 = dbContext.OBJECT.SingleOrDefault(x => x.nr_obj == obj.nr_obj);
                    dbContext.OBJECT.DeleteOnSubmit(obj1);
                    dbContext.SubmitChanges();
                }
                catch (Exception ex)
                {
                    
                }
            }
        }
    }
}
