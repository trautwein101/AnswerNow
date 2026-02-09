using System.Net;
using System.Net.Http.Json;
using AnswerNow.Data.Entities;
using AnswerNow.Data.IRepositories;
using AnswerNow.Data.Repositories;
using AnswerNow.Domain.Enums;
using AnswerNow.Domain.Models;
using AnswerNow.Tests.Infrastructure;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace AnswerNow.Tests.Integration
{
    public class QuestionProblemDetailsTests
    {
        [Fact]
        public async Task GetById_WhenMissing_Returns404ProblemDetails()
        {
            //ARRANGE 
            var repoMock = new Mock<IQuestionRepository>();

            repoMock
                .Setup(r => r.GetByIdWithUserAsync(It.IsAny<int>()))
                .ReturnsAsync((QuestionEntity?)null);

            var factory = new AnswerNowWebAppFactory(
                currentUserFactory: () => new CurrentUser { Role = UserRole.User },
                questionRepository: repoMock.Object);

            var client = factory.CreateClient();

            //ACT
            var response = await client.GetAsync("/api/question/111");
            var body = await response.Content.ReadFromJsonAsync<ProblemDetails>(
                cancellationToken: TestContext.Current.CancellationToken);

            //ASSERT
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            response.Content.Headers.ContentType!.MediaType.Should().Be("application/problem+json");

            body.Should().NotBeNull();
            body!.Status.Should().Be(404);
            body.Instance.Should().Be("/api/question/111");

            //Match middleware key
            body.Extensions.Should().ContainKey("traceId");

        }


        [Fact]
        public async Task Flagged_UnflagAsNormalUser_Returns403ProblemDetails()
        {

            //ARRANGE : question exists and is currently flagged
            var question = new Question { Id = 1, IsFlagged = true };

            var repoMock = new Mock<IQuestionRepository>();

            repoMock
                .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(question);

            var factory = new AnswerNowWebAppFactory(
                currentUserFactory: () => new CurrentUser { Role = UserRole.User },
                questionRepository: repoMock.Object);

            var client = factory.CreateClient();

            //ACT
            var response = await client.PostAsync("/api/question/1/flagged?isFlagged=false", content: null);
            var body = await response.Content.ReadFromJsonAsync<ProblemDetails>(
                cancellationToken: TestContext.Current.CancellationToken);

            //ASSERT
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
            response.Content.Headers.ContentType!.MediaType.Should().Be("application/problem+json");

            body.Should().NotBeNull();
            body!.Status.Should().Be(403);
            body.Instance.Should().Be("/api/question/1/flagged");

            body.Extensions.Should().ContainKey("traceId");


        }



    }
}
