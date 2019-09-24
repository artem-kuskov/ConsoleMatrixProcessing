using ConsoleMatrixProcessing.Core;
using ConsoleMatrixProcessing.Core.Models;
using System.Collections.Generic;
using Xunit;

namespace ConsoleMatrixProcessingTests.Core
{
    public class ProcessorCommandTests
    {
        [Fact]
        public void ProcessorCommand_EqualsForSameCommand()
        {
            //Arrange
            var addCommand = ProcessorCommandFabric.GetProcessor(ProcessorCommandFabric.Operator.Add);
            addCommand.Id = "1";
            var subCommand = ProcessorCommandFabric.GetProcessor(ProcessorCommandFabric.Operator.Subtract);
            subCommand.Id = "1";
            var tranCommand = ProcessorCommandFabric.GetProcessor(ProcessorCommandFabric.Operator.Transpose);
            tranCommand.Id = "1";
            var multCommand = ProcessorCommandFabric.GetProcessor(ProcessorCommandFabric.Operator.Multiply);
            multCommand.Id = "1";
            var badCommand = ProcessorCommandFabric.GetProcessor(ProcessorCommandFabric.Operator.Bad);
            badCommand.Id = "1";

            var addCommand3 = ProcessorCommandFabric.GetProcessor(ProcessorCommandFabric.Operator.Add);
            addCommand3.Id = "1";
            var subCommand3 = ProcessorCommandFabric.GetProcessor(ProcessorCommandFabric.Operator.Subtract);
            subCommand3.Id = "1";
            var tranCommand3 = ProcessorCommandFabric.GetProcessor(ProcessorCommandFabric.Operator.Transpose);
            tranCommand3.Id = "1";
            var multCommand3 = ProcessorCommandFabric.GetProcessor(ProcessorCommandFabric.Operator.Multiply);
            multCommand3.Id = "1";
            var badCommand3 = ProcessorCommandFabric.GetProcessor(ProcessorCommandFabric.Operator.Bad);
            badCommand3.Id = "1";

            //Act

            //Assert
            Assert.Equal(addCommand, addCommand3);
            Assert.Equal(subCommand, subCommand3);
            Assert.Equal(tranCommand, tranCommand3);
            Assert.Equal(multCommand, multCommand3);
            Assert.Equal(badCommand, badCommand3);
        }

        [Fact]
        public void ProcessorCommand_NotEqualsForDifferentCommand()
        {
            //Arrange
            var addCommand = ProcessorCommandFabric.GetProcessor(ProcessorCommandFabric.Operator.Add);
            addCommand.Id = "1";
            var subCommand = ProcessorCommandFabric.GetProcessor(ProcessorCommandFabric.Operator.Subtract);
            subCommand.Id = "1";
            var tranCommand = ProcessorCommandFabric.GetProcessor(ProcessorCommandFabric.Operator.Transpose);
            tranCommand.Id = "1";
            var multCommand = ProcessorCommandFabric.GetProcessor(ProcessorCommandFabric.Operator.Multiply);
            multCommand.Id = "1";
            var badCommand = ProcessorCommandFabric.GetProcessor(ProcessorCommandFabric.Operator.Bad);
            badCommand.Id = "1";

            var addCommand2 = ProcessorCommandFabric.GetProcessor(ProcessorCommandFabric.Operator.Add);
            addCommand2.Id = "2";
            var subCommand2 = ProcessorCommandFabric.GetProcessor(ProcessorCommandFabric.Operator.Subtract);
            subCommand2.Id = "2";
            var tranCommand2 = ProcessorCommandFabric.GetProcessor(ProcessorCommandFabric.Operator.Transpose);
            tranCommand2.Id = "2";
            var multCommand2 = ProcessorCommandFabric.GetProcessor(ProcessorCommandFabric.Operator.Multiply);
            multCommand2.Id = "2";
            var badCommand2 = ProcessorCommandFabric.GetProcessor(ProcessorCommandFabric.Operator.Bad);
            badCommand2.Id = "2";

            var lstMatrix = new List<Matrix<int>>
            {
                new Matrix<int>
                {
                    Data = new int[,] { {1, 2, 3, 4, 5}, }
                }
            };
            var addCommand4 = ProcessorCommandFabric.GetProcessor(ProcessorCommandFabric.Operator.Add, lstMatrix);
            addCommand4.Id = "1";
            var subCommand4 = ProcessorCommandFabric.GetProcessor(ProcessorCommandFabric.Operator.Subtract, lstMatrix);
            subCommand4.Id = "1";
            var tranCommand4 = ProcessorCommandFabric.GetProcessor(ProcessorCommandFabric.Operator.Transpose, lstMatrix);
            tranCommand4.Id = "1";
            var multCommand4 = ProcessorCommandFabric.GetProcessor(ProcessorCommandFabric.Operator.Multiply, lstMatrix);
            multCommand4.Id = "1";
            var badCommand4 = ProcessorCommandFabric.GetProcessor(ProcessorCommandFabric.Operator.Bad, lstMatrix);
            badCommand4.Id = "1";

            var lstMatrix3 = new List<Matrix<int>>
            {
                new Matrix<int>
                {
                    Data = new int[,] { {100, 2, 3, 4, 5}, }
                }
            };
            var addCommand3 = ProcessorCommandFabric.GetProcessor(ProcessorCommandFabric.Operator.Add, lstMatrix3);
            addCommand3.Id = "1";
            var subCommand3 = ProcessorCommandFabric.GetProcessor(ProcessorCommandFabric.Operator.Subtract, lstMatrix3);
            subCommand3.Id = "1";
            var tranCommand3 = ProcessorCommandFabric.GetProcessor(ProcessorCommandFabric.Operator.Transpose, lstMatrix3);
            tranCommand3.Id = "1";
            var multCommand3 = ProcessorCommandFabric.GetProcessor(ProcessorCommandFabric.Operator.Multiply, lstMatrix3);
            multCommand3.Id = "1";
            var badCommand3 = ProcessorCommandFabric.GetProcessor(ProcessorCommandFabric.Operator.Bad, lstMatrix3);
            badCommand3.Id = "1";

            //Act

            //Assert
            Assert.NotEqual(addCommand, subCommand);
            Assert.NotEqual(addCommand, tranCommand);
            Assert.NotEqual(addCommand, multCommand);
            Assert.NotEqual(addCommand, badCommand);

            Assert.NotEqual(addCommand, addCommand2);
            Assert.NotEqual(subCommand, subCommand2);
            Assert.NotEqual(tranCommand, tranCommand2);
            Assert.NotEqual(multCommand, multCommand2);
            Assert.NotEqual(badCommand, badCommand2);

            Assert.NotEqual(addCommand, addCommand4);
            Assert.NotEqual(subCommand, subCommand4);
            Assert.NotEqual(tranCommand, tranCommand4);
            Assert.NotEqual(multCommand, multCommand4);
            Assert.NotEqual(badCommand, badCommand4);

            Assert.NotEqual(addCommand3, addCommand4);
            Assert.NotEqual(subCommand3, subCommand4);
            Assert.NotEqual(tranCommand3, tranCommand4);
            Assert.NotEqual(multCommand3, multCommand4);
            Assert.NotEqual(badCommand3, badCommand4);

            Assert.NotEqual(addCommand.GetHashCode(), subCommand.GetHashCode());
            Assert.NotEqual(addCommand.GetHashCode(), tranCommand.GetHashCode());
            Assert.NotEqual(addCommand.GetHashCode(), multCommand.GetHashCode());
            Assert.NotEqual(addCommand.GetHashCode(), badCommand.GetHashCode());
        }

