using Xunit;
using Moq;
using AnswerNow.Business.Services;
using AnswerNow.Data.IRepositories;
using AnswerNow.Business.IServices;
using AnswerNow.Data.Entities;


namespace AnswerNow.Tests.Services
{
    public class QuestionServiceTest
    {
        private readonly Mock<IQuestionRepository> _questionRepositoryMock = new();
        private readonly Mock<ICurrentUserService> _currentUserServiceMock = new();

        private readonly QuestionService _sut; //sut ~ system under test

        public QuestionServiceTest()
        {
            _sut = new QuestionService(
                _questionRepositoryMock.Object,
                _currentUserServiceMock.Object
             );
        }


        [Fact] //NEG Test
        public async Task GetByIdDtoAsync_WhenEntityDoesNotExist_ReturnsNull()
        {
            //If repository returns null, the service will return null

            //ARRANGE
            var questionId = 100;

            //Mok the question dos not exist
            _questionRepositoryMock
                .Setup(r => r.GetByIdWithUserAsync(questionId))
                .ReturnsAsync((QuestionEntity?)null);

            //ACT
            var result = await _sut.GetByIdDtoAsync(questionId);

            //ASSERT ~ since entity is null we return null
            Assert.Null(result);

        }



        [Fact] //NEG Test
        public async Task FlaggedAsync_WhenQuestionDoesNotExist_ReturnsNull()
        {


            //If qustion does not exist as null you cannot update it.

            //ARRANGE
            var questionId = 42;

            //When service calls with ID, return null.
            _questionRepositoryMock
                .Setup(r => r.GetByIdAsync(questionId))
                .ReturnsAsync((Domain.Models.Question?)null);

            //ACT
            var result = await _sut.FlaggedAsync(questionId, isFlagged: true);

            //ASSERT
            Assert.Null(result);

            //note: If nothing was found, no update should be attempted
            _questionRepositoryMock.Verify(
                r => r.UpdateAsync(It.IsAny<Domain.Models.Question>()),
                Times.Never
            );
        }


        [Fact] // POS Test
        public async Task FlaggedAsync_NormalUser_FalseToTrue_FlagsAndUpdates()
        {

            //Normal user will try to flag something that is unflagged, this is allowed

            //ARRANGE ~ question exists but is not flagged
            var question = new Domain.Models.Question
            {
                Id = 1,
                IsFlagged = false
            };

            //When service asks for question, return our in-memory object
            _questionRepositoryMock
                .Setup(r => r.GetByIdAsync(question.Id))
                .ReturnsAsync(question);

            //Simulate logged-in normal user ~ read user and apply role rules
            var normalUser = new Domain.Models.CurrentUser
            {
                Role = Domain.Enums.UserRole.User
            };

            _currentUserServiceMock
                .Setup(s => s.Get())
                .Returns(normalUser);

            //Configure UpdateAsync to return updated entity.
            _questionRepositoryMock
                .Setup(r => r.UpdateAsync(It.IsAny<Domain.Models.Question>()))
                .ReturnsAsync((Domain.Models.Question q) => q);

            //ACT ~ normal user attempt to flag the question
            var result = await _sut.FlaggedAsync(question.Id, isFlagged: true);

            //ASSERT
            Assert.NotNull(result);

            //Business rule ~ normal users are allowed to flag content if (false -> true)
            Assert.True(result!.IsFlagged);

            //Prove the persistance was attempted exactly once with expected state
            _questionRepositoryMock.Verify(
                r => r.UpdateAsync(It.Is<Domain.Models.Question>(q => q.IsFlagged == true)),
                Times.Once
            );
        }


        [Fact] //POS
        public async Task FlaggedAsync_NormalUser_TrueToFalse_DoesNotUnflagAndDoesNotUpdate()
        {
            
            //Normal user will try to unflag something that is already flagged and not allowed
            
            //ARRANGE ~ Question exists but is flagged
            var question = new Domain.Models.Question
            {
                Id = 2,
                IsFlagged = true
            };

            //When the service loads our in-memory object
            _questionRepositoryMock
                .Setup(r => r.GetByIdAsync(question.Id))
                .ReturnsAsync(question);

            //Simulate the logged in user with the role
            var normalUser = new Domain.Models.CurrentUser
            {
                Role = Domain.Enums.UserRole.User
            };

            _currentUserServiceMock
                .Setup(s => s.Get())
                .Returns(normalUser);

            //ACT ~ normal user will try to unflag but not allowed
            var result = await _sut.FlaggedAsync(question.Id, isFlagged: false);

            //ASSERT
            Assert.NotNull(result);

            //Busines rule ~ normal user cannot unflag once content is flagged
            Assert.True(result!.IsFlagged);

            //Safeguard ~ since action not allowed, we must not write to persistance
            _questionRepositoryMock.Verify(
                r => r.UpdateAsync(It.IsAny<Domain.Models.Question>()),
                Times.Never
            );
        }

