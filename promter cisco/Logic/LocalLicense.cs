using System;
using System.Collections.Generic;

namespace Suggeritore_Cisco.Logic
{
    internal static class LocalLicense
    {
        public static Tuple<string, int, string> CheckIfEligible()
        {
            //Account name | License value | Account Description
            Tuple<string, int, string> userDetail = new Tuple<string, int, string>("ADelt", 100, "ADeltaX Project"); //Edit required

            var usersWhiteList = new List<Tuple<string, int, string>>
            {
                new Tuple<string, int, string>("ADelt", 100, "ADeltaX Project"), //for example, me
            };

            usersWhiteList.ForEach(c =>
            {
                if (Environment.UserName.ToLower() == c.Item1.ToLower()) //Check if the opened account session's username is in the whitelist
                    userDetail = new Tuple<string, int, string>(c.Item1, c.Item2, c.Item3); //Then assign it to the userDetail var.
            });
            return userDetail; //Return it
        }
    }
}
