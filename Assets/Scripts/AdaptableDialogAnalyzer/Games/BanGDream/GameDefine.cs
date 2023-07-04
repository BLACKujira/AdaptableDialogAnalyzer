using System.Collections.Generic;

namespace AdaptableDialogAnalyzer.Games.BanGDream
{
    public static class GameDefine
    {
        public enum MedleyType
        {
            medley_theme = 0,
            medley_free = 1
        }

        public enum LiveEntryType
        {
            OTHER = 0,
            ROOM_MASTER = 1,
            GPS = 2,
            URL = 3
        }

        public enum PrivateRoomSearchType
        {
            None = 0,
            GPS = 1,
            URL = 2
        }

        public enum PracticeTicketType
        {
            practice = 0,
            skill_practice = 1,
            potential_practice = 2
        }

        public enum StampType
        {
            standard = 0,
            rare = 1
        }

        public enum AppStatus
        {
            beta = 0,
            maintenance = 1
        }

        public enum ClearStatus
        {
            not_cleared = 0,
            cleared = 1,
            full_combo = 2,
            all_perfect = 3
        }

        public enum SituationParameter
        {
            none = 0,
            Visual = 1,
            Performance = 2,
            Technique = 3
        }

        public enum UserEpisodeStatus
        {
            unread = 0,
            already_read = 1
        }

        public enum LiveClearStatus
        {
            failure = 0,
            success = 1,
            great_success = 2
        }

        public enum ResourceTypeCategory
        {
            none = 0,
            star = 1,
            live_boost_recovery_item = 2,
            fragment_item = 3,
            ticket = 4,
            coin = 5,
            live_skin = 6,
            deco_parts = 7,
            other = 8
        }

        public enum ResourceType
        {
            none = 0,
            coin = 1,
            star = 2,
            paid_star = 3,
            item = 4,
            area_item = 5,
            situation = 6,
            music = 7,
            practice_ticket = 8,
            rank_exp = 9,
            band_exp = 10,
            live_boost = 11,
            gacha_ticket = 12,
            degree = 13,
            degree_set = 14,
            stamp = 15,
            costume = 16,
            event_item = 17,
            live_boost_recovery_item = 18,
            miracle_ticket = 19,
            music_video = 20,
            michelle_seal = 21,
            election_ticket = 22,
            in_game_skin_lane = 23,
            in_game_skin_background = 24,
            deco_frame = 25,
            deco_pins = 26,
            deco_pins_set = 27,
            limited_item = 28,
            star_seal = 29,
            gacha_seal = 30,
            costume_3d_dress = 31,
            costume_3d_hairstyle = 32,
            limit_break_item = 33,
            costume_3d_making_item = 34,
            live_point = 35,
            deco_character_3d_motion = 36,
            deco_character_3d_motion_set = 37,
            deco_character_background = 38,
            music_video_3d = 39,
            costume_3d_set = 40,
            premium_pass = 41,
            voice_stamp = 42
        }

        public enum Rarity
        {
            None = 0,
            Star1 = 1,
            Star2 = 2,
            Star3 = 3,
            Star4 = 4,
            Star5 = 5
        }

        public enum CharacterIdName : uint
        {
            None = 0u,
            ToyamaKasumi = 1u,
            HanazonoTae = 2u,
            UshigomeRimi = 3u,
            YamabukiSaya = 4u,
            IchigayaArisa = 5u,
            MitakeRan = 6u,
            AobaMoca = 7u,
            UeharaHimari = 8u,
            UdagawaTomoe = 9u,
            HazawaTsugumi = 10u,
            TsurumakiKokoro = 11u,
            SetaKaoru = 12u,
            KitazawaHagumi = 13u,
            MatsubaraKanon = 14u,
            Michelle = 15u,
            MaruyamaAya = 16u,
            HikawaHina = 17u,
            ShirasagiChisato = 18u,
            YamatoMaya = 19u,
            WakamiyaEve = 20u,
            MinatoYukina = 21u,
            HikawaSayo = 22u,
            ImaiLisa = 23u,
            UdagawaAko = 24u,
            ShirokaneRinko = 25u,
            KurataMashiro = 26u,
            KirigayaToko = 27u,
            HiromachiNanami = 28u,
            FutabaTsukushi = 29u,
            YashioRui = 30u,
            Layer = 31u,
            Lock = 32u,
            Masking = 33u,
            Pareo = 34u,
            Chu2 = 35u
        }

        public enum BandIdName
        {
            None = 0,
            PoppinParty = 1,
            Afterglow = 2,
            HelloHappyWorld = 3,
            PastelPalettes = 4,
            Roselia = 5,
            RaiseASuilen = 18,
            Morfonica = 21
        }