        [Theory] //THEORY POS
        [InlineData(Domain.Enums.UserRole.Moderator)]
        [InlineData(Domain.Enums.UserRole.Admin)]
        public async Task FlaggedAsync_ElevatedRole_TrueToFalse_UnflagsAndUpdates(Domain.Enums.UserRole role)
        {
            //Business logic ~ admins and moderators should be able to unflag conent true-> false

            //ARRANGE
            var question = new Domain.Models.Question
            {
                Id = 3,
                IsFlagged = true
            };

            _questionRepositoryMock
                .Setup(r => r.GetByIdAsync(question.Id))
                .ReturnsAsync(question);

            //Role paramater will come from [InlineData] for theory
            var elevatedUser = new Domain.Models.CurrentUser
            {
                Role = role
            };

            _currentUserServiceMock
                .Setup(s => s.Get())
                .Returns(elevatedUser);

            _questionRepositoryMock
                .Setup( r => r.UpdateAsync(It.IsAny<Domain.Models.Question>()))
                .ReturnsAsync((Domain.Models.Question q) => q);

            //ACT ~ moderator/admin will unflag the question
            var result = await _sut.FlaggedAsync(question.Id, isFlagged: false);

            //ASSERT
            Assert.NotNull(result);

            Assert.False(result!.IsFlagged);

            //Verify update happened with correct state
            _questionRepositoryMock.Verify(
                r => r.UpdateAsync(It.Is<Domain.Models.Question>(q => q.IsFlagged == false)),
                Times.Once
             );

        }


        [Theory] //THEORY POS
        [InlineData(Domain.Enums.UserRole.Moderator)]
        [InlineData(Domain.Enums.UserRole.Admin)]
        public async Task FlaggedAsync_ElevatedRole_FalseToTrue_FlagsAndUpdates(Domain.Enums.UserRole role)
        {

            //Business logic ~ admins and moderators should be able to flag content false->true

            //ARRANGE
            var question = new Domain.Models.Question
            {
                Id = 4,
                IsFlagged = false
            };

            _questionRepositoryMock
                .Setup(r => r.GetByIdAsync(question.Id))
                .ReturnsAsync(question);

            var elevatedUser = new Domain.Models.CurrentUser
            {
                Role = role
            };

            _currentUserServiceMock
                .Setup(s => s.Get())
                .Returns(elevatedUser);

            _questionRepositoryMock
                .Setup(r => r.UpdateAsync(It.IsAny<Domain.Models.Question>()))
                .ReturnsAsync((Domain.Models.Question q) => q);

            //ACT ~ moderator/admin will flag the question
            var result = await _sut.FlaggedAsync(question.Id, isFlagged: true);

            //ASSERT
            Assert.NotNull(result);

            Assert.True(result.IsFlagged);

            //Verify udpate happened with correct state
            _questionRepositoryMock.Verify(
                r => r.UpdateAsync(It.Is<Domain.Models.Question>(q => q.IsFlagged == true)),
                Times.Once
             );

        }


        [Theory] //THEORY NEG NO UPDATE
        [InlineData(Domain.Enums.UserRole.Moderator)]
        [InlineData(Domain.Enums.UserRole.Admin)]
        public async Task FlaggedAsync_ElevatedRole_AlreadyFlagged_FlagAgain_NoUpdate(Domain.Enums.UserRole role)
        {
            //Business logic ~ As Flagged is already true and request is true there will be no change

            //ARRANGE
            var question = new Domain.Models.Question
            {
                Id = 5,
                IsFlagged = true
            };

            _questionRepositoryMock
                .Setup(r => r.GetByIdAsync(question.Id))
                .ReturnsAsync(question);

            var elevatedUser = new Domain.Models.CurrentUser
            {
                Role = role
            };

            _currentUserServiceMock
                .Setup(s => s.Get())
                .Returns(elevatedUser);

            //strict testing approach so no UpdateAsync

            //ACT ~ try to flag something already flagged
            var result = await _sut.FlaggedAsync(question.Id, isFlagged: true);

            //ASSERT
            Assert.NotNull(result);

            Assert.True(result.IsFlagged);

            //Update should never be called for optimization
            _questionRepositoryMock.Verify(
                r => r.UpdateAsync(It.IsAny<Domain.Models.Question>()),
                Times.Never
             );
               
        }


        [Theory] //THEORY NEG NO CHANGE
        [InlineData(Domain.Enums.UserRole.Moderator)]
        [InlineData(Domain.Enums.UserRole.Admin)]
        public async Task FlaggedAsync_ElevatedRole_AlreadyUnflagged_UnflagAgain_NoUpdate(Domain.Enums.UserRole role)
        {
            // Elevated role with unflagged attempt on unflagged for no change

            //ARRANGE
            var question = new Domain.Models.Question
            {
                Id = 6,
                IsFlagged = true
            };

            _questionRepositoryMock
                .Setup(r => r.GetByIdAsync(question.Id))
                .ReturnsAsync(question);

            var elevatedUser = new Domain.Models.CurrentUser
            {
                Role = role
            };

            _currentUserServiceMock
                .Setup(s => s.Get())
                .Returns(elevatedUser);

            //ACT 
            var result = await _sut.FlaggedAsync(question.Id, isFlagged: true);

            //ASSERT
            Assert.NotNull(result);

            Assert.True(result!.IsFlagged);

            //Update should never be called for optimization
            _questionRepositoryMock.Verify(
                r => r.UpdateAsync(It.IsAny<Domain.Models.Question>()),
                Times.Never
            );
               
        }


    }
}
