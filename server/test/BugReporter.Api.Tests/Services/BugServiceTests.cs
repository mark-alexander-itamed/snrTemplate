using System;
using System.Linq;
using System.Threading.Tasks;
using BugReporter.Api.Forms;
using BugReporter.Api.Models;
using BugReporter.Api.Services;
using BugReporter.Api.Tests.Fakes;
using BugReporter.Api.Utils;
using BugReporter.Api.ViewModels;
using Microsoft.EntityFrameworkCore.Update.Internal;
using Xunit;

namespace BugReporter.Api.Tests.Services
{
    public class BugServiceTests
    {
        private class Fixture : DbTestFixture
        {
            public BugService GetSut() => new BugService(CleanDbContext);
        }

        private readonly Fixture _fixture = new Fixture();

        [Fact]
        public async Task Query_GetsAllInDatabase()
        {
            _fixture.AddEntities(FakeBug.I.Generate(10));
            var sut = _fixture.GetSut();
            var found = await sut.Query();
            Assert.Equal(10, found.Count());
        }

        [Fact]
        public async Task Query_SortsAllInTimeOrderDescending()
        {
            _fixture.AddEntities(FakeBug.I.Generate(10));
            var sut = _fixture.GetSut();
            var found = (await sut.Query()).ToList();
            var sorted = found.OrderByDescending(x => x.ReportedAt).ToList();
            
            Assert.All(found.Zip(sorted), pair =>
            {
                var (first, second) = pair;
                Assert.Equal(first.ReportedAt, second.ReportedAt);
            });
        }

        [Fact]
        public async Task Find_BugExists_ReturnsSome()
        {
            var bug = FakeBug.I.Generate();
            _fixture.AddEntity(bug);
            var sut = _fixture.GetSut();
            var option = await sut.Find(bug.Id);
            var some = Assert.IsType<Option<BugViewModel>.Some>(option);
            Assert.Equal(bug.Id, some.Value.Id);
        }

        [Fact]
        public async Task Find_BugNotFound_ReturnsNone()
        {
            var sut = _fixture.GetSut();
            var option = await sut.Find(Guid.NewGuid());
            Assert.IsType<Option<BugViewModel>.None>(option);
        }

        [Theory]
        [InlineData(nameof(Bug.Id), nameof(BugViewModel.Id))]
        [InlineData(nameof(Bug.ReportedAt), nameof(BugViewModel.ReportedAt))]
        [InlineData(nameof(Bug.ReportedBy), nameof(BugViewModel.ReportedBy))]
        [InlineData(nameof(Bug.Title), nameof(BugViewModel.Title))]
        [InlineData(nameof(Bug.Description), nameof(BugViewModel.Description))]
        public async Task Find_BugExists_ReturnsCorrectlyMappedViewModel(string entityPropName, string viewModelPropName)
        {
            var bug = FakeBug.I.Generate();
            _fixture.AddEntity(bug);
            var sut = _fixture.GetSut();
            var option = Assert.IsType<Option<BugViewModel>.Some>( await sut.Find(bug.Id) );
            var viewModel = option.Value;

            var entityValue = typeof(Bug).GetProperty(entityPropName)!.GetValue(bug)!;
            var viewModelValue = typeof(BugViewModel).GetProperty(viewModelPropName)!.GetValue(viewModel);
            Assert.Equal(entityValue, viewModelValue);
        }

        [Fact]
        public async Task Find_BugHasComments_ViewModelHasAllCommentsInChronologicalOrder()
        {
            var bug = FakeBug.I.Generate();
            bug.Comments.AddRange(FakeComment.I.Generate(10));
            _fixture.AddEntity(bug);
            var sut = _fixture.GetSut();
            var option = Assert.IsType<Option<BugViewModel>.Some>(await sut.Find(bug.Id));
            var comments = option.Value.Comments;
            var sortedComments = comments.OrderBy(x => x.CommentedAt).ToList();
            
            Assert.Equal(10, comments.Count);
            Assert.All(comments.Zip(sortedComments), pair =>
            {
                var (first, second) = pair;
                Assert.Equal(first.Id, second.Id);
            });
        }

        [Fact]
        public async Task Create_AddsBugToDatabase()
        {
            var sut = _fixture.GetSut();
            var form = new CreateBugForm
            {
                Title = "test title",
                Description = "test description",
                ReportedBy = "test user"
            };

            await sut.Create(form);

            Assert.Single(_fixture.CleanDbContext.Bugs);
        }

