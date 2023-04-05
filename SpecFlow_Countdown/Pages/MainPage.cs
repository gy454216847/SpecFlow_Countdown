using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecFlow_Countdown.Pages
{
    public class MainPage
    {
        public static string SignInOrRegister = "xpath=/html/body/wnz-content/global-nav/div[1]/div[1]/global-nav-header/header/div[4]/section/global-nav-quick-nav/nav/ul/li[4]/cdx-popup/button";
        public static string SignIn = "classname=signIn-button";
        public static string DownArrow = "name=chevron-bottom";
        public static string SignOut = "class=myAccount-signOut";
    }
}
