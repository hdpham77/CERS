using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CERS.Guidance;
using CERS.Model;
using CERS.Repository;

namespace CERS
{
    public static class GuidanceMessageTemplates
    {
        #region Fields

        private static object _Lock = new object();
        private static GuidanceMessageTemplateCollection _MessageCache;
        private static bool _CacheLoaded;

        #endregion Fields

        #region Properties

        public static GuidanceMessageTemplateCollection Cache
        {
            get
            {
                Initialize();
                return _MessageCache;
            }
        }

        #endregion Properties

        #region Initialize Method

        public static void Initialize()
        {
            if (!_CacheLoaded)
            {
                lock (_Lock)
                {
                    _MessageCache = GuidanceMessageTemplateRepository.GetMessages();
                }
            }
        }

        #endregion Initialize Method

        #region GetByCode

        public static GuidanceMessage GetNewGuidanceMessage(GuidanceMessageCode code, params object[] formatArguments)
        {
            var template = GetByCode(code);
            GuidanceMessage gm = new GuidanceMessage();
            gm.GuidanceMessageTemplateID = template.ID;
            gm.Message = GetFormattedTemplateMessage(template, formatArguments);
            return gm;
        }

        public static IGuidanceMessageTemplate GetByCode(int code)
        {
            IGuidanceMessageTemplate template = null;
            template = Cache.SingleOrDefault(p => p.Code == code);
            if (template == null)
            {
                throw new ArgumentNullException("Cannot find the GuidanceMessageTemplate for Code: " + code.ToString() + ".");
            }
            return template;
        }

        #endregion GetByCode

        #region GetByCode

        public static IGuidanceMessageTemplate GetByCode(GuidanceMessageCode code)
        {
            IGuidanceMessageTemplate template = null;

            template = Cache.SingleOrDefault(p => p.Code == (int)code);
            if (template == null)
            {
                throw new ArgumentNullException("Cannot find the GuidanceMessageTemplate for \"" + code.ToString() + "\" (Code: " + ((int)code).ToString() + ").");
            }
            return template;
        }

        #endregion GetByCode

        public static string GetFormattedTemplateMessage(IGuidanceMessageTemplate template, params object[] formatArguments)
        {
            string result = template.TemplateMessage;
            if (formatArguments != null)
            {
                result = string.Format(result, formatArguments);
            }
            return result;
        }

        /// <summary>
        /// Gets a message from the template by the specified <paramref name="code"/> <see cref="GuidanceMessageCode"/> and formats it with the format arguments specified.
        /// </summary>
        /// <param name="code">The <see cref="GuidanceMessageCode"/> representing the message to be used.</param>
        /// <param name="formatArguments">An array of <see cref="System.Object"/> containing the values to format with.</param>
        /// <returns>Either an unformatted string if no <paramref name="formatArguments"/> where specified or a formatted string.</returns>
        public static string GetFormattedTemplateMessage(GuidanceMessageCode code, params object[] formatArguments)
        {
            var template = GetByCode(code);
            return GetFormattedTemplateMessage(template, formatArguments);
        }

        /// <summary>
        /// Formats the specified <paramref name="message"/> (<see cref="String"/>)  with the format arguments specified.
        /// </summary>
        /// <param name="message">The <see cref="String"/> containing the message to format.</param>
        /// <param name="formatArguments">An array of <see cref="System.Object"/> containing the values to format with.</param>
        /// <returns>Either an unformatted string if no <paramref name="formatArguments"/> where specified or a formatted string.</returns>
        public static string GetFormattedTemplateMessage(string message, params object[] formatArguments)
        {
            string result = message;
            if (!string.IsNullOrWhiteSpace(message))
            {
                if (formatArguments != null)
                {
                    if (formatArguments.Count() > 0)
                    {
                        result = string.Format(message, formatArguments);
                    }
                }
            }
            else
            {
                result = string.Empty;
            }
            return result;
        }

        #region RebuildCache

        public static void RebuildCache()
        {
            _CacheLoaded = false;
            Initialize();
        }

        #endregion RebuildCache
    }
}