using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flights.Helper
{
    public class Helper
    {
        public static string Administrator = "Administrator";
        public static string Agent = "Agent";
        public static string Posetilac = "Posetilac";

        public static List<SelectListItem> GetRolesForDropDown()
        {
            return new List<SelectListItem>
            {
                new SelectListItem{Value = Helper.Administrator, Text = Helper.Administrator},
                new SelectListItem{Value = Helper.Agent, Text = Helper.Agent},
                new SelectListItem{Value = Helper.Posetilac, Text = Helper.Posetilac}
            };
        }
    }
}
