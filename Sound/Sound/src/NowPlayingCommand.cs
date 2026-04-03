using System;
using System.Linq;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace MyMusicPack
{
    public class MyMusicCommand : ModSystem
    {
        private ICoreClientAPI? clientApi;
        private MusicConfig? musicConfig;

        public override void StartClientSide(ICoreClientAPI api)
        {
            clientApi = api;

            var asset = api.Assets.Get(new AssetLocation("sound", "music/musicconfig.json"));
            if (asset != null)
            {
                musicConfig = asset.ToObject<MusicConfig>();
            }

            api.ChatCommands.Create("mymusic")
                .WithDescription("Информация о музыкальном моде")
                .HandleWith(OnMyMusic);

        }

        private TextCommandResult OnMyMusic(TextCommandCallingArgs args)
        {
            if (musicConfig == null)
                return TextCommandResult.Error("Не удалось загрузить конфигурацию музыки");

            var calm = musicConfig.tracks.Count(t => t.situation == "calm");
            var danger = musicConfig.tracks.Count(t => t.situation == "danger");
            var cave = musicConfig.tracks.Count(t => t.situation == "cave");
            var adventure = musicConfig.tracks.Count(t => t.situation == "adventure");
            var other = musicConfig.tracks.Count(t => !(new[] { "calm", "danger", "cave", "adventure" }.Contains(t.situation)));

            var result = $" Мод: lost land Pack\n";
            result += $" Автор: Logins\n";
            result += $" Всего треков: {musicConfig.tracks.Length}\n";
            result += $" Спокойных: {calm}\n";
            result += $" Опасных: {danger}\n";
            result += $" Пещерных: {cave}\n";
            result += $" Приключенческих: {adventure}\n";

            return TextCommandResult.Success(result);
        }

    }

    public class MusicConfig
    {
        public Track[] tracks { get; set; } = new Track[0];
    }

    public class Track
    {
        public string name { get; set; } = "";
        public string situation { get; set; } = "";
    }

}