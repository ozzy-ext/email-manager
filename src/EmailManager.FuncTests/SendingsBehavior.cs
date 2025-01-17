﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyLab.EmailManager.Client.Common;
using MyLab.EmailManager.Client.Sendings;
using MyLab.EmailManager.Domain.ValueObjects;
using MyLab.EmailManager.Infrastructure;
using MyLab.EmailManager.Infrastructure.Db.EfModels;

namespace EmailManager.FuncTests
{
    public partial class SendingsBehavior
    {
        [Fact]
        public async Task ShouldCreateSending()
        {
            //Arrange
            var clientAsset = _apiFxt.StartWithProxy();
            var emailId = await CreateAndConfirmEmail(clientAsset.ServiceProvider, "foo", "bar");
            await CreateAndConfirmEmail(clientAsset.ServiceProvider, "foo", "qoz");
            var client = clientAsset.ApiClient;

            var dbCtx = clientAsset.ServiceProvider.GetRequiredService<ReadDbContext>();

            //Act
            var sendingId = await client.CreateAsync
            (
                new SendingDefDto
                {
                    Selection = new Dictionary<string, string>
                    {
                        {"marker1", "foo"},
                        {"marker2", "bar"}
                    },
                    Title = "Hello!",
                    SimpleContent = "The sun is yellow"
                }
            );

            var sending = await dbCtx.Sendings
                .Include(dbSending => dbSending.Messages)
                .FirstOrDefaultAsync(s => s.Id == sendingId);

            //Assert
            Assert.NotNull(sending);
            Assert.NotNull(sending.Selection);
            Assert.Equal(SendingStatus.Pending.ToLiteral(), sending.SendingStatus);
            Assert.NotEqual(default, sending.SendingStatusDt);
            Assert.Null(sending.TemplateId);
            Assert.Null(sending.TemplateArgs);
            Assert.Equal("The sun is yellow", sending.SimpleContent);
            Assert.NotNull(sending.Messages);
            Assert.Single(sending.Messages);

            var foundMessage = sending.Messages.First();

            Assert.True(foundMessage is
            {
                Title:"Hello!",
                Content: "The sun is yellow",
                IsHtml: false,
                EmailAddress: "foo@bar.com",
            });
            Assert.Equal(emailId, foundMessage.EmailId);
            Assert.NotEqual(default, foundMessage.CreateDt);
            Assert.False(foundMessage.SendDt.HasValue);
            Assert.Equal(SendingStatus.Pending.ToLiteral(), foundMessage.SendingStatus);
            Assert.NotEqual(default, foundMessage.SendingStatusDt);
        }

        [Fact]
        public async Task ShouldGetSending()
        {
            //Arrange
            var clientAsset = _apiFxt.StartWithProxy();
            var emailId = await CreateAndConfirmEmail(clientAsset.ServiceProvider, "foo", "bar");
            await CreateAndConfirmEmail(clientAsset.ServiceProvider, "foo", "qoz");
            var client = clientAsset.ApiClient;

            var sendingId = await client.CreateAsync
            (
                new SendingDefDto
                {
                    Selection = new Dictionary<string, string>
                    {
                        {"marker1", "foo"},
                        {"marker2", "bar"}
                    },
                    Title = "Hello!",
                    SimpleContent = "The sun is yellow"
                }
            );

            //Act
            var sending = await client.GetAsync(sendingId);

            //Assert
            Assert.NotNull(sending);
            Assert.NotNull(sending.Selection);
            Assert.Equal(SendingStatusDto.Pending, sending.SendingStatus);
            Assert.NotEqual(default, sending.SendingStatusDt);
            Assert.Null(sending.TemplateId);
            Assert.True(sending.TemplateArgs is not { Count: > 0 });
            Assert.Equal("The sun is yellow", sending.SimpleContent);
            Assert.NotNull(sending.Messages);
            Assert.Single(sending.Messages);

            var foundMessage = sending.Messages.First();

            Assert.True(foundMessage is
            {
                Title: "Hello!",
                Content: "The sun is yellow",
                IsHtml: false
            });
            Assert.Equal(emailId, foundMessage.EmailId);
            Assert.NotEqual(default, foundMessage.CreateDt);
            Assert.False(foundMessage.SendDt.HasValue);
            Assert.Equal(SendingStatusDto.Pending, foundMessage.SendingStatus);
            Assert.NotEqual(default, foundMessage.SendingStatusDt);
        }
    }
}
