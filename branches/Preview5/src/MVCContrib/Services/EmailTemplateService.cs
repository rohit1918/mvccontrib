using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace MvcContrib.Services
{
    /// <remarks>
    /// Inspired by Castle's EmailTemplateService.
    /// </remarks>
    public class EmailTemplateService : IEmailTemplateService
    {
        private static readonly String HeaderPattern = @"[ \t]*(?<header>(to|from|cc|bcc|subject|X-\w+)):[ \t]*(?<value>(.)+)(\r*\n*)?";
        private static readonly Regex HeaderRegEx = new Regex(HeaderPattern, RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private readonly IViewEngine _viewEngine;

        public EmailTemplateService(IViewEngine viewEngine)
        {
            if (viewEngine == null) throw new ArgumentNullException("viewEngine");
            _viewEngine = viewEngine;
        }

        #region Message Processing

        private bool IsLineAHeader(string line, out string header, out string value)
        {
            Match match = HeaderRegEx.Match(line);

            if (match.Success)
            {
                header = match.Groups["header"].ToString();
                value = match.Groups["value"].ToString();
                return true;
            }
            else
            {
                header = value = null;
                return false;
            }
        }

        private void ProcessHeader(MailMessage message, string header, string value)
        {
            switch (header.ToLowerInvariant())
            {
                case "to":
                    message.To.Add(new MailAddress(value));
                    break;

                case "cc":
                    message.CC.Add(new MailAddress(value));
                    break;

                case "bcc":
                    message.Bcc.Add(new MailAddress(value));
                    break;

                case "subject":
                    message.Subject = value;
                    break;

                case "from":
                    message.From = new MailAddress(value);
                    break;

                default:
                    message.Headers[header] = value;
                    break;
            }
        }

        private MailMessage ProcessContentStream(Stream stream, Encoding encoding)
        {
            var message = new MailMessage();

            stream.Position = 0;
            using (var reader = new StreamReader(stream, encoding))
            {
                bool isInBody = false;
                var body = new StringBuilder();
                string line, header, value;

                while ((line = reader.ReadLine()) != null)
                {
                    if (!isInBody && String.IsNullOrEmpty(line))
                        continue; //skip blank lines in beginning of message

                    if (!isInBody && IsLineAHeader(line, out header, out value))
                    {
                        ProcessHeader(message, header, value);
                    }
                    else
                    {
                        isInBody = true;
                        body.AppendLine(line);
                    }
                }

                message.Body = body.ToString();
            }

            if (message.Body.ToLowerInvariant().Contains("<html>"))
                message.IsBodyHtml = true;

            return message;
        }

        #endregion

        public virtual MailMessage RenderMessage(ViewContext viewContext)
        {
            HttpResponseBase response = viewContext.HttpContext.Response;

            response.Flush(); //clear out anything that is in there already

            MailMessage message;
            Stream filter = null;

            Stream oldFilter = response.Filter;
            try
            {
                filter = new MemoryStream();
                response.Filter = filter;

                _viewEngine.RenderView(viewContext);

                response.Flush(); //flush content to our filter
                message = ProcessContentStream(filter, response.ContentEncoding);
            }
            finally
            {
                if (filter != null)
                    filter.Dispose();

                response.Filter = oldFilter;
            }

            return message;
        }
    }
}
