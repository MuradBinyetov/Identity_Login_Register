using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityTest.Models
{
    public class Mapper :Profile
    {
        public Mapper()
        {
            this.CreateMap<AppUser, Register>().ReverseMap();
               
        }
    }
}
