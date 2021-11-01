using Application.Code;
using Application.User.Queries;
using Autofac;
using Domain.Exceptions;
using FluentAssertions;
using Infrastructure.Write;
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

            BsonClassMapHelper.Clear();
            MongoDBInstallmentMap.Map();
        }

        [Fact]
        public async Task get_user_with_token_return_correct_user()
        {
            //ARRANGE
            var name = "John";
            var surname = "Wick";
            var email = "john@wick.it";
            var internalToken = Guid.NewGuid().ToString();
            _sandbox.Scenario.WithGoogleUser(Guid.NewGuid(), name, surname, email, "pippo:12", internalToken, DateTime.Now.AddDays(5));

            //ACT
            var user = await _sandbox.Mediator.Send(new GetUser(internalToken));

            //ASSERT
            user.Name.Should().Be(name);
            user.Surname.Should().Be(surname);
            user.Email.Should().Be(email);
            user.InternalToken.Should().Be(internalToken);
        }

        [Fact]
        public void get_user_should_token_not_exist()
        {
            //ARRANGE
            _sandbox.Scenario.WithGoogleUser(Guid.NewGuid(), "Fabio", "Test", "email@notexist.net", "123456", "123456", DateTime.Now.AddDays(5));

            //ACT 
            Func<Task> fn = async () => { await _sandbox.Mediator.Send(new GetUser("Fake")); };

            //ASSERT
            fn.Should().Throw<NotFoundItemException>();
        }

        [Fact]
        public void get_user_should_token_is_expired()
        {
            //ARRANGE
            var internalToken = Guid.NewGuid().ToString();
            _sandbox.Scenario.WithGoogleUser(Guid.NewGuid(), "Fabio", "Test", "email@expiredtoken.net", "123456", internalToken, DateTime.UtcNow.AddSeconds(1));

            System.Threading.Thread.Sleep(1000);

            //ACT 
            Func<Task> fn = async () => { await _sandbox.Mediator.Send(new GetUser(internalToken)); };

            //ASSERT
            fn.Should().Throw<NotFoundItemException>();
        }

        [Fact]
        public async Task get_user_after_update_with_token_return_correct_user()
        {
            //ARRANGE
            var name = "John";
            var surname = "Wick";
            var email = "john@wick.it";
            var internalToken = Guid.NewGuid().ToString();
            _sandbox.Scenario
                     .WithGoogleUser(Guid.NewGuid(), name, surname, email, "pippo:12", Guid.NewGuid().ToString(), DateTime.Now.AddDays(5))
                     .WithGoogleUser(Guid.NewGuid(), name, surname, email, "pippo:12", internalToken, DateTime.Now.AddDays(5));

            //ACT
            var user = await _sandbox.Mediator.Send(new GetUser(internalToken));

            //ASSERT
            user.Name.Should().Be(name);
            user.Surname.Should().Be(surname);
            user.Email.Should().Be(email);
            user.InternalToken.Should().Be(internalToken);
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
