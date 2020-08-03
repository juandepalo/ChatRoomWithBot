# ChatRoomWithBot
This is a Chat Room application with a bot capabilities.

## Features
  - Chat Room for talking with other users
  - .NET Identity Core for users authentication
  - Consume Chat Bots

## Technical Details
The back end was built in [.NET Core 3.1](https://dotnet.microsoft.com/download), which implements a microservices architecture using [CQRS Pattern](https://docs.microsoft.com/en-us/azure/architecture/patterns/cqrs) with [MeadiatR](https://github.com/jbogard/MediatR).

For real time communications, [SignalR](https://github.com/dotnet/AspNetCore/tree/master/src/SignalR) was used between users, which is consumed over [MeadiatR](https://github.com/jbogard/MediatR) notifications and events, and [RabbitMQ](https://www.rabbitmq.com/install-windows.html) for handling bots requests and responses.

The front end was made in [Angular 8](https://angular.io/)

### Projects Structure

| Project | Description |
| ------ | ------ |
| ChatRoomWithBot | Web Application |
| ChatRoom.Appication | Application Core Class Library | 
| ChatRoom.ComService | Class Library for communication services: SignalR & RabbitMQ | 
| ChatRoom.Domain | Class Library for application related domain classes | 
| ChatRoom.Persistence | Data layer project | 
| ChatRoom.ChatBot.Domain | Class Library for Chatbots related domain classes | 
| ChatRoom.ChatBot | Console App for handling Bot events | 
| ChatRoom.Test | Test Project |  

### Requirements

  - [RabbitMQ](https://www.rabbitmq.com/install-windows.html) Server (Requires [ERLang/OTP](https://www.erlang.org/downloads))
  - [.NET Core 3.1](https://dotnet.microsoft.com/download)
  - [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
  - [NodeJS 12](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
  - [Angular CLI](https://cli.angular.io/)
 
