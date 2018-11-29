using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LG.Data;
using LG.Common;

namespace LG.Models
{
    public class ProfileServer : Profile
    {
        public ProfileServer(ProfileDAL pd) 
        {
            this.TakePhotoFromServer = true;
        }
    }
}
