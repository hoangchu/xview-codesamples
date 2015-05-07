using System;
using System.Diagnostics;

using Tridion.ContentManager.Templating;

namespace Chimote.Tridion.Templating.Intranet.Common
{
    public class TridionLogger
    {
        private readonly TemplatingLogger logger;

        public TridionLogger(TemplatingLogger logger)
        {
            this.logger = logger;
        }

        public void Error(string message)
        {
            this.logger.Error(message);
        }

        public void Error(string message, Exception exception)
        {
            this.logger.Error(message, exception);
        }

        [Conditional("DEBUG")]
        public void Debug(string message)
        {
            this.logger.Debug(message);
        }

        public void Info(string message)
        {
            this.logger.Info(message);
        }

        public void Warning(string message)
        {
            this.logger.Warning(message);
        }
    }
}