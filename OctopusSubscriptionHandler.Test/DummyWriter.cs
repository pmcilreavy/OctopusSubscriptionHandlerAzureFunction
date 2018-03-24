using System;
using System.Diagnostics;
using Microsoft.Azure.WebJobs.Host;

namespace OctopusSubscriptionHandler.Test
{
    public class DummyWriter : TraceWriter
    {
        public DummyWriter(TraceLevel level) : base(level)
        {
        }

        public override void Trace(TraceEvent traceEvent)
        {

        }

        public new void Error(string message,
            Exception ex = null,
            string source = null)
        {
        }


        public new void Info(string message,
            string source = null)
        {
        }

        public new void Verbose(string message,
            string source = null)
        {
        }

        public new void Warning(string message,
            string source = null)
        {
        }

        public override void Flush()
        {

        }
    }
}