        [Fact]
        public void ProcessorCommand_NotEqualsHashCodesForDifferentCommand()
        {
            //Arrange
            var addCommand = ProcessorCommandFabric.GetProcessor(ProcessorCommandFabric.Operator.Add);
            addCommand.Id = "1";
            var subCommand = ProcessorCommandFabric.GetProcessor(ProcessorCommandFabric.Operator.Subtract);
            subCommand.Id = "1";
            var tranCommand = ProcessorCommandFabric.GetProcessor(ProcessorCommandFabric.Operator.Transpose);
            tranCommand.Id = "1";
            var multCommand = ProcessorCommandFabric.GetProcessor(ProcessorCommandFabric.Operator.Multiply);
            multCommand.Id = "1";
            var badCommand = ProcessorCommandFabric.GetProcessor(ProcessorCommandFabric.Operator.Bad);
            badCommand.Id = "1";

            //Act

            //Assert
            Assert.NotEqual(addCommand.GetHashCode(), subCommand.GetHashCode());
            Assert.NotEqual(addCommand.GetHashCode(), tranCommand.GetHashCode());
            Assert.NotEqual(addCommand.GetHashCode(), multCommand.GetHashCode());
            Assert.NotEqual(addCommand.GetHashCode(), badCommand.GetHashCode());
        }

        [Fact]
        public void ProcessorCommand_EqualsHashCodesForSameCommand()
        {
            //Arrange
            var addCommand = ProcessorCommandFabric.GetProcessor(ProcessorCommandFabric.Operator.Add);
            addCommand.Id = "1";
            var subCommand = ProcessorCommandFabric.GetProcessor(ProcessorCommandFabric.Operator.Subtract);
            subCommand.Id = "1";
            var tranCommand = ProcessorCommandFabric.GetProcessor(ProcessorCommandFabric.Operator.Transpose);
            tranCommand.Id = "1";
            var multCommand = ProcessorCommandFabric.GetProcessor(ProcessorCommandFabric.Operator.Multiply);
            multCommand.Id = "1";
            var badCommand = ProcessorCommandFabric.GetProcessor(ProcessorCommandFabric.Operator.Bad);
            badCommand.Id = "1";

            var addCommand2 = ProcessorCommandFabric.GetProcessor(ProcessorCommandFabric.Operator.Add);
            addCommand2.Id = "1";
            var subCommand2 = ProcessorCommandFabric.GetProcessor(ProcessorCommandFabric.Operator.Subtract);
            subCommand2.Id = "1";
            var tranCommand2 = ProcessorCommandFabric.GetProcessor(ProcessorCommandFabric.Operator.Transpose);
            tranCommand2.Id = "1";
            var multCommand2 = ProcessorCommandFabric.GetProcessor(ProcessorCommandFabric.Operator.Multiply);
            multCommand2.Id = "1";
            var badCommand2 = ProcessorCommandFabric.GetProcessor(ProcessorCommandFabric.Operator.Bad);
            badCommand2.Id = "1";
            //Act

            //Assert
            Assert.Equal(addCommand.GetHashCode(), addCommand2.GetHashCode());
            Assert.Equal(subCommand.GetHashCode(), subCommand2.GetHashCode());
            Assert.Equal(tranCommand.GetHashCode(), tranCommand2.GetHashCode());
            Assert.Equal(multCommand.GetHashCode(), multCommand2.GetHashCode());
            Assert.Equal(badCommand2.GetHashCode(), badCommand.GetHashCode());
        }
    }
}