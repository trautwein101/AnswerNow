using AnswerNow.Business.IServices;
using AnswerNow.Data.IRepositories;
using AnswerNow.Domain.Enums;
using AnswerNow.Domain.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;


namespace AnswerNow.Tests.Infrastructure
{
    public sealed class AnswerNowWebAppFactory : WebApplicationFactory<global::Program>
    {
        private readonly Func<CurrentUser> _currentUserFactory;
        private readonly IQuestionRepository _questionRepository;

        public AnswerNowWebAppFactory(Func<CurrentUser> currentUserFactory, IQuestionRepository questionRepository)
        {
            _currentUserFactory = currentUserFactory;
            _questionRepository = questionRepository;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                //Replaces the runtime service with controlled test doubles
                services.RemoveAll<ICurrentUserService>();
                services.RemoveAll<IQuestionRepository>();

                services.AddSingleton<ICurrentUserService>(new TestCurrentUserService(_currentUserFactory));
                services.AddSingleton<IQuestionRepository>(_questionRepository);


            });
        }

        private sealed class TestCurrentUserService : ICurrentUserService
        {
            private readonly Func<CurrentUser> _factory;

            public TestCurrentUserService(Func<CurrentUser> factory)
            {
                _factory = factory;
            }

            public CurrentUser Get() => _factory();
        }

    }
}
