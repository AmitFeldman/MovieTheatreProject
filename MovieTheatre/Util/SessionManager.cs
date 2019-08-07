using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieTheatre.Util
{
    public static class SessionManager
    {
        public static Boolean isUserLoggedOn(HttpSessionStateBase Session)
        {
            try
            {
                var isUserLoggedOn = (Boolean)Session["isUserLoggedOn"];
                return isUserLoggedOn;
            }
            catch
            {
                return false;
            }
        } 

        public static int getUserID(HttpSessionStateBase Session)
        {
            try
            {
                var currentUserID = (int)Session["userID"];
                return currentUserID;
            }
            catch
            {
                return 0;
            }
        }

        public static Boolean isCurrentUserManager(HttpSessionStateBase Session)
        {
            try
            {
                var isCurrentUserManager = (Boolean)Session["isCurrentUserManager"];
                return isCurrentUserManager;
            }
            catch
            {
                return false;
            }
        }

        public static void setUserLoggedOn(HttpSessionStateBase Session, int userID, Boolean isManager)
        {
            Session.Add("isUserLoggedOn", true);
            Session.Add("userID", userID);
            Session.Add("isCurrentUserManager", isManager);
        }

        public static void setUserLoggedOff(HttpSessionStateBase Session)
        {
            // Overrides data if exists
            Session.Add("isUserLoggedOn", false);
            Session.Add("userID", 0);
            Session.Add("isCurrentUserManager", false);
        }

        public static void clearSession(HttpSessionStateBase Session)
        {
            Session.Remove("isUserLoggedOn");
            Session.Remove("userID");
            Session.Remove("isCurrentUserManager");
        }
    }
}