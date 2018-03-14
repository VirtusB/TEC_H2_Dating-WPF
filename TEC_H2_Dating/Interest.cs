using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEC_H2_Dating
{
    public class Interest
    {
        #region private attributes
        private string interestname;
        
        #endregion

        #region getters and setters
        public string InterestName
        {
            get
            {
                return interestname;
            }
            set
            {
                interestname = value;
            }
        }

        


        #endregion

        public Interest()
        {
            InterestName = "Intet";          
        }
    }
}