        public enum PlayableBandSeq
        {
            None = 0,
            PoppinParty = 1,
            Afterglow = 2,
            PastelPalettes = 3,
            Roselia = 4,
            HelloHappyWorld = 5,
            Morfonica = 6,
            RaiseASuilen = 7
        }

        public enum EpisodeType
        {
            None = 0,
            Standard = 1,
            Memorial = 2,
            Self_Introduction = 3,
            Animation = 4
        }

        public enum SituationType
        {
            None = 0,
            Normal = 1,
            Birthday = 2,
            Animation = 3
        }

        public enum AnimationEpisodeFilterRadioButtonType
        {
            All = 0,
            ExistAnimationEpisode = 1
        }

        public enum LiveClearRank
        {
            S = 0,
            A = 1,
            B = 2,
            C = 3,
            D = 4,
            SS = 5,
            SSS = 6,
            S_plus = 7,
            A_plus = 8,
            B_plus = 9,
            C_plus = 10,
            D_plus = 11,
            SS_plus = 12
        }

        public enum MultiLiveRewardType
        {
            Normal = 0,
            Special = 1
        }

        public enum LogoPosition
        {
            center = 0,
            right = 1,
            left = 2
        }

        public enum CheatUserType
        {
            None = 0,
            Deck_Paramater = 1,
            Play_Level_Score_Rate = 2,
            InGame_Score = 3,
            ScoreUp_Rate = 4,
            Note_Result = 5
        }

        public enum Season
        {
            spring = 0,
            summer = 1,
            autumn = 2,
            winter = 3
        }

        public enum MVPlayMode
        {
            View = 0,
            Live = 1
        }

        public enum StorySeason
        {
            season_1 = 0,
            season_2 = 1,
            season_3 = 2
        }

        public enum StoryChapter : uint
        {
            chapter_0 = 0u,
            chapter_1 = 1u,
            chapter_2 = 2u,
            chapter_3 = 3u,
            limit_chapter_1 = 4u
        }

        public enum ShowMode
        {
            None = 0,
            MainStory = 1,
            BandStory = 2,
            EventStory = 3,
            MemorialStory = 4,
            PrecedingStory = 5,
            DigestStory = 6
        }

        public enum GachaType
        {
            normal = 0,
            free = 1,
            band_separate = 2,
            stepup = 3,
            stepup_allband = 4,
            filter = 5,
            birthday = 6,
            birthday_2021 = 7,
            not_duplicate = 8
        }

        public enum BackstageTalkSetType
        {
            normal = 0,
            birth_day = 1
        }

        public enum ViewProfileSituationStatus
        {
            deck_leader = 0,
            profile_situation = 1
        }

        public enum InGameSDSpriteType
        {
            Normal = 0,
            Happy = 1,
            Sad = 2
        }

        public enum BandDeckRankSymbol
        {
            Hyphen = 0,
            C = 1,
            B = 2,
            A = 3,
            S = 4,
            SS = 5
        }

        public enum CutIn3DMode
        {
            None = 0,
            On = 1,
            Off = 2
        }

        public enum Star3DLiveOrCutIn3DQuality
        {
            None = -1,
            High = 0,
            Normal = 1,
            Low = 2
        }

        public enum DeckMemberType
        {
            Invalid = 0,
            Leader = 1,
            Member1 = 2,
            Member2 = 3,
            Member3 = 4,
            Member4 = 5
        }

        public enum HeaderInformationType
        {
            MainHeaderInformation = 0,
            GachaHeaderInformation = 1
        }

        public const uint ResourceTypeItemMusicCrystalResourceId = 13u;

        public const uint ResourceTypeItemMiracleCrystalResourceId = 14u;

        public const uint ResourceTypeItemMichelleMonakaResourceId = 15u;

        public const uint NotSelectedMusicId = 0u;

        public const uint MedleyLiveMusicCount = 3u;

        public const uint StageChallengeStageNoAchievementCount = 3u;

        public const int GarbageCollectorEnableMemory = 2048;

        public const int FullMVDefaultOffMemory = 5120;

        public const int PERSONAL_SCORE_MAX_DIGIT = 7;

        public const int TOTAL_SCORE_MAX_DIGIT = 8;

        public const int JUDGE_MAX_DIGIT = 4;

        public const int LIVE_BOOST_RECOVER_3_COST = 50;

        public const int LIVE_BOOST_RECOVER_10_COST = 100;

        public const int CONSUME_50_STAR_HEAL_AMOUNT = 3;

        public const int CONSUME_100_STAR_HEAL_AMOUNT = 10;

        public const uint REVIEW_DIALOG_RANK = 5u;

        public const uint BAND_MEMBER_COUNT = 5u;

        public const uint CHARACTER_ID_KASUMI = 1u;

