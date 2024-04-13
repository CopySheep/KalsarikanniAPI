using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelFuen31.APIs.Models;
using HotelFuen31.APIs.Interface.Guanyu;
using Microsoft.IdentityModel.JsonWebTokens;
using HotelFuen31.APIs.Services.Guanyu;
using HotelFuen31.APIs.Dtos;
using System.Security.Cryptography;
using HotelFuen31.APIs.Dtos.Guanyu;
using System.Net.Http;

namespace HotelFuen31.APIs.Controllers.Guanyu
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private AppDbContext _db;
        private readonly IUser _iuser;

        public MembersController(AppDbContext db, IUser iuser)
        {
            _db = db;
            _iuser = iuser;
        }

        //GET: api/Members/密文
        [HttpGet("{str}")]
        public IActionResult GetMember(string str)
        {
            if(_iuser.GetMember(str) == "401") return Unauthorized();
            return Content(_iuser.GetMember(str));
        }

        // GET: api/Members/Login?
        [HttpGet("Login")]
        public string MemberLogin([FromQuery] string phone, [FromQuery] string pwd)
        {
            return _iuser.GetCryptostring(phone, pwd);
        }

        //GET: api/Members?
        [HttpGet]
        public MemberDto GetMembers([FromQuery] string token)
        {
            int id = int.Parse(_iuser.GetMember(token));
            Member member = _db.Members.Find(id);
            var MemberData = new MemberDto();

            MemberData.Name = member.Name;
            MemberData.Phone = member.Phone;
            MemberData.Email = member.Email;
            MemberData.IdentityNumber = member.IdentityNumber;
            MemberData.BirthDay = (DateTime)member.BirthDay;
            MemberData.Gender = member.Gender == true? "男性":"女性";
            MemberData.Address = member.Address;
            MemberData.Ban = member.Ban;
            MemberData.Level = _db.MemberLevels.Find(member.LevelId).Name;


            return MemberData;
        }

        [HttpGet("pwd")]
        public string GetEncryptPwd([FromQuery] string pwd, [FromQuery] string key)
        {
            return _iuser.CryptoHash(pwd, key);
        }

        [HttpPost]
        public string NewMember(Member member)
        {
            return _iuser.NewMember(member);
        }

        [HttpPut]
        public string EditMember(MemberDto memberdto)
        {
            return _iuser.EditMember(memberdto);
        }

        [HttpPut("EditPwd")]
        public string EditPassword(EditPwdDto editpwddto)
        {
            return _iuser.EditPwd(editpwddto);
        }
    }
}
