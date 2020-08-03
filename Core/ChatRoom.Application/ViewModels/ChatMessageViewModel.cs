using AutoMapper;
using ChatRoom.Application.Mappings;
using ChatRoom.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatRoom.Application.ViewModels
{
    public class ChatMessageViewModel : IMapFrom<ChatMessage>
    {
        public int Id { get; set; }
        public string NickName { get; set; }
        public string Message { get; set; }
        public DateTime CreationDate { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<ChatMessage, ChatMessageViewModel>()
                .ForMember(dest => dest.Id, map => map.MapFrom(s => s.Id))
                .ForMember(dest => dest.NickName, map => map.MapFrom(s => s.ApplicationUser.NickName ?? s.ApplicationUser.UserName))
                .ForMember(dest => dest.Message, map => map.MapFrom(s => s.Message))
                .ForMember(dest => dest.CreationDate, map => map.MapFrom(s => s.CreationDate));

        }
    }
}
