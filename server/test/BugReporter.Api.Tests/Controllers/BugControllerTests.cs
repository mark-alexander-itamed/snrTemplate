using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using BugReporter.Api.Controllers;
using BugReporter.Api.Forms;
using BugReporter.Api.Services;
using BugReporter.Api.Tests.Fakes;
using BugReporter.Api.Utils;
using BugReporter.Api.ViewModels;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Xunit;

namespace BugReporter.Api.Tests.Controllers
{
    public class BugContollerTests
    {
        private class Fixture
        {
            public IBugService BugService { get; } = Substitute.For<IBugService>();

            public BugController GetSut() => new BugController(BugService);
        }

        private readonly Fixture _fixture = new Fixture();

        [Fact]
        public async Task Query_ReturnsOk()
        {
            _fixture.BugService.Query().Returns(FakeBug.I.Generate(10).Select(BugViewModel.FromData));
            var sut = _fixture.GetSut();
            var response = Assert.IsAssignableFrom<ObjectResult>(await sut.Query());
            Assert.Equal(200, response.StatusCode);
        }

        [Fact]
        public async Task Query_ReturnsAllDataRetrieved()
        {
            var bugs = FakeBug.I.Generate(10).Select(BugViewModel.FromData).ToList();
            _fixture.BugService.Query().Returns(bugs);
            var sut = _fixture.GetSut();
            var response = Assert.IsAssignableFrom<ObjectResult>(await sut.Query());
            Assert.Same(bugs, response.Value);
        }

        [Fact]
        public async Task Find_BugIsFound_Returns200()
        {
            var bug = BugViewModel.FromData(FakeBug.I.Generate());
            _fixture.BugService.Find(bug.Id).Returns(new Option<BugViewModel>.Some(bug));
            var sut = _fixture.GetSut();
            var response = Assert.IsAssignableFrom<ObjectResult>(await sut.Find(bug.Id));
            Assert.Equal(200, response.StatusCode);
        }

        [Fact]
        public async Task Find_BugIsFound_ReturnsBugInContent()
        {
            var bug = BugViewModel.FromData(FakeBug.I.Generate());
            _fixture.BugService.Find(bug.Id).Returns(new Option<BugViewModel>.Some(bug));
            var sut = _fixture.GetSut();
            var response = Assert.IsAssignableFrom<ObjectResult>(await sut.Find(bug.Id));
            Assert.Same(bug, response.Value);
        }

        [Fact]
        public async Task Find_BugIsNotFound_ReturnsNotFoundResult()
        {
            _fixture.BugService.Find(Arg.Any<Guid>()).Returns(new Option<BugViewModel>.None());
            var sut = _fixture.GetSut();
            var response = Assert.IsAssignableFrom<StatusCodeResult>(await sut.Find(Guid.NewGuid()));
            Assert.Equal(404, response.StatusCode);
        }

        [Fact]
        public async Task Create_ResponseCodeIs201()
        {
            var bug = BugViewModel.FromData(FakeBug.I.Generate());
            _fixture.BugService.Create(Arg.Any<CreateBugForm>()).Returns(bug);
            var sut = _fixture.GetSut();
            var response = Assert.IsAssignableFrom<ObjectResult>(await sut.Create(new CreateBugForm()));
            Assert.Equal(201, response.StatusCode);
        }

        [Fact]
        public async Task Create_RoutePointsToFind()
        {
            var bug = BugViewModel.FromData(FakeBug.I.Generate());
            _fixture.BugService.Create(Arg.Any<CreateBugForm>()).Returns(bug);
            var sut = _fixture.GetSut();
            var response = Assert.IsAssignableFrom<CreatedAtActionResult>(await sut.Create(new CreateBugForm()));
            Assert.Equal(nameof(BugController.Find), response.ActionName);
            Assert.Equal(bug.Id, response.RouteValues["id"]);
        }

        [Fact]
        public async Task Create_ResponseBodyIsCreatedBug()
        {
            var bug = BugViewModel.FromData(FakeBug.I.Generate());
            _fixture.BugService.Create(Arg.Any<CreateBugForm>()).Returns(bug);
            var sut = _fixture.GetSut();
            var response = Assert.IsAssignableFrom<CreatedAtActionResult>(await sut.Create(new CreateBugForm()));
            Assert.Same(bug, response.Value);
        }

        [Fact]
        public async Task Update_Successful_StatusCodeIs200()
        {
            var bug = BugViewModel.FromData(FakeBug.I.Generate());
            _fixture.BugService.Update(Arg.Any<Guid>(), Arg.Any<UpdateBugForm>())
                .Returns(new Option<BugViewModel>.Some(bug));
            
            var sut = _fixture.GetSut();
            var response = Assert.IsAssignableFrom<ObjectResult>(await sut.Update(bug.Id, new UpdateBugForm()));
            
            Assert.Equal(200, response.StatusCode);
        }

        [Fact]
        public async Task Update_Successful_ReturnsUpdatedBug()
        {
            var bug = BugViewModel.FromData(FakeBug.I.Generate());
            _fixture.BugService.Update(Arg.Any<Guid>(), Arg.Any<UpdateBugForm>())
                .Returns(new Option<BugViewModel>.Some(bug));
            
            var sut = _fixture.GetSut();
            var response = Assert.IsAssignableFrom<ObjectResult>(await sut.Update(bug.Id, new UpdateBugForm()));
            
            Assert.Same(bug, response.Value);
        }

        [Fact]
        public async Task Update_BugNotFound_Returns404()
        {
            _fixture.BugService.Update(Arg.Any<Guid>(), Arg.Any<UpdateBugForm>())
                .Returns(new Option<BugViewModel>.None());
            
            var sut = _fixture.GetSut();
            var response = Assert.IsAssignableFrom<StatusCodeResult>(await sut.Update(Guid.Empty, new UpdateBugForm()));
            Assert.Equal(404, response.StatusCode);
        }

        [Fact]
        public async Task AddComment_Successful_StatusCodeIs200()
        {
            _fixture.BugService.AddComment(Arg.Any<Guid>(), Arg.Any<AddCommentForm>())
                .Returns(new Option<BugViewModel>.Some(new BugViewModel()));

            var sut = _fixture.GetSut();
            var response = Assert.IsAssignableFrom<ObjectResult>(await sut.AddComment(Guid.Empty, new AddCommentForm()));
            Assert.Equal(200, response.StatusCode);
        }

        [Fact]
        public async Task AddComment_Successful_ReturnsViewModel()
        {
            var vm = new BugViewModel();
            _fixture.BugService.AddComment(Arg.Any<Guid>(), Arg.Any<AddCommentForm>())
                .Returns(new Option<BugViewModel>.Some(vm));

            var sut = _fixture.GetSut();
            var response = Assert.IsAssignableFrom<ObjectResult>(await sut.AddComment(Guid.Empty, new AddCommentForm()));
            Assert.Same(vm, response.Value);
        }

        [Fact]
        public async Task AddComment_NotFound_Returns404()
        {
            _fixture.BugService.AddComment(Arg.Any<Guid>(), Arg.Any<AddCommentForm>())
                .Returns(new Option<BugViewModel>.None());
            var sut = _fixture.GetSut();
            var response = Assert.IsAssignableFrom<StatusCodeResult>(await sut.AddComment(Guid.Empty, new AddCommentForm()));
            Assert.Equal(404, response.StatusCode);
        }
    }
}