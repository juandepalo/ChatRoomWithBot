#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-nanoserver-1903 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-nanoserver-1903 AS build
WORKDIR /src
COPY ["ChatRoomWithBot/ChatRoomWithBot.csproj", "ChatRoomWithBot/"]
COPY ["Data/ChatRoom.Persistence/ChatRoom.Persistence.csproj", "Data/ChatRoom.Persistence/"]
COPY ["Core/ChatRoom.Application/ChatRoom.Application.csproj", "Core/ChatRoom.Application/"]
COPY ["Core/ChatRoom.ComService/ChatRoom.ComService.csproj", "Core/ChatRoom.ComService/"]
COPY ["ChatBot/ChatRoom.ChatBot.Domain/ChatRoom.ChatBot.Domain.csproj", "ChatBot/ChatRoom.ChatBot.Domain/"]
COPY ["ChatBot/ChatRoom.ChatBot/ChatRoom.ChatBot.csproj", "ChatBot/ChatRoom.ChatBot/"]
COPY ["Core/ChatRoom.Domain/ChatRoom.Domain.csproj", "Core/ChatRoom.Domain/"]
RUN dotnet restore "ChatRoomWithBot/ChatRoomWithBot.csproj"
COPY . .
WORKDIR "/src/ChatRoomWithBot"
RUN dotnet build "ChatRoomWithBot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ChatRoomWithBot.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ChatRoomWithBot.dll"]