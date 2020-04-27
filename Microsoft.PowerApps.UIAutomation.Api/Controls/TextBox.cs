﻿using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.PowerApps.UIAutomation.Api.Controls
{
    public class TextBox : PlayerControl
    {
        public override void SetValue(InteractiveBrowser browser)
        {
            if (browser.Driver.HasElement(By.XPath($"//div[contains(@data-control-name,'{this.ControlName}')]")))
            {
                //Click the down arrow
                var appControl = browser.Driver.FindElement(By.XPath($"//div[@data-control-name='{this.ControlName}']"));

                if (appControl.FindElements(By.TagName("input")).Count > 0)
                    appControl.FindElement(By.TagName("input")).SendKeys(this.ControlValue);
                else
                    throw new NotFoundException($"Unable to find { this.ControlName } text box.");

                browser.ThinkTime(100);

            }
        }
    }
}
