using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;
using MVCTeknolojikMarketYonetimPaneli.Models.Model;

namespace MVCTeknolojikMarketYonetimPaneli.Security
{
    public class MVCTeknolojikMarketYonetimPaneliRoleProvider : RoleProvider
    {

        

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            TeknolojikMarketEntities db = new TeknolojikMarketEntities();

            var personel = db.TBL_PERSONEL.Where(m => m.KULLANICIADI == username).FirstOrDefault();
            var yonetici = db.TBL_YONETICI.Where(m => m.KULLANICIADI == username).FirstOrDefault();
            var admin = db.TBL_ADMIN.Where(m => m.KULLANICIADI == username).FirstOrDefault();

            if (personel != null)
            {

                return new string[] { personel.KULLANICITIPI };

            }
            else if (yonetici != null)
            {

                return new string[] { yonetici.KULLANICITIPI };

            }
            else if (admin != null)
            {

                return new string[] { admin.KULLANICITIPI };

            }else{

                return new string[]{""};
            }



        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}