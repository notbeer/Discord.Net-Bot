using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;


namespace Discord_cs_Bot.Commands.Modules.Misc
{
    public class UserInfoCommand : ModuleBase<SocketCommandContext>
    {
        [Command("userinfo", RunMode = RunMode.Async)]
        public async Task UserInfoAsync(string id = null)
        {
            try
            {
                SocketGuildUser user;
                var stpwatch = new Stopwatch();
                stpwatch.Start();
                if (id == null)
                {
                    user = Context.Guild.GetUser(Context.User.Id);
                }
                else
                {
                    try
                    {
                        var b = MentionUtils.ParseUser(id);
                        user = Context.Guild.GetUser(b);
                    }
                    catch (Exception)
                    {
                        user = Context.Guild.GetUser(Convert.ToUInt64(id));
                    }
                }
                var joinspan = DateTime.Now.Subtract(user.JoinedAt.HasValue ? user.JoinedAt.Value.Date : DateTime.Now);
                var premiumspan = DateTime.Now.Subtract(user.PremiumSince.HasValue ? user.PremiumSince.Value.Date : DateTime.Now);
                var discordjoinspan = DateTime.Now.Subtract(user.CreatedAt.DateTime.Date); ;
                await ReplyAsync(embed: new EmbedBuilder()
                {
                    Author = new EmbedAuthorBuilder()
                    { 
                        Name = Context.User.Username,
                        IconUrl = Context.User.GetAvatarUrl()
                    },
                    Color = Color.Blue,
                    ThumbnailUrl = user.GetAvatarUrl(),
                    Footer = new EmbedFooterBuilder()
                    {
                        Text = $" - Requested By {Context.User.Username} - Done in 0.{stpwatch.ElapsedMilliseconds}s",
                        IconUrl = Context.User.GetAvatarUrl()
                    },
                    Timestamp = DateTimeOffset.Now
                }
                    .AddField("**Joined Server at: **", user.JoinedAt.HasValue ? user.JoinedAt.Value.Date + $" - ({joinspan.Days} days ago)" : "Date Not Found")
                    .AddField("**Account Created At: **", user.CreatedAt.DateTime.Date + $" - ({discordjoinspan.Days} days ago)")
                    .AddField("**Username: **", user.Username)
                    .AddField("**Discriminator: **", user.Discriminator)
                    .AddField("**ID: **", user.Id)
                    .AddField("**Nickname: **", user.Nickname ?? "None")
                    .AddField("**User type: **", user.IsBot ? "Human" : "Bot")
                    .AddField("**Roles[" + user.Roles.Count + "]: **", string.Join(", ", user.Roles))
                    .Build());
                stpwatch.Stop();
            }
            catch (Exception e)
            {
                await ReplyAsync(embed: new EmbedBuilder()
                {
                    Title = "An Error Occured!",
                    Description = e.Message
                }.Build());
            }
        }
    }
}
