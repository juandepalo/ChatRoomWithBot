using ChatRoom.Application.Commands;
using ChatRoom.Application.ViewModels;
using ChatRoomWithBot.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ChatRoom.XUnitTest
{
    public class ChatsController_Test
    {
        ChatsController _controller;
        IMediator _mediator;

        public ChatsController_Test()
        {

            _controller = new ChatsController();
        }


        [Fact]
        public void GetMessages_WhenCalled_ReturnsAllItems()
        {
            // Act
            var okResult = _controller.GetMessages(50).Result as OkObjectResult;

            // Assert
            var items = Assert.IsType<List<ChatMessageViewModel>>(okResult.Value);
            Assert.Equal(3, items.Count);
        }
        [Theory]
        [InlineData(null)]
        public void GetMessages_WhenCalled_ShouldBe(int? qty)
        {
            // Act
            var okResult = _controller.GetMessages(qty).Result as OkObjectResult;

            // Assert
            var items = Assert.IsType<List<ChatMessageViewModel>>(okResult.Value);
            Assert.Equal(3, items.Count);
        }

        [Theory]
        [InlineData(0)]
        public void GetMessages_WhenCalled_ShouldBeFalse(int? qty)
        {
            // Act
            var badResponse = _controller.GetMessages(qty).Result;

            // Assert
            Assert.IsType<BadRequestObjectResult>(badResponse);
        }

        [Fact]
        public void PostMessage_InvalidObjectPassed_ReturnsBadRequest()
        {
            // Arrange
            var nameMissingItem = new PostMessageCommand()
            {
                Message = ""
            };
            _controller.ModelState.AddModelError("Message", "Required");

            // Act
            var badResponse = _controller.PostMessage(nameMissingItem);

            // Assert
            Assert.IsType<BadRequestObjectResult>(badResponse);
        }


        [Fact]
        public void PostMessage_ValidObjectPassed_ReturnsCreatedResponse()
        {
            // Arrange
            PostMessageCommand testItem = new PostMessageCommand()
            {
                Message = "Hi there"
            };

            // Act
            var createdResponse = _controller.PostMessage(testItem);

            // Assert
            Assert.IsType<CreatedAtActionResult>(createdResponse);
        }

    }
}
