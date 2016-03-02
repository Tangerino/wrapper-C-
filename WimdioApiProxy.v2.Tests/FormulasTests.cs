using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WimdioApiProxy.v2.DataTransferObjects.Formulas;

namespace WimdioApiProxy.v2.Tests
{
    [TestClass()]
    public class FormulasTests : BaseTests
    {
        protected List<Formula> FormulasCreated = new List<Formula>();

        public new void Dispose()
        {
            FormulasCreated.ForEach(async p => await Client.DeleteFormula(p.Id));
            base.Dispose();
        }

        [TestMethod()]
        public void ReadFormulas_Positive()
        {
            IEnumerable<Formula> actual = null;
            Func<Task> asyncFunction = async () =>
            {
                actual = await Client.ReadFormulas();
            };
            asyncFunction.ShouldNotThrow("Method should not throw");
            actual.Should().NotBeNull("Result list should not be NULL");
        }

        [TestMethod()]
        public void CreateFormula_Positive()
        {
            Formula actual = null;
            Func<Task> asyncFunction = async () =>
            {
                actual = await CreateFormula(Client, FormulasCreated);
            };

            asyncFunction.ShouldNotThrow("Method should not throw");
            actual.Should().NotBeNull("Actual value should not be NULL");
        }

        [TestMethod()]
        public void ReadFormula_Positive()
        {
            Formula expected = null;
            Formula actual = null;
            Func<Task> asyncFunction = async () =>
            {
                expected = await CreateFormula(Client, FormulasCreated);
                actual = await Client.ReadFormula(expected.Id);
                actual.Code = await Client.ReadFormulaCode(expected.Id);
            };
            asyncFunction.ShouldNotThrow("Method should not throw");
            actual.Should().NotBeNull("Actual value should not be NULL");
            actual.Id.Should().Be(expected.Id, "Unexpected id");
            actual.Name.Should().Be(expected.Name, "Unexpected name");
            actual.Code.Should().Be(expected.Code, "Unexpected code");
            actual.Library.Should().Be(expected.Library, "Unexpected library");
        }

        [TestMethod()]
        public void UpdateFormula_Positive()
        {
            NewFormula expected = null;
            Formula actual = null;
            Func<Task> asyncFunction = async () =>
            {
                var formula = await CreateFormula(Client, FormulasCreated);
                expected = new NewFormula
                {
                    Name = formula.Name + "Updated",
                    Code = formula.Code + " * 1",
                };
                actual = await Client.UpdateFormula(formula.Id, expected);
                actual.Code = await Client.ReadFormulaCode(formula.Id);
            };
            asyncFunction.ShouldNotThrow("Method should not throw");
            actual.Should().NotBeNull("Actual value should not be NULL");
            actual.Name.Should().Be(expected.Name, "Unexpected name");
            actual.Code.Should().Be(expected.Code, "Unexpected code");
            actual.Library.Should().Be(expected.Library, "Unexpected library");
        }

        [TestMethod()]
        public void DeleteFormula_Positive()
        {
            Formula actual = null;
            Func<Task> asyncFunction = async () =>
            {
                actual = await CreateFormula(Client, FormulasCreated);
                await Client.DeleteFormula(actual.Id);
            };
            asyncFunction.ShouldNotThrow("Method should not throw");
        }
    }
}
