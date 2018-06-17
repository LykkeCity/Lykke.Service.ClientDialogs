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
        private readonly IDialogConditionsRepository _dialogConditionsRepository;

        public ClientDialogsService(
            IClientDialogsRepository dialogsRepository,
            IClientDialogSubmitsRepository dialogSubmitsRepository,
            IDialogConditionsRepository dialogConditionsRepository
            )
        {
            _dialogsRepository = dialogsRepository;
            _dialogSubmitsRepository = dialogSubmitsRepository;
            _dialogConditionsRepository = dialogConditionsRepository;
        }

        public async Task<IClientDialog> AddDialogAsync(IClientDialog clientDialog)
        {
            var dialog = await _dialogsRepository.AddDialogAsync(clientDialog);

            if (dialog.IsCommon)
            {
                await _dialogsRepository.AssignDialogToAllAsync(dialog.Id);
            }
            else
            {
                await _dialogsRepository.UnAssignCommonDialogAsync(dialog.Id);
            }

            return dialog;
        }

        public async Task<IEnumerable<IClientDialog>> GetDialogsAsync()
        {
            return await _dialogsRepository.GetDialogsAsync();
        }

        public Task<IClientDialog> GetDialogAsync(string dialogId)
        {
            return _dialogsRepository.GetDialogAsync(dialogId);
        }

        public Task AssignDialogToAllAsync(string dialogId)
        {
            return _dialogsRepository.AssignDialogToAllAsync(dialogId);
        }

        public Task UnAssignCommonDialogAsync(string dialogId)
        {
            return _dialogsRepository.UnAssignCommonDialogAsync(dialogId);
        }

        public Task DeleteDialogAsync(string dialogId)
        {
            var tasks = new List<Task>
            {
                _dialogsRepository.DeleteDialogAsync(dialogId),
                _dialogConditionsRepository.DeleteDialogConditionsAsync(dialogId)
            };
            
            return Task.WhenAll(tasks);
        }

        public async Task<IEnumerable<IClientDialog>> GetClientDialogsAsync(string clientId)
        {
            var clientDialogs = await _dialogsRepository.GetClientDialogsAsync(clientId);
            var submittedDialogs = await _dialogSubmitsRepository.GetSubmittedDialogsAsync(clientId);

            //don't return submitted dialogs
            return clientDialogs.Where(item => submittedDialogs.All(i => i.DialogId != item.Id));
        }

        public Task<IClientDialog> GetClientDialogAsync(string clientId, string dialogId)
        {
            return _dialogsRepository.GetClientDialogAsync(clientId, dialogId);
        }

        public Task AssignDialogToClientAsync(string clientId, string dialogId)
        {
            return _dialogsRepository.AssignDialogToClientAsync(clientId, dialogId);
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

        public Task<IEnumerable<IClientDialog>> GetCommonDialogsAsync()
        {
            return _dialogsRepository.GetCommonDialogsAsync();
        }
    }
}
