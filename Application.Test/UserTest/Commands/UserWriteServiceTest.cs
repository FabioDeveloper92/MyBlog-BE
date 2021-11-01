using Application.Code;
using Application.User.Commands;
using Autofac;
using Domain.Exceptions;
using FluentAssertions;
using Infrastructure.Write;
using NSubstitute;
using System;
using System.Threading.Tasks;
using Test.Common.Builders;
using Test.Common.Builders.Commands;
using Test.Infrastructure.Common;
using Xunit;

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

            BsonClassMapHelper.Clear();
            MongoDBInstallmentMap.Map();
        }

        [Fact]
        public async Task create_user_should_create_a_new_user()
        {
            //ARRANGE
            var createUser = new CreateGoogleUserBuilder().WithDefaults().WithEmail("test1@mail.it").Build();

            //ACT
            await _sandbox.Mediator.Send(createUser);
        }

        [Fact]
        public async Task create_user_should_update_a_new_user()
        {
            //ARRANGE
            var email = "test2@mail.it";
            var updateUser = new CreateGoogleUserBuilder().WithDefaults().WithEmail(email).Build();

            _sandbox.Scenario.WithGoogleUser(Guid.NewGuid(), "Fabio", "Test", email, "123456", "123456", DateTime.Now.AddDays(5));

            //ACT
            await _sandbox.Mediator.Send(updateUser);
        }

        [Fact]
        public void create_google_user_should_update_with_another_login_provider()
        {
            //ARRANGE
            var email = "test3@mail.it";
            var updateUser = new CreateJwtUserBuilder().WithDefaults().WithEmail(email).Build();

            _sandbox.Scenario.WithGoogleUser(Guid.NewGuid(), "Fabio", "Test", email, "123456", "123456", DateTime.Now.AddDays(5));

            //ACT 
            Func<Task> fn = async () => { await _sandbox.Mediator.Send(updateUser); };

            //ASSERT
            fn.Should().Throw<UserAlreadyExistException>();
        }

        [Fact]
        public void create_jwt_user_should_update_with_another_login_provider()
        {
            //ARRANGE
            var email = "test4@mail.it";
            var updateUser = new CreateGoogleUserBuilder().WithDefaults().WithEmail(email).Build();

            _sandbox.Scenario.WithJWTUser(Guid.NewGuid(), "Fabio", "Test", email, "Test200*", "123456", DateTime.Now.AddDays(5));

            //ACT 
            Func<Task> fn = async () => { await _sandbox.Mediator.Send(updateUser); };

            //ASSERT
            fn.Should().Throw<LoginWithWrongProviderException>();
        }

        [Fact]
        public void create_user_with_empty_internal_token_should_exception()
        {
            //ARRANGE
            var createUser = new CreateGoogleUserBuilder().WithDefaults().WithEmail("test5@mail.it").WithInternalToken("").Build();

            //ACT 
            Func<Task> fn = async () => { await _sandbox.Mediator.Send(createUser); };

            //ASSERT
            fn.Should().Throw<EmptyFieldException>();
        }

        [Fact]
        public void create_user_with_invalid_date_should_exception()
        {
            //ARRANGE
            var createUser = new CreateGoogleUserBuilder().WithDefaults().WithEmail("test6@mail.it").WithExpiredDate(DateTime.Now.AddDays(-1)).Build();

            //ACT 
            Func<Task> fn = async () => { await _sandbox.Mediator.Send(createUser); };

            //ASSERT
            fn.Should().Throw<InvalidDateException>();
        }

        [Fact]
        public void create_user_with_name_is_void_should_exception()
        {
            //ARRANGE
            var createUser = new CreateGoogleUserBuilder()
                .WithName("")
                .WithSurname("Surname")
                .WithEmail("test7@mail.it")
                .WithExternalToken("externalToken")
                .Build();

            //ACT 
            Func<Task> fn = async () => { await _sandbox.Mediator.Send(createUser); };

            //ASSERT
            fn.Should().Throw<EmptyFieldException>();
        }

        [Fact]
        public void create_user_with_surname_is_void_should_exception()
        {
            //ARRANGE
            var createUser = new CreateGoogleUserBuilder()
                .WithName("abcde")
                .WithSurname("")
                .WithEmail("test8@mail.it")
                .WithExternalToken("externalToken")
                .Build();

            //ACT 
            Func<Task> fn = async () => { await _sandbox.Mediator.Send(createUser); };

            //ASSERT
            fn.Should().Throw<EmptyFieldException>();
        }

        [Fact]
        public void create_user_with_email_is_void_should_exception()
        {
            //ARRANGE
            var createUser = new CreateGoogleUserBuilder()
                .WithName("abcde")
                .WithSurname("aaaa")
                .WithEmail("")
                .WithExternalToken("externalToken")
                .Build();

            //ACT 
            Func<Task> fn = async () => { await _sandbox.Mediator.Send(createUser); };

            //ASSERT
            fn.Should().Throw<EmptyFieldException>();
        }

        [Fact]
        public void create_user_with_external_token_is_void_should_exception()
        {
            //ARRANGE
            var createUser = new CreateGoogleUserBuilder()
               .WithName("name")
               .WithSurname("Surname")
               .WithEmail("test9@mail.it")
               .WithExternalToken("")
               .Build();

            //ACT 
            Func<Task> fn = async () => { await _sandbox.Mediator.Send(createUser); };

            //ASSERT
            fn.Should().Throw<EmptyFieldException>();
        }

        [Fact]
        public async void logout_user_with_internaltoken()
        {
            //ARRANGE
            var internalToken =  Guid.NewGuid().ToString();
            _sandbox.Scenario.WithJWTUser(Guid.NewGuid(), "Fabio", "Test", "test11@mail.it", "Test200*", internalToken, DateTime.Now.AddDays(5));

            //ACT 
            await _sandbox.Mediator.Send(new LogoutUser(internalToken));
        }

        [Fact]
        public void logout_user_without_internaltoken()
        {
            //ARRANGE
            _sandbox.Scenario.WithJWTUser(Guid.NewGuid(), "Fabio", "Test", "test11_B@mail.it", "Test200*", Guid.NewGuid().ToString(), DateTime.Now.AddDays(5));

            //ACT 
            Func<Task> fn = async () => { await _sandbox.Mediator.Send(new LogoutUser(Guid.NewGuid().ToString())); };

            //ASSERT
            fn.Should().Throw<UserNotExistException>();
        }

        [Fact]
        public async Task create_user_with_JWT()
        {
            //ARRANGE
            var createUser = new CreateJwtUserBuilder().WithDefaults().WithEmail("test13@mail.it").Build();

            //ACT
            await _sandbox.Mediator.Send(createUser);
        }

        [Fact]
        public void create_user_with_JWT_with_same_email_should_exception()
        {
            //ARRANGE
            var email = "test14@mail.it";
            var createUser = new CreateJwtUserBuilder().WithDefaults().WithEmail(email).Build();

            _sandbox.Scenario.WithJWTUser(Guid.NewGuid(), "Fabio", "Test", email, "Test200*", "123456", DateTime.Now.AddDays(5));

            //ACT 
            Func<Task> fn = async () => { await _sandbox.Mediator.Send(createUser); };

            //ASSERT
            fn.Should().Throw<UserAlreadyExistException>();
        }

        [Fact]
        public void create_user_with_JWT_with_password_invalid_should_exception()
        {
            //ARRANGE
            var createUser = new CreateJwtUserBuilder().WithDefaults().WithEmail("test15@mail.it").WithPassword("notgood").Build();

            //ACT 
            Func<Task> fn = async () => { await _sandbox.Mediator.Send(createUser); };

            //ASSERT
            fn.Should().Throw<PasswordNotValidException>();
        }

        [Fact]
        public async void login_with_user()
        {
            //ARRANGE
            var mail = "test16@mail.it";
            var pwd = "Test200*";
            _sandbox.Scenario.WithJWTUser(Guid.NewGuid(), "Fabio", "Test", mail, pwd, "1234556", DateTime.Now.AddDays(5));

            var updateUserTokenJwt = new UpdateUserTokenJwt(mail, pwd, Guid.NewGuid().ToString(), DateTime.Now.AddDays(5));

            //ACT 
            await _sandbox.Mediator.Send(updateUserTokenJwt);
        }

        [Fact]
        public void login_with_user_not_exist()
        {
            //ARRANGE
            _sandbox.Scenario.WithJWTUser(Guid.NewGuid(), "Fabio", "Test", "test17@mail.it", "Test200*", "1234556", DateTime.Now.AddDays(5));

            var updateUserTokenJwt = new UpdateUserTokenJwt("email@notexist.it", "Test200*", Guid.NewGuid().ToString(), DateTime.Now.AddDays(5));

            //ACT 
            Func<Task> fn = async () => { await _sandbox.Mediator.Send(updateUserTokenJwt); };

            //ASSERT
            fn.Should().Throw<UserNotExistException>();
        }

        [Fact]
        public void login_with_user_exist_wrong_password()
        {
            //ARRANGE
            var mail = "test18@mail.it";
            var pwd = "Test200*";
            _sandbox.Scenario.WithJWTUser(Guid.NewGuid(), "Fabio", "Test", mail, pwd, "1234556", DateTime.Now.AddDays(5));

            var updateUserTokenJwt = new UpdateUserTokenJwt(mail, pwd + "WRONG", Guid.NewGuid().ToString(), DateTime.Now.AddDays(5));

            //ACT 
            Func<Task> fn = async () => { await _sandbox.Mediator.Send(updateUserTokenJwt); };

            //ASSERT
            fn.Should().Throw<UserNotExistException>();
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
