using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;
using System.Threading.Tasks;


namespace BTP.Slash
{
    public class Commands : ApplicationCommandModule
    {
        [SlashCommand("punkty", "Dodaj/Odejmij punkty tygodnia postaci dla użytkownika")]
        [SlashRequirePermissions(DSharpPlus.Permissions.ManageMessages)]
        public async Task Punkty(InteractionContext ctx, [Option("osoba", "Spinguj osobę której chcesz dać punkty")] DiscordUser user, [Option("Ilość", "Podaj ilość punktów")] double amountOfPoints)
        {
            await ctx.Interaction.CreateResponseAsync(DSharpPlus.InteractionResponseType.DeferredChannelMessageWithSource);

            
            // połączenie się z bazą danych i wysyłanie punktów ogólnie tutaj będą wszystkie funkcje

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent($"Przydzielono użytkownikowi <@{user.Id}> {amountOfPoints} punktów"));
        }
    }
    
}
