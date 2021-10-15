using System;
using Application.Code;
using Autofac;
using Domain.Exceptions;
using NSubstitute;
using Test.Common.Builders;
using Test.Common.Builders.Commands;
using Test.Infrastructure.Common;
using Xunit;
using Tasks = System.Threading.Tasks;
using FluentAssertions;

namespace Application.Test.UserTest.Commands
{
    [Trait("Type", "Integration")]
    [Trait("Category", "Database")]
    [Collection("DropCreateDatabase Collection")]
    public class UserWriteServiceTest : IDisposable
    {
        private readonly Sandbox _sandbox;
        private readonly IContextProvider _contextProvider;
        public UserWriteServiceTest()
        {
            var configBuilder = new ConfigBuilder();

            _contextProvider = Substitute.For<IContextProvider>();

            _sandbox = new Sandbox(configBuilder.BuildModule(), new Application.Ioc.Module(), new MockedDotnetCoreModuleTest(),
                new MockModule(_contextProvider));
        }

        [Fact]
        public async Tasks.Task create_user_should_create_a_new_user()
        {
            //ARRANGE
            var createUser = new CreateUserBuilder().WithDefaults().Build();

            //ACT
            await _sandbox.Mediator.Send(createUser);
        }

        [Fact]
        public async Tasks.Task create_user_should_update_a_new_user()
        {
            //ARRANGE
            var email = "fabio@test.it";
            var updateUser = new CreateUserBuilder().WithDefaults().WithEmail(email).Build();

            _sandbox.Scenario.WithUser(Guid.NewGuid(), "Fabio", "Test", email, "123456", Domain.LoginProvider.Google, "123456", DateTime.Now.AddDays(5));

            //ACT
            await _sandbox.Mediator.Send(updateUser);
        }

        [Fact]
        public void create_user_should_update_with_another_login_provider()
        {
            //ARRANGE
            var email = "fabio@test.it";
            var updateUser = new CreateUserBuilder().WithDefaults().WithEmail(email).WithLoginWith(Domain.LoginProvider.Jwt).Build();

            _sandbox.Scenario.WithUser(Guid.NewGuid(), "Fabio", "Test", email, "123456", Domain.LoginProvider.Google, "123456", DateTime.Now.AddDays(5));

            //ACT 
            Func<Tasks.Task> fn = async () => { await _sandbox.Mediator.Send(updateUser); };

            //ASSERT
            fn.Should().Throw<LoginWithWrongProviderException>();
        }

        [Fact]
        public void create_user_with_empty_internal_token_should_exception()
        {
            //ARRANGE
            var createUser = new CreateUserBuilder().WithDefaults().WithInternalToken("").Build();

            //ACT 
            Func<Tasks.Task> fn = async () => { await _sandbox.Mediator.Send(createUser); };

            //ASSERT
            fn.Should().Throw<EmptyFieldException>();
        }

        [Fact]
        public void create_user_with_invalid_date_should_exception()
        {
            //ARRANGE
            var createUser = new CreateUserBuilder().WithDefaults().WithExpiredDate(DateTime.Now.AddDays(-1)).Build();

            //ACT 
            Func<Tasks.Task> fn = async () => { await _sandbox.Mediator.Send(createUser); };

            //ASSERT
            fn.Should().Throw<InvalidDateException>();
        }

        [Fact]
        public void create_user_with_name_is_void_should_exception()
        {
            //ARRANGE
            var createUser = new CreateUserBuilder()
                .WithName("")
                .WithSurname("Surname")
                .WithEmail("default@test.it")
                .WithExternalToken("externalToken")
                .WithLoginWith(Domain.LoginProvider.Google)
                .Build();

            //ACT 
            Func<Tasks.Task> fn = async () => { await _sandbox.Mediator.Send(createUser); };

            //ASSERT
            fn.Should().Throw<EmptyFieldException>();
        }

        [Fact]
        public void create_post_with_surname_is_void_should_exception()
        {
            //ARRANGE
            var createUser = new CreateUserBuilder()
                .WithName("abcde")
                .WithSurname("")
                .WithEmail("default@test.it")
                .WithExternalToken("externalToken")
                .WithLoginWith(Domain.LoginProvider.Google)
                .Build();

            //ACT 
            Func<Tasks.Task> fn = async () => { await _sandbox.Mediator.Send(createUser); };

            //ASSERT
            fn.Should().Throw<EmptyFieldException>();
        }

        [Fact]
        public void create_post_with_email_is_void_should_exception()
        {
            //ARRANGE
            var createUser = new CreateUserBuilder()
               .WithName("name")
               .WithSurname("Surname")
               .WithEmail("")
               .WithExternalToken("externalToken")
               .WithLoginWith(Domain.LoginProvider.Google)
               .Build();

            //ACT 
            Func<Tasks.Task> fn = async () => { await _sandbox.Mediator.Send(createUser); };

            //ASSERT
            fn.Should().Throw<EmptyFieldException>();
        }

        [Fact]
        public void create_post_with_external_token_is_void_should_exception()
        {
            //ARRANGE
            var createUser = new CreateUserBuilder()
               .WithName("name")
               .WithSurname("Surname")
               .WithEmail("email@test.it")
               .WithExternalToken("")
               .WithLoginWith(Domain.LoginProvider.Google)
               .Build();

            //ACT 
            Func<Tasks.Task> fn = async () => { await _sandbox.Mediator.Send(createUser); };

            //ASSERT
            fn.Should().Throw<EmptyFieldException>();
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
