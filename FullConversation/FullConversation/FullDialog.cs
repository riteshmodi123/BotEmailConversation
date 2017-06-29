using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Threading.Tasks;


namespace FullConversation
{
    [Serializable()]
    public class FullDialog : IDialog<object>
    {

        private void Logger(IDialogContext context,  string value)
        {

                string logger;
                bool aa = context.ConversationData.TryGetValue("logger", out logger);
                if (aa == false)
                    logger = "";
                context.ConversationData.SetValue("logger", logger + Environment.NewLine + value);

        }
        public async Task StartAsync(IDialogContext context)
        {

            Logger(context, "Hi There!! Lets get started !!");
            await context.PostAsync("Hi There!! Lets get started !!");
            


            context.Wait(GetStarted);
        }

        public virtual async Task GetStarted(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var name = await result;
            Logger(context, name.Text);
            Logger(context, "Please provide your name !!");
            await context.PostAsync("Please provide your name !!");
            context.Wait(NameFromUserMethod);
        }

        public virtual async Task NameFromUserMethod(IDialogContext context, IAwaitable<IMessageActivity> result)
        {

            var name = await result;
            Logger(context, name.Text);
            context.UserData.SetValue("username", name.Text);
            Logger(context, "Please provide your City name !!");
            await context.PostAsync("Please provide your City name !!");
            context.Wait(CityFromUserMethod);
        }

        public virtual async Task CityFromUserMethod(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var city = await result;
            Logger(context, city.Text);

            string userName = context.UserData.GetValue<string>("username");

            context.ConversationData.SetValue("cityname", city.Text);
            Logger(context, $"Hello {userName} !! you stay in {city.Text}..Provide a rating (1-10) for this conversation");
            await context.PostAsync($"Hello {userName} !! you stay in {city.Text}..Provide a rating (1-10) for this conversation");
            context.Wait(RatingFromUserMethod);
        }

        public virtual async Task RatingFromUserMethod(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var rating = await result;
            Logger(context, rating.Text);

            string userName = context.UserData.GetValue<string>("username");
            string cityName = context.ConversationData.GetValue<string>("cityname");

            context.PrivateConversationData.SetValue("rating", rating.Text);
            Logger(context, $"Hello {userName} !! Enter 'over' to know your details !!");

            await context.PostAsync($"Hello {userName} !! Enter 'over' to know your details !!");
            context.Wait(closingConversation);
        }

        public virtual async Task closingConversation(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var rating = await result;
            Logger(context, rating.Text);
            string userName = context.UserData.GetValue<string>("username");
            string cityName = context.ConversationData.GetValue<string>("cityname");
            string score = context.PrivateConversationData.GetValue<string>("rating");
            Logger(context, $"Hello {userName} !! you stay in {cityName}.. and you have rated this conversation with a score of {score} .. Thank you!!");
            await context.PostAsync($"Hello {userName} !! you stay in {cityName}.. and you have rated this conversation with a score of {score} .. Thank you!!");
            context.Wait(NameFromUserMethod);
        }
    }
}