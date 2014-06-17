using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineTestingService
{
    public static class HtmlHelpers
    {
        public static HtmlString SetGridInsertButtonText(this HtmlHelper helper, string gridName, string buttonText)
        {
            string script =
    "<script type=\"text/javascript\">" +
        "insertButton = document.getElementById('" + gridName + "').getElementsByClassName('t-toolbar')[0].getElementsByClassName('t-grid-add')[0];" +
        "insertButton.removeChild(insertButton.lastChild);" +
        "insertButton.appendChild(document.createTextNode('" + buttonText + "'));" +
    "</script>";

            return new HtmlString(script);
        }

        public static HtmlString CustomAddButton(this HtmlHelper helper, string buttonType, string buttonText)
        {
            string button =
            "<button class=\"t-button t-button-icontext t-grid-add\" type=\"" + buttonType + "\">" +
                "<span class=\"t-icon t-add\"></span>" +
                buttonText +
            "</button>";

            return new HtmlString(button);
        }

        public static HtmlString CustomUpdateButton(this HtmlHelper helper, string buttonType, string buttonText)
        {
            string button =
            "<button class=\"t-button t-button-icontext t-grid-update\" type=\"" + buttonType + "\">" +
                "<span class=\"t-icon t-update\"></span>" +
                buttonText +
            "</button>";

            return new HtmlString(button);
        }

        public static string CustomReviewButton(this HtmlHelper helper, string url, string buttonText)
        {
            string button =
            "<a class=\"t-button t-button-icontext t-grid-update\" href=\"" + url + "\">" +
                "<span class=\"t-review\"></span>" +
                buttonText +
            "</button>";

            return button;
        }
    }

    public class DetailsImage
    {
        public string style { get; private set; }

        private DetailsImage()
        {
            style = "background-image : url(/Content/details.png);";
        }

        private static DetailsImage image;
        public static DetailsImage _
        {
            get
            {
                if (image == null) image = new DetailsImage();
                return image;
            }
        }
    }
}