using AutoMapper;
using Lykke.Service.ClientDialogs.Client.Models;
using Lykke.Service.ClientDialogs.Core.Domain;

namespace Lykke.Service.ClientDialogs.Profiles
{
    public class ServiceProfile : Profile
    {
        public ServiceProfile()
        {
            CreateMap<IClientDialog, DialogModel>(MemberList.Source);
            CreateMap<IClientDialog, ClientDialogModel>(MemberList.Source)
                .ForSourceMember(s => s.IsGlobal, o => o.Ignore());
            CreateMap<DialogAction, DialogActionModel>(MemberList.Source);
            CreateMap<IClientDialogSubmit, SubmittedDialogModel>(MemberList.Source)
                .ForSourceMember(s => s.ClientId, o => o.Ignore());
            CreateMap<Core.Domain.DialogConditionType, Client.Models.DialogConditionType>(MemberList.Source);
            CreateMap<IDialogCondition, DialogConditionModel>(MemberList.Source);
        }
    }
}
