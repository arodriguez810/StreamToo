using Admin.CustomCode;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Admin.Models.ViewModels
{
    public class PermissionsModel
    {
        Context db;
        public PermissionsModel()
        {
            //Inicializations
            db = new Context();
            menus = new List<BaseMenu>();
            data = new JsonResult();

            //Fill Menu
            Admin.Models.BaseUser user = Helper.GetUser(db);
            if (user.superUser)
            {
                var byOwnPermission = db.BaseMenus.Where(d => d.menuID == null).OrderBy(d => d.noOrder).ToList();
                foreach (var item in byOwnPermission)
                {
                    if (Menus.Where(d => d.id == item.id).Count() == 0)
                    {
                        Menus.Add(item);
                    }
                }
            }
            else
            {
                var byOwnPermission = Helper.GetUser(db).BaseUserMenus.Where(d => d.BaseMenu.menuID == null).OrderBy(d => d.noOrder);
                foreach (var item in byOwnPermission)
                {
                    if (Menus.Where(d => d.id == item.BaseMenu.id).Count() == 0)
                    {
                        Menus.Add(item.BaseMenu);
                    }
                }
                var profiles = Helper.GetUser(db).BaseProfiles;
                foreach (var profile in profiles)
                {
                    var byProfile = profile.BaseProfileMenus.OrderBy(d => d.noOrder);
                    foreach (var item in byProfile)
                    {
                        if (Menus.Where(d => d.id == item.BaseMenu.id).Count() == 0)
                        {
                            Menus.Add(item.BaseMenu);
                        }
                    }
                }
            }
        }

        //Variables
        List<BaseMenu> menus;
        object data;

        //Properties
        public List<BaseMenu> Menus
        {
            get { return menus; }
            set { menus = value; }
        }
        public object Data
        {
            get { return data; }
            set { data = value; }
        }
    }
}