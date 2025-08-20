//using QShirt.Application.Contracts.Infrastructure;
//using QShirt.Application.SystemVariables;
//using QShirt.Domain.Notifications;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Telegram.Bot;
//using Telegram.Bot.Types;

//namespace QShirt.Infrastructure
//{
//    /// <summary>
//    /// TelegramBot provider
//    /// </summary>
//    public class TelegramBotProvider : ITelegramBotProvider
//    {
//        #region Fields

//        private readonly SystemParamsProvider systemVariablesProvider;
//        private TelegramBotClient bot;
//        private SystemParamTelegramSettings telegramSettings;

//        #endregion

//        #region Constructor

//        public TelegramBotProvider(SystemParamsProvider systemVariablesProvider)
//        {
//            this.systemVariablesProvider = systemVariablesProvider;
//            InitializeBot().Wait();
//        }

//        #endregion Constructor

//        /// <summary>
//        /// Send message
//        /// </summary>
//        public async Task<string> SendTextMessage(string text)
//        {
//            //maximum message size 4096 characters
//            List<string> messages = Split(text, 4000);

//            string sentMessagesIds = string.Empty;

//            foreach (string message in messages)
//            {
//                if (!string.IsNullOrWhiteSpace(message))
//                {
//                    OnRequestSending($"Sending to TelegramBot chatId: {telegramSettings.ChatId}, message: {message}");

//                    var result = await bot.SendTextMessageAsync(telegramSettings.ChatId, message);

//                    OnResponceGetting(result.Text);

//                    sentMessagesIds += "id: " + result.Text;
//                }
//            }

//            return sentMessagesIds;
//        }

//        /// <summary>
//        /// Send document
//        /// </summary>
//        public async Task SendDocument(string filePath, string fileName)
//        {
//            //File sending
//            using (FileStream stream = System.IO.File.Open(filePath, FileMode.Open))
//            {
//                OnRequestSending($"Sending to TelegramBot chatId: {telegramSettings.ChatId}, error file: {filePath}");
//                var result = await bot.SendDocumentAsync(telegramSettings.ChatId, new FileToSend(fileName, stream));
//                OnResponceGetting(result.Type.ToString() + result.MessageId);
//            }
//        }

//        #region PrivateMethods

//        /// <summary>
//        /// Message processing
//        /// </summary>
//        private static List<string> Split(string str, int chunkSize)
//        {
//            //Message splitting
//            List<string> messages = new List<string>();

//            //All lines
//            List<string> lines = str.Split(new[] { Environment.NewLine }, StringSplitOptions.None).ToList();
//            StringBuilder currentMessage = new StringBuilder();
//            foreach (var line in lines)
//            {
//                string currentLine = line;
//                if (currentLine.StartsWith("url"))
//                {
//                    currentLine = currentLine.Replace("http", "xttp");
//                    int lenght = currentLine.IndexOf("?", StringComparison.Ordinal);
//                    if (lenght > 0)
//                        currentLine = currentLine.Substring(0, lenght) + "?...";
//                }

//                if (currentMessage.Length + currentLine.Length < chunkSize)
//                    currentMessage.AppendLine(currentLine);
//                else
//                {
//                    messages.Add(currentMessage.ToString());
//                    currentMessage.Clear();
//                }
//            }
//            if (!string.IsNullOrEmpty(currentMessage.ToString()))
//                messages.Add(currentMessage.ToString());
//            return messages;
//        }

//        /// <summary>
//        /// TelegramBot initialization
//        /// </summary>
//        private async Task InitializeBot()
//        {
//            //Get telegram settings
//            telegramSettings = await systemVariablesProvider.GetTelegramSettings();

//            if (telegramSettings != null)
//                bot = new TelegramBotClient(telegramSettings.Token);
//        }

//        #endregion

//        #region Events

//        /// <summary>
//        /// Request sending event
//        /// </summary>
//        public event EventHandler<string> SendRequestEvent;

//        private void OnRequestSending(string request)
//        {
//            SendRequestEvent?.Invoke(this, request);
//        }

//        /// <summary>
//        /// Response receiving event
//        /// </summary>
//        public event EventHandler<string> GetResponceEvent;

//        private void OnResponceGetting(string response)
//        {
//            GetResponceEvent?.Invoke(this, response);
//        }

//        public object As<T>()
//        {
//            throw new NotImplementedException();
//        }

//        #endregion
//    }
//}
