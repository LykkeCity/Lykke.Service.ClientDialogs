using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lykke.Service.ClientDialogs.Core.Domain;
using Lykke.Service.ClientDialogs.Core.Services;

namespace Lykke.Service.ClientDialogs.Services
{
    public class ClientDialogsService : IClientDialogsService
    {
        private readonly IClientDialogsRepository _dialogsRepository;
        private readonly IClientDialogSubmitsRepository _dialogSubmitsRepository;

        public ClientDialogsService(
            IClientDialogsRepository dialogsRepository,
            IClientDialogSubmitsRepository dialogSubmitsRepository
            )
        {
            _dialogsRepository = dialogsRepository;
            _dialogSubmitsRepository = dialogSubmitsRepository;
        }
        
        public Task AddDialogAsync(IClientDialog clientDialog)
        {
            return _dialogsRepository.AddDialogAsync(clientDialog);
        }

        public Task<IClientDialog> GetDialogAsync(string clientId, string dialogId)
        {
            return _dialogsRepository.GetDialogAsync(clientId, dialogId);
        }

        public async Task<IEnumerable<IClientDialog>> GetDialogsAsync(string clientId)
        {
            var clientDialogs = await _dialogsRepository.GetDialogsAsync(clientId);
            var submittedDialogs = await _dialogSubmitsRepository.GetSubmittedDialogsAsync(clientId);

            //don't return submitted common dialogs
            return clientDialogs.Where(item => submittedDialogs.All(i => i.DialogId != item.Id));
        }

        public Task<IEnumerable<IClientDialog>> GetCommonDialogsAsync()
        {
            return _dialogsRepository.GetCommonDialogsAsync();
        }

        public Task DeleteDialogAsync(string clientId, string dialogId)
        {
            return _dialogsRepository.DeleteDialogAsync(clientId, dialogId);
        }

        public async Task SubmitDialogAsync(string clientId, string dialogId, string actionId)
        {
            await _dialogSubmitsRepository.SubmitDialogAsync(clientId, dialogId, actionId);
            await _dialogsRepository.DeleteDialogAsync(clientId, dialogId);
        }

        public Task<IEnumerable<IClientDialogSubmit>> GetSubmittedDialogsAsync(string clientId)
        {
            return _dialogSubmitsRepository.GetSubmittedDialogsAsync(clientId);
        }

        public Task<bool> IsDialogSubmittedAsync(string clientId, string dialogId, string actionId)
        {
            return _dialogSubmitsRepository.IsDialogSubmittedAsync(clientId, dialogId, actionId);
        }
    }
}
