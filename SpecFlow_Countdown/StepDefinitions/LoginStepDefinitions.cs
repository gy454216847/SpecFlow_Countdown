using NUnit.Framework;

using SpecFlow_Countdown.Core;
using SpecFlow_Countdown.Pages;

using System;

using TechTalk.SpecFlow;

namespace SpecFlow_Countdown.StepDefinitions
{
    [Binding]
    public class LoginStepDefinitions
    {
        [Given(@"I am on the Countdown page")]
        public void GivenIAmOnTheCountdownPage()
        {
            //throw new PendingStepException();
        }

        [When(@"I click Sign in link")]
        public void WhenIClickSignInLink()
        {
            CommonWeb.Click(MainPage.SignInOrRegister);
            CommonWeb.Click(MainPage.SignIn);
        }

        [When(@"I input right (.*) and (.*)")]
        public void WhenIInputRightQq_ComAndGy(string username, string passwords)
        {
            CommonWeb.Type(SignInPage.Email, username);
            CommonWeb.Type(SignInPage.Password, passwords);
        }

        [When(@"I click login button")]
        public void WhenIClickLoginButton()
        {
            Thread.Sleep(2000);
            CommonWeb.Click(SignInPage.SignIn);
        }

        [Then(@"I can login my account")]
        public void ThenICanLoginMyAccount()
        {
            Assert.IsTrue(CommonWeb.IsElementExist(MainPage.DownArrow));
        }

        [When(@"I input wrong (.*) and wrong (.*)")]
        public void WhenIInputWrongQq_ComAndWrong(string username, string passwords)
        {
            CommonWeb.Type(SignInPage.Email, username);
            CommonWeb.Type(SignInPage.Password, passwords);
        }

        [Then(@"I can see the An invalid email and/or password has been entered\. Please try again\. Please note, passwords are case-sensitive\.")]
        public void ThenICanSeeTheAnInvalidEmailAndOrPasswordHasBeenEntered_PleaseTryAgain_PleaseNotePasswordsAreCase_Sensitive_()
        {
            Assert.True(CommonWeb.IsElementExist(SignInPage.Label));
        }


        [When(@"I  do not input username and passwords")]
        public void WhenIDoNotInputUsernameAndPasswords()
        {
            Thread.Sleep(2000);
            CommonWeb.Click(SignInPage.SignIn);
        }

        [Then(@"I can see “This field is required""")]
        public void ThenICanSeeThisFieldIsRequired()
        {
            string errorText = CommonWeb.GetText(SignInPage.ErrorText);
            Assert.AreEqual("This field is required", errorText);
        }

        [When(@"I click Sign out link")]
        public void WhenIClickSignOutLink()
        {
            //throw new PendingStepException();
            CommonWeb.Click(MainPage.DownArrow);
            CommonWeb.Click(MainPage.SignOut);
        }

        [Then(@"I can logout my account")]
        public void ThenICanLogoutMyAccount()
        {
            //throw new PendingStepException();
            Assert.IsTrue(CommonWeb.IsElementExist(MainPage.SignInOrRegister));
        }
    }
}