        [Theory]
        [InlineData(nameof(Bug.Title), nameof(CreateBugForm.Title))]
        [InlineData(nameof(Bug.Description), nameof(CreateBugForm.Description))]
        [InlineData(nameof(Bug.ReportedBy), nameof(CreateBugForm.ReportedBy))]
        public async Task Create_CopiesPropertiesFromForm(string entityPropName, string formPropName)
        {
            var form = new CreateBugForm
            {
                Title = "test title",
                Description = "description",
                ReportedBy = "test user"
            };

            var sut = _fixture.GetSut();
            await sut.Create(form);

            var createdBug = _fixture.CleanDbContext.Bugs.Single();

            var entityProp = typeof(Bug).GetProperty(entityPropName)!.GetValue(createdBug)!;
            var formProp = typeof(CreateBugForm).GetProperty(formPropName)!.GetValue(form)!;

            Assert.Equal(entityProp, formProp);
        }

        [Fact]
        public async Task Create_BugHasNoComments()
        {
            var form = new CreateBugForm
            {
                Title = "test title",
                Description = "description",
                ReportedBy = "test user"
            };

            var sut = _fixture.GetSut();
            await sut.Create(form);

            var createdBug = _fixture.CleanDbContext.Bugs.Single();
            Assert.Empty(createdBug.Comments);
        }

        [Theory]
        [InlineData(nameof(Bug.Id))]
        [InlineData(nameof(Bug.Title))]
        [InlineData(nameof(Bug.Description))]
        [InlineData(nameof(Bug.ReportedBy))]
        public async Task Create_ReturnsViewModel_MatchingProperties(string entityPropertyName, string viewModelPropertyName = null)
        {
            viewModelPropertyName ??= entityPropertyName;
            
            var sut = _fixture.GetSut();
            var form = new CreateBugForm
            {
                Title = "test title",
                Description = "test description",
                ReportedBy = "test user"
            };

            var viewModel = await sut.Create(form);
            var created = _fixture.CleanDbContext.Bugs.Single();

            var entityProperty = typeof(Bug).GetProperty(entityPropertyName)!.GetValue(created)!;
            var viewModelProperty = typeof(BugViewModel).GetProperty(viewModelPropertyName)!.GetValue(viewModel)!;
            Assert.Equal(entityProperty, viewModelProperty);
        }

        [Fact]
        public async Task Create_ReturnsViewModel_MatchingStateProperty()
        {
            var sut = _fixture.GetSut();
            var form = new CreateBugForm
            {
                Title = "test title",
                Description = "test description",
                ReportedBy = "test user"
            };

            var viewModel = await sut.Create(form);
            var created = _fixture.CleanDbContext.Bugs.Single();

            Assert.Equal(created.State.ToString("G"), viewModel.State);
        }

        [Theory]
        [InlineData(nameof(Bug.Id), nameof(BugViewModel.Id))]
        [InlineData(nameof(Bug.ReportedAt), nameof(BugViewModel.ReportedAt))]
        [InlineData(nameof(Bug.ReportedBy), nameof(BugViewModel.ReportedBy))]
        [InlineData(nameof(Bug.Title), nameof(BugViewModel.Title))]
        [InlineData(nameof(Bug.Description), nameof(BugViewModel.Description))]
        public async Task Update_BugFound_ReturnsUpdatedBugWithMatchingProperties(string entityName, string viewModelName)
        {
            var bug = FakeBug.I.Generate();
            var form = new UpdateBugForm
            {
                Title = "title",
                Description = "description",
                State = BugState.Closed
            };
            
            _fixture.AddEntity(bug);
            var sut = _fixture.GetSut();
            var option = Assert.IsType<Option<BugViewModel>.Some>(await sut.Update(bug.Id, form));
            var entity = _fixture.CleanDbContext.Bugs.Single();

            var entityProp = typeof(Bug).GetProperty(entityName)?.GetValue(entity)!;
            var viewModelProp = typeof(BugViewModel).GetProperty(viewModelName)?.GetValue(option.Value)!;
            Assert.Equal(entityProp, viewModelProp);
        }

        [Fact]
        public async Task Update_BugHasComments_ReturnsBugWithAllCommentsInChronologicalOrder()
        {
            var bug = FakeBug.I.Generate();
            bug.Comments.AddRange(FakeComment.I.Generate(10));
            _fixture.AddEntity(bug);

            var form = new UpdateBugForm
            {
                Title = "title",
                Description = "description",
                State = BugState.Closed
            };
            
            var sut = _fixture.GetSut();
            var option = Assert.IsType<Option<BugViewModel>.Some>(await sut.Update(bug.Id, form));
            var comments = option.Value.Comments;
            var sortedComments = comments.OrderBy(x => x.CommentedAt).ToList();
            
            Assert.Equal(10, comments.Count);
            Assert.All(comments.Zip(sortedComments), pair =>
            {
                var (first, second) = pair;
                Assert.Equal(first.Id, second.Id);
            });
        }