        public const uint CharacterIdMisaki = 601u;

        public const uint NPC_MARINA_CHARACTER_ID = 201u;

        public const uint CIRCLE_AREA_ID = 4u;

        public const uint LoungeAreaId = 13u;

        public const uint ONE_MEGABYTE = 1048576u;

        public const uint DaredemoRoomId = 1u;

        public const uint STORAGE_LACK_MB_WITH_DL_DATA = 50u;

        public const uint STORAGE_FEW_MB_FOR_ALERT_PLAY_GAME = 100u;

        public const float ScoreRankMaxFromTopRankCoefficient = 1.111111f;

        public const string IN_GAME_MV_FILE_NAME = "rgmv_";

        public const float BGM_FADE_TIME = 0.2f;

        public const uint TUTORIAL_ASSETBUNDLE_DOWNLOAD_MAX = 3u;

        public const string TALK_SET_STATUS_ALREADY_READ = "already_read";

        public const string PROFILE_DEGREE_TYPE_FIRST = "first";

        public const string PROFILE_DEGREE_TYPE_SECOND = "second";

        public const int LAYER_DIALOG_DEPTH = 30;

        public const int RequirePlayOpeningMovieDLSize = 400;

        public const string EventStoryReleaseConditionRelationBandRank = "relation_band_rank";

        public const string EventStoryReleaseConditionBandRank = "band_rank";

        public const string EventStoryReleaseConditionUserRank = "user_rank";

        public const float SETTING_DEFAULT_VALUE_HI_SPEED = 5f;

        public const int SETTING_DEFAULT_VALUE_NOTE_SIZE = 100;

        public const int SETTING_DEFAULT_VALUE_JUDGEMENT = 0;

        public const float SETTING_DEFAULT_VALUE_SE_VOLUME = 0.7f;

        public const float SETTING_DEFAULT_VALUE_BGM_VOLUME = 0.7f;

        public const float SETTING_DEFAULT_VALUE_VOICE_VOLUME = 0.7f;

        public const uint SETTING_DEFAULT_VALUE_LONG_NOTE_LINE_BRIGHTNESS = 80u;

        public const uint SettingDefaultValueMvDarkness = 20u;

        public const uint SettingMaxValueMvDarkness = 70u;

        public const uint SettingDefaultValueStar3DLiveDarkness = 30u;

        public const uint SettingMaxValueStar3DLiveDarkness = 70u;

        public const uint SettingDefaultValueCutIn3DDarkness = 20u;

        public const uint SettingMaxValueCutIn3DDarkness = 70u;

        public static readonly IReadOnlyDictionary<Star3DLiveOrCutIn3DQuality, float> CutIn3DRenderTextureScaleRates;

        public const uint SettingDefaultValueLightModeBrightness = 80u;

        public const int DeckMemberMaxCount = 5;

        public const int DeckCount = 30;

        public const int Star3DLiveCustomizeDeckCount = 15;

        public const int LeaderIndexAtInGameDeck = 2;

        public const long MUSIC_SELECT_LIMITED_TIME = 20L;

        public const long DIFFICULTY_SELECT_LIMITED_TIME = 30L;

        public const int PRIVATE_ROOM_PASSWORD_DIGIT = 5;

        public const int Live2dModeDisableMemory = 2048;

        public const string ACHIEVE_KEY_TUTORIAL = "tutorial";

        public const string ACHIEVE_KEY_RANK_3 = "rank3";

        public const string ACHIEVE_KEY_RANK_5 = "rank5";

        public const string ACHIEVE_KEY_RANK_10 = "rank10";

        public const string ACHIEVE_KEY_RANK_15 = "rank15";

        public const string ACHIEVE_KEY_RANK_20 = "rank20";

        public const string ACHIEVE_KEY_RANK_25 = "rank25";

        public const string ACHIEVE_KEY_RANK_30 = "rank30";

        public const int DECK_NAME_LENGTH = 8;

        public const int ROOM_NAME_LENGTH = 10;

        public const int USER_NAME_LENGTH = 10;

        public const int USER_INTRODUCTION_LENGTH = 20;

        public const int NOTICE_BILLING_AMOUNT = 100000;

        public const int CONTINUE_STAR = 50;

        public const string InGameSDSpriteNormal = "anim001";

        public const string InGameSDSpriteHappy = "anim003";

        public const string InGameSDSpriteSad = "anim004";

        public const string AttributeIconImageNamePrefix = "icon_attribute_";

        public const string AttributeNameImageNamePrefix = "txt_type_";

        public const string AttributeLabelImageNamePrefix = "label_type_";

        public const string HEADER_INFORMATION_PREFAB_FOLDER_PATH = "Prefabs/HeaderInformation/";
    }
}