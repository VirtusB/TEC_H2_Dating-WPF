using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEC_H2_Dating
{
    public class Profile
    {
        #region private attributes
        private string firstname;
        private int age;
        private int zip;
        private byte[] profileimage;
        private string profilebio;
        private string city;
        private int profileid;
        #endregion

        #region getters and setters
        public string FirstName
        {
            get
            {
                return firstname;
            }
            set
            {
                firstname = value;
            }
        }

        public int ProfileID
        {
            get
            {
                return profileid;
            }
            set
            {
                profileid = value;
            }
        }

        public string City
        {
            get
            {
                return city;
            }
            set
            {
                city = value;
            }
        }


        public int Age
        {
            get
            {
                return age;
            }
            set
            {
                age = value;
            }
        }

        public int Zip
        {
            get
            {
                return zip;
            }
            set
            {
                zip = value;
            }
        }

        public byte[] ProfileImage
        {
            get
            {
                return profileimage;
            }
            set
            {
                profileimage = value;
            }
        }

        public string ProfileBio
        {
            get
            {
                return profilebio;
            }
            set
            {
                profilebio = value;
            }
        }


        #endregion

        public Profile()
        {
            FirstName = "Intet fornavn";
            Age = 0;
            Zip = 0000;
            ProfileImage = null;
            ProfileBio = "No bio";
            City = "Ingen by";
            ProfileID = 0;
        }

        
    }
}
