using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace JUST.UnitTests
{
    public class DateFunctionsTests
    {

        [Test]
        public void DateFormat()
        {
            var transformer = "{ \"result\": { \"us-format\": \"#formatdate(#valueof($.lease.start),MM/dd/yyyy)\",\"eu-format\": \"#formatdate(#valueof($.lease.end),dd/MM/yyyy)\" }}";
            var result = new JsonTransformer(new JUSTContext { EvaluationMode = EvaluationMode.Strict })
                .Transform(transformer, ExampleInputs.Date);

            Assert.AreEqual("{\"result\":{\"us-format\":\"02/10/2020\",\"eu-format\":\"10/03/2021\"}}", result);
        }
    }
}