        [Theory]
        [InlineData("test title")]
        [InlineData(null, "test description")]
        [InlineData(null, null, true)]
        public async Task Update_BugFound_UpdatesFieldsOnBug(string title = null, string description = null, bool changeBugState = false)
        {
            var bug = FakeBug.I.Generate();
            title ??= bug.Title;
            description ??= bug.Description;
            var state = changeBugState
                ? bug.State switch
                {
                    BugState.Open => BugState.Closed,
                    _ => BugState.Open
                }
                : bug.State;

            var form = new UpdateBugForm
            {
                Title = title,
                Description = description,
                State = state
            };

            _fixture.AddEntity(bug);
            var sut = _fixture.GetSut();
            await sut.Update(bug.Id, form);
            var updatedEntity = _fixture.CleanDbContext.Bugs.Single();

            Assert.Equal(title, updatedEntity.Title);
            Assert.Equal(description, updatedEntity.Description);
            Assert.Equal(state, updatedEntity.State);
        }

        [Fact]
        public async Task Update_BugNotFound_ReturnsNone()
        {
            var sut = _fixture.GetSut();
            var result = await sut.Update(Guid.NewGuid(), new UpdateBugForm());
            Assert.IsType<Option<BugViewModel>.None>(result);
        }

        [Fact]
        public async Task AddComment_BugNotFound_ReturnsNone()
        {
            var sut = _fixture.GetSut();
            var result = await sut.AddComment(Guid.NewGuid(), new AddCommentForm());
            Assert.IsType<Option<BugViewModel>.None>(result);
        }

        [Fact]
        public async Task AddComment_BugHasNoPreviousComments_Added()
        {
            var bug = FakeBug.I.Generate();
            _fixture.AddEntity(bug);
            var sut = _fixture.GetSut();

            await sut.AddComment(bug.Id, TestCommentForm);
            Assert.Single(_fixture.CleanDbContext.BugComments);
        }

        [Theory]
        [InlineData(nameof(BugComment.Text), nameof(AddCommentForm.Text))]
        public async Task AddComment_NewCommentHasCorrectProperties(string entityPropName, string formPropName)
        {
            var bug = FakeBug.I.Generate();
            _fixture.AddEntity(bug);
            var sut = _fixture.GetSut();

            await sut.AddComment(bug.Id, TestCommentForm);
            var bugEntity = Assert.Single(_fixture.CleanDbContext.BugComments);

            var entityValue = typeof(BugComment).GetProperty(entityPropName)!.GetValue(bugEntity)!;
            var formValue = typeof(AddCommentForm).GetProperty(formPropName)!.GetValue(TestCommentForm)!;

            Assert.Equal(entityValue, formValue);
        }

        [Fact]
        public async Task AddComment_BugAlreadyHasAComment_BugViewModelReturnedHasBothComments()
        {
            var bug = FakeBug.I.Generate();
            bug.Comments.Add(FakeComment.I.Generate());
            _fixture.AddEntity(bug);
            var sut = _fixture.GetSut();

            var viewModel = Assert.IsType<Option<BugViewModel>.Some>(await sut.AddComment(bug.Id, TestCommentForm));
            Assert.Equal(2, viewModel.Value.Comments.Count);
        }

        [Fact]
        public async Task AddComment_BugAlreadyHasComments_CommentsReturnedAreChronological()
        {
            var bug = FakeBug.I.Generate();
            bug.Comments.AddRange(FakeComment.I.Generate(10));
            _fixture.AddEntity(bug);
            
            var sut = _fixture.GetSut();
            var viewModel = Assert.IsType<Option<BugViewModel>.Some>(await sut.AddComment(bug.Id, TestCommentForm));

            var comments = viewModel.Value.Comments;
            var sortedComments = comments
                .OrderBy(x => x.CommentedAt)
                .ToList();
            
            Assert.All(comments.Zip(sortedComments), pair =>
            {
                var (first, second) = pair;
                Assert.Equal(first.Id, second.Id);
            });
        }

        [Theory]
        [InlineData(nameof(BugComment.Id), nameof(BugCommentViewModel.Id))]
        [InlineData(nameof(BugComment.Text), nameof(BugCommentViewModel.Text))]
        public async Task AddComment_ViewModelReturnedHasCorrectProperties(string entityPropName, string viewModelPropName)
        {
            var bug = FakeBug.I.Generate();
            _fixture.AddEntity(bug);
            
            var sut = _fixture.GetSut();
            var bugViewModel = Assert.IsType<Option<BugViewModel>.Some>(await sut.AddComment(bug.Id, TestCommentForm));

            var commentEntity = _fixture.CleanDbContext.BugComments.Single();
            var commentViewModel = bugViewModel.Value.Comments.Single();

            var entityValue = typeof(BugComment).GetProperty(entityPropName)!.GetValue(commentEntity)!;
            var viewModelValue = typeof(BugCommentViewModel).GetProperty(viewModelPropName)!.GetValue(commentViewModel);
            Assert.Equal(entityValue, viewModelValue);
        }

        private readonly AddCommentForm TestCommentForm = new AddCommentForm
        {
            Text = "test comment"
        };
    }
}