using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Email
{
    public class EmailBodyModel
    {
        public string Value { get; }

        public struct BodyTemplateName
        {
            private BodyTemplateName(string value) { Value = value; }

            public string Value { get; set; }

            public static BodyTemplateName Vencimiento { get { return new BodyTemplateName("vencimiento.html"); } }
            
            // Agreagr mas templates
        }

        private EmailBodyModel(string body)
        {
            Value = body;
        }

        public static EmailBodyModel CreateBodyHtmlTemplate(string bodyHtmlTemplatePath, BodyTemplateName bodyTemplateName, Dictionary<string, string> bodyHtmlTemplateValues = null)
        {
            Utils.IsNullOrEmptyThrowException(bodyHtmlTemplatePath, nameof(bodyHtmlTemplatePath));

            var filePath = Path.Combine(bodyHtmlTemplatePath, bodyTemplateName.Value);

            Utils.IsFileExistsThrowException(filePath);

            var template = new StreamReader(filePath).ReadToEnd();

            if (bodyHtmlTemplateValues?.Any() == true)
            {
                foreach (var item in bodyHtmlTemplateValues)
                {
                    template = template.Replace(item.Key, item.Value);
                }
            }

            return new EmailBodyModel(template);
        }

        public static EmailBodyModel CreateBodyHtml(string bodyHtml)
        {
            Utils.IsNullOrEmptyThrowException(bodyHtml, nameof(bodyHtml));

            return new EmailBodyModel(bodyHtml);
        }

        public static EmailBodyModel CreateBodyText(string bodyPlainText)
        {
            Utils.IsNullOrEmptyThrowException(bodyPlainText, nameof(bodyPlainText));

            return new EmailBodyModel($"<p>{bodyPlainText}</p>");
        }
    }
}
