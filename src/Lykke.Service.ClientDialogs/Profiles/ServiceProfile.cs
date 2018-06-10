using AutoMapper;
using Lykke.Service.ClientDialogs.Client.Models;
using Lykke.Service.ClientDialogs.Core.Domain;

namespace Lykke.Service.ClientDialogs.Profiles
{
    public class ServiceProfile : Profile
    {
        public ServiceProfile()
        {
            CreateMap<IClientDialog, ClientDialogModel>(MemberList.Source);
            CreateMap<DialogAction, DialogActionModel>(MemberList.Source);
            CreateMap<IClientDialogSubmit, SubmittedDialogModel>(MemberList.Source)
                .ForSourceMember(s => s.ClientId, o => o.Ignore());
        }
    }
}
