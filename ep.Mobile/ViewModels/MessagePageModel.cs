using MvvmHelpers.Commands;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using ep.Mobile.Interfaces;
using ep.Mobile.Models;
using ep.Mobile.ViewModels.Base;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ep.Mobile.ViewModels
{
    public class MessagePageModel : ViewModelBase
    {
        //private readonly ICustomerService _customerService;
        //private readonly IMessageHistoryService _historyService;
        //private readonly IMessageService _messageService;
        //private readonly ISmsService _smsService;
        //private ObservableCollection<Message> _messages;

        //public AsyncCommand<MessageParam> SMSCommand { get; private set; }
        //public IEnumerable<Message> TodaysMessages => GetTodaysMessages();

        //public MessagePageModel()
        //{
        //    _historyService = DependencyService.Get<IMessageHistoryService>();
        //    _messageService = DependencyService.Get<IMessageService>();
        //    _smsService = DependencyService.Get<ISmsService>();
        //    SMSCommand = new AsyncCommand<MessageParam>(HandleMessage);
        //}

        //private async Task HandleMessage(MessageParam param)
        //{
        //    if (param.Status is MessageStatus.Sent || param.Status is MessageStatus.Resent)
        //    {
        //        await SendSMS(param);
        //    }
        //    else if (param.Status is MessageStatus.Completed)
        //    {
        //        await Complete(param.MessageId);
        //    }
        //    else
        //    {
        //        throw new ArgumentException("Invalid status for command", nameof(param.Status));
        //    }
        //}

        //private async Task Complete(int id)
        //{
        //    var message = await _messageService.GetById(id);
        //    _messages.Remove(message);

        //    message.UpdatedOn = DateTimeOffset.UtcNow;
        //    message.Status = MessageStatus.Completed;
        //    message.Image = "complete.png";
        //    await Update(message);
        //}

        //private async Task GetMessages()
        //{
        //    var messages = await _messageService.GetAll();
        //    _messages = new ObservableCollection<Message>(messages);
        //}

        //public IEnumerable<Message> GetTodaysMessages()
        //{
        //    return _messageService.GetTodaysMessages();
        //}

        //private async Task SendSMS(MessageParam param)
        //{
        //    var message = await _messageService.GetById(param.MessageId);
        //    if (param.Status is MessageStatus.Sent)
        //    {
        //        message.Count = 1;
        //        message.Image = "sent.png";
        //        message.Status = MessageStatus.Sent;
        //    }
        //    else
        //    {
        //        message.Count = message.Count++;
        //        message.Image = "resent.png";
        //        message.Status = MessageStatus.Resent;
        //    }
        //    message.UpdatedOn = DateTimeOffset.UtcNow;
        //    message.Text = $"Your Order: {message.OrderNo} is ready to pick up";

        //    await SendMessage(message);
        //    await Update(message);
        //}

        //private async Task SendMessage(Message message)
        //{
        //    try
        //    {
        //        await _smsService.SendMessage(message.Text, message.Recipient);
        //    }
        //    catch (FeatureNotSupportedException ex)
        //    {
        //        Console.WriteLine("Sms is not supported on this device", ex.Message);
        //        throw new FeatureNotSupportedException();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Error occurred", ex.Message);
        //        throw new Exception();
        //    }
        //}

        //private async Task Update(Message message)
        //{
        //    await _messageService.Update(message);

        //    var history = new MessageHistory
        //    {
        //        CreatedOn = DateTimeOffset.UtcNow,
        //        MessageId = message.Id,
        //        Status = message.Status,
        //        SentDateTime = message.UpdatedOn
        //    };
        //    await _historyService.Insert(history);
        //}
    }
}
