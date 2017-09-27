using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizzLayer
{
    public class ObjectClientActivityRequestWorker
    {
        public int id_client { get; set; }
        public int nr_obj { get; set; }
        public String code { get; set; }
        public String code_type { get; set; }
        public String name { get; set; }
        public String fname { get; set; }
        public String lname { get; set; }
        public int tel { get; set; }
        public int id_req { get; set; }
        public int id_act { get; set; }
        public String act_type { get; set; }
        public String wrk_fname { get; set; }
        public String wrk_lname { get; set; }
        public int wrk_id { get; set; }
        public int sequence { get; set; }
        public String status { get; set; }
        public String description { get; set; }
        public String result { get; set; }
        public DateTime dt_req { get; set; }
        public DateTime dt_fin_cancel { get; set; }

    }

    public class SearchActivity
    {
        //client
        public String nameC { get; set; }
        public String fnameC { get; set; }
        public String lnameC { get; set; }
        //worker
        public String fnameW { get; set; }
        public String lnameW { get; set; }
        public String act_type { get; set; }
        public int id_req { get; set; }
        public String name_type { get; set; }
        public String code_type { get; set; }
    }
    public class ActivityController
    {
        public static bool AddNewActivity(ACTIVITY activity)
        {
            using (ServiceSystemDataContext dbContext = new ServiceSystemDataContext())
            {
                dbContext.ACTIVITY.InsertOnSubmit(activity);
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

        public static List<ObjectClientActivityRequestWorker> GetAllActivities()
        {
            var dbContext = new ServiceSystemDataContext();
            var query = (from a in dbContext.ACTIVITY
                         join r in dbContext.REQUEST
                         on a.id_req equals r.id_req
                         join o in dbContext.OBJECT
                         on r.nr_obj equals o.nr_obj
                         join c in dbContext.CLIENT
                         on o.id_cli equals c.id_client
                         join p in dbContext.PERSONEL
                         on a.id_wrk equals p.id_pers
                         select new
                         {
                             id_client = c.id_client,
                             code = o.code,
                             code_type = o.code_type,
                             nr_obj = o.nr_obj,
                             name = c.name,
                             fname = c.fname,
                             lname = c.lname,
                             tel = c.tel,
                             id_req = r.id_req,
                             id_act = a.id_act,
                             act_type = a.act_type,
                             status = a.status,
                             result = a.result,
                             sequence = a.seq_no,
                             dt_req = a.dt_req,
                             dt_fin_cancel = a.dt_fin_cancel,
                             description = a.descr,
                             wrk_fname = p.fname,
                             wrk_lname = p.lname,
                             wrk_id = p.id_pers
              
                         });

            List<ObjectClientActivityRequestWorker> ocarwList = new List<ObjectClientActivityRequestWorker>();
            foreach (var q in query)
            {
                ObjectClientActivityRequestWorker ocarw = new ObjectClientActivityRequestWorker();
                ocarw.id_client = q.id_client;
                ocarw.code = q.code;
                ocarw.nr_obj = q.nr_obj;
                ocarw.code_type = q.code_type;
                ocarw.tel = q.tel;
                ocarw.name = q.name;
                ocarw.fname = q.fname;
                ocarw.lname = q.lname;
                ocarw.id_req = q.id_req;
                ocarw.id_act = q.id_act;
                ocarw.status = q.status;
                ocarw.description = q.description;
                ocarw.result = q.result;
                ocarw.sequence = q.sequence;
                ocarw.dt_req = q.dt_req;
                ocarw.dt_fin_cancel = (DateTime)q.dt_fin_cancel;
                ocarw.act_type = q.act_type;
                ocarw.wrk_fname = q.wrk_fname;
                ocarw.wrk_lname = q.wrk_lname;
                ocarw.wrk_id = q.wrk_id;
                ocarwList.Add(ocarw);
            }
            return ocarwList;
        }

        public static List<ObjectClientActivityRequestWorker> GetActivitiesByWrkId(int id) {
            var q = GetAllActivities();
            var query = (from qs in q
                         where qs.wrk_id == id
                         select qs).ToList();
            return query;
        }

        public static bool ChangeActivity(ACTIVITY act) {
            using (ServiceSystemDataContext dbContext = new ServiceSystemDataContext())
            {
                try
                {
                    ACTIVITY activity = dbContext.ACTIVITY.SingleOrDefault(x => x.id_act == act.id_act);
                    activity.status = act.status;
                    activity.descr = act.descr;
                    activity.result = act.result;
                    activity.seq_no = act.seq_no;
                    activity.dt_fin_cancel = act.dt_fin_cancel;
                    dbContext.SubmitChanges();
                    return true;
                }
                catch (Exception ex) {
                    return false;
                }
                
            }

        }

        public static void DeleteActivity(int id)
        {
            using (ServiceSystemDataContext dbContext = new ServiceSystemDataContext())
            {
                try
                {

                    ACTIVITY act = dbContext.ACTIVITY.SingleOrDefault(x => x.id_act == id);
                    dbContext.ACTIVITY.DeleteOnSubmit(act);
                    dbContext.SubmitChanges();
                }
                catch (Exception ex)
                {

                }
            }
        }

        public static String[] GetAllObjectTypes()
        {
            using (ServiceSystemDataContext dbContext = new ServiceSystemDataContext())
            {
                try
                {
                    var objectsTypes = from o in dbContext.OBJ_TYPE
                                       select o;
                    String[] objNames = new String[objectsTypes.Count()];
                    List<OBJ_TYPE> objectsTypesNames = objectsTypes.ToList();

                    int i = 0;
                    foreach (OBJ_TYPE name in objectsTypesNames)
                    {
                        objNames[i] = name.name_type;
                        i++;
                    }
                    return objNames;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public static String getActivityByName(String activityName)
        {
            if (!activityName.Equals(null) && !activityName.Equals(""))
            {
                using (ServiceSystemDataContext dbContext = new ServiceSystemDataContext())
                {
                    try
                    {
                        var activityTypes = from a in dbContext.ACT_DICT
                                            select a;
                        if (activityTypes.Count() > 0)
                        {
                            activityTypes = activityTypes.Where(a => a.act_name == activityName);

                            List<ACT_DICT> activityTypesNames = activityTypes.ToList();
                            return activityTypesNames.Last().act_type;
                        }
                        else return null;
                    }
                    catch (Exception ex)
                    {
                        Console.Out.Write("Exception in database");
                        return null;
                    }
                }
            }
            else
            {
                Console.Out.Write("Empty string");
                return null;
            }

        }

        public static List<ObjectClientActivityRequestWorker> GetActivityBySearchCriteria(SearchActivity srchCriteria)
        {
            if (!srchCriteria.Equals(null))
            {
                using (ServiceSystemDataContext dbContext = new ServiceSystemDataContext())
                {
                    try
                    {
                        var q = GetAllActivities();
                        var activities = from qs in q
                                         select qs;

                        
                        if (activities.Count() > 0)
                        {
                            
                            activities = activities.Where(a => a.act_type == srchCriteria.act_type);

                            if (srchCriteria.act_type.Length != 0)
                            {
                                activities = activities.Where(a => a.act_type == srchCriteria.act_type);
                            }
                            if (srchCriteria.id_req > 0)
                            {
                                activities = activities.Where(a => a.id_req == srchCriteria.id_req);
                            }
                            if (srchCriteria.nameC.Length != 0)
                            {
                                activities = activities.Where(a => a.name == srchCriteria.nameC);
                            }
                            if (srchCriteria.fnameC.Length != 0)
                            {
                                activities = activities.Where(a => a.fname == srchCriteria.fnameC);
                            }
                            if (srchCriteria.lnameC.Length != 0)
                            {
                                activities = activities.Where(a => a.lname == srchCriteria.lnameC);
                            }
                            
                            if (srchCriteria.lnameW.Length != 0)
                            {
                                activities = activities.Where(a => a.wrk_lname == srchCriteria.lnameW);
                            }
                            if (srchCriteria.fnameW.Length != 0)
                            {
                                activities = activities.Where(a => a.wrk_fname == srchCriteria.fnameW);
                            }
                            if (srchCriteria.name_type.Length != 0)
                            {
                                activities = activities.Where(a => a.act_type == srchCriteria.name_type);
                            }
                            if (srchCriteria.code_type.Length != 0)
                            {
                                activities = activities.Where(a => a.code == srchCriteria.code_type);
                            }

                            var activitiesResult = activities.ToList();
                            List<ObjectClientActivityRequestWorker> activitesWithName = new List<ObjectClientActivityRequestWorker>();
                            int i = 0;
                            foreach (var actResult in activitiesResult)
                            {

                                ObjectClientActivityRequestWorker act = new ObjectClientActivityRequestWorker();
                               
                                act.id_act = actResult.id_act;
                                act.id_req = actResult.id_req;
                                act.wrk_id = actResult.wrk_id;
                                act.code = actResult.code;
                                act.code_type = actResult.code_type;
                                act.id_client = actResult.id_client;
                                act.nr_obj = actResult.nr_obj;
                                act.act_type = actResult.act_type;
                                act.sequence = actResult.sequence;
                                act.description = actResult.description;
                                act.result = actResult.result;
                                act.status = actResult.status;
                                act.dt_req = actResult.dt_req;
                                act.dt_fin_cancel = (DateTime)actResult.dt_fin_cancel;
                                act.fname = actResult.name;
                                act.lname = actResult.lname;
                                act.name = actResult.fname;
                                act.wrk_fname= actResult.wrk_fname;
                                act.wrk_lname= actResult.wrk_lname;

                                activitesWithName.Add(act);
                            }
                            return activitesWithName;

                        }
                        else return null;
                    }
                    catch (Exception ex)
                    {
                        Console.Out.Write("Exception in database");
                        return null;
                    }
                }
            }
            else
            {
                Console.Out.Write("Empty string");
                return null;
            }
        }

        public class SearchActivity
        {
            //client
            public String nameC { get; set; }
            public String fnameC { get; set; }
            public String lnameC { get; set; }
            //worker
            public String fnameW { get; set; }
            public String lnameW { get; set; }
            public String act_type { get; set; }
            public int id_req { get; set; }
            public String name_type { get; set; }
            public String code_type { get; set; }
        }

        public class ActivityDict
        {
            public String act_type { get; set; }
            public String act_name { get; set; }
        }
    }
}
