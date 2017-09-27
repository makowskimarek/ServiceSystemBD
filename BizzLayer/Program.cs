using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Data;

namespace BizzLayer
{
    public class WorkerActivity
    {
        public int id_req { get; set; }
        public int id_wrk { get; set; }
        public string act_type { get; set; }
        public int seq_no { get; set; }
        public string descr { get; set; }
        public string result { get; set; }
        public string status { get; set; }
        public DateTime dt_req { get; set; }
        public DateTime dt_fin_cancel { get; set; }
    }

    public class WorkerActivityFacade
    {
        public static List<ACTIVITY> GetAllWorkerActivities(WorkerActivity searchCrit)
        {
            using (ServiceSystemDataContext dbContext = new ServiceSystemDataContext())
            {
                var query = (from a in dbContext.ACTIVITY
                             select a).ToList();
                return query;

            }
        }

    }
}


/*static public class RegistrationFacade
    {
        
        public static List<Patient> GetPatients(Patient searchCrit)

        {
            var pat = new List<Patient> ();
            pat.Add(new Patient {FName = "aaaa", LName = "AAAAA", BDate = System.DateTime.Now, Height = 180});
            pat.Add(new Patient { FName = "aabbaa", LName = "AABBAA", BDate = System.DateTime.Now, Height = 200});
            pat.Add(new Patient { FName = "bbbb", LName = "BBBBB", BDate = DateTime.Parse("1991-01-01"), Height = 170 });
            var res = (from element in pat where element.LName.StartsWith(searchCrit.LName) select element).ToList();
            return res;
        }


    }
*/