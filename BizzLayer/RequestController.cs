using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizzLayer
{
    public class RequestController
    {
        public static List<REQUEST> GetAllRequests()
        {
            var dbContext = new ServiceSystemDataContext();
            var query = (from r in dbContext.REQUEST
                         select r).ToList();
            return query;
        }

        public static bool AddNewRequest(REQUEST newRequest)
        {
            using (ServiceSystemDataContext dbContext = new ServiceSystemDataContext())
            {
                dbContext.REQUEST.InsertOnSubmit(newRequest);
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

        public static bool ChangeRequestDetails(REQUEST req)
        {
            using (ServiceSystemDataContext dbContext = new ServiceSystemDataContext())
            {
                try
                {
                    REQUEST request = dbContext.REQUEST.SingleOrDefault(x => x.id_req == req.id_req);
                    
                    request.status = req.status;
                    request.result = req.result;
                    request.descr = req.descr;
                    request.dt_req = req.dt_req;
                    request.dt_fin_cancel = req.dt_fin_cancel;
                    dbContext.SubmitChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
                
            }
        }

        public static List<REQUEST> GetRequestsByCriteria(CLIENT client, OBJECT obj)
        {
            using (ServiceSystemDataContext dbContext = new ServiceSystemDataContext())
            {
                var requests = GetAllRequests();
                var r = from rs in requests
                        select rs;
                var objects = ObjectController.GetObjectsByCriteria(client, obj);
                var o = from ob in objects
                        select ob;
                var q = (from rs in r
                        join ob in o on rs.nr_obj equals ob.nr_obj
                        select rs).ToList();
                return q;
            }
        }

        public static void DeleteRequest(REQUEST request)
        {
            using (ServiceSystemDataContext dbContext = new ServiceSystemDataContext())
            {
                var activities = from a in dbContext.ACTIVITY
                               where a.id_req == request.id_req
                               select a;
                foreach (var act in activities)
                {
                    dbContext.ACTIVITY.DeleteOnSubmit(act);
                }

                try
                {
                    REQUEST req = dbContext.REQUEST.SingleOrDefault(x => x.id_req == request.id_req);
                    dbContext.REQUEST.DeleteOnSubmit(req);
                    dbContext.SubmitChanges();
                }
                catch (Exception ex)
                {
                }
            }
        }
    }
}
