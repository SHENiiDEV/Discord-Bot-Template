![](https://i.imgur.com/My0vD8D.png)

### Configuration


1. Open `Program.cs>`
1.1. Replace `replace_with_bot_mongodb_connection` to your mongo connection string.
1.2. Replace `replace_with_bot_token` to your bot token.
2.  Open `MongoManager.cs` dir `/Services/`.
2.1. Line `28` Replace `replace_with_database_name` with database name.
2.2 Line `41` Replace `replace_with_collection_name` with collection name.
3. Inside `/Entities/` MongoDB Models, you can modify that data you need.

Copy this method
```
public static async Task<SocketInteraction> WaitForInteractionAsync(BaseSocketClient client, TimeSpan timeout,
              Predicate<SocketInteraction> predicate, CancellationToken cancellationToken = default)
        {
            var tcs = new TaskCompletionSource<SocketInteraction>();

            var waitCancelSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            Task wait = Task.Delay(timeout, waitCancelSource.Token)
                .ContinueWith((t) =>
                {
                    if (!t.IsCanceled)
                        tcs.SetResult(null);
                });

            cancellationToken.Register(() => tcs.SetCanceled());

            client.InteractionCreated += HandleInteraction;
            var result = await tcs.Task.ConfigureAwait(false);
            client.InteractionCreated -= HandleInteraction;

            return result;

            Task HandleInteraction(SocketInteraction interaction)
            {
                if (predicate(interaction))
                {
                    waitCancelSource.Cancel();
                    tcs.SetResult(interaction);
                }

                return Task.CompletedTask;
            }
        }

        public static Task<SocketInteraction> WaitForMessageComponentAsync(BaseSocketClient client, IUserMessage fromMessage, IUser fromUser, TimeSpan timeout,
            CancellationToken cancellationToken = default)
        {
            bool Predicate(SocketInteraction interaction) => interaction is SocketMessageComponent component &&
                                                             component.Message.Id == fromMessage.Id && fromUser.Id == component.User.Id;

            return WaitForInteractionAsync(client, timeout, Predicate, cancellationToken);
        }

        public static async Task<bool> ConfirmAsync(BaseSocketClient client, IUser user, IMessageChannel channel, TimeSpan timeout, string message = null,
            CancellationToken cancellationToken = default)
        {
            message ??= "Would you like to continue?";
            var confirmId = $"confirm";
            var declineId = $"decline";

            var embed = EmbedHelper.SendEmbed("Notification ", user, message, null, null, true, 0, user.GetAvatarUrl());

            var component = new ComponentBuilder()
                .WithButton("Yes", confirmId, ButtonStyle.Success)
                .WithButton("No", declineId, ButtonStyle.Danger)
                .Build();

            var prompt = await channel.SendMessageAsync(message, component: component, embed:embed).ConfigureAwait(false);

            var response = await WaitForMessageComponentAsync(client, prompt, user, timeout, cancellationToken).ConfigureAwait(false) as SocketMessageComponent;

            await prompt.DeleteAsync().ConfigureAwait(false);

            if (response != null && response.Data.CustomId == confirmId)
                return true;
            else
                return false;
        }
```

Usage:
```
_ = Task.Run(async () => {

var confirm = await ConfirmButtons.ConfirmAsync(Global.Client, button.User, button.Channel, TimeSpan.FromSeconds(30), $"Are you sure?");
  if (confirm){
  //Do the code if yes
  }
);
```
