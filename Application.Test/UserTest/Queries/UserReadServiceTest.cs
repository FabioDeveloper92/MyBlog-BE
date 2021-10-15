using Application.Code;
using Application.User.Queries;
using Autofac;
using Domain.Exceptions;
using FluentAssertions;
using NSubstitute;
using System;
using System.Threading.Tasks;
using Test.Common.Builders;
using Test.Infrastructure.Common;
using Xunit;

namespace Application.Test.UserTest.Queries
{
    [Trait("Type", "Integration")]
    [Trait("Category", "Database")]
    [Collection("DropCreateDatabase Collection")]
    public class UserReadServiceTest : IDisposable
    {
        private readonly Sandbox _sandbox;
        private readonly IContextProvider _contextProvider;

        public UserReadServiceTest()
        {
            var configBuilder = new ConfigBuilder();

            _contextProvider = Substitute.For<IContextProvider>();

            _sandbox = new Sandbox(configBuilder.BuildModule(), new Application.Ioc.Module(), new MockedDotnetCoreModuleTest(), new MockModule(_contextProvider));
        }

        [Fact]
        public async Task get_user_with_token_return_correct_user()
        {
            //ARRANGE
            var name = "John";
            var surname = "Wick";
            var email = "john@wick.it";
            var internalToken = "123456";
            _sandbox.Scenario.WithUser(Guid.NewGuid(), name, surname, email, "pippo:12", Domain.LoginProvider.Google, internalToken, DateTime.Now.AddDays(5));

            //ACT
            var user = await _sandbox.Mediator.Send(new GetUser(internalToken));

            //ASSERT
            user.Name.Should().Be(name);
            user.Surname.Should().Be(surname);
            user.Email.Should().Be(email);
            user.InternalToken.Should().Be(internalToken);
        }

        [Fact]
        public void get_user_should_not_valid()
        {
            //ARRANGE
            _sandbox.Scenario.WithUser(Guid.NewGuid(), "Fabio", "Test", "email@test.net", "123456", Domain.LoginProvider.Google, "123456", DateTime.Now.AddDays(5));

            //ACT 
            Func<Task> fn = async () => { await _sandbox.Mediator.Send(new GetUser("Fake")); };

            //ASSERT
            fn.Should().Throw<NotFoundItemException>();
        }

        public void Dispose()
        {
            _sandbox?.Dispose();
        }

        private class MockModule : Autofac.Module
        {
            private readonly IContextProvider _contextProvider;

            public MockModule(IContextProvider contextProvider)
            {
                _contextProvider = contextProvider;
            }

            protected override void Load(ContainerBuilder builder)
            {
                builder.Register(ctx => _contextProvider).As<IContextProvider>().SingleInstance();
            }
        }
    }
}
