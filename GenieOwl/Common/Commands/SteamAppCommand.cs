﻿namespace GenieOwl.Common.Commands
{
    using Discord.Commands;
    using GenieOwl.Common.Interfaces;
    using GenieOwl.Integrations.Entities;
    using GenieOwl.Utilities.Messages;
    using GenieOwl.Utilities.Types;
    using Microsoft.Extensions.Configuration;

    public class SteamAppCommand : ModuleBase<SocketCommandContext>
    {
        private readonly IDiscordService _DiscordService;

        private readonly ISteamService _SteamService;

        public SteamAppCommand(IConfiguration configuration, ISteamService steamService, IDiscordService discordService)
        {
            this._SteamService = steamService;
            this._DiscordService = discordService;
        }

        /// <summary>
        /// Orquesta las respuestas de la búsqueda de logros por aplicación de Steam en inglés
        /// </summary>
        /// <param name="gameName">Nombre aplicación de Steam a búscar</param>
        /// <returns>Respuesta asincrona para el mensaje de Discord</returns>
        [Command("game")]
        [Summary("Search achievements for game in english")]
        public async Task ExecuteAsync([Remainder][Summary("A steam game")] string gameName)
        {
            await Execute(gameName, LenguageType.English);
        }

        /// <summary>
        /// Orquesta las respuestas de la búsqueda de logros por aplicación de Steam en inglés
        /// </summary>
        /// <param name="gameName">Nombre aplicación de Steam a búscar</param>
        /// <returns>Respuesta asincrona para el mensaje de Discord</returns>
        [Command("g")]
        [Summary("Search achievements for game in english. Short command")]
        public async Task ShortExecuteAsync([Remainder][Summary("A steam game")] string gameName)
        {
            await Execute(gameName, LenguageType.English);
        }

        /// <summary>
        /// Orquesta las respuestas de la búsqueda de logros por aplicación de Steam en inglés
        /// </summary>
        /// <param name="gameName">Nombre aplicación de Steam a búscar</param>
        /// <returns>Respuesta asincrona para el mensaje de Discord</returns>
        [Command("game-es")]
        [Summary("Search achievements for game in spanish")]
        public async Task ExecuteSpanishAsync([Remainder][Summary("A steam game")] string gameName)
        {
            await Execute(gameName, LenguageType.Spanish);
        }

        [Command("g-es")]
        [Summary("Search achievements for game in spanish. Short command")]
        public async Task ShortExecuteSpanishAsync([Remainder][Summary("A steam game")] string gameName)
        {
            await Execute(gameName, LenguageType.Spanish);
        }

        private async Task Execute(string gameName, LenguageType lenguage)
        {
            if (string.IsNullOrEmpty(gameName))
            {
                await ReplyAsync(CustomMessages.GetMessage(MessagesType.HelpGameCommand));
                return;
            }

            try
            {
                List<SteamApp> appsResult = this._SteamService.GetSteamAppsByMatches(gameName, this.Context.Message.Author.IsBot);

                if (appsResult.Count == 1)
                {
                    await this._DiscordService.GetAppAchivementsButtons(appsResult.FirstOrDefault(), this.Context, lenguage);
                }
                else
                {
                    await this._DiscordService.GetAppsButtons(appsResult, this.Context, lenguage);
                }
            }
            catch (Exception ex)
            {
                await ReplyAsync(ex.Message);
            }
        }
    }
}
