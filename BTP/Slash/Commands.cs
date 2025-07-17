using BTP.DBConnection;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BTP.Slash
{
    public class Commands : ApplicationCommandModule
    {
        databaseConnection dbConnection = new databaseConnection();

        [SlashCommand("punkty", "Dodaj/Odejmij punkty tygodnia postaci dla użytkownika")]
        [SlashRequirePermissions(DSharpPlus.Permissions.ManageMessages)]
        public async Task Punkty(InteractionContext ctx,
        [Option("operacja", "Wybierz operację (dodaj/odejmij)")]
        [Choice("Dodaj", "dodaj")]
        [Choice("Odejmij", "odejmij")] string action,
        [Option("osoba", "Spinguj osobę której chcesz dać punkty")] DiscordUser user,
        [Option("Ilość", "Podaj ilość punktów")] double amountOfPoints)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource, new DiscordInteractionResponseBuilder().AsEphemeral());

            double finalPoints = action == "odejmij" ? -amountOfPoints : amountOfPoints;
            Console.WriteLine(finalPoints);

            var parameters = new Dictionary<string, object>
            {
                { "@userId", user.Id },
                { "@username", user.Username },
                { "@points", finalPoints }
            };

            string query = @"
                INSERT INTO `punkty` (`user_id`, `username`, `points`) 
                VALUES (@userId, @username, @points) 
                ON DUPLICATE KEY UPDATE 
                    points = points + VALUES(points), 
                    username = VALUES(username);
            ";

            await dbConnection.DatabaseCommand(query, parameters);

            if(action == "odejmij")
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent($"Odjęto użytkownikowi <@{user.Id}> {amountOfPoints} punktów"));
            }
            else
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent($"Dodano użytkownikowi <@{user.Id}> {amountOfPoints} punktów"));
            }
        }
        [SlashCommand("sprawdz", "Sprawdź punkty tygodnia postaci dla użytkownika")]
        public async Task Sprawdz(InteractionContext ctx,
        [Option("osoba", "Spinguj osobę której chcesz sprawdzić punkty")] DiscordUser user = null)
        {
            if (user == null)
            {
                user = ctx.User;
            }

            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource, new DiscordInteractionResponseBuilder().AsEphemeral());
            var parameters = new Dictionary<string, object>
            {
                { "@userId", user.Id }
            };
            string query = "SELECT points FROM punkty WHERE user_id = @userId";
            double points = 0;
            try
            {
                using (var reader = await dbConnection.ExecuteReaderAsync(query, parameters))
                {
                    if (await reader.ReadAsync())
                    {
                        points = reader.GetDouble(0);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Błąd podczas odczytu punktów: " + ex.Message);
            }
            await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent($"Użytkownik <@{user.Id}> ma {points} punktów tygodnia postaci."));
        }
    }
    
}
